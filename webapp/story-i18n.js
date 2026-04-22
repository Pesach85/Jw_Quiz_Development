export const AppLanguages = Object.freeze({
  Italian: "Italian",
  English: "English"
});

const EXACT_ITALIAN_TO_ENGLISH = [
  pair("Il Giardino di Eden", "The Garden of Eden"),
  pair("Sansone e Dalila", "Samson and Delilah"),
  pair("Giona e il Pesce", "Jonah and the Fish"),
  pair("Le Pecore e le Capre", "The Sheep and the Goats"),
  pair("Le 10 Piaghe d'Egitto", "The 10 Plagues of Egypt"),
  pair("Elia e la Siccita'", "Elijah and the Drought"),
  pair("Ester Salva il Popolo", "Esther Saves Her People"),
  pair("Abramo e Isacco al Monte Moria", "Abraham and Isaac on Mount Moriah"),
  pair("Il Figlio Prodigo", "The Prodigal Son"),
  pair("La Profezia della Pace di Isaia", "Isaiah's Prophecy of Peace"),
  pair("Noe' e il Diluvio", "Noah and the Flood"),
  pair("Filippo e l'Eunuco Etiope", "Philip and the Ethiopian Eunuch"),
  pair("Davide e Golia", "David and Goliath"),
  pair("Giuseppe Perdona i Fratelli", "Joseph Forgives His Brothers"),
  pair("Rut e Boaz", "Ruth and Boaz"),
  pair("La Nascita di Mose'", "The Birth of Moses"),
  pair("Anna e Samuele", "Hannah and Samuel"),
  pair("Il Buon Samaritano", "The Good Samaritan"),
  pair("Obbedienza", "Obedience"),
  pair("Fedelta'", "Faithfulness"),
  pair("Misericordia", "Mercy"),
  pair("Giudizio", "Judgment"),
  pair("Potere di Dio", "God's Power"),
  pair("Preghiera", "Prayer"),
  pair("Coraggio", "Courage"),
  pair("Fede", "Faith"),
  pair("Perdono", "Forgiveness"),
  pair("Profezia", "Prophecy"),
  pair("Salvezza", "Salvation"),
  pair("Buona Novella", "Good News"),
  pair("Devozione", "Devotion"),
  pair("Protezione", "Protection"),
  pair("Amore per il Prossimo", "Love for Neighbor"),
  pair("Storia Utente", "User Story"),
  pair("N/D", "N/A"),
  pair("Tema", "Theme"),
  pair("Indizio non disponibile", "Hint not available"),
  pair("Soluzione non disponibile", "Solution not available"),
  pair("Nota non disponibile", "Note not available"),
  pair("Un giovane pastore", "A young shepherd"),
  pair("Un piccolo gregge", "A small flock"),
  pair("Armi e combattimento", "Weapons and battle"),
  pair("Paura nel campo", "Fear in the camp"),
  pair("Qualcosa manca in questa storia...", "Something is missing from this story..."),
  pair("La caduta del nemico", "The enemy's fall"),
  pair("Una vittoria inattesa", "An unexpected victory"),
  pair("Una forza piu' grande delle apparenze", "A strength greater than appearances"),
  pair("Un uomo con grande autorita'", "A man with great authority"),
  pair("Il viaggio nel deserto", "The journey through the desert"),
  pair("Argento e commercio", "Silver and trade"),
  pair("Anni di sofferenza", "Years of suffering"),
  pair("Un incontro inatteso", "An unexpected meeting"),
  pair("Un abbraccio ritrovato", "A restored embrace"),
  pair("Un legame ricostruito", "A rebuilt bond"),
  pair("Una svolta guidata da Dio", "A turn guided by God"),
  pair("Una donna in cammino", "A woman on a journey"),
  pair("Una figura di famiglia", "A family figure"),
  pair("Un campo di raccolta", "A harvest field"),
  pair("Un uomo con un ruolo decisivo", "A man with a decisive role"),
  pair("Un legame che nasce", "A bond taking shape"),
  pair("Un diritto di riscatto", "A right of redemption"),
  pair("Una discendenza benedetta", "A blessed lineage"),
  pair("Scegliere il popolo di Geova", "Choosing Jehovah's people"),
  pair("Un neonato da proteggere", "A newborn to protect"),
  pair("Acqua in movimento", "Moving water"),
  pair("Un luogo di potere", "A place of power"),
  pair("Una donna di alto rango", "A high-ranking woman"),
  pair("Piante lungo la riva", "Plants along the riverbank"),
  pair("Le mani che lo tengono al sicuro", "Hands keeping him safe"),
  pair("Protetto in modo inatteso", "Protected in an unexpected way"),
  pair("Uno sguardo vigile dall'alto", "A watchful gaze from above"),
  pair("Una preghiera intensa", "An intense prayer"),
  pair("Dolore interiore", "Inner pain"),
  pair("Un luogo di adorazione", "A place of worship"),
  pair("Un canto di gratitudine", "A song of gratitude"),
  pair("Un cammino di fede", "A path of faith"),
  pair("Un dono atteso a lungo", "A long-awaited gift"),
  pair("Una vita dedicata a Geova", "A life dedicated to Jehovah"),
  pair("Una richiesta ascoltata", "A prayer that was heard"),
  pair("Un uomo ferito lungo la strada", "A wounded man on the road"),
  pair("La violenza dei briganti", "The violence of bandits"),
  pair("Un animale da viaggio", "A travel animal"),
  pair("Un pagamento per le cure", "A payment for the care"),
  pair("Chi si fermera' ad aiutare?", "Who will stop to help?"),
  pair("Chi scelse di tirare dritto", "Who chose to keep walking"),
  pair("Un gesto di misericordia", "An act of mercy"),
  pair("Conta chi si ferma ad aiutare", "What matters is who stops to help")
];

