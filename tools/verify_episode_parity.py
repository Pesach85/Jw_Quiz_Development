# -*- coding: utf-8 -*-
"""Structural parity: data/episodes.json ↔ generated artifacts (+ adapter).

Usage (repo root):
  python tools/verify_episode_parity.py
  python tools/verify_episode_parity.py --in data/episodes.json
  python tools/verify_episode_parity.py --skip-adapter
  python tools/verify_episode_parity.py --skip-cs
  python tools/verify_episode_parity.py --skip-hash

Exit 0 only when all checks pass.
"""
from __future__ import annotations

import argparse
import hashlib
import json
import re
import subprocess
import sys
import tempfile
from pathlib import Path
from typing import Any

ROOT = Path(__file__).resolve().parents[1]
DEFAULT_IN = ROOT / "data" / "episodes.json"
DEFAULT_JS = ROOT / "webapp" / "stories.js"
DEFAULT_CS = ROOT / "StoryLibrary.cs"
DEFAULT_INDEX = ROOT / "webapp" / "index.html"
ASSETS = ROOT / "webapp" / "assets"
RESOURCES = ROOT / "Resources"

FAILS = 0
WARNS = 0


def ok(msg: str) -> None:
    print(f"OK {msg}")


def fail(msg: str) -> None:
    global FAILS
    FAILS += 1
    print(f"FAIL {msg}")


def warn(msg: str) -> None:
    global WARNS
    WARNS += 1
    print(f"WARN {msg}")


def sha256_bytes(data: bytes) -> str:
    return hashlib.sha256(data).hexdigest()


def extract_header_hash(path: Path) -> str | None:
    text = path.read_text(encoding="utf-8")
    m = re.search(r"source-sha256:\s*([0-9a-fA-F]{64})", text)
    return m.group(1).lower() if m else None


def load_episodes(path: Path) -> list[dict[str, Any]]:
    doc = json.loads(path.read_text(encoding="utf-8"))
    return sorted(doc.get("episodes") or [], key=lambda e: int(e["id"]))


def expected_jw_story(ep: dict[str, Any]) -> dict[str, Any]:
    r = ep["rebus"]
    return {
        "id": int(ep["id"]),
        "title": r["titleIt"],
        "scriptureReference": ep["scriptureReference"],
        "keyword": r["keywordIt"],
        "hint": r["hintIt"],
        "solution": r["solutionIt"],
        "scriptureQuote": r["scriptureQuoteIt"],
        "engagementNote": r["engagementNoteIt"],
        "visibleKeys": list(r["visibleKeys"]),
        "hiddenKeys": list(r["hiddenKeys"]),
        "hintKey": r["hintKey"],
        "imageCaptions": list(r["imageCaptionsIt"]),
    }


def expected_jw_immersive(ep: dict[str, Any]) -> dict[str, Any]:
    im = ep["immersive"]
    return {
        "id": int(ep["id"]),
        "titleEn": im["titleEn"],
        "themeIt": im["themeIt"],
        "themeEn": im["themeEn"],
        "intro": {"it": im["intro"]["it"], "en": im["intro"]["en"]},
        "questions": im["questions"],
        "moral": {"it": im["moral"]["it"], "en": im["moral"]["en"]},
        "theaterQuote": {
            "it": im["theaterQuote"]["it"],
            "en": im["theaterQuote"]["en"],
        },
    }


def deep_diff(path: str, expected: Any, actual: Any, ep_id: int | None = None) -> None:
    prefix = f"{ep_id} " if ep_id is not None else ""
    if type(expected) != type(actual) and not (
        isinstance(expected, (int, float)) and isinstance(actual, (int, float))
    ):
        # allow list vs tuple etc.
        if isinstance(expected, list) and isinstance(actual, list):
            pass
        elif isinstance(expected, dict) and isinstance(actual, dict):
            pass
        else:
            fail(f"{prefix}{path} type {type(expected).__name__} vs {type(actual).__name__}")
            return
    if isinstance(expected, dict):
        ek, ak = set(expected), set(actual)
        for k in sorted(ek - ak):
            fail(f"{prefix}{path}.{k} missing in actual")
        for k in sorted(ak - ek):
            fail(f"{prefix}{path}.{k} unexpected in actual")
        for k in sorted(ek & ak):
            deep_diff(f"{path}.{k}", expected[k], actual[k], ep_id)
    elif isinstance(expected, list):
        if len(expected) != len(actual):
            fail(f"{prefix}{path} len {len(expected)} vs {len(actual)}")
            return
        for i, (e, a) in enumerate(zip(expected, actual)):
            deep_diff(f"{path}[{i}]", e, a, ep_id)
    else:
        if expected != actual:
            fail(f"{prefix}{path} expected={expected!r} actual={actual!r}")


