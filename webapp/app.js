(function () {
  var KEY_UNKNOWN = "2753";
  var KEY_HINT_PLACEHOLDER = "1F525";
  var USER_STORIES_KEY = "jwquiz_web_user_stories_v1";

  var state = {
    storyIndex: 0,
    revealCount: 0,
    hintRevealed: false,
    solutionVisible: false,
    totalXp: Number(localStorage.getItem("jwquiz_web_xp") || "0")
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

  var builtInStories = sanitizeStories(window.JW_STORIES || []);
  var slotInputs = {
    visible: [],
    hidden: [],
    hint: []
  };
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

  var assetLookup = buildAssetLookup(window.JW_ASSET_KEYS || []);

  function normalizeAssetToken(value) {
    return String(value || "").toLowerCase().replace(/[^a-z0-9]/g, "");
  }

  function buildAssetLookup(keys) {
    var lookup = {};
    keys.forEach(function (key) {
      lookup[normalizeAssetToken(key)] = key;
    });
    return lookup;
  }

  function resolveAssetKey(key) {
    var normalized = normalizeAssetToken(key);
    return assetLookup[normalized] || key || KEY_UNKNOWN;
  }

  function imageUrl(key) {
    return "assets/" + resolveAssetKey(key) + ".png";
  }

  function sanitizeStory(raw) {
    var visibleKeys = (raw.visibleKeys || []).slice(0, 5);
    var hiddenKeys = (raw.hiddenKeys || []).slice(0, 2);
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
      hintKey: raw.hintKey || KEY_HINT_PLACEHOLDER,
      imageCaptions: captions,
      isUserCreated: Boolean(raw.isUserCreated)
    };
  }

  function sanitizeStories(list) {
    return list.map(sanitizeStory);
  }

  function getUserStories() {
    try {
      return sanitizeStories(JSON.parse(localStorage.getItem(USER_STORIES_KEY) || "[]"));
    } catch (error) {
      return [];
    }
  }

  function saveUserStories(stories) {
    localStorage.setItem(USER_STORIES_KEY, JSON.stringify(stories));
  }

  function getAllStories() {
    return builtInStories.concat(getUserStories());
  }

  function story() {
    var stories = getAllStories();
    return stories[state.storyIndex] || stories[0];
  }

  function nextStoryId() {
    var maxId = 18;
    getAllStories().forEach(function (item) {
      if (item.id > maxId) {
        maxId = item.id;
      }
    });
    return maxId + 1;
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

    stories.forEach(function (s, i) {
      var option = document.createElement("option");
      option.value = String(i);
      option.textContent = "Episodio " + s.id;
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
    for (var i = 0; i < 5; i++) {
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

  function createSlotEditorRow(container, label, groupName, index) {
    var row = document.createElement("div");
    row.className = "slot-editor-row slot-editor-row-extended";

    var title = document.createElement("div");
    title.className = "slot-editor-label";
    title.textContent = label;

    var keyInput = document.createElement("input");
    keyInput.type = "text";
    keyInput.placeholder = "Chiave PNG";

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

    setEditorStatus("I nuovi episodi vengono salvati solo nel browser corrente.", false);
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

    (window.JW_ASSET_KEYS || []).forEach(function (key) {
      if (query && key.toLowerCase().indexOf(query) === -1) {
        return;
      }

      var card = document.createElement("button");
      card.type = "button";
      card.className = "asset-card";

      var img = document.createElement("img");
      img.alt = key;
      img.src = imageUrl(key);
      img.onerror = function () {
        img.src = imageUrl(KEY_UNKNOWN);
      };

      var name = document.createElement("span");
      name.textContent = key;

      card.appendChild(img);
      card.appendChild(name);
      card.addEventListener("click", function () {
        if (pickerTarget) {
          pickerTarget.value = key;
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
        key: resolveAssetKey(item.key.value.trim()),
        caption: item.caption.value.trim()
      };
    });
  }

  function defaultCaptionFromKey(key, index) {
    return "Immagine " + (index + 1) + ": " + resolveAssetKey(key);
  }

  function validateNewStory(payload) {
    if (!payload.title || !payload.scriptureReference || !payload.keyword || !payload.hint || !payload.solution) {
      return "Compila titolo, riferimento biblico, parola chiave, indizio e soluzione.";
    }
    if (payload.visibleKeys.length !== 5 || payload.hiddenKeys.length !== 2 || !payload.hintKey) {
      return "Servono 5 immagini visibili, 2 immagini nascoste e 1 immagine indizio.";
    }
    if (payload.visibleKeys.concat(payload.hiddenKeys).concat([payload.hintKey]).some(function (key) { return !assetLookup[normalizeAssetToken(key)]; })) {
      return "Una o più chiavi immagine non esistono negli asset web. Usa il pulsante Scegli....";
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
      id: nextStoryId(),
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

  function saveEditorStory() {
    try {
      var userStories = getUserStories();
      var newStory = createStoryFromEditor();
      userStories.push(newStory);
      saveUserStories(userStories);
      setEditorStatus("Episodio " + newStory.id + " salvato in locale. Lo carico subito nella lista.", false);
      closeEditor();
      buildSelector();
      loadStory(getAllStories().length - 1);
    } catch (error) {
      setEditorStatus(error.message, true);
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
    var next = (state.storyIndex + 1) % getAllStories().length;
    loadStory(next);
  });

  createBtn.addEventListener("click", openEditor);
  closeEditorBtn.addEventListener("click", closeEditor);
  saveStoryBtn.addEventListener("click", saveEditorStory);
  closePickerBtn.addEventListener("click", closePicker);
  assetSearchEl.addEventListener("input", function () {
    renderAssetGrid(assetSearchEl.value);
  });

  buildEditorRows();
  renderAssetGrid("");
  buildSelector();
  loadStory(0);
})();