const FRAGMENT_ITALIAN_TO_ENGLISH = [
  pair("Rivela", "Reveal"),
  pair("Nascondi", "Hide"),
  pair("Storia successiva", "Next story"),
  pair("La vera forza non sta nei capelli, ma nell'alleanza con Dio.", "True strength is not in the hair, but in one's covenant with God."),
  pair("Un serpente, un albero proibito e una scelta fatale.", "A serpent, a forbidden tree, and a fatal choice."),
  pair("Tre giorni nel buio insegnano cosa significa correre verso Dio, non lontano da Lui.", "Three days in the darkness teach what it means to run toward God, not away from Him."),
  pair("Il padre vede il figlio da lontano e corre ad abbracciarlo.", "The father sees the son from far away and runs to embrace him."),
  pair("Una regina rischia la vita presentandosi al re senza essere stata convocata.", "A queen risks her life by appearing before the king uninvited."),
  pair("Un uomo giusto costruisce una grande arca mentre il mondo lo deride.", "A righteous man builds a great ark while the world mocks him."),
  pair("Un carro nel deserto, un rotolo di Isaia e un incontro guidato dagli angeli.", "A chariot in the desert, a scroll of Isaiah, and a meeting guided by angels."),
  pair("Sacerdote e levita passarono oltre. Solo uno straniero si fermo'.", "A priest and a Levite passed by. Only a stranger stopped."),
  pair("Una vedova straniera scelse di seguire il Dio della suocera.", "A foreign widow chose to follow the God of her mother-in-law."),
  pair("Una madre intreccia una cesta di giunchi per salvare il suo bambino dal Nilo.", "A mother weaves a basket of reeds to save her baby from the Nile."),
  pair("Una donna in lacrime al tempio prega con tale fervore da sembrare ubriaca.", "A woman in tears at the sanctuary prays so intensely that she seems drunk."),
  pair("Anni di schiavitu' e prigione nascondevano un piano di Dio.", "Years of slavery and prison were hiding God's purpose."),
  pair("Con Geova, anche i giganti cadono.", "With Jehovah, even giants fall."),
  pair("Anche le ingiustizie piu' dure possono far parte del piano di Dio.", "Even the harshest injustices can be part of God's purpose."),
  pair("La lealta' a Geova va oltre le frontiere etniche e culturali.", "Loyalty to Jehovah goes beyond ethnic and cultural boundaries."),
  pair("Geova usa anche le circostanze piu' disperate per proteggere i Suoi servitori.", "Jehovah can use even the most desperate circumstances to protect His servants."),
  pair("La preghiera sincera viene sempre ascoltata da Geova.", "Sincere prayer is always heard by Jehovah."),
  pair("L'amore per il prossimo non ha confini etnici o religiosi.", "Love for one's neighbor has no ethnic or religious boundaries.")
];