def load_js_catalogs(js_path: Path) -> tuple[list[dict], list[dict]]:
    """Load window.JW_STORIES / JW_IMMERSIVE via Node (stdlib-adjacent; Node required for JS)."""
    script = r"""
const fs = require('fs');
const vm = require('vm');
const code = fs.readFileSync(process.argv[2], 'utf8');
const window = {};
vm.runInNewContext(code, { window, console });
if (!Array.isArray(window.JW_STORIES) || !Array.isArray(window.JW_IMMERSIVE)) {
  console.error('JW_STORIES/JW_IMMERSIVE missing');
  process.exit(2);
}
process.stdout.write(JSON.stringify({
  stories: window.JW_STORIES,
  immersive: window.JW_IMMERSIVE
}));
"""
    with tempfile.NamedTemporaryFile(
        "w", suffix=".cjs", delete=False, encoding="utf-8"
    ) as tmp:
        tmp.write(script)
        tmp_path = tmp.name
    try:
        proc = subprocess.run(
            ["node", tmp_path, str(js_path)],
            capture_output=True,
            text=True,
            encoding="utf-8",
            cwd=str(ROOT),
        )
    finally:
        Path(tmp_path).unlink(missing_ok=True)
    if proc.returncode != 0:
        fail(f"node load stories.js: {proc.stderr.strip() or proc.stdout.strip()}")
        return [], []
    data = json.loads(proc.stdout)
    return data["stories"], data["immersive"]


def parse_cs_stories(cs_path: Path) -> dict[int, dict[str, Any]]:
    text = cs_path.read_text(encoding="utf-8")
    blocks = re.split(r"\n\s*new Story\s*\n\s*\{", text)
    out: dict[int, dict[str, Any]] = {}
    for block in blocks[1:]:
        id_m = re.search(r"Id\s*=\s*(\d+)\s*,", block)
        if not id_m:
            continue
        eid = int(id_m.group(1))

        def field(name: str) -> str | None:
            m = re.search(rf"{name}\s*=\s*\"((?:\\.|[^\"\\])*)\"\s*,", block)
            if not m:
                return None
            return bytes(m.group(1), "utf-8").decode("unicode_escape") if "\\" in m.group(1) else m.group(1)

        # Prefer simple unescape for \" and \\
        def field_simple(name: str) -> str | None:
            m = re.search(rf"{name}\s*=\s*\"((?:\\.|[^\"\\])*)\"\s*,", block)
            if not m:
                return None
            return m.group(1).replace('\\"', '"').replace("\\\\", "\\")

        def arr(name: str) -> list[str]:
            m = re.search(rf"{name}\s*=\s*new\[\]\s*\{{([^}}]*)\}}", block)
            if not m:
                return []
            return [x.strip().strip('"') for x in m.group(1).split(",") if x.strip()]

        caps_m = re.search(r"ImageCaptions\s*=\s*new\[\]\s*\n\s*\{([^}]*)\}", block, re.S)
        caps: list[str] = []
        if caps_m:
            caps = re.findall(r"\"((?:\\.|[^\"\\])*)\"", caps_m.group(1))
            caps = [c.replace('\\"', '"').replace("\\\\", "\\") for c in caps]

        out[eid] = {
            "Title": field_simple("Title"),
            "ScriptureReference": field_simple("ScriptureReference"),
            "Keyword": field_simple("Keyword"),
            "Hint": field_simple("Hint"),
            "Solution": field_simple("Solution"),
            "ScriptureQuote": field_simple("ScriptureQuote"),
            "EngagementNote": field_simple("EngagementNote"),
            "HintEmoji": field_simple("HintEmoji"),
            "VisibleEmojis": arr("VisibleEmojis"),
            "HiddenEmojis": arr("HiddenEmojis"),
            "ImageCaptions": caps,
        }
    return out


def extract_decor_symbols(index_path: Path) -> dict[int, list[str]]:
    text = index_path.read_text(encoding="utf-8")
    m = re.search(r"const DECOR_SYMBOLS\s*=\s*\{([\s\S]*?)\n\s*\};", text)
    if not m:
        fail("index.html DECOR_SYMBOLS block missing")
        return {}
    body = m.group(1)
    found: dict[int, list[str]] = {}
    for sm in re.finditer(r"(\d+)\s*:\s*\[([^\]]*)\]", body):
        eid = int(sm.group(1))
        parts = re.findall(r"\"((?:\\.|[^\"\\])*)\"", sm.group(2))
        found[eid] = parts
    return found


