using System;

namespace Jw_Quiz_Development
{
    [Serializable]
    public class StoryLocalizedText
    {
        public string Title { get; set; }
        public string ScriptureReference { get; set; }
        public string Keyword { get; set; }
        public string Hint { get; set; }
        public string Solution { get; set; }
        public string EngagementNote { get; set; }
        public string ScriptureQuote { get; set; }
        public string[] ImageCaptions { get; set; }

        public StoryLocalizedText Clone()
        {
            return new StoryLocalizedText
            {
                Title = Title,
                ScriptureReference = ScriptureReference,
                Keyword = Keyword,
                Hint = Hint,
                Solution = Solution,
                EngagementNote = EngagementNote,
                ScriptureQuote = ScriptureQuote,
                ImageCaptions = ImageCaptions != null ? (string[])ImageCaptions.Clone() : null
            };
        }
    }
}