const ITALIAN_TO_ENGLISH_WORDS = {
  adorazione: "worship", ai: "to the", al: "to the", alla: "to the", amore: "love",
  angelo: "angel", angeli: "angels", animale: "animal", animali: "animals", arca: "ark",
  ascoltato: "heard", "autorita'": "authority", bambino: "baby", battaglia: "battle",
  benedetta: "blessed", briganti: "bandits", campo: "camp", cammino: "journey", care: "care",
  carro: "chariot", cesta: "basket", chi: "who", cielo: "heaven", "citta'": "city",
  compassione: "compassion", coraggio: "courage", creatore: "Creator", cure: "care",
  dalla: "from the", dalle: "from the", dal: "from the", "dell'": "of the ", della: "of the",
  delle: "of the", deserto: "desert", dio: "God", dolore: "pain", donna: "woman",
  dono: "gift", dritto: "straight", e: "and", figlio: "son", fiume: "river", forza: "strength",
  fuga: "escape", fede: "faith", famiglia: "family", geova: "Jehovah", gesto: "act",
  gigante: "giant", giorno: "day", giovane: "young", grande: "great", gregge: "flock",
  guerra: "war", guida: "guide", ha: "has", il: "the", imprevisto: "unexpected",
  indizio: "hint", incontro: "meeting", ingiustizie: "injustices", interiore: "inner",
  lacrime: "tears", legame: "bond", lungo: "along", luogo: "place", mani: "hands",
  misericordia: "mercy", movimento: "motion", nascita: "birth", neonato: "newborn",
  nemico: "enemy", note: "note", nuovo: "new", pagamento: "payment", parola: "word",
  pastore: "shepherd", paura: "fear", per: "for", persone: "people", "piu'": "more",
  popolo: "people", potere: "power", preghiera: "prayer", prossimo: "neighbor",
  proteggere: "protect", protezione: "protection", raccolta: "harvest", rango: "rank",
  richiesta: "request", riscatto: "redemption", riva: "shore", ruolo: "role",
  salvezza: "salvation", santuario: "sanctuary", scena: "scene", scegliere: "choosing",
  sicuro: "safe", sicurezza: "safety", soluzione: "solution", sofferenza: "suffering",
  sorpresa: "surprise", storia: "story", strada: "road", tempio: "temple", terra: "earth",
  tirare: "keep", uomo: "man", una: "a", un: "a", viaggio: "journey", vigile: "watchful",
  vittoria: "victory", volta: "time"
};