def run_adapter_check(js_path: Path, index_path: Path, episodes: list[dict[str, Any]]) -> None:
    decor = extract_decor_symbols(index_path)
    if not decor:
        return
    for ep in episodes:
        eid = int(ep["id"])
        syms = decor.get(eid)
        if not syms:
            fail(f"{eid} adapter DECOR_SYMBOLS missing or empty")
        elif len(syms) != 3:
            fail(f"{eid} adapter DECOR_SYMBOLS length {len(syms)} != 3")
    stories, immersive = load_js_catalogs(js_path)
    if not stories:
        return
    imm_by_id = {int(x["id"]): x for x in immersive}
    adapted = []
    for rebus in stories:
        eid = int(rebus["id"])
        imm = imm_by_id.get(eid)
        if not imm:
            fail(f"{eid} adapter missing immersive")
            continue
        adapted.append(
            {
                "id": eid,
                "title": {"it": rebus["title"], "en": imm["titleEn"]},
                "theme": {"it": imm["themeIt"], "en": imm["themeEn"]},
                "scripture": rebus["scriptureReference"],
                "symbols": decor.get(eid, []),
                "intro": imm["intro"],
                "questions": imm["questions"],
                "moral": imm["moral"],
                "quote": imm["theaterQuote"],
            }
        )

    # Expected from episodes.json + DECOR_SYMBOLS (same adapter semantics)
    expected = []
    for ep in episodes:
        eid = int(ep["id"])
        r = ep["rebus"]
        im = ep["immersive"]
        expected.append(
            {
                "id": eid,
                "title": {"it": r["titleIt"], "en": im["titleEn"]},
                "theme": {"it": im["themeIt"], "en": im["themeEn"]},
                "scripture": ep["scriptureReference"],
                "symbols": decor.get(eid, []),
                "intro": im["intro"],
                "questions": im["questions"],
                "moral": im["moral"],
                "quote": im["theaterQuote"],
            }
        )

    if len(adapted) != len(expected):
        fail(f"adapter count {len(adapted)} vs expected {len(expected)}")
        return
    for exp, act in zip(expected, adapted):
        deep_diff("adapter", exp, act, exp["id"])
    if FAILS == 0:
        ok("adapter STORIES == JW_STORIES x JW_IMMERSIVE + DECOR_SYMBOLS")


def png_exists(key: str) -> bool:
    if not key:
        return False
    return (ASSETS / f"{key}.png").is_file() or (RESOURCES / f"{key}.png").is_file()


def check_structural(ep: dict[str, Any]) -> None:
    eid = int(ep["id"])
    r = ep["rebus"]
    vis = list(r["visibleKeys"])
    hid = list(r["hiddenKeys"])
    hint = r["hintKey"]

    if len(vis) != 5:
        fail(f"{eid} visibleKeys.length {len(vis)} != 5")
    if len(hid) != 2:
        fail(f"{eid} hiddenKeys.length {len(hid)} != 2")
    if not isinstance(hint, str) or not hint.strip():
        fail(f"{eid} hintKey missing/empty")

    overlap = set(vis) & set(hid)
    if overlap:
        fail(f"{eid} visible∩hidden {sorted(overlap)}")

    if hint in vis:
        fail(f"{eid} hintKey in visibleKeys ({hint})")
    elif hint == hid[0]:
        ok(f"{eid} hintKey==hiddenKeys[0] (documented)")
    elif len(hid) > 1 and hint == hid[1]:
        warn(f"{eid} hintKey==hiddenKeys[1]")

    caps = r["imageCaptionsIt"]
    if len(caps) != 8:
        fail(f"{eid} imageCaptionsIt.length {len(caps)} != 8")
    for i, c in enumerate(caps):
        if not str(c).strip():
            fail(f"{eid} imageCaptionsIt[{i}] empty")

    qs = ep["immersive"]["questions"]
    if len(qs) != 2:
        fail(f"{eid} questions.length {len(qs)} != 2")
    for qi, q in enumerate(qs):
        ans = q.get("answers") or []
        if len(ans) != 3:
            fail(f"{eid} questions[{qi}].answers.length {len(ans)} != 3")
        oks = sum(1 for a in ans if a.get("ok") is True)
        if oks != 1:
            fail(f"{eid} questions[{qi}] ok=true count {oks} != 1")

    for key in vis + hid + [hint]:
        if not png_exists(key):
            fail(f"{eid} PNG missing for key {key}")


