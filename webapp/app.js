(function () {
  var KEY_UNKNOWN = "2753";
  var KEY_HINT_PLACEHOLDER = "1F525";
  var LOCAL_STORIES_KEY = "jwquiz_web_user_stories_v1";

  var state = {
    storyIndex: 0,
    revealCount: 0,
    hintRevealed: false,
    solutionVisible: false,
    totalXp: Number(localStorage.getItem("jwquiz_web_xp") || "0"),
    sharedStories: [],
    customAssets: [],
    apiAvailable: false
  };

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

  var pickerPanel = document.getElementById("pickerPanel");
  var closePickerBtn = document.getElementById("btnClosePicker");
  var assetGridEl = document.getElementById("assetGrid");
  var assetSearchEl = document.getElementById("assetSearch");
  var assetUploadNameEl = document.getElementById("assetUploadName");
  var assetUploadFileEl = document.getElementById("assetUploadFile");
  var assetUploadStatusEl = document.getElementById("assetUploadStatus");
  var uploadAssetBtn = document.getElementById("btnUploadAsset");

  var builtInStories = sanitizeStories(window.JW_STORIES || []);
  var builtInAssetKeys = (window.JW_ASSET_KEYS || []).slice();
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

  var builtInAssetLookup = buildAssetLookup(builtInAssetKeys);

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

    return {
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
      isUserCreated: Boolean(raw.isUserCreated)
    };
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
  }

  function buildSelector() {
    var stories = getAllStories();
    selectorEl.innerHTML = "";
    stories.forEach(function (item, index) {
      var option = document.createElement("option");
      option.value = String(index);
      option.textContent = "Episodio " + item.id;
      selectorEl.appendChild(option);
    });
    if (state.storyIndex >= stories.length) {
      state.storyIndex = 0;
    }
    selectorEl.value = String(state.storyIndex);
  }

  function onSlotClick(index) {
    var text = (story().imageCaptions && story().imageCaptions[index]) || "";
    if (!text) {
      return;
    }
    captionEl.classList.add("caption-highlight");
    captionEl.textContent = "Indicazione: " + text;
  }

  function createSlot(index, key, isHintSlot) {
    var slot = document.createElement("button");
    slot.type = "button";
    slot.className = "slot";
    if (isHintSlot && !state.hintRevealed) {
      slot.classList.add("pulse");
    }

    var img = document.createElement("img");
    img.alt = "immagine storia " + (index + 1);
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
    var currentStory = story();
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
    var currentStory = story();
    if (state.solutionVisible) {
      titleEl.textContent = "Episodio " + currentStory.id + " - " + currentStory.title;
      refEl.textContent = currentStory.scriptureReference + " | Categoria: " + currentStory.keyword;
    } else {
      titleEl.textContent = "Episodio " + currentStory.id + " - Indovina la storia!";
      refEl.textContent = "Categoria: " + currentStory.keyword;
    }
  }

  function renderSolution() {
    var currentStory = story();
    solutionEl.hidden = !state.solutionVisible;
    solutionBtn.textContent = state.solutionVisible ? "Nascondi soluzione" : "Rivela soluzione";
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
    captionEl.textContent = "Clicca su una immagine per leggere un indizio breve";
  }

  function loadStory(index) {
    var stories = getAllStories();
    state.storyIndex = Math.max(0, Math.min(index, stories.length - 1));
    state.revealCount = 0;
    state.hintRevealed = false;
    state.solutionVisible = false;
    resetCaption();
    render();
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
    keyInput.placeholder = "Chiave PNG o custom:...";

    var captionInput = document.createElement("input");
    captionInput.type = "text";
    captionInput.placeholder = "Didascalia immagine";

    var pickButton = document.createElement("button");
    pickButton.type = "button";
    pickButton.className = "btn btn-reveal";
    pickButton.textContent = "Scegli...";

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
      key: keyInput,
      caption: captionInput,
      refresh: syncPreview
    };
  }

  function buildEditorRows() {
    createSlotEditorRow(document.getElementById("editorVisibleImages"), "Visibile 1", "visible", 0);
    createSlotEditorRow(document.getElementById("editorVisibleImages"), "Visibile 2", "visible", 1);
    createSlotEditorRow(document.getElementById("editorVisibleImages"), "Visibile 3", "visible", 2);
    createSlotEditorRow(document.getElementById("editorVisibleImages"), "Visibile 4", "visible", 3);
    createSlotEditorRow(document.getElementById("editorVisibleImages"), "Visibile 5", "visible", 4);
    createSlotEditorRow(document.getElementById("editorHiddenImages"), "Nascosta 1", "hidden", 0);
    createSlotEditorRow(document.getElementById("editorHiddenImages"), "Nascosta 2", "hidden", 1);
    createSlotEditorRow(document.getElementById("editorHintImage"), "Indizio", "hint", 0);
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
        ? "I nuovi episodi saranno condivisi tra tutti gli utenti tramite Cloudflare."
        : "API Cloudflare non disponibile: salvataggio temporaneo solo locale.",
      false
    );
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
    return "Immagine " + (index + 1) + ": " + (isCustomAssetKey(key) ? customAssetName(key) : resolveBuiltInAssetKey(key));
  }

  function validateNewStory(payload) {
    var allKeys;
    if (!payload.title || !payload.scriptureReference || !payload.keyword || !payload.hint || !payload.solution) {
      return "Compila titolo, riferimento biblico, parola chiave, indizio e soluzione.";
    }
    if (payload.visibleKeys.length !== 5 || payload.hiddenKeys.length !== 2 || !payload.hintKey) {
      return "Servono 5 immagini visibili, 2 immagini nascoste e 1 immagine indizio.";
    }
    allKeys = payload.visibleKeys.concat(payload.hiddenKeys).concat([payload.hintKey]);
    if (allKeys.some(function (key) { return !isKnownAsset(key); })) {
      return "Una o più chiavi immagine non esistono nel picker. Usa il pulsante Scegli....";
    }
    return "";
  }

  function createStoryFromEditor() {
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
      setUploadStatus("Storage Cloudflare attivo: episodi e PNG custom saranno condivisi.", false);
      setEditorStatus("Cloudflare attivo: i nuovi episodi saranno condivisi tra tutti gli utenti.", false);
    } catch (error) {
      state.sharedStories = getLocalStories();
      state.customAssets = [];
      state.apiAvailable = false;
      setUploadStatus("API Cloudflare non trovata in locale. I PNG custom condivisi saranno attivi dopo il deploy con KV/R2 configurati.", true);
      setEditorStatus("Modalità locale: gli episodi creati ora restano solo nel browser corrente.", true);
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
        setEditorStatus("Episodio " + payload.story.id + " salvato e condiviso online.", false);
      } else {
        state.sharedStories.push(sanitizeStory(newStory));
        state.sharedStories[state.sharedStories.length - 1].id = getAllStories().reduce(function (maxId, item) {
          return Math.max(maxId, Number(item.id || 0));
        }, 18) + 1;
        saveLocalStories(state.sharedStories);
        setEditorStatus("API non disponibile: episodio salvato solo in locale.", true);
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
      setUploadStatus("Seleziona un file PNG prima di caricare.", true);
      return;
    }
    if (file.type !== "image/png") {
      setUploadStatus("Sono accettati solo file PNG.", true);
      return;
    }
    if (!state.apiAvailable) {
      setUploadStatus("Upload condiviso disponibile solo dopo il deploy Cloudflare con storage attivo.", true);
      return;
    }

    formData = new FormData();
    formData.append("file", file);
    formData.append("name", name);

    try {
      uploadAssetBtn.disabled = true;
      setUploadStatus("Caricamento PNG in corso...", false);
      var payload = await requestJson("/api/assets", {
        method: "POST",
        body: formData
      });
      state.customAssets.unshift(payload.asset);
      renderAssetGrid(assetSearchEl.value);
      setUploadStatus("PNG caricato: " + (payload.asset.label || payload.asset.key), false);
      if (pickerTarget) {
        pickerTarget.value = payload.asset.key;
        pickerTarget.dispatchEvent(new Event("input"));
      }
      assetUploadFileEl.value = "";
      assetUploadNameEl.value = "";
    } catch (error) {
      setUploadStatus("Errore upload PNG: " + error.message, true);
    } finally {
      uploadAssetBtn.disabled = false;
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

  async function initialize() {
    buildEditorRows();
    await refreshSharedData();
    renderAssetGrid("");
    buildSelector();
    loadStory(0);
  }

  initialize();
})();