const WEB_TEXT = {
  Italian: {
    LanguageLabel: "Lingua",
    LanguageItalian: "Italiano",
    LanguageEnglish: "English",
    StorySelectTitle: "Seleziona episodio",
    TotalXpLabel: "XP Totali",
    ExpectedXpLabel: "XP Previsti",
    CreateEpisodeButton: "Crea episodio",
    RevealTwoImagesButton: "Rivela 2 immagini",
    ShowHintButton: "Mostra indizio",
    RevealSolutionButton: "Rivela soluzione",
    HideSolutionButton: "Nascondi soluzione",
    NextStoryButton: "Prossima storia",
    SolutionHeading: "Soluzione",
    NoteLabel: "Nota:",
    EpisodePrefix: "Episodio",
    GuessStoryTitle: "Indovina la storia!",
    CategoryLabel: "Categoria",
    CaptionDefault: "Clicca su una immagine per leggere un indizio breve",
    CaptionPrefix: "Indicazione",
    ImagePrefix: "Immagine",
    StoryImageAlt: "immagine storia",
    ChooseButton: "Scegli...",
    VisibleLabel: "Visibile",
    HiddenLabel: "Nascosta",
    HintLabel: "Indizio",
    CreateNewEpisodeHeading: "Crea nuovo episodio",
    CreateNewEpisodeHelp: "Compila i dati come nell'editor desktop. Su Cloudflare gli episodi saranno condivisi tra tutti gli utenti e tradotti automaticamente in italiano e inglese.",
    CloseButton: "Chiudi",
    TitleLabel: "Titolo",
    ScriptureReferenceLabel: "Riferimento biblico",
    KeywordLabel: "Parola chiave",
    InputLanguageLabel: "Lingua di inserimento",
    HintTextLabel: "Indizio testo",
    SolutionLabel: "Soluzione",
    ScriptureQuoteLabel: "Citazione scritturale",
    EngagementNoteLabel: "Nota engagement",
    VisibleImagesHeading: "5 immagini visibili",
    HiddenImagesHeading: "2 immagini nascoste",
    HintImageHeading: "Immagine indizio",
    SaveEpisodeButton: "Salva episodio",
    PickImageHeading: "Scegli un'immagine",
    CustomPngNameLabel: "Nome PNG custom",
    CustomPngNamePlaceholder: "Es. corona-personalizzata",
    CustomPngFileLabel: "File PNG custom",
    UploadPngButton: "Carica PNG",
    UploadStatusDefault: "I PNG custom saranno condivisi online dopo il deploy Cloudflare con storage configurato.",
    SearchAssetLabel: "Cerca per chiave PNG",
    SearchAssetPlaceholder: "Es. 1F411, boy, crown",
    EditorSaveCloudStatus: "I nuovi episodi saranno condivisi tra tutti gli utenti tramite Cloudflare, con traduzione automatica it/en.",
    EditorLocalOnlyStatus: "API Cloudflare non disponibile: salvataggio temporaneo solo locale.",
    CloudflareActiveStatus: "Cloudflare attivo: i nuovi episodi saranno condivisi tra tutti gli utenti.",
    LocalModeStatus: "Modalita' locale: gli episodi creati ora restano solo nel browser corrente.",
    StorageActiveStatus: "Storage Cloudflare attivo: episodi e PNG custom saranno condivisi.",
    UploadDeferredStatus: "API Cloudflare non trovata in locale. I PNG custom condivisi saranno attivi dopo il deploy con KV/R2 configurati.",
    StorySavedShared: "Episodio {id} salvato e condiviso online.",
    StorySavedLocal: "API non disponibile: episodio salvato solo in locale.",
    FillRequiredFields: "Compila titolo, riferimento biblico, parola chiave, indizio e soluzione.",
    FillImageSlots: "Servono 5 immagini visibili, 2 immagini nascoste e 1 immagine indizio.",
    InvalidImageKey: "Una o piu' chiavi immagine non esistono nel picker. Usa il pulsante Scegli....",
    SelectPngFirst: "Seleziona un file PNG prima di caricare.",
    PngOnly: "Sono accettati solo file PNG.",
    SharedUploadOnly: "Upload condiviso disponibile solo dopo il deploy Cloudflare con storage attivo.",
    UploadInProgress: "Caricamento PNG in corso...",
    UploadSuccess: "PNG caricato: {name}",
    UploadErrorPrefix: "Errore upload PNG: ",
    ApiUnavailableStorySaved: "API non disponibile: episodio salvato solo in locale.",
    ApiUnavailableSharedStorage: "API Cloudflare non disponibile",
    UnknownStoryTitle: "Storia Utente"
  },
  English: {
    LanguageLabel: "Language",
    LanguageItalian: "Italian",
    LanguageEnglish: "English",
    StorySelectTitle: "Select episode",
    TotalXpLabel: "Total XP",
    ExpectedXpLabel: "Expected XP",
    CreateEpisodeButton: "Create episode",
    RevealTwoImagesButton: "Reveal 2 images",
    ShowHintButton: "Show hint",
    RevealSolutionButton: "Reveal solution",
    HideSolutionButton: "Hide solution",
    NextStoryButton: "Next story",
    SolutionHeading: "Solution",
    NoteLabel: "Note:",
    EpisodePrefix: "Episode",
    GuessStoryTitle: "Guess the story!",
    CategoryLabel: "Category",
    CaptionDefault: "Click an image to read a short clue",
    CaptionPrefix: "Clue",
    ImagePrefix: "Image",
    StoryImageAlt: "story image",
    ChooseButton: "Choose...",
    VisibleLabel: "Visible",
    HiddenLabel: "Hidden",
    HintLabel: "Hint",
    CreateNewEpisodeHeading: "Create new episode",
    CreateNewEpisodeHelp: "Fill in the data like the desktop editor. On Cloudflare, episodes will be shared with all users and translated automatically into Italian and English.",
    CloseButton: "Close",
    TitleLabel: "Title",
    ScriptureReferenceLabel: "Scripture reference",
    KeywordLabel: "Keyword",
    InputLanguageLabel: "Input language",
    HintTextLabel: "Hint text",
    SolutionLabel: "Solution",
    ScriptureQuoteLabel: "Scripture quotation",
    EngagementNoteLabel: "Engagement note",
    VisibleImagesHeading: "5 visible images",
    HiddenImagesHeading: "2 hidden images",
    HintImageHeading: "Hint image",
    SaveEpisodeButton: "Save episode",
    PickImageHeading: "Choose an image",
    CustomPngNameLabel: "Custom PNG name",
    CustomPngNamePlaceholder: "Ex. custom-crown",
    CustomPngFileLabel: "Custom PNG file",
    UploadPngButton: "Upload PNG",
    UploadStatusDefault: "Custom PNGs will be shared online after Cloudflare deploy with configured storage.",
    SearchAssetLabel: "Search by PNG key",
    SearchAssetPlaceholder: "Ex. 1F411, boy, crown",
    EditorSaveCloudStatus: "New episodes will be shared with all users through Cloudflare, with automatic it/en translation.",
    EditorLocalOnlyStatus: "Cloudflare API unavailable: temporary local-only save.",
    CloudflareActiveStatus: "Cloudflare active: new episodes will be shared with all users.",
    LocalModeStatus: "Local mode: episodes created now remain only in this browser.",
    StorageActiveStatus: "Cloudflare storage active: episodes and custom PNGs will be shared.",
    UploadDeferredStatus: "Cloudflare API not found locally. Shared custom PNGs will become available after deploy with KV/R2 configured.",
    StorySavedShared: "Episode {id} saved and shared online.",
    StorySavedLocal: "API unavailable: episode saved locally only.",
    FillRequiredFields: "Fill in title, scripture reference, keyword, hint, and solution.",
    FillImageSlots: "You need 5 visible images, 2 hidden images, and 1 hint image.",
    InvalidImageKey: "One or more image keys do not exist in the picker. Use the Choose... button.",
    SelectPngFirst: "Select a PNG file before uploading.",
    PngOnly: "Only PNG files are accepted.",
    SharedUploadOnly: "Shared upload is available only after Cloudflare deploy with active storage.",
    UploadInProgress: "Uploading PNG...",
    UploadSuccess: "PNG uploaded: {name}",
    UploadErrorPrefix: "PNG upload error: ",
    ApiUnavailableStorySaved: "API unavailable: episode saved locally only.",
    ApiUnavailableSharedStorage: "Cloudflare API unavailable",
    UnknownStoryTitle: "User Story"
  }
};

