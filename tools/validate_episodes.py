# -*- coding: utf-8 -*-
"""Validate episode catalog JSON (Option B schema).

Usage (from repo root):
  python tools/validate_episodes.py data/fixtures/episodes.minimal.ok.json
  python tools/validate_episodes.py data/episodes.json --full-catalog

Exit 0 if valid, 1 if validation errors, 2 if file/JSON load error.
"""
from __future__ import annotations

import argparse
import json
import re
import sys
from pathlib import Path
from typing import Any

ROOT = Path(__file__).resolve().parents[1]
DEFAULT_WEB_ASSETS = ROOT / "webapp" / "assets"
DEFAULT_DESKTOP_RESOURCES = ROOT / "Resources"
BUILTIN_IDS = set(range(1, 24))  # production catalog ids 1-23
PNG_KEY_RE = re.compile(r"^(custom:[A-Za-z0-9._-]+|[A-Za-z0-9._-]+)$")


def _err(errors: list[str], msg: str) -> None:
    errors.append(msg)


def _require_dict(errors: list[str], value: Any, path: str) -> dict | None:
    if not isinstance(value, dict):
        _err(errors, f"{path}: expected object")
        return None
    return value


def _require_str(errors: list[str], value: Any, path: str) -> None:
    if not isinstance(value, str) or not value.strip():
        _err(errors, f"{path}: non-empty string required")


def _validate_localized_pair(errors: list[str], value: Any, path: str) -> None:
    obj = _require_dict(errors, value, path)
    if obj is None:
        return
    extra = set(obj.keys()) - {"it", "en"}
    if extra:
        _err(errors, f"{path}: unexpected keys {sorted(extra)}")
    _require_str(errors, obj.get("it"), f"{path}.it")
    _require_str(errors, obj.get("en"), f"{path}.en")


def _validate_question(errors: list[str], value: Any, path: str) -> None:
    obj = _require_dict(errors, value, path)
    if obj is None:
        return
    _validate_localized_pair(errors, obj.get("prompt"), f"{path}.prompt")
    answers = obj.get("answers")
    if not isinstance(answers, list) or len(answers) != 3:
        _err(errors, f"{path}.answers: exactly 3 answers required")
        return
    ok_count = 0
    for i, ans in enumerate(answers):
        a = _require_dict(errors, ans, f"{path}.answers[{i}]")
        if a is None:
            continue
        _require_str(errors, a.get("it"), f"{path}.answers[{i}].it")
        _require_str(errors, a.get("en"), f"{path}.answers[{i}].en")
        if a.get("ok") is True:
            ok_count += 1
        elif a.get("ok") is not False:
            _err(errors, f"{path}.answers[{i}].ok: boolean required")
    if ok_count != 1:
        _err(errors, f"{path}.answers: exactly one answer must have ok=true (found {ok_count})")


def _validate_png_key(errors: list[str], key: Any, path: str) -> None:
    if not isinstance(key, str) or not PNG_KEY_RE.match(key):
        _err(errors, f"{path}: invalid PNG key {key!r}")


def _png_exists(key: str, web_assets: Path, desktop_resources: Path) -> bool:
    name = key.split(":", 1)[-1]
    if not name.lower().endswith(".png"):
        name = name + ".png"
    return (web_assets / name).is_file() or (desktop_resources / name).is_file()


def _validate_rebus(
    errors: list[str],
    rebus: Any,
    path: str,
    web_assets: Path,
    desktop_resources: Path,
    check_png_files: bool,
) -> None:
    obj = _require_dict(errors, rebus, path)
    if obj is None:
        return
    for field in (
        "titleIt",
        "keywordIt",
        "hintIt",
        "solutionIt",
        "scriptureQuoteIt",
        "engagementNoteIt",
    ):
        _require_str(errors, obj.get(field), f"{path}.{field}")

    visible = obj.get("visibleKeys")
    if not isinstance(visible, list) or len(visible) != 5:
        _err(errors, f"{path}.visibleKeys: exactly 5 keys required")
    else:
        for i, key in enumerate(visible):
            _validate_png_key(errors, key, f"{path}.visibleKeys[{i}]")
            if check_png_files and isinstance(key, str) and not _png_exists(key, web_assets, desktop_resources):
                _err(errors, f"{path}.visibleKeys[{i}]: PNG not found for key {key!r}")

    hidden = obj.get("hiddenKeys")
    if not isinstance(hidden, list) or len(hidden) != 2:
        _err(errors, f"{path}.hiddenKeys: exactly 2 keys required")
    else:
        for i, key in enumerate(hidden):
            _validate_png_key(errors, key, f"{path}.hiddenKeys[{i}]")
            if check_png_files and isinstance(key, str) and not _png_exists(key, web_assets, desktop_resources):
                _err(errors, f"{path}.hiddenKeys[{i}]: PNG not found for key {key!r}")

    hint_key = obj.get("hintKey")
    _validate_png_key(errors, hint_key, f"{path}.hintKey")
    if check_png_files and isinstance(hint_key, str) and not _png_exists(hint_key, web_assets, desktop_resources):
        _err(errors, f"{path}.hintKey: PNG not found for key {hint_key!r}")

    captions = obj.get("imageCaptionsIt")
    if not isinstance(captions, list) or len(captions) != 8:
        _err(errors, f"{path}.imageCaptionsIt: exactly 8 captions required")
    elif not all(isinstance(c, str) for c in captions):
        _err(errors, f"{path}.imageCaptionsIt: all entries must be strings")


