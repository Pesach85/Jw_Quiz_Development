import {
  AppLanguages,
  ensureStoryTranslations,
  getLanguageLabel,
  getLocalizedStory,
  getWebText,
  normalizeLanguage,
  translateText
} from "./story-i18n.js";

(function () {
  var KEY_UNKNOWN = "2753";
  var KEY_HINT_PLACEHOLDER = "1F525";
  var LOCAL_STORIES_KEY = "jwquiz_web_user_stories_v1";
  var LANGUAGE_STORAGE_KEY = "jwquiz_web_language";

  function generateClientId() {
    var stored = sessionStorage.getItem("jwquiz_cid");
    if (stored && stored.length >= 8 && stored.length <= 60 && /^[a-z0-9_]+$/.test(stored)) return stored;
    var id = "u" + Date.now().toString(36) + "_" + Math.random().toString(36).slice(2, 9);
    sessionStorage.setItem("jwquiz_cid", id);
    return id;
  }

  var state = {
    storyIndex: 0,
    revealCount: 0,
    hintRevealed: false,
    solutionVisible: false,
    totalXp: Number(localStorage.getItem("jwquiz_web_xp") || "0"),
    sharedStories: [],
    customAssets: [],
    apiAvailable: false,
    currentLanguage: normalizeLanguage(localStorage.getItem(LANGUAGE_STORAGE_KEY) || AppLanguages.Italian),
    clientId: generateClientId(),
    heartbeatInterval: null,
    adminAuthenticated: false
  };

  var languageSelectEl = document.getElementById("languageSelect");
  var slotsEl = document.getElementById("slots");
  var captionEl = document.getElementById("caption");
  var titleEl = document.getElementById("storyTitle");
  var refEl = document.getElementById("storyRef");
  var xpEl = document.getElementById("xpValue");
  var expectedXpEl = document.getElementById("xpExpected");
  var solutionEl = document.getElementById("solutionPanel");
  var scriptureEl = document.getElementById("scriptureQuote");
  var noteEl = document.getElementById("engagementNote");
  var selectorEl = document.getElementById("storySelect");

  var revealBtn = document.getElementById("btnReveal");
  var hintBtn = document.getElementById("btnHint");
  var solutionBtn = document.getElementById("btnSolution");
  var nextBtn = document.getElementById("btnNext");
  var createBtn = document.getElementById("btnCreateStory");

  var editorPanel = document.getElementById("editorPanel");
  var closeEditorBtn = document.getElementById("btnCloseEditor");
  var saveStoryBtn = document.getElementById("btnSaveStory");
  var editorStatusEl = document.getElementById("editorStatus");
  var editorSourceLanguageEl = document.getElementById("editorSourceLanguage");

  var pickerPanel = document.getElementById("pickerPanel");
  var closePickerBtn = document.getElementById("btnClosePicker");
  var assetGridEl = document.getElementById("assetGrid");
  var assetSearchEl = document.getElementById("assetSearch");
  var assetUploadNameEl = document.getElementById("assetUploadName");
  var assetUploadFileEl = document.getElementById("assetUploadFile");
  var assetUploadStatusEl = document.getElementById("assetUploadStatus");
  var uploadAssetBtn = document.getElementById("btnUploadAsset");

  var onlineBadgeEl = document.getElementById("onlineBadge");
  var onlineCountEl = document.getElementById("onlineCount");
  var starsBoxEl = document.getElementById("starsBox");
  var adminPanelEl = document.getElementById("adminPanel");
  var adminLoginAreaEl = document.getElementById("adminLoginArea");
  var adminStatsEl = document.getElementById("adminStats");
  var adminSecretInputEl = document.getElementById("adminSecret");
  var btnAdminLoginEl = document.getElementById("btnAdminLogin");
  var adminErrorEl = document.getElementById("adminError");
  var statOnlineEl = document.getElementById("statOnline");
  var statViewsEl = document.getElementById("statViews");
  var statCompletionsEl = document.getElementById("statCompletions");
  var statSessionsEl = document.getElementById("statSessions");
  var btnAdminRefreshEl = document.getElementById("btnAdminRefresh");
  var adminStatusMsgEl = document.getElementById("adminStatusMsg");
  var adminOfflineNoteEl = document.getElementById("adminOfflineNote");

  var builtInAssetKeys = (window.JW_ASSET_KEYS || []).slice();
  var builtInAssetLookup = buildAssetLookup(builtInAssetKeys);
  var builtInStories = sanitizeStories(window.JW_STORIES || []);
  var slotInputs = { visible: [], hidden: [], hint: [] };
  var pickerTarget = null;

  var editorFields = {
    title: document.getElementById("editorTitle"),
    reference: document.getElementById("editorReference"),
    keyword: document.getElementById("editorKeyword"),
    hint: document.getElementById("editorHint"),
    solution: document.getElementById("editorSolution"),
    quote: document.getElementById("editorQuote"),
    note: document.getElementById("editorNote")
  };

  function normalizeAssetToken(value) {
    return String(value || "").toLowerCase().replace(/[^a-z0-9]/g, "");
  }

  function isCustomAssetKey(key) {
    return typeof key === "string" && key.indexOf("custom:") === 0;
  }

  function customAssetName(key) {
    return String(key || "").slice(7);
  }

  function buildAssetLookup(keys) {
    var lookup = {};
    keys.forEach(function (key) {
      lookup[normalizeAssetToken(key)] = key;
    });
    return lookup;
  }

  function resolveBuiltInAssetKey(key) {
    var normalized = normalizeAssetToken(key);
    return builtInAssetLookup[normalized] || key || KEY_UNKNOWN;
  }

  function resolveStoryAssetKey(key) {
    if (isCustomAssetKey(key)) {
      return key;
    }
    return resolveBuiltInAssetKey(key);
  }

  function imageUrl(key) {
    if (isCustomAssetKey(key)) {
      return "/api/assets/" + encodeURIComponent(customAssetName(key));
    }
    return "assets/" + resolveBuiltInAssetKey(key) + ".png";
  }

  function sanitizeStory(raw) {
    var visibleKeys = (raw.visibleKeys || []).slice(0, 5).map(resolveStoryAssetKey);
    var hiddenKeys = (raw.hiddenKeys || []).slice(0, 2).map(resolveStoryAssetKey);
    var captions = (raw.imageCaptions || []).slice(0, 8);

    while (visibleKeys.length < 5) {
      visibleKeys.push(KEY_UNKNOWN);
    }
    while (hiddenKeys.length < 2) {
      hiddenKeys.push(KEY_UNKNOWN);
    }
    while (captions.length < 8) {
      captions.push("");
    }

    return ensureStoryTranslations({
      id: Number(raw.id || 0),
      title: raw.title || "",
      scriptureReference: raw.scriptureReference || "",
      keyword: raw.keyword || "",
      hint: raw.hint || "",
      solution: raw.solution || "",
      scriptureQuote: raw.scriptureQuote || "",
      engagementNote: raw.engagementNote || "",
      visibleKeys: visibleKeys,
      hiddenKeys: hiddenKeys,
      hintKey: resolveStoryAssetKey(raw.hintKey || KEY_HINT_PLACEHOLDER),
      imageCaptions: captions,
      sourceLanguage: normalizeLanguage(raw.sourceLanguage || AppLanguages.Italian),
      translations: raw.translations || null,
      isUserCreated: Boolean(raw.isUserCreated)
    });
  }

  function sanitizeStories(list) {
    return list.map(sanitizeStory);
  }

  function getLocalStories() {
    try {
      return sanitizeStories(JSON.parse(localStorage.getItem(LOCAL_STORIES_KEY) || "[]"));
    } catch (error) {
      return [];
    }
  }

  function saveLocalStories(stories) {
    localStorage.setItem(LOCAL_STORIES_KEY, JSON.stringify(stories));
  }

  function getAllStories() {
    return builtInStories.concat(state.sharedStories);
  }

  function getAllAssets() {
    var builtIn = builtInAssetKeys.map(function (key) {
      return { key: key, label: key, builtIn: true };
    });
    return builtIn.concat(state.customAssets);
  }

  function story() {
    var stories = getAllStories();
    return stories[state.storyIndex] || stories[0];
  }

  function storyView() {
    return getLocalizedStory(story(), state.currentLanguage);
  }

  function normalizeCaptionToken(value) {
    if (!value) {
      return "";
    }
    var normalized = String(value).toLowerCase();
    if (typeof normalized.normalize === "function") {
      normalized = normalized.normalize("NFD").replace(/[\u0300-\u036f]/g, "");
    }
    return normalized.replace(/[^a-z0-9]+/g, " ").trim();
  }

  function captionStopWords() {
    return {
      a: true, ad: true, agli: true, ai: true, al: true, alla: true, alle: true, allo: true, anche: true,
      chi: true, che: true, come: true, con: true, da: true, dagli: true, dai: true, dal: true,
      dalla: true, dalle: true, dallo: true, dei: true, degli: true, del: true, della: true,
      delle: true, dello: true, di: true, dove: true, e: true, ed: true, fra: true, gli: true,
      ha: true, ho: true, i: true, il: true, in: true, indizio: true, l: true, la: true, le: true,
      lo: true, ma: true, mi: true, ne: true, nei: true, negli: true, nel: true, nella: true,
      nelle: true, nello: true, no: true, non: true, o: true, per: true, piu: true, questa: true,
      queste: true, questi: true, questo: true, quella: true, quelle: true, quelli: true,
      quello: true, racconto: true, solo: true, storia: true, su: true, sua: true, sue: true,
      suo: true, suoi: true, tnm: true, tra: true, un: true, una: true, uno: true, va: true,
      about: true, all: true, an: true, and: true, along: true, another: true, around: true,
      be: true, by: true, clue: true, does: true, from: true, help: true, important: true,
      into: true, more: true, note: true, one: true, road: true, scene: true, some: true,
      story: true, that: true, the: true, their: true, there: true, these: true, this: true,
      two: true, was: true, what: true, who: true, with: true
    };
  }

  function tokenizeCaptionText(value) {
    var stopWords = captionStopWords();
    var result = {};
    normalizeCaptionToken(value).split(/\s+/).forEach(function (piece) {
      if (!piece || piece.length < 4 || stopWords[piece]) {
        return;
      }
      result[piece] = true;
    });
    return result;
  }

  function addCapitalizedCaptionTerms(target, value) {
    var matches = String(value || "").match(/[A-Za-zÀ-ÿ']+/g) || [];
    matches.forEach(function (word) {
      if (!word || word.charAt(0) !== word.charAt(0).toUpperCase()) {
        return;
      }
      var token = normalizeCaptionToken(word);
      if (!token || token.length < 4 || captionStopWords()[token]) {
        return;
      }
      target[token] = true;
    });
  }

  function mergeCaptionTerms(target, source) {
    Object.keys(source).forEach(function (key) {
      target[key] = true;
    });
  }

  function buildStrongCaptionTerms(currentStory) {
    var result = {};
    [currentStory.title, currentStory.keyword].forEach(function (value) {
      mergeCaptionTerms(result, tokenizeCaptionText(value));
      addCapitalizedCaptionTerms(result, value);
    });
    [currentStory.hint, currentStory.solution, currentStory.scriptureQuote].forEach(function (value) {
      addCapitalizedCaptionTerms(result, value);
    });
    return result;
  }

  function buildContextCaptionTerms(currentStory) {
    var result = {};
    [currentStory.title, currentStory.keyword, currentStory.hint, currentStory.solution, currentStory.scriptureQuote].forEach(function (value) {
      mergeCaptionTerms(result, tokenizeCaptionText(value));
    });
    return result;
  }

  function imageKeyForIndex(currentStory, index) {
    if (index < 5) {
      return currentStory.visibleKeys[index] || KEY_UNKNOWN;
    }
    if (index < 7) {
      return currentStory.hiddenKeys[index - 5] || KEY_UNKNOWN;
    }
    return currentStory.hintKey || KEY_HINT_PLACEHOLDER;
  }

  function neutralCaptionByImageKey(language, key) {
    var neutral = {
      "038-boy-1": "Un giovane coinvolto nel racconto",
      "039-baby": "Un bambino al centro della scena",
      "036-man-1": "Un uomo nella scena",
      "031-man-2": "Un altro uomo nella scena",
      "094-user": "Una persona importante",
      "093-users": "Un gruppo di persone",
      "1F411": "Un animale del gregge",
      "1F42A": "Un animale da carico",
      "Hackney-100": "Un animale da viaggio",
      "2694": "Armi e conflitto",
      "1F632": "Sorpresa e tensione",
      "1F629": "Dolore e fatica",
      "1F498": "Un gesto di amore o compassione",
      "1F4B0": "Un pagamento o una ricompensa",
      "1F3DB": "Un luogo di adorazione",
      "1F30A": "Acqua in movimento",
      "1F3F0": "Un luogo di potere",
      "1F451": "Un'autorita' importante",
      "1F333": "Piante lungo il cammino",
      "1F932-1F3FC": "Una preghiera o una supplica",
      "1F47C": "Un aiuto inatteso",
      "1F3B6": "Un canto o una celebrazione",
      "1F5FA": "Un percorso da seguire",
      "1F440": "Un dettaglio da osservare bene",
      "203C": "Un richiamo importante",
      "1F4D6": "Un messaggio da comprendere",
      "26D4": "Un rifiuto o un ostacolo",
      "2753": "Un dettaglio ancora nascosto"
    };
    var value = neutral[key] || "";
    return language === AppLanguages.English ? translateText(value, AppLanguages.Italian, AppLanguages.English) : value;
  }

  function fallbackCaption(currentStory, index, language) {
    var neutral = neutralCaptionByImageKey(language, imageKeyForIndex(currentStory, index));
    if (neutral) {
      return neutral;
    }
    if (index === 7) {
      return language === AppLanguages.English ? "A visual hint to interpret" : "Un indizio visivo da interpretare";
    }
    if (index >= 5) {
      return language === AppLanguages.English ? "A detail that will be revealed later" : "Un dettaglio che si rivelera' piu' avanti";
    }
    return language === AppLanguages.English ? "An important element of the story" : "Un elemento importante del racconto";
  }

  function isCaptionTooExplicit(currentStory, caption) {
    var captionTerms = tokenizeCaptionText(caption);
    var strongTerms = buildStrongCaptionTerms(currentStory);
    var contextTerms = buildContextCaptionTerms(currentStory);
    var overlap = 0;

    return Object.keys(captionTerms).some(function (term) {
      if (strongTerms[term]) {
        return true;
      }
      if (contextTerms[term]) {
        overlap += 1;
      }
      return overlap >= 2;
    });
  }

  function getDisplayCaption(currentStory, index) {
    var language = state.currentLanguage;
    var raw = (currentStory.imageCaptions && currentStory.imageCaptions[index]) || "";
    if (!raw) {
      return fallbackCaption(currentStory, index, language);
    }
    return isCaptionTooExplicit(currentStory, raw) ? fallbackCaption(currentStory, index, language) : raw;
  }

  function isKnownAsset(key) {
    if (isCustomAssetKey(key)) {
      return state.customAssets.some(function (item) { return item.key === key; });
    }
    return Boolean(builtInAssetLookup[normalizeAssetToken(key)]);
  }

  function calcExpectedXp() {
    var used = state.revealCount + (state.hintRevealed ? 1 : 0);
    return Math.max(20, 100 - used * 20);
  }

  function updateXpUi() {
    expectedXpEl.textContent = calcExpectedXp();
    xpEl.textContent = state.totalXp;
    updateStarsUi();
  }

  function buildSelector() {
    var stories = getAllStories();
    selectorEl.innerHTML = "";
    stories.forEach(function (item, index) {
      var option = document.createElement("option");
      option.value = String(index);
      option.textContent = getWebText(state.currentLanguage, "EpisodePrefix") + " " + item.id;
      selectorEl.appendChild(option);
    });
    if (state.storyIndex >= stories.length) {
      state.storyIndex = 0;
    }
    selectorEl.value = String(state.storyIndex);
  }

  function onSlotClick(index) {
    var text = getDisplayCaption(storyView(), index);
    if (!text) {
      return;
    }
    captionEl.classList.add("caption-highlight");
    captionEl.textContent = getWebText(state.currentLanguage, "CaptionPrefix") + ": " + text;
  }

  function createSlot(index, key, isHintSlot) {
    var slot = document.createElement("button");
    slot.type = "button";
    slot.className = "slot";
    if (isHintSlot && !state.hintRevealed) {
      slot.classList.add("pulse");
    }

    var img = document.createElement("img");
    img.alt = getWebText(state.currentLanguage, "StoryImageAlt") + " " + (index + 1);
    img.src = imageUrl(key);
    img.onerror = function () {
      img.src = imageUrl(KEY_UNKNOWN);
    };

    slot.appendChild(img);
    slot.addEventListener("click", function () {
      onSlotClick(index);
    });
    return slot;
  }

  function renderSlots() {
    slotsEl.innerHTML = "";
    var currentStory = storyView();
    var keys = [];
    var i;

    for (i = 0; i < 5; i++) {
      keys.push(currentStory.visibleKeys[i] || KEY_UNKNOWN);
    }

    keys.push(state.revealCount > 0 ? currentStory.hiddenKeys[0] : KEY_UNKNOWN);
    keys.push(state.revealCount > 1 ? currentStory.hiddenKeys[1] : KEY_UNKNOWN);
    keys.push(state.hintRevealed ? currentStory.hintKey : KEY_HINT_PLACEHOLDER);

    keys.forEach(function (key, index) {
      slotsEl.appendChild(createSlot(index, key, index === 7));
    });
  }

  function renderHeader() {
    var currentStory = storyView();
    if (state.solutionVisible) {
      titleEl.textContent = getWebText(state.currentLanguage, "EpisodePrefix") + " " + currentStory.id + " - " + currentStory.title;
      refEl.textContent = currentStory.scriptureReference + " | " + getWebText(state.currentLanguage, "CategoryLabel") + ": " + currentStory.keyword;
    } else {
      titleEl.textContent = getWebText(state.currentLanguage, "EpisodePrefix") + " " + currentStory.id + " - " + getWebText(state.currentLanguage, "GuessStoryTitle");
      refEl.textContent = getWebText(state.currentLanguage, "CategoryLabel") + ": " + currentStory.keyword;
    }
  }

  function renderSolution() {
    var currentStory = storyView();
    solutionEl.hidden = !state.solutionVisible;
    solutionBtn.textContent = state.solutionVisible ? getWebText(state.currentLanguage, "HideSolutionButton") : getWebText(state.currentLanguage, "RevealSolutionButton");
    document.getElementById("solutionText").textContent = currentStory.solution;
    scriptureEl.textContent = currentStory.scriptureQuote || "";
    noteEl.textContent = currentStory.engagementNote || "";
  }

  function renderButtons() {
    revealBtn.disabled = state.revealCount >= 2;
    hintBtn.disabled = state.hintRevealed;
  }

  function render() {
    buildSelector();
    renderHeader();
    renderSlots();
    renderSolution();
    renderButtons();
    updateXpUi();
  }

  function resetCaption() {
    captionEl.classList.remove("caption-highlight");
    captionEl.textContent = getWebText(state.currentLanguage, "CaptionDefault");
  }

  function updateLanguageSelectors() {
    [languageSelectEl, editorSourceLanguageEl].forEach(function (selectEl) {
      if (!selectEl) {
        return;
      }

      var currentValue = selectEl === editorSourceLanguageEl ? normalizeLanguage(selectEl.value || state.currentLanguage) : state.currentLanguage;
      selectEl.innerHTML = "";

      [AppLanguages.Italian, AppLanguages.English].forEach(function (language) {
        var option = document.createElement("option");
        option.value = language;
        option.textContent = getLanguageLabel(state.currentLanguage, language);
        selectEl.appendChild(option);
      });

      selectEl.value = currentValue;
    });
  }

  function applyUiText() {
    document.documentElement.lang = state.currentLanguage === AppLanguages.English ? "en" : "it";
    document.querySelectorAll("[data-i18n]").forEach(function (element) {
      element.textContent = getWebText(state.currentLanguage, element.getAttribute("data-i18n"));
    });
    document.querySelectorAll("[data-i18n-placeholder]").forEach(function (element) {
      element.placeholder = getWebText(state.currentLanguage, element.getAttribute("data-i18n-placeholder"));
    });
    document.querySelectorAll("[data-i18n-title]").forEach(function (element) {
      element.title = getWebText(state.currentLanguage, element.getAttribute("data-i18n-title"));
    });
    updateLanguageSelectors();
    updateEditorRowCopy();
  }

  function setLanguage(language) {
    state.currentLanguage = normalizeLanguage(language);
    localStorage.setItem(LANGUAGE_STORAGE_KEY, state.currentLanguage);
    applyUiText();
    resetCaption();
    render();
  }

  function loadStory(index) {
    var stories = getAllStories();
    state.storyIndex = Math.max(0, Math.min(index, stories.length - 1));
    state.revealCount = 0;
    state.hintRevealed = false;
    state.solutionVisible = false;
    resetCaption();
    render();
    sendAnalyticsEvent("story_view", { storyId: story().id });
  }

  function setEditorStatus(text, isError) {
    editorStatusEl.textContent = text;
    editorStatusEl.style.color = isError ? "#a11d1d" : "#684700";
  }

  function setUploadStatus(text, isError) {
    assetUploadStatusEl.textContent = text;
    assetUploadStatusEl.style.color = isError ? "#a11d1d" : "#6f4a00";
  }

  function createSlotEditorRow(container, label, groupName, index) {
    var row = document.createElement("div");
    row.className = "slot-editor-row slot-editor-row-extended";

    var title = document.createElement("div");
    title.className = "slot-editor-label";
    title.textContent = label;

    var keyInput = document.createElement("input");
    keyInput.type = "text";
    keyInput.placeholder = "PNG key or custom:...";

    var captionInput = document.createElement("input");
    captionInput.type = "text";
    captionInput.placeholder = state.currentLanguage === AppLanguages.English ? "Image caption" : "Didascalia immagine";

    var pickButton = document.createElement("button");
    pickButton.type = "button";
    pickButton.className = "btn btn-reveal";
    pickButton.textContent = getWebText(state.currentLanguage, "ChooseButton");

    var preview = document.createElement("div");
    preview.className = "slot-editor-preview";
    var previewImg = document.createElement("img");
    previewImg.alt = label;
    previewImg.src = imageUrl(KEY_UNKNOWN);
    preview.appendChild(previewImg);

    function syncPreview() {
      previewImg.src = imageUrl(keyInput.value || KEY_UNKNOWN);
    }

    keyInput.addEventListener("input", syncPreview);
    pickButton.addEventListener("click", function () {
      pickerTarget = keyInput;
      openPicker();
    });
    previewImg.onerror = function () {
      previewImg.src = imageUrl(KEY_UNKNOWN);
    };

    var keyField = document.createElement("label");
    keyField.className = "field";
    keyField.appendChild(document.createTextNode(""));
    keyField.appendChild(keyInput);

    var captionField = document.createElement("label");
    captionField.className = "field";
    captionField.appendChild(document.createTextNode(""));
    captionField.appendChild(captionInput);

    row.appendChild(title);
    row.appendChild(keyField);
    row.appendChild(pickButton);
    row.appendChild(preview);
    row.appendChild(captionField);

    container.appendChild(row);
    slotInputs[groupName][index] = {
      title: title,
      key: keyInput,
      caption: captionInput,
      pickButton: pickButton,
      groupName: groupName,
      index: index,
      refresh: syncPreview
    };
  }

  function updateEditorRowCopy() {
    [slotInputs.visible, slotInputs.hidden, slotInputs.hint].forEach(function (group) {
      group.forEach(function (item) {
        if (!item) {
          return;
        }

        if (item.groupName === "visible") {
          item.title.textContent = getWebText(state.currentLanguage, "VisibleLabel") + " " + (item.index + 1);
        } else if (item.groupName === "hidden") {
          item.title.textContent = getWebText(state.currentLanguage, "HiddenLabel") + " " + (item.index + 1);
        } else {
          item.title.textContent = getWebText(state.currentLanguage, "HintLabel");
        }

        item.pickButton.textContent = getWebText(state.currentLanguage, "ChooseButton");
        item.caption.placeholder = state.currentLanguage === AppLanguages.English ? "Image caption" : "Didascalia immagine";
      });
    });
  }

  function buildEditorRows() {
    createSlotEditorRow(document.getElementById("editorVisibleImages"), getWebText(state.currentLanguage, "VisibleLabel") + " 1", "visible", 0);
    createSlotEditorRow(document.getElementById("editorVisibleImages"), getWebText(state.currentLanguage, "VisibleLabel") + " 2", "visible", 1);
    createSlotEditorRow(document.getElementById("editorVisibleImages"), getWebText(state.currentLanguage, "VisibleLabel") + " 3", "visible", 2);
    createSlotEditorRow(document.getElementById("editorVisibleImages"), getWebText(state.currentLanguage, "VisibleLabel") + " 4", "visible", 3);
    createSlotEditorRow(document.getElementById("editorVisibleImages"), getWebText(state.currentLanguage, "VisibleLabel") + " 5", "visible", 4);
    createSlotEditorRow(document.getElementById("editorHiddenImages"), getWebText(state.currentLanguage, "HiddenLabel") + " 1", "hidden", 0);
    createSlotEditorRow(document.getElementById("editorHiddenImages"), getWebText(state.currentLanguage, "HiddenLabel") + " 2", "hidden", 1);
    createSlotEditorRow(document.getElementById("editorHintImage"), getWebText(state.currentLanguage, "HintLabel"), "hint", 0);
  }

  function resetEditor() {
    Object.keys(editorFields).forEach(function (key) {
      editorFields[key].value = "";
    });

    [slotInputs.visible, slotInputs.hidden, slotInputs.hint].forEach(function (group) {
      group.forEach(function (item) {
        item.key.value = "";
        item.caption.value = "";
        item.refresh();
      });
    });

    setEditorStatus(
      state.apiAvailable
        ? getWebText(state.currentLanguage, "EditorSaveCloudStatus")
        : getWebText(state.currentLanguage, "EditorLocalOnlyStatus"),
      false
    );
    editorSourceLanguageEl.value = state.currentLanguage;
  }

  function openEditor() {
    resetEditor();
    editorPanel.hidden = false;
  }

  function closeEditor() {
    editorPanel.hidden = true;
  }

  function openPicker() {
    pickerPanel.hidden = false;
    assetSearchEl.value = "";
    renderAssetGrid("");
  }

  function closePicker() {
    pickerPanel.hidden = true;
    pickerTarget = null;
  }

  function renderAssetGrid(filterText) {
    var query = String(filterText || "").toLowerCase();
    assetGridEl.innerHTML = "";

    getAllAssets().forEach(function (asset) {
      var searchHaystack = (asset.key + " " + (asset.label || "")).toLowerCase();
      if (query && searchHaystack.indexOf(query) === -1) {
        return;
      }

      var card = document.createElement("button");
      card.type = "button";
      card.className = "asset-card";

      var img = document.createElement("img");
      img.alt = asset.label || asset.key;
      img.src = imageUrl(asset.key);
      img.onerror = function () {
        img.src = imageUrl(KEY_UNKNOWN);
      };

      var name = document.createElement("span");
      name.textContent = asset.builtIn ? asset.key : (asset.label || asset.key);

      card.appendChild(img);
      card.appendChild(name);
      card.addEventListener("click", function () {
        if (pickerTarget) {
          pickerTarget.value = asset.key;
          if (typeof pickerTarget.dispatchEvent === "function") {
            pickerTarget.dispatchEvent(new Event("input"));
          }
        }
        closePicker();
      });
      assetGridEl.appendChild(card);
    });
  }

  function collectGroup(group) {
    return group.map(function (item) {
      return {
        key: resolveStoryAssetKey(item.key.value.trim()),
        caption: item.caption.value.trim()
      };
    });
  }

  function defaultCaptionFromKey(key, index) {
    return getWebText(normalizeLanguage(editorSourceLanguageEl.value || state.currentLanguage), "ImagePrefix") + " " + (index + 1) + ": " + (isCustomAssetKey(key) ? customAssetName(key) : resolveBuiltInAssetKey(key));
  }

  function validateNewStory(payload) {
    var allKeys;
    if (!payload.title || !payload.scriptureReference || !payload.keyword || !payload.hint || !payload.solution) {
      return getWebText(state.currentLanguage, "FillRequiredFields");
    }
    if (payload.visibleKeys.length !== 5 || payload.hiddenKeys.length !== 2 || !payload.hintKey) {
      return getWebText(state.currentLanguage, "FillImageSlots");
    }
    allKeys = payload.visibleKeys.concat(payload.hiddenKeys).concat([payload.hintKey]);
    if (allKeys.some(function (key) { return !isKnownAsset(key); })) {
      return getWebText(state.currentLanguage, "InvalidImageKey");
    }
    return "";
  }

  function createStoryFromEditor() {
    var sourceLanguage = normalizeLanguage(editorSourceLanguageEl.value || state.currentLanguage);
    var visible = collectGroup(slotInputs.visible);
    var hidden = collectGroup(slotInputs.hidden);
    var hint = collectGroup(slotInputs.hint)[0];
    var allCaptions = visible.concat(hidden).concat([hint]).map(function (item, index) {
      return item.caption || defaultCaptionFromKey(item.key, index);
    });
    var payload = sanitizeStory({
      id: 0,
      title: editorFields.title.value.trim(),
      scriptureReference: editorFields.reference.value.trim(),
      keyword: editorFields.keyword.value.trim(),
      hint: editorFields.hint.value.trim(),
      solution: editorFields.solution.value.trim(),
      scriptureQuote: editorFields.quote.value.trim(),
      engagementNote: editorFields.note.value.trim(),
      visibleKeys: visible.map(function (item) { return item.key; }),
      hiddenKeys: hidden.map(function (item) { return item.key; }),
      hintKey: hint.key,
      imageCaptions: allCaptions,
      sourceLanguage: sourceLanguage,
      isUserCreated: true
    });
    var error = validateNewStory(payload);
    if (error) {
      throw new Error(error);
    }
    return payload;
  }

  async function requestJson(url, options) {
    var response = await fetch(url, options || {});
    if (!response.ok) {
      throw new Error("HTTP " + response.status);
    }
    return response.json();
  }

  async function refreshSharedData() {
    try {
      var results = await Promise.all([
        requestJson("/api/stories"),
        requestJson("/api/assets")
      ]);
      state.sharedStories = sanitizeStories(results[0].stories || []);
      state.customAssets = results[1].assets || [];
      state.apiAvailable = true;
      setUploadStatus(getWebText(state.currentLanguage, "StorageActiveStatus"), false);
      setEditorStatus(getWebText(state.currentLanguage, "CloudflareActiveStatus"), false);
    } catch (error) {
      state.sharedStories = getLocalStories();
      state.customAssets = [];
      state.apiAvailable = false;
      setUploadStatus(getWebText(state.currentLanguage, "UploadDeferredStatus"), true);
      setEditorStatus(getWebText(state.currentLanguage, "LocalModeStatus"), true);
    }
  }

  async function saveEditorStory() {
    try {
      var newStory = createStoryFromEditor();
      if (state.apiAvailable) {
        var payload = await requestJson("/api/stories", {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(newStory)
        });
        state.sharedStories.push(sanitizeStory(payload.story));
        setEditorStatus(getWebText(state.currentLanguage, "StorySavedShared", { id: String(payload.story.id) }), false);
      } else {
        state.sharedStories.push(sanitizeStory(newStory));
        state.sharedStories[state.sharedStories.length - 1].id = getAllStories().reduce(function (maxId, item) {
          return Math.max(maxId, Number(item.id || 0));
        }, 18) + 1;
        saveLocalStories(state.sharedStories);
        setEditorStatus(getWebText(state.currentLanguage, "StorySavedLocal"), true);
      }
      closeEditor();
      buildSelector();
      loadStory(getAllStories().length - 1);
    } catch (error) {
      setEditorStatus(error.message, true);
    }
  }

  async function uploadCustomAsset() {
    var file = assetUploadFileEl.files && assetUploadFileEl.files[0];
    var name = assetUploadNameEl.value.trim();
    var formData;

    if (!file) {
      setUploadStatus(getWebText(state.currentLanguage, "SelectPngFirst"), true);
      return;
    }
    if (file.type !== "image/png") {
      setUploadStatus(getWebText(state.currentLanguage, "PngOnly"), true);
      return;
    }
    if (!state.apiAvailable) {
      setUploadStatus(getWebText(state.currentLanguage, "SharedUploadOnly"), true);
      return;
    }

    formData = new FormData();
    formData.append("file", file);
    formData.append("name", name);

    try {
      uploadAssetBtn.disabled = true;
      setUploadStatus(getWebText(state.currentLanguage, "UploadInProgress"), false);
      var payload = await requestJson("/api/assets", {
        method: "POST",
        body: formData
      });
      state.customAssets.unshift(payload.asset);
      renderAssetGrid(assetSearchEl.value);
      setUploadStatus(getWebText(state.currentLanguage, "UploadSuccess", { name: payload.asset.label || payload.asset.key }), false);
      if (pickerTarget) {
        pickerTarget.value = payload.asset.key;
        pickerTarget.dispatchEvent(new Event("input"));
      }
      assetUploadFileEl.value = "";
      assetUploadNameEl.value = "";
    } catch (error) {
      setUploadStatus(getWebText(state.currentLanguage, "UploadErrorPrefix") + error.message, true);
    } finally {
      uploadAssetBtn.disabled = false;
    }
  }

  languageSelectEl.addEventListener("change", function () {
    setLanguage(languageSelectEl.value);
  });

  // ── Stars UI ─────────────────────────────────────────────────────────────
  function calcStars() {
    var used = state.revealCount + (state.hintRevealed ? 1 : 0);
    return Math.max(1, 3 - used);
  }
  function updateStarsUi() {
    if (!starsBoxEl) return;
    var s = calcStars();
    starsBoxEl.textContent = "\u2605".repeat(s) + "\u2606".repeat(3 - s);
    starsBoxEl.style.color = s === 3 ? "#d4a000" : s === 2 ? "#888" : "#a05820";
  }

  // ── Analytics / Presence ─────────────────────────────────────────────────
  function updateOnlineBadge(count) {
    if (!onlineBadgeEl) return;
    if (count > 0) {
      onlineBadgeEl.hidden = false;
      if (onlineCountEl) onlineCountEl.textContent = String(count);
    } else {
      onlineBadgeEl.hidden = true;
    }
  }
  function sendAnalyticsEvent(type, extra) {
    if (!state.apiAvailable) return;
    var body = Object.assign(
      { type: type, clientId: state.clientId, language: state.currentLanguage },
      extra || {}
    );
    fetch("/api/analytics", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(body)
    })
      .then(function (r) { return r.ok ? r.json() : null; })
      .then(function (data) {
        if (data && typeof data.online === "number") updateOnlineBadge(data.online);
      })
      .catch(function () {});
  }
  function startHeartbeat() {
    if (!state.apiAvailable) return;
    var send = function () {
      sendAnalyticsEvent("heartbeat", { storyId: story().id });
    };
    send();
    if (state.heartbeatInterval) clearInterval(state.heartbeatInterval);
    state.heartbeatInterval = setInterval(send, 30000);
  }

  // ── Admin panel ──────────────────────────────────────────────────────────
  function displayAdminStats(data) {
    if (statOnlineEl)      statOnlineEl.textContent      = String(data.online      !== undefined ? data.online      : "-");
    if (statViewsEl)       statViewsEl.textContent       = String(data.views       !== undefined ? data.views       : "-");
    if (statCompletionsEl) statCompletionsEl.textContent = String(data.completions !== undefined ? data.completions : "-");
    if (statSessionsEl)    statSessionsEl.textContent    = String(data.sessions    !== undefined ? data.sessions    : "-");
  }
  async function loadAdminStats(secret) {
    if (!state.apiAvailable) {
      if (adminOfflineNoteEl) adminOfflineNoteEl.style.display = "block";
      return;
    }
    try {
      var data = await requestJson("/api/analytics", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ type: "admin_stats", secret: secret })
      });
      displayAdminStats(data);
      if (adminStatusMsgEl) adminStatusMsgEl.textContent = "Aggiornato: " + new Date().toLocaleTimeString("it-IT");
      if (adminOfflineNoteEl) adminOfflineNoteEl.style.display = "none";
    } catch (_err) {
      if (adminOfflineNoteEl) adminOfflineNoteEl.style.display = "block";
    }
  }
  function openAdmin() {
    if (!adminPanelEl) return;
    var saved = sessionStorage.getItem("jwquiz_admin_session");
    if (saved) {
      state.adminAuthenticated = true;
      if (adminLoginAreaEl) adminLoginAreaEl.hidden = true;
      if (adminStatsEl)     adminStatsEl.hidden     = false;
      if (adminOfflineNoteEl) adminOfflineNoteEl.style.display = state.apiAvailable ? "none" : "block";
      loadAdminStats(saved);
    } else {
      state.adminAuthenticated = false;
      if (adminLoginAreaEl) adminLoginAreaEl.hidden = false;
      if (adminStatsEl)     adminStatsEl.hidden     = true;
      if (adminErrorEl)     adminErrorEl.style.display = "none";
    }
    adminPanelEl.hidden = false;
    adminPanelEl.focus();
  }
  function closeAdmin() {
    if (adminPanelEl) adminPanelEl.hidden = true;
  }
  async function doAdminLogin() {
    if (!adminSecretInputEl) return;
    var secret = adminSecretInputEl.value.trim();
    if (!secret) return;
    try {
      var data = await requestJson("/api/analytics", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ type: "admin_stats", secret: secret })
      });
      sessionStorage.setItem("jwquiz_admin_session", secret);
      state.adminAuthenticated = true;
      if (adminErrorEl)     adminErrorEl.style.display = "none";
      if (adminLoginAreaEl) adminLoginAreaEl.hidden = true;
      if (adminStatsEl)     adminStatsEl.hidden = false;
      if (adminOfflineNoteEl) adminOfflineNoteEl.style.display = "none";
      displayAdminStats(data);
      if (adminStatusMsgEl) adminStatusMsgEl.textContent = "\u2705 Autenticato \u2014 " + new Date().toLocaleTimeString("it-IT");
    } catch (_err) {
      if (adminErrorEl) adminErrorEl.style.display = "block";
      adminSecretInputEl.value = "";
    }
  }

  selectorEl.addEventListener("change", function () {
    loadStory(Number(selectorEl.value));
  });

  revealBtn.addEventListener("click", function () {
    if (state.revealCount < 2) {
      state.revealCount += 1;
      render();
    }
  });

  hintBtn.addEventListener("click", function () {
    if (!state.hintRevealed) {
      state.hintRevealed = true;
      render();
    }
  });

  solutionBtn.addEventListener("click", function () {
    state.solutionVisible = !state.solutionVisible;
    if (state.solutionVisible) {
      sendAnalyticsEvent("story_complete", { storyId: story().id, xp: calcExpectedXp() });
    }
    render();
  });

  nextBtn.addEventListener("click", function () {
    var gained = calcExpectedXp();
    state.totalXp += gained;
    localStorage.setItem("jwquiz_web_xp", String(state.totalXp));
    loadStory((state.storyIndex + 1) % getAllStories().length);
  });

  createBtn.addEventListener("click", openEditor);
  closeEditorBtn.addEventListener("click", closeEditor);
  saveStoryBtn.addEventListener("click", saveEditorStory);
  closePickerBtn.addEventListener("click", closePicker);
  assetSearchEl.addEventListener("input", function () {
    renderAssetGrid(assetSearchEl.value);
  });
  uploadAssetBtn.addEventListener("click", uploadCustomAsset);

  // Admin panel event listeners
  var _btnAdmin      = document.getElementById("btnAdmin");
  var _btnCloseAdmin = document.getElementById("btnCloseAdmin");
  if (_btnAdmin)        _btnAdmin.addEventListener("click", openAdmin);
  if (_btnCloseAdmin)   _btnCloseAdmin.addEventListener("click", closeAdmin);
  if (btnAdminLoginEl)  btnAdminLoginEl.addEventListener("click", doAdminLogin);
  if (adminSecretInputEl) {
    adminSecretInputEl.addEventListener("keydown", function (e) {
      if (e.key === "Enter") { e.preventDefault(); doAdminLogin(); }
    });
  }
  if (btnAdminRefreshEl) {
    btnAdminRefreshEl.addEventListener("click", function () {
      var s = sessionStorage.getItem("jwquiz_admin_session");
      if (s) loadAdminStats(s);
    });
  }
  if (adminPanelEl) {
    adminPanelEl.addEventListener("click", function (e) {
      if (e.target === adminPanelEl) {
        closeAdmin();
      }
    });
  }
  document.addEventListener("keydown", function (e) {
    if (e.key === "Escape" && adminPanelEl && !adminPanelEl.hidden) {
      closeAdmin();
    }
  });

  async function initialize() {
    // Admin panel must never auto-open on site load.
    if (adminPanelEl) {
      adminPanelEl.hidden = true;
    }
    buildEditorRows();
    applyUiText();
    await refreshSharedData();
    renderAssetGrid("");
    buildSelector();
    loadStory(0);
    startHeartbeat();
  }

  initialize();
})();