export function normalizeLanguage(value) {
  return String(value || "").toLowerCase() === "english" ? AppLanguages.English : AppLanguages.Italian;
}

export function getWebText(language, key, replacements) {
  const text = (WEB_TEXT[normalizeLanguage(language)] || WEB_TEXT.Italian)[key] || key;
  if (!replacements) {
    return text;
  }
  return Object.keys(replacements).reduce(function (result, replacementKey) {
    return result.replace(new RegExp("\\{" + escapeRegex(replacementKey) + "\\}", "g"), replacements[replacementKey]);
  }, text);
}

export function getLanguageLabel(language, optionLanguage) {
  return optionLanguage === AppLanguages.English
    ? getWebText(language, "LanguageEnglish")
    : getWebText(language, "LanguageItalian");
}

export function translateText(value, sourceLanguage, targetLanguage) {
  if (!value) {
    return "";
  }

  const source = normalizeLanguage(sourceLanguage);
  const target = normalizeLanguage(targetLanguage);
  if (source === target) {
    return String(value);
  }

  if (source === AppLanguages.Italian && target === AppLanguages.English) {
    return translateItalianToEnglish(String(value));
  }

  if (source === AppLanguages.English && target === AppLanguages.Italian) {
    return translateEnglishToItalian(String(value));
  }

  return String(value);
}

