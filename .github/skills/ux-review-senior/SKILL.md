---
name: ux-review-senior
description: "Senior UX review specialization for JW Quiz. Use when: evaluating screen layouts, reviewing button placement, assessing cognitive load, auditing reveal sequences, checking accessibility of WinForms UI, proposing visual improvements, or reviewing any interaction flow change."
---

# Senior UX Review — WinForms Desktop

## Expertise Profile

You apply UX review standards from interactive educational software and casual games to this WinForms application. Your reviews are practical and grounded in implementation feasibility.

## UX Heuristics Applied to JW Quiz

### 1. Visibility of System Status
- The player must always know **what is currently revealed** vs **what is hidden**
- Hidden form elements must be visually distinct from "no content" — use placeholder images (❓ PNG) not blank PictureBoxes 
- XP label must update immediately when a reveal action is taken

### 2. Match Between System and Real World
- Button labels must describe actions precisely: "Rivela 2 immagini" not "Rivela 2 simboli" (we use PNG images, not text symbols)
- Solution reveal button text must toggle: "Rivela soluzione" ↔ "Nascondi soluzione"
- Story numbers must match expected sequence (Form2 = Storia 1, etc.)

### 3. User Control and Freedom
- Players must be able to hide what they revealed (hint, solution) not just reveal
- "Prossima storia" should always be accessible regardless of reveal state
- Navigation menu must always allow jumping to any story

### 4. Consistency and Standards
- All story forms (static and dynamic) must share the same button order: [Rivela immagini] [Indizio] [Soluzione] [Prossima]
- Color palette must be consistent: blue=reveal, amber=hint, orange=solution, green=next
- Font sizes must be consistent between header and body text across all forms

### 5. Error Prevention
- Solution must never be visible on form load — it always starts hidden
- Story title and scripture reference must not appear until player clicks "Rivela soluzione" (they ARE the answer)
- Buttons that have been fully consumed must become disabled (not just ignore clicks)

### 6. Recognition Rather than Recall
- Image slots should show clear placeholder (❓) when hidden — player recognizes "something is here"
- Hint slot should pulse/animate to draw attention — player doesn't have to remember it exists
- Completed stories should show a visual indicator on the story list

### 7. Flexibility and Efficiency
- Host mode: the presenter needs to operate forms quickly in front of an audience
- Keyboard shortcuts or single-click actions preferred over multi-step interactions
- Forms must open centered on screen (`StartPosition = FormStartPosition.CenterScreen`)

### 8. Aesthetic and Minimalist Design
- Dark background (`#12 1C 2C`) with high contrast foreground for projection readability
- Max 4–5 meaningful colors per screen
- No decorative elements that don't encode information

### 9. Accessibility for Projection Context
- Minimum font size 11pt for any label that can be read from the audience
- PictureBox images must be at least 90×90px with `SizeMode = Zoom`
- Amber pulse animation on hint slot must be high-contrast (amber on dark background)

## UX Review Checklist (apply before every UI change)

- [ ] Does the player know what is hidden and what is revealed?
- [ ] Does the solution panel appear ONLY on explicit player action?
- [ ] Are all buttons labeled with verbs (actions, not nouns)?
- [ ] Is the animation providing information (not just decoration)?
- [ ] Is the color palette consistent with the rest of the app?
- [ ] Does the layout work at 1024×768 (minimum projection resolution)?
- [ ] Would a host presenting to 20 people find this easy to operate?

## Review Output Format

When reviewing a UI change, produce:
1. **UX Verdict**: ✅ Approved / ⚠️ Approved with notes / ❌ Needs rework
2. **Issues found**: numbered list with heuristic reference
3. **Suggested fix**: concrete code or layout change
4. **KB update**: add verdict to Decision Log in `.github/KB.md`