def _validate_immersive(errors: list[str], immersive: Any, path: str) -> None:
    obj = _require_dict(errors, immersive, path)
    if obj is None:
        return
    forbidden = {"symbols", "scriptureQuote", "scriptureQuoteIt"}
    for key in forbidden:
        if key in obj:
            _err(errors, f"{path}: forbidden key {key!r} (use theaterQuote / rebus.scriptureQuoteIt tiers)")
    extra = set(obj.keys()) - {
        "titleEn",
        "themeIt",
        "themeEn",
        "intro",
        "questions",
        "moral",
        "theaterQuote",
    }
    if extra:
        _err(errors, f"{path}: unexpected keys {sorted(extra)}")

    _require_str(errors, obj.get("titleEn"), f"{path}.titleEn")
    _require_str(errors, obj.get("themeIt"), f"{path}.themeIt")
    _require_str(errors, obj.get("themeEn"), f"{path}.themeEn")
    _validate_localized_pair(errors, obj.get("intro"), f"{path}.intro")
    _validate_localized_pair(errors, obj.get("moral"), f"{path}.moral")
    _validate_localized_pair(errors, obj.get("theaterQuote"), f"{path}.theaterQuote")

    questions = obj.get("questions")
    if not isinstance(questions, list) or len(questions) != 2:
        _err(errors, f"{path}.questions: exactly 2 questions required")
    else:
        for i, q in enumerate(questions):
            _validate_question(errors, q, f"{path}.questions[{i}]")


def _validate_episode(
    errors: list[str],
    episode: Any,
    path: str,
    web_assets: Path,
    desktop_resources: Path,
    check_png_files: bool,
) -> int | None:
    obj = _require_dict(errors, episode, path)
    if obj is None:
        return None
    extra = set(obj.keys()) - {"id", "scriptureReference", "rebus", "immersive"}
    if extra:
        _err(errors, f"{path}: unexpected keys {sorted(extra)}")
    ep_id = obj.get("id")
    if not isinstance(ep_id, int) or ep_id < 1:
        _err(errors, f"{path}.id: positive integer required")
        return None
    _require_str(errors, obj.get("scriptureReference"), f"{path}.scriptureReference")
    _validate_rebus(errors, obj.get("rebus"), f"{path}.rebus", web_assets, desktop_resources, check_png_files)
    _validate_immersive(errors, obj.get("immersive"), f"{path}.immersive")
    return ep_id


def validate_document(
    data: Any,
    *,
    full_catalog: bool,
    web_assets: Path,
    desktop_resources: Path,
    check_png_files: bool,
) -> list[str]:
    errors: list[str] = []
    root = _require_dict(errors, data, "$")
    if root is None:
        return errors

    if root.get("schemaVersion") != 1:
        _err(errors, "$.schemaVersion: must be 1")

    episodes = root.get("episodes")
    if not isinstance(episodes, list) or not episodes:
        _err(errors, "$.episodes: non-empty array required")
        return errors

    seen_ids: list[int] = []
    for i, ep in enumerate(episodes):
        ep_id = _validate_episode(
            errors,
            ep,
            f"$.episodes[{i}]",
            web_assets,
            desktop_resources,
            check_png_files,
        )
        if ep_id is not None:
            seen_ids.append(ep_id)

    if len(seen_ids) != len(set(seen_ids)):
        _err(errors, "$.episodes: duplicate id values")

    if full_catalog:
        ids = set(seen_ids)
        if ids != BUILTIN_IDS:
            missing = sorted(BUILTIN_IDS - ids)
            extra = sorted(ids - BUILTIN_IDS)
            if missing:
                _err(errors, f"$.episodes: missing builtin ids {missing}")
            if extra:
                _err(errors, f"$.episodes: unexpected ids outside 1-23 {extra}")

    return errors


def main() -> int:
    parser = argparse.ArgumentParser(description="Validate JW Quiz episodes JSON.")
    parser.add_argument("path", type=Path, help="Path to episodes JSON file")
    parser.add_argument(
        "--full-catalog",
        action="store_true",
        help="Require exactly episode ids 1-23 (production data/episodes.json).",
    )
    parser.add_argument(
        "--skip-png-files",
        action="store_true",
        help="Do not check PNG files on disk (structure only).",
    )
    parser.add_argument(
        "--web-assets",
        type=Path,
        default=DEFAULT_WEB_ASSETS,
        help="Directory for webapp PNG assets",
    )
    parser.add_argument(
        "--desktop-resources",
        type=Path,
        default=DEFAULT_DESKTOP_RESOURCES,
        help="Directory for desktop Resources PNG",
    )
    args = parser.parse_args()

    path = args.path if args.path.is_absolute() else ROOT / args.path
    if not path.is_file():
        print(f"ERROR: file not found: {path}", file=sys.stderr)
        return 2

    try:
        data = json.loads(path.read_text(encoding="utf-8"))
    except (OSError, json.JSONDecodeError) as exc:
        print(f"ERROR: cannot read JSON: {exc}", file=sys.stderr)
        return 2

    errors = validate_document(
        data,
        full_catalog=args.full_catalog,
        web_assets=args.web_assets,
        desktop_resources=args.desktop_resources,
        check_png_files=not args.skip_png_files,
    )

    if errors:
        print(f"INVALID ({len(errors)} issue(s)):", file=sys.stderr)
        for line in errors:
            print(f"  - {line}", file=sys.stderr)
        return 1

    label = "full catalog" if args.full_catalog else "structure"
    print(f"OK: {path} ({label}, {len(data.get('episodes', []))} episode(s))")
    return 0


if __name__ == "__main__":
    sys.exit(main())