export function translateLocalizedText(source, sourceLanguage, targetLanguage) {
  const normalized = normalizeLocalizedText(source);
  return {
    Title: translateText(normalized.Title, sourceLanguage, targetLanguage),
    ScriptureReference: translateText(normalized.ScriptureReference, sourceLanguage, targetLanguage),
    Keyword: translateText(normalized.Keyword, sourceLanguage, targetLanguage),
    Hint: translateText(normalized.Hint, sourceLanguage, targetLanguage),
    Solution: translateText(normalized.Solution, sourceLanguage, targetLanguage),
    EngagementNote: translateText(normalized.EngagementNote, sourceLanguage, targetLanguage),
    ScriptureQuote: translateText(normalized.ScriptureQuote, sourceLanguage, targetLanguage),
    ImageCaptions: normalized.ImageCaptions.map(function (caption) {
      return translateText(caption, sourceLanguage, targetLanguage);
    })
  };
}

export function ensureStoryTranslations(story) {
  if (!story) {
    return story;
  }

  const sourceLanguage = normalizeLanguage(story.sourceLanguage);
  const translations = normalizeTranslations(story.translations);
  const sourceText = hasLocalizedTextContent(buildSourceText(story))
    ? buildSourceText(story)
    : (translations[sourceLanguage] || createEmptyLocalizedText());
  const targetLanguage = sourceLanguage === AppLanguages.Italian ? AppLanguages.English : AppLanguages.Italian;
  const targetText = hasLocalizedTextContent(translations[targetLanguage])
    ? normalizeLocalizedText(translations[targetLanguage])
    : translateLocalizedText(sourceText, sourceLanguage, targetLanguage);

  return Object.assign({}, story, {
    title: sourceText.Title,
    scriptureReference: sourceText.ScriptureReference,
    keyword: sourceText.Keyword,
    hint: sourceText.Hint,
    solution: sourceText.Solution,
    engagementNote: sourceText.EngagementNote,
    scriptureQuote: sourceText.ScriptureQuote,
    imageCaptions: sourceText.ImageCaptions.slice(0, 8),
    sourceLanguage: sourceLanguage,
    translations: {
      Italian: sourceLanguage === AppLanguages.Italian ? sourceText : translateLocalizedText(sourceText, sourceLanguage, AppLanguages.Italian),
      English: sourceLanguage === AppLanguages.English ? sourceText : targetText
    }
  });
}

export function getLocalizedStory(story, language) {
  const normalizedStory = ensureStoryTranslations(story);
  const selectedLanguage = normalizeLanguage(language);
  const localized = normalizeLocalizedText(normalizedStory.translations[selectedLanguage]);

  return Object.assign({}, normalizedStory, {
    title: localized.Title,
    scriptureReference: localized.ScriptureReference,
    keyword: localized.Keyword,
    hint: localized.Hint,
    solution: localized.Solution,
    engagementNote: localized.EngagementNote,
    scriptureQuote: localized.ScriptureQuote,
    imageCaptions: localized.ImageCaptions.slice(0, 8)
  });
}

function buildSourceText(story) {
  return normalizeLocalizedText({
    Title: story.title || story.Title,
    ScriptureReference: story.scriptureReference || story.ScriptureReference,
    Keyword: story.keyword || story.Keyword,
    Hint: story.hint || story.Hint,
    Solution: story.solution || story.Solution,
    EngagementNote: story.engagementNote || story.EngagementNote,
    ScriptureQuote: story.scriptureQuote || story.ScriptureQuote,
    ImageCaptions: story.imageCaptions || story.ImageCaptions
  });
}

