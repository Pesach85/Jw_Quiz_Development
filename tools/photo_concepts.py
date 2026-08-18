# -*- coding: utf-8 -*-
"""Photorealistic asset concept map for JW Quiz.
Each concept is generated once; aliases copy to matching rebus keys.
"""
from __future__ import annotations

# concept_id -> (filename_stem for generated master, prompt, list of rebus keys)
CONCEPTS = {
    "people_group": (
        "people_group",
        "Photorealistic close-up of two people standing together outdoors warm golden light, natural skin, soft biblical era clothing suggestion, no text no logo no watermark, square composition centered subject",
        ["093-users", "1F46D"],
    ),
    "person": (
        "person",
        "Photorealistic portrait of a single adult person facing camera, warm natural light, soft earth-tone clothing, no text no logo, square composition",
        ["094-user", "036-man-1", "139-man"],
    ),
    "boy": (
        "boy",
        "Photorealistic young boy portrait, gentle expression, warm natural outdoor light, simple tunic style clothing, no text no logo, square",
        ["038-boy-1"],
    ),
    "baby": (
        "baby",
        "Photorealistic newborn baby wrapped in soft cloth, tender warm light, peaceful, no text no logo, square composition",
        ["039-baby", "1F6BC"],
    ),
    "dancer": (
        "dancer",
        "Photorealistic elegant dancer woman mid-motion in soft fabric dress, dramatic warm stage light, no text no logo, square",
        ["159-dancer", "412-4121149_twins_clipart_emoji_dancing_girls_emoji_png_transparent"],
    ),
    "old_woman": (
        "old_woman",
        "Photorealistic elderly woman wise kind face, soft window light, earth-tone shawl, no text no logo, square portrait",
        ["75-750986_download_svg_download_png_old_woman_emoji_transparent"],
    ),
    "tree": (
        "tree",
        "Photorealistic majestic olive tree or garden tree with lush green canopy, golden hour sunlight, no text no logo, square",
        ["1F333"],
    ),
    "palm": (
        "palm",
        "Photorealistic tall palm tree against blue sky, Mediterranean light, no text no logo, square",
        ["1F334"],
    ),
    "apple": (
        "apple",
        "Photorealistic red apple with water droplets on wood surface, dramatic soft light, no text no logo, square macro",
        ["1F34E"],
    ),
    "snake": (
        "snake",
        "Photorealistic serpent coiled on rock in garden light, detailed scales, natural and cinematic, no text no logo, square",
        ["1F40D"],
    ),
    "sheep": (
        "sheep",
        "Photorealistic white sheep in green pasture, soft daylight, pastoral Bible landscape feel, no text no logo, square",
        ["1F411", "1f411-w", "1f4112"],
    ),
    "goat": (
        "goat",
        "Photorealistic goat standing on rocky hillside, natural daylight, no text no logo, square",
        ["1F413"],
    ),
    "pig": (
        "pig",
        "Photorealistic pig in farmyard soft natural light, no text no logo, square",
        ["1F416"],
    ),
    "camel": (
        "camel",
        "Photorealistic dromedary camel in desert dunes golden light, no text no logo, square",
        ["1F42A"],
    ),
    "whale": (
        "whale",
        "Photorealistic great whale surfacing in deep blue ocean spray, cinematic, no text no logo, square",
        ["1F433"],
    ),
    "frog": (
        "frog",
        "Photorealistic frog on wet stone near water, detailed, natural light, no text no logo, square",
        ["1F438"],
    ),
    "cow": (
        "cow",
        "Photorealistic cow in pastoral field soft daylight, no text no logo, square",
        ["1F404"],
    ),
    "lion": (
        "lion",
        "Photorealistic male lion face close-up, dramatic natural light, no text no logo, square",
        ["1F981"],
    ),
    "horse": (
        "horse",
        "Photorealistic brown horse portrait, soft daylight, no text no logo, square",
        ["Hackney-100", "Hackney_100"],
    ),
    "cricket": (
        "cricket",
        "Photorealistic cricket insect on leaf macro detail, no text no logo, square",
        ["1F997"],
    ),
    "ocean": (
        "ocean",
        "Photorealistic powerful ocean wave crashing blue water foam, cinematic, no text no logo, square",
        ["1F30A"],
    ),
    "rain": (
        "rain",
        "Photorealistic heavy rain falling on dark ground with puddles, moody light, no text no logo, square",
        ["1F327"],
    ),
    "cloud": (
        "cloud",
        "Photorealistic dramatic storm clouds in sky, cinematic light, no text no logo, square",
        ["2601"],
    ),
    "umbrella": (
        "umbrella",
        "Photorealistic open umbrella in raindrops soft bokeh, no text no logo, square",
        ["2614"],
    ),
    "rainbow": (
        "rainbow",
        "Photorealistic bright rainbow over green hills after rain, hopeful light, no text no logo, square",
        ["1F308"],
    ),
    "moon": (
        "moon",
        "Photorealistic crescent moon in night sky stars soft glow, no text no logo, square",
        ["1F318"],
    ),
    "fire": (
        "fire",
        "Photorealistic intense flame fire close-up orange embers, cinematic, no text no logo, square",
        ["1F525"],
    ),
    "question": (
        "question",
        "Photorealistic carved wooden question mark on dark stone pedestal, soft studio light, mysterious, no text no logo besides the mark, square",
        ["2753"],
    ),
    "swords": (
        "swords",
        "Photorealistic two crossed ancient bronze swords, dramatic studio light, no text no logo, square",
        ["2694"],
    ),
    "knife": (
        "knife",
        "Photorealistic ancient bronze knife dagger on dark cloth, studio light, no text no logo, square",
        ["1F52A"],
    ),
    "stop": (
        "stop",
        "Photorealistic red prohibition stop sign circle with slash, clean studio, no extra text, square",
        ["26D4"],
    ),
    "exclaim": (
        "exclaim",
        "Photorealistic glowing golden double exclamation marks carved relief on dark metal, no other text, square",
        ["203C"],
    ),
    "sailboat": (
        "sailboat",
        "Photorealistic wooden sailboat on calm sea sunset light, no text no logo, square",
        ["26F5"],
    ),
    "ship": (
        "ship",
        "Photorealistic large wooden ancient ship on ocean waves, cinematic, no text no logo, square",
        ["1F6A2"],
    ),
    "swimmer": (
        "swimmer",
        "Photorealistic man swimming in sea surface splash, natural light, no text no logo, square",
        ["1F3CA-1F3FB-200D-2642-FE0F"],
    ),
    "castle": (
        "castle",
        "Photorealistic ancient stone castle fortress on hill golden hour, no text no logo, square",
        ["1F3F0"],
    ),
    "temple": (
        "temple",
        "Photorealistic ancient stone temple columns classical architecture blue sky, no text no logo, square",
        ["1F3DB", "1F5FB"],
    ),
    "crown": (
        "crown",
        "Photorealistic ornate gold royal crown on velvet, studio light, no text no logo, square",
        ["1F451"],
    ),
    "money": (
        "money",
        "Photorealistic leather pouch of silver coins spilled, warm light, no text no logo, square",
        ["1F4B0"],
    ),
    "book": (
        "book",
        "Photorealistic open ancient leather-bound Bible-like book parchment pages, soft light, no readable modern text, square",
        ["1F4D6"],
    ),
    "megaphone": (
        "megaphone",
        "Photorealistic brass megaphone on wooden table, studio light, no text no logo, square",
        ["1F4E3", "1F4E31"],
    ),
    "music": (
        "music",
        "Photorealistic ancient harp or lyre musical instrument warm light, no text no logo, square",
        ["1F3B6", "1F3B8"],
    ),
    "heart": (
        "heart",
        "Photorealistic red rose petals forming heart shape soft romantic light, no text no logo, square",
        ["1F498"],
    ),
    "eyes": (
        "eyes",
        "Photorealistic close-up of human eyes looking intently, natural light, no text no logo, square",
        ["1F440"],
    ),
    "ear": (
        "ear",
        "Photorealistic close-up human ear listening, soft light, no text no logo, square",
        ["1F442-1F3FE"],
    ),
    "strong_arm": (
        "strong_arm",
        "Photorealistic muscular arm flexing strength natural outdoor light, no text no logo, square",
        ["1F4AA-1F3FD"],
    ),
    "pray": (
        "pray",
        "Photorealistic hands clasped in prayer close-up warm candlelight, reverent, no text no logo, square",
        ["1F932-1F3FC", "1F932-1F3FD"],
    ),
    "angel": (
        "angel",
        "Photorealistic soft ethereal angelic child figure with gentle light wings suggestion artistic, not cartoon, no text no logo, square",
        ["1F47C"],
    ),
    "skull": (
        "skull",
        "Photorealistic human skull on dark stone dramatic light, solemn, no text no logo, square",
        ["1F480"],
    ),
    "surprise": (
        "surprise",
        "Photorealistic person face expression of shock surprise, natural light, no text no logo, square",
        ["1F632"],
    ),
    "tired": (
        "tired",
        "Photorealistic weary person face exhausted expression soft light, no text no logo, square",
        ["1F629"],
    ),
    "sleep": (
        "sleep",
        "Photorealistic person sleeping peacefully soft moonlight, no text no logo, square",
        ["1F634"],
    ),
    "haircut": (
        "haircut",
        "Photorealistic scissors cutting long hair close-up dramatic light, no text no logo, square",
        ["1F487-1F3FC-200D-2642-FE0F"],
    ),
    "dress": (
        "dress",
        "Photorealistic elegant historical dress garment hanging soft studio light, no text no logo, square",
        ["1F457"],
    ),
    "shoes": (
        "shoes",
        "Photorealistic worn leather sandals ancient style on dusty path, no text no logo, square",
        ["1F456", "1F463"],
    ),
    "farmer": (
        "farmer",
        "Photorealistic farmer in field harvest grain golden hour, no text no logo, square",
        ["1F468-1F3FB-200D-1F33E"],
    ),
    "judge": (
        "judge",
        "Photorealistic wise judge figure in robes solemn courtroom light, no text no logo, square",
        ["1F468-1F3FD-200D-2696-FE0F"],
    ),
    "turban_man": (
        "turban_man",
        "Photorealistic man wearing turban Middle Eastern attire warm light portrait, no text no logo, square",
        ["1F473-200D-2642-FE0F"],
    ),
    "speech": (
        "speech",
        "Photorealistic speech bubble carved wooden prop on table soft light metaphorical, no readable text, square",
        ["1F4AC"],
    ),
    "map": (
        "map",
        "Photorealistic ancient parchment map with routes soft desk light, minimal unreadable script, no logo, square",
        ["1F5FA"],
    ),
    "walk_man": (
        "walk_man",
        "Photorealistic man walking on dusty road from behind cinematic, no text no logo, square",
        ["1F6B6-1F3FF-200D-2642-FE0F"],
    ),
    "walk_woman": (
        "walk_woman",
        "Photorealistic woman walking on dusty road cinematic golden light, no text no logo, square",
        ["1F6B6-200D-2640-FE0F"],
    ),
    "children_crossing": (
        "children_crossing",
        "Photorealistic caution road sign for children crossing, clear photo, minimal text, square",
        ["1F6B8"],
    ),
    "toilet": (
        "toilet",
        "Photorealistic clean bathroom wash basin faucet modern photo, no text no logo, square",
        ["1F6BE"],
    ),
    "blood": (
        "blood",
        "Photorealistic drop of red liquid on stone metaphorical sacrifice theme artistic, no gore excess, no text, square",
        ["1FA78"],
    ),
    "free": (
        "free",
        "Photorealistic open bird cage with bird flying free soft light freedom metaphor, no text no logo, square",
        ["1F193"],
    ),
    "up": (
        "up",
        "Photorealistic stone arrow pointing up carved in rock, soft light, no text no logo, square",
        ["1F199", "2935"],
    ),
    "flag_it": (
        "flag_it",
        "Photorealistic Italian flag fabric waving soft wind daylight, no extra logos, square",
        ["1F1EE-1F1F1"],
    ),
    "beer": (
        "beer",
        "Photorealistic clay ancient cup with drink on wooden table warm light, no brand, square",
        ["1F37A"],
    ),
    "hand_stop": (
        "hand_stop",
        "Photorealistic raised open palm hand stop gesture close-up, natural light, no text no logo, square",
        ["270B-1F3FD"],
    ),
    "end": (
        "end",
        "Photorealistic closed heavy wooden door end of path metaphor, dramatic light, no text no logo, square",
        ["1F51A"],
    ),
    "ten": (
        "ten",
        "Photorealistic carved stone number ten relief, soft studio light, no other text, square",
        ["1F51F"],
    ),
    "dolls": (
        "dolls",
        "Photorealistic pair of traditional decorative dolls on shelf soft light, no text no logo, square",
        ["japanese_dolls_facebook"],
    ),
}

if __name__ == "__main__":
    keys = set()
    for _, _, aliases in CONCEPTS.values():
        keys.update(aliases)
    print("concepts", len(CONCEPTS))
    print("keys covered", len(keys))
