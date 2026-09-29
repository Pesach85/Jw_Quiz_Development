# -*- coding: utf-8 -*-
"""One-shot Step 1: build data/episodes.json from STORIES + stories.js + StoryLibrary.cs.

Run from repo root:
  python tools/migrate_episodes_step1.py
  python tools/validate_episodes.py data/episodes.json --full-catalog
"""
from __future__ import annotations

import json
import re
import subprocess
import sys
from pathlib import Path
from typing import Any

ROOT = Path(__file__).resolve().parents[1]
INDEX = ROOT / "webapp" / "index.html"
STORIES_JS = ROOT / "webapp" / "stories.js"
STORY_CS = ROOT / "StoryLibrary.cs"
OUT = ROOT / "data" / "episodes.json"
REPORT = ROOT / "data" / "episodes_migration_report.json"

EP8_TITLE_IT = "Abramo e Isacco al Monte Moria"


def _run_node(script: str) -> Any:
    proc = subprocess.run(
        ["node", "-e", script],
        cwd=str(ROOT),
        capture_output=True,
        text=True,
        encoding="utf-8",
        check=False,
    )
    if proc.returncode != 0:
        raise RuntimeError(f"Node failed:\n{proc.stderr}\n{proc.stdout}")
    return json.loads(proc.stdout.strip())


def load_immersive_from_index() -> dict[int, dict[str, Any]]:
    script = r"""
const fs = require('fs');
const html = fs.readFileSync('webapp/index.html', 'utf8');
const i0 = html.indexOf('const STORIES = [');
const i1 = html.indexOf('\n    function pair(it, en)', i0);
if (i0 < 0 || i1 < 0) throw new Error('STORIES block not found');
const body = html.slice(i0, i1);
function pair(it, en) { return { it, en }; }
function q(prompt, answers) { return { prompt, answers }; }
function story(id, titleIt, titleEn, themeIt, themeEn, scripture, symbols, intro, questions, moral, quote) {
  return {
    id,
    title: pair(titleIt, titleEn),
    theme: pair(themeIt, themeEn),
    scripture,
    symbols,
    intro,
    questions,
    moral,
    quote
  };
}
const STORIES = eval(body.replace('const STORIES = ', ''));
console.log(JSON.stringify(STORIES));
"""
    rows = _run_node(script)
    by_id: dict[int, dict[str, Any]] = {}
    for row in rows:
        by_id[int(row["id"])] = row
    return by_id


def load_rebus_from_stories_js() -> dict[int, dict[str, Any]]:
    script = r"""
const fs = require('fs');
const code = fs.readFileSync('webapp/stories.js', 'utf8');
eval(code.replace('window.JW_STORIES', 'var JW_STORIES'));
console.log(JSON.stringify(JW_STORIES));
"""
    rows = _run_node(script)
    by_id: dict[int, dict[str, Any]] = {}
    for row in rows:
        by_id[int(row["id"])] = row
    return by_id


def _cs_unescape(s: str) -> str:
    return s.replace("\\'", "'").replace('\\"', '"')


def load_rebus_from_cs() -> dict[int, dict[str, Any]]:
    text = STORY_CS.read_text(encoding="utf-8")
    blocks = re.split(r"\n\s*new Story\s*\{", text)[1:]
    by_id: dict[int, dict[str, Any]] = {}
    for block in blocks:
        id_m = re.search(r"Id\s*=\s*(\d+)", block)
        if not id_m:
            continue
        ep_id = int(id_m.group(1))

        def grab_string(name: str) -> str | None:
            m = re.search(rf"{name}\s*=\s*\"((?:[^\"\\]|\\.)*)\"", block)
            return _cs_unescape(m.group(1)) if m else None

        def grab_array(name: str) -> list[str]:
            m = re.search(rf"{name}\s*=\s*new\[\]\s*\{{([^}}]+)\}}", block, re.DOTALL)
            if not m:
                return []
            return [_cs_unescape(x.strip().strip('"')) for x in re.findall(r"\"((?:[^\"\\]|\\.)*)\"", m.group(1))]

        by_id[ep_id] = {
            "title": grab_string("Title"),
            "keyword": grab_string("Keyword"),
            "scriptureReference": grab_string("ScriptureReference"),
            "scriptureQuote": grab_string("ScriptureQuote"),
            "hint": grab_string("Hint"),
            "solution": grab_string("Solution"),
            "engagementNote": grab_string("EngagementNote"),
            "visibleKeys": grab_array("VisibleEmojis"),
            "hiddenKeys": grab_array("HiddenEmojis"),
            "hintKey": grab_string("HintEmoji"),
            "imageCaptions": grab_array("ImageCaptions"),
        }
    return by_id


