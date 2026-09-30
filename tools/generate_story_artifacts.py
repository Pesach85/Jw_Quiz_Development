# -*- coding: utf-8 -*-
"""Generate webapp/stories.js and StoryLibrary.cs from data/episodes.json.

Usage (repo root):
  python tools/generate_story_artifacts.py
  python tools/generate_story_artifacts.py --check
  python tools/generate_story_artifacts.py --dry-run
"""
from __future__ import annotations

import argparse
import hashlib
import json
import sys
from pathlib import Path
from typing import Any

ROOT = Path(__file__).resolve().parents[1]
DEFAULT_IN = ROOT / "data" / "episodes.json"
DEFAULT_JS = ROOT / "webapp" / "stories.js"
DEFAULT_CS = ROOT / "StoryLibrary.cs"

# Desktop-only fields not in episodes.json (preserved from legacy catalog).
DESKTOP_META: dict[int, dict[str, Any]] = {
    1: {"ImageResourceName": "Eden", "IsDynamic": False},
    2: {"ImageResourceName": "Samson", "IsDynamic": False},
    3: {"ImageResourceName": "Jonah", "IsDynamic": False},
    4: {"ImageResourceName": "SheepGoats", "IsDynamic": False},
    5: {"ImageResourceName": "Plagues", "IsDynamic": False},
    6: {"ImageResourceName": "Elijah", "IsDynamic": False},
    7: {"ImageResourceName": "Esther", "IsDynamic": False},
    8: {"ImageResourceName": "Abraham", "IsDynamic": False},
    9: {"ImageResourceName": "ProdigalSon", "IsDynamic": False},
    10: {"ImageResourceName": "Isaiah", "IsDynamic": False},
    11: {"ImageResourceName": "Noah", "IsDynamic": False},
    12: {"ImageResourceName": "Philip", "IsDynamic": False},
    13: {"ImageResourceName": "DavidGoliath", "IsDynamic": True},
    14: {"ImageResourceName": "Joseph", "IsDynamic": True},
    15: {"ImageResourceName": "Ruth", "IsDynamic": True},
    16: {"ImageResourceName": "Moses", "IsDynamic": True},
    17: {"ImageResourceName": "Samuel", "IsDynamic": True},
    18: {"ImageResourceName": "Samaritan", "IsDynamic": True},
    19: {"ImageResourceName": "Babel", "IsDynamic": True},
    20: {"ImageResourceName": "DanielLions", "IsDynamic": True},
    21: {"ImageResourceName": "SaulPaul", "IsDynamic": True},
    22: {"ImageResourceName": "Jericho", "IsDynamic": True},
    23: {"ImageResourceName": "MarthaMary", "IsDynamic": True},
}


def _sha256_bytes(data: bytes) -> str:
    return hashlib.sha256(data).hexdigest()


def _load_episodes(path: Path) -> list[dict[str, Any]]:
    doc = json.loads(path.read_text(encoding="utf-8"))
    episodes = doc.get("episodes") or []
    return sorted(episodes, key=lambda e: int(e["id"]))


def _js_string(value: str) -> str:
    return json.dumps(value, ensure_ascii=False)


def _episode_comment(ep_id: int, title: str) -> str:
    bar = "─" * max(8, 60 - len(title))
    return f"  // ── Episodio {ep_id}: {title} {bar}"


def _append_js_object_block(lines: list[str], body_lines: list[str], *, trailing_comma: bool) -> None:
    lines.extend(body_lines)
    if trailing_comma:
        lines[-1] = lines[-1] + ","
    lines.append("")


