---
name: jw-quiz-workflow
description: "Workflow skill for JW Quiz agent: KB read/update protocol, git commit/push discipline, regression prevention checklist, and deterministic action sequence. Use when starting any work session, before committing, or after completing any change."
---

# JW Quiz — Workflow Protocol

## Session Start Protocol (OBBLIGATORIO)

Prima di qualsiasi azione tecnica, eseguire in ordine:

1. **Leggi la KB**: `read_file(".github/KB.md")` — sezione per sezione
2. **Verifica stato git**: `git status --short` — deve essere `0` file non tracciati
3. **Identifica vincoli tecnici**: sezione "8. Convoluzioni/Vincoli Tecnici Noti" della KB
4. **Consulta Next Best Decisions**: sezione "11. Next Best Decisions" per priorità attive

## Ciclo Decisionale

```
Richiesta utente
    → Leggi KB
    → Valuta se proposta o implementazione diretta
    → Se proposta: presenta, aspetta approvazione, poi implementa
    → Se implementazione diretta: procedi
    → Build validation
    → UX review (se cambiamento UI)
    → Aggiorna KB
    → Commit + Push
```

## Quando Proporre vs Quando Implementare Direttamente

| Tipo di lavoro | Azione |
|----------------|--------|
| Bug fix ovvio | Implementa direttamente |
| Miglioramento già approvato in KB | Implementa direttamente |
| Nuova feature o UX change | Proponi prima |
| Cambiamento architetturale | Proponi con valutazione tecnica |
| Aggiornamento KB o documentazione | Implementa direttamente |

## KB Update Protocol

Aggiornare `.github/KB.md` dopo ogni:
- Decisione approvata o rifiutata
- Modifica al codice
- Proposta "Next Best Decision"
- Scoperta di un nuovo vincolo tecnico

**Sezioni da aggiornare:**
- Sezione "10. Decisioni e Valutazioni" → aggiungi riga alla tabella con data e esito
- Sezione "11. Next Best Decisions" → aggiungi o sposta proposte
- Sezione rilevante (es. "3. Gameplay" se si cambia meccanica) → aggiorna descrizione

## Build Validation (PRIMA di ogni commit)

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" `
  Jw_Quiz_Development.csproj /p:Configuration=Debug /nologo /verbosity:quiet
```

**Exit code deve essere 0.** Se non lo è, correggere gli errori prima di procedere. Mai committare codice che non compila.

## Git Protocol

```powershell
# 1. Verifica build
# 2. Stage tutto
git add -A
# 3. Commit con messaggio convenzionale in italiano
git commit -m "tipo: descrizione breve in italiano"
# 4. Push
git push
```

**Prefissi commit:**
- `feat:` nuova funzionalità
- `fix:` correzione bug
- `refactor:` ristrutturazione senza cambiamento comportamento
- `kb:` solo aggiornamento Knowledge Base
- `style:` modifiche puramente visive/UI
- `chore:` manutenzione, build, config

## Regression Prevention Checklist

Prima di ogni commit verificare:

- [ ] Build passa senza errori (`exit code 0`)
- [ ] Nessuna modifica accidentale ai file `*.Designer.cs` dei form statici
- [ ] `Story.VisibleEmojis/HiddenEmojis/HintEmoji` contengono chiavi PNG, non caratteri Unicode
- [ ] `DynamicStoryForm`: slot 5/6 mostrano ❓ all'avvio; slot 7 mostra 🔥 pulsante
- [ ] `DynamicStoryForm`: header nasconde titolo e scripture al caricamento
- [ ] Nessuna API net5+ usata (es. `PlaceholderText`, `ArgumentNullException.ThrowIfNull`)
- [ ] `ProgressTracker.CompleteStory` viene chiamato alla chiusura di ogni form storia
- [ ] `git status --short` pulito dopo il commit
- [ ] KB aggiornata prima del push

## Deterministic Action Sequence

Ogni sessione deve essere riproducibile. Seguire sempre questo ordine fisso:

1. Leggi KB
2. Analizza richiesta
3. Proponi (se non ovvio) o implementa
4. Build
5. UX review (se UI)
6. Aggiorna KB
7. Commit
8. Push
9. Riporta risultato all'utente