def pick_rebus_string(
    ep_id: int,
    field: str,
    js: dict[str, Any],
    cs: dict[str, Any],
    report_notes: list[str],
) -> str:
    js_val = js.get(field)
    cs_map = {
        "title": "title",
        "keyword": "keyword",
        "hint": "hint",
        "solution": "solution",
        "scriptureQuote": "scriptureQuote",
        "engagementNote": "engagementNote",
    }
    cs_val = cs.get(cs_map.get(field, field))

    if js_val == cs_val or not cs_val:
        return str(js_val or "")
    if not js_val:
        report_notes.append(f"id {ep_id} rebus.{field}: using C# (JS empty)")
        return str(cs_val)
    # Prefer JS (stories.js canonical for web rebus); note drift
    if js_val != cs_val:
        report_notes.append(
            f"id {ep_id} rebus.{field}: JS vs C# differ; kept stories.js ({len(str(js_val))} vs {len(str(cs_val))} chars)"
        )
    return str(js_val)


def pick_keys(ep_id: int, js: dict[str, Any], cs: dict[str, Any], report_notes: list[str]) -> tuple[list[str], list[str], str]:
    vk = list(js.get("visibleKeys") or [])
    hk = list(js.get("hiddenKeys") or [])
    hk_key = str(js.get("hintKey") or "")
    cs_vk = list(cs.get("visibleKeys") or [])
    cs_hk = list(cs.get("hiddenKeys") or [])
    cs_hint = str(cs.get("hintKey") or "")

    if len(vk) != 5 and len(cs_vk) == 5:
        report_notes.append(f"id {ep_id} visibleKeys: stories.js has {len(vk)} — using StoryLibrary.cs (5 keys)")
        vk = cs_vk
    elif vk and cs_vk and vk != cs_vk:
        report_notes.append(f"id {ep_id} visibleKeys: kept stories.js (differs from C#)")

    if len(hk) != 2 and len(cs_hk) == 2:
        report_notes.append(f"id {ep_id} hiddenKeys: stories.js has {len(hk)} — using StoryLibrary.cs (2 keys)")
        hk = cs_hk
    elif not hk and cs_hk:
        hk = cs_hk
        report_notes.append(f"id {ep_id} hiddenKeys: fallback C#")

    if not hk_key and cs_hint:
        hk_key = cs_hint
        report_notes.append(f"id {ep_id} hintKey: fallback C#")
    elif hk_key and cs_hint and hk_key != cs_hint:
        report_notes.append(f"id {ep_id} hintKey: kept stories.js ({hk_key!r} vs C# {cs_hint!r})")

    if len(vk) != 5 or len(hk) != 2 or not hk_key:
        raise ValueError(f"id {ep_id}: cannot resolve rebus keys (visible={len(vk)}, hidden={len(hk)}, hint={hk_key!r})")
    return vk, hk, hk_key


def pick_captions(ep_id: int, js: dict[str, Any], cs: dict[str, Any], report_notes: list[str]) -> list[str]:
    cap = list(js.get("imageCaptions") or [])
    cs_cap = list(cs.get("imageCaptions") or [])
    if len(cap) == 8:
        if cs_cap and cap != cs_cap:
            report_notes.append(f"id {ep_id} imageCaptionsIt: kept stories.js (C# differs)")
        return cap
    if len(cs_cap) == 8:
        report_notes.append(
            f"id {ep_id} imageCaptionsIt: stories.js has {len(cap)} — using StoryLibrary.cs (8 captions)"
        )
        return cs_cap
    raise ValueError(f"id {ep_id}: cannot resolve 8 captions (js={len(cap)}, cs={len(cs_cap)})")


def build_episode(
    ep_id: int,
    immersive: dict[str, Any],
    js: dict[str, Any],
    cs: dict[str, Any],
    report_notes: list[str],
) -> dict[str, Any]:
    title_it = pick_rebus_string(ep_id, "title", js, cs, report_notes)
    if ep_id == 8:
        if title_it != EP8_TITLE_IT:
            report_notes.append(f"id 8 titleIt: unified Q1 '{EP8_TITLE_IT}' (was stories.js '{title_it}')")
        title_it = EP8_TITLE_IT

    visible, hidden, hint_key = pick_keys(ep_id, js, cs, report_notes)
    captions = pick_captions(ep_id, js, cs, report_notes)

    scripture_ref = (
        js.get("scriptureReference")
        or cs.get("scriptureReference")
        or immersive.get("scripture")
        or ""
    )
    if js.get("scriptureReference") != cs.get("scriptureReference") and js.get("scriptureReference") and cs.get("scriptureReference"):
        report_notes.append(f"id {ep_id} scriptureReference: kept stories.js")

    intro = immersive["intro"]
    if isinstance(intro, dict) and "it" in intro and "en" in intro:
        intro_pair = {"it": intro["it"], "en": intro["en"]}
    else:
        intro_pair = {"it": str(intro.get("it", "")), "en": str(intro.get("en", ""))}

    questions = []
    for q in immersive["questions"]:
        answers = []
        for a in q["answers"]:
            answers.append({"it": a["it"], "en": a["en"], "ok": bool(a["ok"])})
        questions.append({"prompt": {"it": q["prompt"]["it"], "en": q["prompt"]["en"]}, "answers": answers})

    moral = immersive["moral"]
    quote = immersive["quote"]
    title_en = immersive["title"]["en"]
    theme_it = immersive["theme"]["it"]
    theme_en = immersive["theme"]["en"]

    return {
        "id": ep_id,
        "scriptureReference": scripture_ref,
        "rebus": {
            "titleIt": title_it,
            "keywordIt": pick_rebus_string(ep_id, "keyword", js, cs, report_notes),
            "hintIt": pick_rebus_string(ep_id, "hint", js, cs, report_notes),
            "solutionIt": pick_rebus_string(ep_id, "solution", js, cs, report_notes),
            "scriptureQuoteIt": pick_rebus_string(ep_id, "scriptureQuote", js, cs, report_notes),
            "engagementNoteIt": pick_rebus_string(ep_id, "engagementNote", js, cs, report_notes),
            "visibleKeys": visible,
            "hiddenKeys": hidden,
            "hintKey": hint_key,
            "imageCaptionsIt": captions,
        },
        "immersive": {
            "titleEn": title_en,
            "themeIt": theme_it,
            "themeEn": theme_en,
            "intro": intro_pair,
            "questions": questions,
            "moral": {"it": moral["it"], "en": moral["en"]},
            "theaterQuote": {"it": quote["it"], "en": quote["en"]},
        },
    }


