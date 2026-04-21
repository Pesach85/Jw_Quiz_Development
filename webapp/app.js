(function () {
  var KEY_UNKNOWN = "2753";
  var KEY_HINT_PLACEHOLDER = "1F525";

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

  function story() {
    return window.JW_STORIES[state.storyIndex];
  }

  function imageUrl(key) {
    return "assets/" + key + ".png";
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
    window.JW_STORIES.forEach(function (s, i) {
      var option = document.createElement("option");
      option.value = String(i);
      option.textContent = "Episodio " + s.id + " - " + s.title;
      selectorEl.appendChild(option);
    });

    selectorEl.addEventListener("change", function () {
      loadStory(Number(selectorEl.value));
    });
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

    var s = story();
    var keys = [];
    for (var i = 0; i < 5; i++) {
      keys.push(s.visibleKeys[i] || KEY_UNKNOWN);
    }

    keys.push(state.revealCount > 0 ? s.hiddenKeys[0] : KEY_UNKNOWN);
    keys.push(state.revealCount > 1 ? s.hiddenKeys[1] : KEY_UNKNOWN);
    keys.push(state.hintRevealed ? s.hintKey : KEY_HINT_PLACEHOLDER);

    keys.forEach(function (key, index) {
      slotsEl.appendChild(createSlot(index, key, index === 7));
    });
  }

  function renderHeader() {
    var s = story();
    if (state.solutionVisible) {
      titleEl.textContent = "Episodio " + s.id + " - " + s.title;
      refEl.textContent = s.scriptureReference + " | Categoria: " + s.keyword;
    } else {
      titleEl.textContent = "Episodio " + s.id + " - Indovina la storia!";
      refEl.textContent = "Categoria: " + s.keyword;
    }
  }

  function renderSolution() {
    var s = story();
    solutionEl.hidden = !state.solutionVisible;
    solutionBtn.textContent = state.solutionVisible ? "Nascondi soluzione" : "Rivela soluzione";

    document.getElementById("solutionText").textContent = s.solution;
    scriptureEl.textContent = s.scriptureQuote || "";
    noteEl.textContent = s.engagementNote;
  }

  function renderButtons() {
    revealBtn.disabled = state.revealCount >= 2;
    hintBtn.disabled = state.hintRevealed;
  }

  function render() {
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
    state.storyIndex = index;
    state.revealCount = 0;
    state.hintRevealed = false;
    state.solutionVisible = false;
    selectorEl.value = String(index);
    resetCaption();
    render();
  }

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
    var next = (state.storyIndex + 1) % window.JW_STORIES.length;
    loadStory(next);
  });

  buildSelector();
  loadStory(0);
})();