def _render_js(episodes: list[dict[str, Any]], source_hash: str) -> str:
    lines: list[str] = [
        "// AUTO-GENERATED — DO NOT EDIT — source: data/episodes.json",
        f"// source-sha256: {source_hash}",
        "window.JW_STORIES = [",
    ]
    for idx, ep in enumerate(episodes):
        r = ep["rebus"]
        title = r["titleIt"]
        last = idx == len(episodes) - 1
        block = [_episode_comment(int(ep["id"]), title), "  {"]
        block.append(f"    id: {int(ep['id'])},")
        block.append(f"    title: {_js_string(title)},")
        block.append(f"    scriptureReference: {_js_string(ep['scriptureReference'])},")
        block.append(f"    keyword: {_js_string(r['keywordIt'])},")
        block.append(f"    hint: {_js_string(r['hintIt'])},")
        block.append(f"    solution: {_js_string(r['solutionIt'])},")
        block.append(f"    scriptureQuote: {_js_string(r['scriptureQuoteIt'])},")
        block.append(f"    engagementNote: {_js_string(r['engagementNoteIt'])},")
        vk = ", ".join(_js_string(k) for k in r["visibleKeys"])
        hk = ", ".join(_js_string(k) for k in r["hiddenKeys"])
        block.append(f"    visibleKeys: [{vk}],")
        block.append(f"    hiddenKeys: [{hk}],")
        block.append(f"    hintKey: {_js_string(r['hintKey'])},")
        block.append("    imageCaptions: [")
        for cap in r["imageCaptionsIt"]:
            block.append(f"      {_js_string(cap)},")
        block.append("    ]")
        block.append("  }")
        _append_js_object_block(lines, block, trailing_comma=not last)
    lines.append("];")
    lines.append("")
    lines.append("window.JW_IMMERSIVE = [")
    for idx, ep in enumerate(episodes):
        im = ep["immersive"]
        title_it = ep["rebus"]["titleIt"]
        last = idx == len(episodes) - 1
        block = [_episode_comment(int(ep["id"]), title_it), "  {"]
        block.append(f"    id: {int(ep['id'])},")
        block.append(f"    titleEn: {_js_string(im['titleEn'])},")
        block.append(f"    themeIt: {_js_string(im['themeIt'])},")
        block.append(f"    themeEn: {_js_string(im['themeEn'])},")
        block.append(
            f"    intro: {{ it: {_js_string(im['intro']['it'])}, en: {_js_string(im['intro']['en'])} }},"
        )
        block.append("    questions: [")
        for q in im["questions"]:
            block.append("      {")
            block.append(
                f"        prompt: {{ it: {_js_string(q['prompt']['it'])}, en: {_js_string(q['prompt']['en'])} }},"
            )
            block.append("        answers: [")
            for a in q["answers"]:
                ok = "true" if a["ok"] else "false"
                block.append(
                    f"          {{ it: {_js_string(a['it'])}, en: {_js_string(a['en'])}, ok: {ok} }},"
                )
            block.append("        ]")
            block.append("      },")
        block.append("    ],")
        block.append(
            f"    moral: {{ it: {_js_string(im['moral']['it'])}, en: {_js_string(im['moral']['en'])} }},"
        )
        block.append(
            "    theaterQuote: { it: "
            + _js_string(im["theaterQuote"]["it"])
            + ", en: "
            + _js_string(im["theaterQuote"]["en"])
            + " }"
        )
        block.append("  }")
        _append_js_object_block(lines, block, trailing_comma=not last)
    lines.append("];")
    lines.append("")
    return "\n".join(lines)


def _cs_string(value: str) -> str:
    escaped = value.replace("\\", "\\\\").replace("\"", "\\\"")
    return f"\"{escaped}\""


