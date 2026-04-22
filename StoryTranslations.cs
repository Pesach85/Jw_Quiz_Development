using System;

namespace Jw_Quiz_Development
{
    [Serializable]
    public class StoryTranslations
    {
        public StoryLocalizedText Italian { get; set; }
        public StoryLocalizedText English { get; set; }

        public StoryLocalizedText Get(AppLanguage language)
        {
            return language == AppLanguage.English ? English : Italian;
        }

        public void Set(AppLanguage language, StoryLocalizedText text)
        {
            if (language == AppLanguage.English)
                English = text;
            else
                Italian = text;
        }
    }
}