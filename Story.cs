namespace Jw_Quiz_Development
{
    [System.Serializable]
    public class Story
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ScriptureReference { get; set; }
        public string Keyword { get; set; }
        public string Hint { get; set; }
        public string Solution { get; set; }
        public string EngagementNote { get; set; }
        public string ImageResourceName { get; set; }

        // Citazione biblica completa dalla Traduzione del Nuovo Mondo
        public string ScriptureQuote { get; set; }

        // Campi per storie dinamiche (episodi 13-18 e storie utente)
        public string[] VisibleEmojis { get; set; }   // 5 chiavi PNG visibili dall'inizio
        public string[] HiddenEmojis { get; set; }    // 2 chiavi PNG nascoste da rivelare
        public string HintEmoji { get; set; }         // 1 chiave PNG suggerimento
        // 8 brevi descrizioni (slot 0-4 visibili, 5-6 nascosti, 7 indizio) — mostrate al click
        public string[] ImageCaptions { get; set; }
        public bool IsDynamic { get; set; }           // true = renderizzata da DynamicStoryForm
        public bool IsUserCreated { get; set; }       // true = creata dall'utente nell'app
        public AppLanguage SourceLanguage { get; set; } = AppLanguage.Italian;
        public StoryTranslations Translations { get; set; }
    }
}