def _render_cs(episodes: list[dict[str, Any]], source_hash: str) -> str:
    lines: list[str] = [
        "using System.Collections.Generic;",
        "",
        "namespace Jw_Quiz_Development",
        "{",
        "    public static class StoryLibrary",
        "    {",
        "        public static IReadOnlyList<Story> Stories { get; } = BuildStories();",
        "",
        "        private static List<Story> BuildStories()",
        "        {",
        "            var list = new List<Story>",
        "            {",
        f"            // AUTO-GENERATED — DO NOT EDIT — source: data/episodes.json",
        f"            // source-sha256: {source_hash}",
    ]
    for ep in episodes:
        ep_id = int(ep["id"])
        r = ep["rebus"]
        meta = DESKTOP_META[ep_id]
        is_dyn = "true" if meta["IsDynamic"] else "false"
        lines.append("            new Story")
        lines.append("            {")
        lines.append(f"                Id = {ep_id},")
        lines.append(f"                Title = {_cs_string(r['titleIt'])},")
        lines.append(f"                ScriptureReference = {_cs_string(ep['scriptureReference'])},")
        lines.append(f"                Keyword = {_cs_string(r['keywordIt'])},")
        lines.append(f"                Hint = {_cs_string(r['hintIt'])},")
        lines.append(f"                Solution = {_cs_string(r['solutionIt'])},")
        lines.append(f"                ScriptureQuote = {_cs_string(r['scriptureQuoteIt'])},")
        lines.append(f"                EngagementNote = {_cs_string(r['engagementNoteIt'])},")
        lines.append(f"                ImageResourceName = {_cs_string(meta['ImageResourceName'])},")
        lines.append(f"                IsDynamic = {is_dyn},")
        vk = ", ".join(_cs_string(k) for k in r["visibleKeys"])
        hk = ", ".join(_cs_string(k) for k in r["hiddenKeys"])
        lines.append(f"                VisibleEmojis = new[] {{ {vk} }},")
        lines.append(f"                HiddenEmojis = new[] {{ {hk} }},")
        lines.append(f"                HintEmoji = {_cs_string(r['hintKey'])},")
        lines.append("                ImageCaptions = new[]")
        lines.append("                {")
        for cap in r["imageCaptionsIt"]:
            lines.append(f"                    {_cs_string(cap)},")
        lines.append("                }")
        lines.append("            },")
    lines.extend(
        [
            "            };",
            "            return list;",
            "        }",
            "    }",
            "}",
            "",
        ]
    )
    return "\n".join(lines)


def generate(in_path: Path) -> tuple[str, str, str]:
    raw = in_path.read_bytes()
    source_hash = _sha256_bytes(raw)
    episodes = _load_episodes(in_path)
    if len(episodes) != 23:
        raise ValueError(f"Expected 23 episodes, got {len(episodes)}")
    js_text = _render_js(episodes, source_hash)
    cs_text = _render_cs(episodes, source_hash)
    return js_text, cs_text, source_hash


def main() -> int:
    parser = argparse.ArgumentParser(description="Generate stories.js and StoryLibrary.cs from episodes.json")
    parser.add_argument("--in", dest="in_path", type=Path, default=DEFAULT_IN)
    parser.add_argument("--out-js", type=Path, default=DEFAULT_JS)
    parser.add_argument("--out-cs", type=Path, default=DEFAULT_CS)
    parser.add_argument("--check", action="store_true", help="Exit 0 if outputs match generated content")
    parser.add_argument("--dry-run", action="store_true", help="Print unified diff summary, do not write")
    args = parser.parse_args()

    in_path = args.in_path if args.in_path.is_absolute() else ROOT / args.in_path
    out_js = args.out_js if args.out_js.is_absolute() else ROOT / args.out_js
    out_cs = args.out_cs if args.out_cs.is_absolute() else ROOT / args.out_cs

    js_text, cs_text, source_hash = generate(in_path)

    if args.check:
        ok = True
        for path, expected in ((out_js, js_text), (out_cs, cs_text)):
            if not path.is_file():
                print(f"CHECK FAIL missing {path}", file=sys.stderr)
                ok = False
                continue
            actual = path.read_text(encoding="utf-8")
            if actual != expected:
                print(f"CHECK FAIL content differs: {path}", file=sys.stderr)
                ok = False
        if ok:
            print(f"CHECK OK (source-sha256: {source_hash})")
            return 0
        return 1

    if args.dry_run:
        print(f"DRY-RUN would write {out_js} and {out_cs} (source-sha256: {source_hash})")
        print(f"JS bytes: {len(js_text.encode('utf-8'))}, CS bytes: {len(cs_text.encode('utf-8'))}")
        return 0

    out_js.write_text(js_text, encoding="utf-8", newline="\n")
    out_cs.write_text(cs_text, encoding="utf-8", newline="\n")
    print(f"Wrote {out_js}")
    print(f"Wrote {out_cs}")
    print(f"source-sha256: {source_hash}")
    return 0


if __name__ == "__main__":
    sys.exit(main())
