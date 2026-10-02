#!/usr/bin/env python3
"""Prove canon ids stay < userStoryIdMin and new user-story allocators emit >= that floor."""

from __future__ import annotations

import json
import re
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
CONTRACT_PATH = ROOT / "data" / "id_namespaces.json"
EPISODES_PATH = ROOT / "data" / "episodes.json"
CLOUD_PATH = ROOT / "functions" / "api" / "stories.js"
LOCAL_PATH = ROOT / "webapp" / "app.js"
DESKTOP_PATH = ROOT / "UserStoryLibrary.cs"


def fail(message: str) -> None:
    print(f"FAIL: {message}", file=sys.stderr)
    raise SystemExit(1)


def cloud_next(min_id: int, existing: list[int], counter: int | None = None) -> int:
    """Same rule as functions/api/stories.js: max(counter || MIN, MIN, ...ids) + 1."""
    base = min_id if counter is None else counter
    return max([base, min_id, *existing]) + 1


def local_next(seed: int, existing: list[int]) -> int:
    """Same rule as saveEditorStory: reduce(Math.max, seed) + 1."""
    peak = seed
    for story_id in existing:
        peak = max(peak, story_id)
    return peak + 1


def desktop_next(start_id: int, existing: list[int]) -> int:
    """Empty archive uses START; otherwise max+1; then Max(START, next)."""
    nxt = start_id if not existing else max(existing) + 1
    return max(start_id, nxt)


def extract_cloud_floor(source: str) -> int:
    found = re.findall(r"const\s+MIN_STORY_ID\s*=\s*(\d+)\s*;", source)
    if found != ["1000"] and len(found) != 1:
        if any(item == "18" for item in found):
            fail("cloud allocator floor is still 18")
        fail(f"cloud MIN_STORY_ID must be a single assignment, found {found}")
    if found[0] == "18":
        fail("cloud allocator floor is still 18")
    return int(found[0])


def extract_local_seed(source: str) -> int:
    match = re.search(
        r"async function saveEditorStory\(\) \{(?P<body>.*?)\n  \}",
        source,
        re.S,
    )
    if not match:
        fail("saveEditorStory() not found")
    body = match.group("body")
    seeds = re.findall(r"\}, (\d+)\) \+ 1;", body)
    if seeds == ["18"] or "18" in seeds:
        fail("classic/local allocator floor is still 18")
    if seeds != ["999"]:
        fail(f"saveEditorStory allocator seed must be 999, found {seeds}")
    return int(seeds[0])


def extract_desktop_start(source: str) -> int:
    found = re.findall(r"USER_STORY_START_ID\s*=\s*(\d+)\s*;", source)
    if found != ["1000"]:
        fail(f"USER_STORY_START_ID must be 1000, found {found}")
    match = re.search(
        r"public static Story AddStory\(Story story\)\s*\{(?P<body>.*?)\n        \}",
        source,
        re.S,
    )
    if not match:
        fail("AddStory() not found")
    body = match.group("body")
    if "Math.Max(USER_STORY_START_ID, next)" not in body:
        fail("desktop allocator can ignore USER_STORY_START_ID when the archive is non-empty")
    if re.search(r"newId\s*=\s*userStories\.Count", body):
        fail("desktop allocator still assigns newId without the 1000 floor")
    return 1000


def assert_case(label: str, actual: int, user_min: int, exact: int | None = None) -> None:
    if actual < user_min:
        fail(f"{label} produced {actual}, which is < {user_min}")
    if exact is not None and actual != exact:
        fail(f"{label} produced {actual}, expected {exact}")
    print(f"{label} -> {actual}")


def assert_negative_fixtures() -> None:
    """The old floor must be rejected by the same functions. Does not touch the repo."""
    if cloud_next(18, []) >= 1000:
        fail("negative fixture broken: cloud floor 18 was accepted")
    if local_next(18, []) >= 1000:
        fail("negative fixture broken: local seed 18 was accepted")
    if desktop_next(1000, [19]) < 1000:
        fail("negative fixture broken: desktop Math.Max should lift max 19 to 1000")
    bare = 19 + 1
    if bare >= 1000:
        fail("negative fixture broken: bare max+1 from 19 was treated as in-namespace")


def main() -> None:
    if not CONTRACT_PATH.is_file():
        fail(f"missing {CONTRACT_PATH}")
    try:
        contract = json.loads(CONTRACT_PATH.read_text(encoding="utf-8"))
    except json.JSONDecodeError as exc:
        fail(f"malformed namespace JSON: {exc}")

    if contract.get("schemaVersion") != 1:
        fail("schemaVersion must be 1")
    if contract.get("canonIdMin") != 1:
        fail("canonIdMin must be 1")
    user_min = contract.get("userStoryIdMin")
    if user_min != 1000:
        fail("userStoryIdMin must be 1000")

    episodes = json.loads(EPISODES_PATH.read_text(encoding="utf-8"))
    ids = [int(item["id"]) for item in episodes["episodes"]]
    if not ids:
        fail("canon has no episodes")
    if min(ids) < contract["canonIdMin"]:
        fail(f"canon id {min(ids)} is < canonIdMin")
    if max(ids) >= user_min:
        fail(f"canon id {max(ids)} is >= {user_min}")

    cloud_floor = extract_cloud_floor(CLOUD_PATH.read_text(encoding="utf-8"))
    local_seed = extract_local_seed(LOCAL_PATH.read_text(encoding="utf-8"))
    desktop_start = extract_desktop_start(DESKTOP_PATH.read_text(encoding="utf-8"))

    if cloud_floor != user_min:
        fail(f"cloud allocator floor {cloud_floor} != {user_min}")
    if local_seed != user_min - 1:
        fail(f"local seed {local_seed} does not yield a floor of {user_min}")
    if desktop_start != user_min:
        fail(f"desktop start {desktop_start} != {user_min}")

    print("cloud:")
    assert_case("empty", cloud_next(cloud_floor, []), user_min)
    assert_case("19,23", cloud_next(cloud_floor, [19, 23]), user_min)
    assert_case("1500", cloud_next(cloud_floor, [1500]), user_min)

    print("local:")
    assert_case("no canon", local_next(local_seed, []), user_min, 1000)
    assert_case("canon max23", local_next(local_seed, [23]), user_min, 1000)
    assert_case("shared 500", local_next(local_seed, [500]), user_min, 1000)
    assert_case("shared 999", local_next(local_seed, [999]), user_min, 1000)
    assert_case("shared 1000", local_next(local_seed, [1000]), user_min, 1001)

    print("desktop:")
    assert_case("empty", desktop_next(desktop_start, []), user_min, 1000)
    assert_case("max19", desktop_next(desktop_start, [19]), user_min, 1000)
    assert_case("max1000", desktop_next(desktop_start, [1000]), user_min, 1001)

    assert_negative_fixtures()
    print("OK id namespaces")


if __name__ == "__main__":
    main()