def check_i18n(ep: dict[str, Any]) -> None:
    eid = int(ep["id"])
    im = ep["immersive"]

    def nonempty(label: str, val: Any) -> None:
        if not isinstance(val, str) or not val.strip():
            fail(f"{eid} i18n {label} empty")

    nonempty("titleEn", im.get("titleEn"))
    nonempty("themeIt", im.get("themeIt"))
    nonempty("themeEn", im.get("themeEn"))
    nonempty("intro.it", im.get("intro", {}).get("it"))
    nonempty("intro.en", im.get("intro", {}).get("en"))
    nonempty("moral.it", im.get("moral", {}).get("it"))
    nonempty("moral.en", im.get("moral", {}).get("en"))
    nonempty("theaterQuote.it", im.get("theaterQuote", {}).get("it"))
    nonempty("theaterQuote.en", im.get("theaterQuote", {}).get("en"))
    for qi, q in enumerate(im.get("questions") or []):
        nonempty(f"questions[{qi}].prompt.it", q.get("prompt", {}).get("it"))
        nonempty(f"questions[{qi}].prompt.en", q.get("prompt", {}).get("en"))
        for ai, a in enumerate(q.get("answers") or []):
            # schema uses answers[].it / .en (not .text.it)
            nonempty(f"questions[{qi}].answers[{ai}].it", a.get("it"))
            nonempty(f"questions[{qi}].answers[{ai}].en", a.get("en"))


def main() -> int:
    ap = argparse.ArgumentParser(description="Verify episode dataset parity")
    ap.add_argument("--in", dest="in_path", type=Path, default=DEFAULT_IN)
    ap.add_argument("--skip-adapter", action="store_true")
    ap.add_argument("--skip-cs", action="store_true")
    ap.add_argument("--skip-hash", action="store_true")
    args = ap.parse_args()

    in_path: Path = args.in_path if args.in_path.is_absolute() else ROOT / args.in_path
    if not in_path.is_file():
        fail(f"episodes missing: {in_path}")
        return 1

    raw = in_path.read_bytes()
    source_hash = sha256_bytes(raw)
    episodes = load_episodes(in_path)
    ok(f"loaded {len(episodes)} episode(s) from {in_path.relative_to(ROOT)}")

    # --- Verifica 1: header hash ---
    if not args.skip_hash:
        for path in (DEFAULT_JS, DEFAULT_CS):
            h = extract_header_hash(path)
            if h is None:
                fail(f"source-sha256 missing in {path.name}")
            elif h != source_hash:
                fail(
                    f"generated stale: {path.name} has {h}, episodes.json is {source_hash} — "
                    "run python tools/sync_all.py"
                )
            else:
                ok(f"source-sha256 match {path.name}")
    else:
        ok("skip-hash")

    # --- Verifica 2: campo-per-campo JS ---
    stories, immersive = load_js_catalogs(DEFAULT_JS)
    if stories and immersive:
        by_s = {int(x["id"]): x for x in stories}
        by_i = {int(x["id"]): x for x in immersive}
        for ep in episodes:
            eid = int(ep["id"])
            if eid not in by_s:
                fail(f"{eid} missing in JW_STORIES")
            else:
                deep_diff("JW_STORIES", expected_jw_story(ep), by_s[eid], eid)
            if eid not in by_i:
                fail(f"{eid} missing in JW_IMMERSIVE")
            else:
                deep_diff("JW_IMMERSIVE", expected_jw_immersive(ep), by_i[eid], eid)
        if FAILS == 0:
            ok("JW_STORIES + JW_IMMERSIVE field parity")

    # --- Verifica 2b: StoryLibrary.cs ---
    if not args.skip_cs:
        cs_map = parse_cs_stories(DEFAULT_CS)
        for ep in episodes:
            eid = int(ep["id"])
            r = ep["rebus"]
            row = cs_map.get(eid)
            if not row:
                fail(f"{eid} missing in StoryLibrary.cs")
                continue
            expected_cs = {
                "Title": r["titleIt"],
                "ScriptureReference": ep["scriptureReference"],
                "Keyword": r["keywordIt"],
                "Hint": r["hintIt"],
                "Solution": r["solutionIt"],
                "ScriptureQuote": r["scriptureQuoteIt"],
                "EngagementNote": r["engagementNoteIt"],
                "HintEmoji": r["hintKey"],
                "VisibleEmojis": list(r["visibleKeys"]),
                "HiddenEmojis": list(r["hiddenKeys"]),
                "ImageCaptions": list(r["imageCaptionsIt"]),
            }
            deep_diff("StoryLibrary", expected_cs, row, eid)
        if FAILS == 0:
            ok("StoryLibrary.cs field parity")
    else:
        ok("skip-cs")

    # --- Verifica 2c: adapter ---
    if not args.skip_adapter:
        run_adapter_check(DEFAULT_JS, DEFAULT_INDEX, episodes)
    else:
        ok("skip-adapter")

    # --- Verifica 3 + 4 ---
    for ep in episodes:
        check_structural(ep)
        check_i18n(ep)
    ok("structural + i18n checks completed")

    print("---")
    if FAILS:
        print(f"RESULT FAIL ({FAILS} failure(s), {WARNS} warning(s))")
        return 1
    print(f"RESULT OK ({WARNS} warning(s))")
    return 0


if __name__ == "__main__":
    sys.exit(main())