def inventory(imm, js, cs) -> list[dict[str, Any]]:
    rows = []
    for i in range(1, 19):
        rows.append(
            {
                "id": i,
                "STORIES": i in imm,
                "stories.js": i in js,
                "StoryLibrary.cs": i in cs,
            }
        )
    return rows


def cross_check(
    episodes: list[dict[str, Any]],
    imm: dict[int, dict[str, Any]],
    js: dict[int, dict[str, Any]],
    cs: dict[int, dict[str, Any]],
) -> list[dict[str, Any]]:
    diffs: list[dict[str, Any]] = []
    expected_prefixes = (
        "id 8 titleIt",
        "id ",
    )

    for ep in episodes:
        eid = ep["id"]
        im = imm[eid]
        j = js[eid]
        c = cs[eid]

        # rebus strings vs stories.js (should match except ep8 titleIt intentional)
        for field, js_key in [
            ("keywordIt", "keyword"),
            ("hintIt", "hint"),
            ("solutionIt", "solution"),
            ("scriptureQuoteIt", "scriptureQuote"),
            ("engagementNoteIt", "engagementNote"),
        ]:
            if ep["rebus"][field] != j.get(js_key):
                diffs.append(
                    {
                        "id": eid,
                        "field": f"rebus.{field}",
                        "expected": "stories.js",
                        "unexpected": ep["rebus"][field] != j.get(js_key),
                        "note": "investigate" if ep["rebus"][field] != j.get(js_key) else "",
                    }
                )

        title_it_expected = EP8_TITLE_IT if eid == 8 else j.get("title")
        if ep["rebus"]["titleIt"] != title_it_expected:
            diffs.append({"id": eid, "field": "rebus.titleIt", "unexpected": True})

        js_vk = j.get("visibleKeys") or []
        if ep["rebus"]["visibleKeys"] != js_vk and len(js_vk) == 5:
            diffs.append({"id": eid, "field": "rebus.visibleKeys", "unexpected": True})
        if ep["immersive"]["titleEn"] != im["title"]["en"]:
            diffs.append({"id": eid, "field": "immersive.titleEn", "unexpected": True})
        if ep["immersive"]["theaterQuote"]["it"] != im["quote"]["it"]:
            diffs.append({"id": eid, "field": "immersive.theaterQuote.it", "unexpected": True})

        if ep["immersive"]["themeIt"] != im["theme"]["it"]:
            diffs.append({"id": eid, "field": "immersive.themeIt", "unexpected": True})

    return [d for d in diffs if d.get("unexpected")]


def main() -> int:
    report_notes: list[str] = []
    imm = load_immersive_from_index()
    js = load_rebus_from_stories_js()
    cs = load_rebus_from_cs()

    inv = inventory(imm, js, cs)
    missing = [r for r in inv if not (r["STORIES"] and r["stories.js"] and r["StoryLibrary.cs"])]
    if missing:
        print("INVENTORY FAIL — missing ids:", missing, file=sys.stderr)
        return 2

    episodes = []
    for i in range(1, 19):
        episodes.append(build_episode(i, imm[i], js[i], cs[i], report_notes))

    doc = {"schemaVersion": 1, "episodes": episodes}
    OUT.parent.mkdir(parents=True, exist_ok=True)
    OUT.write_text(json.dumps(doc, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")

    cross = cross_check(episodes, imm, js, cs)
    REPORT.write_text(
        json.dumps(
            {
                "inventory": inv,
                "mergeNotes": report_notes,
                "crossCheckUnexpected": cross,
            },
            ensure_ascii=False,
            indent=2,
        )
        + "\n",
        encoding="utf-8",
    )

    print(f"Wrote {OUT} ({len(episodes)} episodes)")
    print(f"Wrote {REPORT}")
    if cross:
        print(f"WARNING: {len(cross)} unexpected cross-check flags (see report)", file=sys.stderr)
        return 3
    return 0


if __name__ == "__main__":
    sys.exit(main())
