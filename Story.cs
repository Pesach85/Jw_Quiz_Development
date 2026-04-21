namespace Jw_Quiz_Development
{
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

        // Campi per storie dinamiche (episodi 13-18 e storie utente)
        public string[] VisibleEmojis { get; set; }   // 5 emoji visibili dall'inizio
        public string[] HiddenEmojis { get; set; }    // 2 emoji nascosti da rivelare
        public string HintEmoji { get; set; }         // 1 emoji suggerimento
        public bool IsDynamic { get; set; }           // true = renderizzata da DynamicStoryForm
        public bool IsUserCreated { get; set; }       // true = creata dall'utente nell'app
    }
}