function normalizeTranslations(rawTranslations) {
  const raw = rawTranslations || {};
  return {
    Italian: normalizeLocalizedText(raw.Italian || raw.italian),
    English: normalizeLocalizedText(raw.English || raw.english)
  };
}

function normalizeLocalizedText(raw) {
  const text = raw || {};
  const captions = Array.isArray(text.ImageCaptions || text.imageCaptions)
    ? (text.ImageCaptions || text.imageCaptions).slice(0, 8)
    : [];

  while (captions.length < 8) {
    captions.push("");
  }

  return {
    Title: String(text.Title || text.title || "").trim(),
    ScriptureReference: String(text.ScriptureReference || text.scriptureReference || "").trim(),
    Keyword: String(text.Keyword || text.keyword || "").trim(),
    Hint: String(text.Hint || text.hint || "").trim(),
    Solution: String(text.Solution || text.solution || "").trim(),
    EngagementNote: String(text.EngagementNote || text.engagementNote || "").trim(),
    ScriptureQuote: String(text.ScriptureQuote || text.scriptureQuote || "").trim(),
    ImageCaptions: captions.map(function (caption) { return String(caption || "").trim(); })
  };
}

function hasLocalizedTextContent(text) {
  const normalized = normalizeLocalizedText(text);
  return Boolean(
    normalized.Title ||
    normalized.ScriptureReference ||
    normalized.Keyword ||
    normalized.Hint ||
    normalized.Solution ||
    normalized.EngagementNote ||
    normalized.ScriptureQuote ||
    normalized.ImageCaptions.some(function (caption) { return Boolean(caption); })
  );
}

function createEmptyLocalizedText() {
  return normalizeLocalizedText({});
}

function translateItalianToEnglish(value) {
  let translated = applyExactAndFragments(value, EXACT_ITALIAN_TO_ENGLISH, FRAGMENT_ITALIAN_TO_ENGLISH);
  translated = translateWords(translated, ITALIAN_TO_ENGLISH_WORDS);
  return translated.replace(/\s+/g, " ").trim();
}

function translateEnglishToItalian(value) {
  const exact = EXACT_ITALIAN_TO_ENGLISH.map(function (entry) { return pair(entry.value, entry.key); });
  const fragments = FRAGMENT_ITALIAN_TO_ENGLISH.map(function (entry) { return pair(entry.value, entry.key); });
  const words = Object.keys(ITALIAN_TO_ENGLISH_WORDS).reduce(function (result, key) {
    result[ITALIAN_TO_ENGLISH_WORDS[key].toLowerCase()] = key;
    return result;
  }, {});
  let translated = applyExactAndFragments(value, exact, fragments);
  translated = translateWords(translated, words);
  return translated.replace(/\s+/g, " ").trim();
}

function applyExactAndFragments(value, exact, fragments) {
  const trimmed = String(value || "").trim();
  const exactMatch = exact.find(function (entry) {
    return entry.key.toLowerCase() === trimmed.toLowerCase();
  });
  if (exactMatch) {
    return exactMatch.value;
  }

  return fragments.reduce(function (result, entry) {
    return result.replace(new RegExp(escapeRegex(entry.key), "gi"), entry.value);
  }, trimmed);
}

function translateWords(value, glossary) {
  return String(value || "").replace(/[A-Za-zÀ-ÿ']+/g, function (match) {
    const replacement = glossary[match.toLowerCase()];
    return replacement ? preserveWordCase(match, replacement) : match;
  });
}

function preserveWordCase(source, target) {
  if (!source || !target) {
    return target || "";
  }
  if (source.toUpperCase() === source) {
    return target.toUpperCase();
  }
  if (source.charAt(0).toUpperCase() === source.charAt(0)) {
    return target.charAt(0).toUpperCase() + target.slice(1);
  }
  return target;
}

function escapeRegex(value) {
  return String(value).replace(/[.*+?^${}()|[\]\\]/g, "\\$&");
}

function pair(key, value) {
  return { key: key, value: value };
}