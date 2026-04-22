using System;

namespace Jw_Quiz_Development
{
    public static class StoryLocalizationService
    {
        private static readonly IStoryTranslationEngine Translator = new RuleBasedStoryTranslationEngine();

        public static StoryLocalizedText GetText(Story story)
        {
            return GetText(story, LanguageManager.CurrentLanguage);
        }

        public static StoryLocalizedText GetText(Story story, AppLanguage language)
        {
            EnsureTranslations(story);
            if (story == null || story.Translations == null)
                return new StoryLocalizedText();

            StoryLocalizedText localized = story.Translations.Get(language);
            if (localized != null)
                return localized;

            StoryLocalizedText source = story.Translations.Get(GetSourceLanguage(story));
            if (source == null)
                return new StoryLocalizedText();

            localized = Translator.Translate(source, GetSourceLanguage(story), language);
            story.Translations.Set(language, localized);
            return localized;
        }

        public static void EnsureTranslations(Story story)
        {
            if (story == null)
                return;

            if (story.Translations == null)
                story.Translations = new StoryTranslations();

            AppLanguage sourceLanguage = GetSourceLanguage(story);
            StoryLocalizedText sourceText = BuildSourceText(story);
            story.Translations.Set(sourceLanguage, sourceText);

            AppLanguage targetLanguage = sourceLanguage == AppLanguage.Italian ? AppLanguage.English : AppLanguage.Italian;
            if (story.Translations.Get(targetLanguage) == null)
                story.Translations.Set(targetLanguage, Translator.Translate(sourceText, sourceLanguage, targetLanguage));
        }

        public static StoryLocalizedText BuildSourceText(Story story)
        {
            return new StoryLocalizedText
            {
                Title = story.Title,
                ScriptureReference = story.ScriptureReference,
                Keyword = story.Keyword,
                Hint = story.Hint,
                Solution = story.Solution,
                EngagementNote = story.EngagementNote,
                ScriptureQuote = story.ScriptureQuote,
                ImageCaptions = story.ImageCaptions != null ? (string[])story.ImageCaptions.Clone() : null
            };
        }

        public static void SetSourceText(Story story, StoryLocalizedText text, AppLanguage sourceLanguage)
        {
            if (story == null)
                return;

            story.SourceLanguage = sourceLanguage;
            story.Title = text.Title;
            story.ScriptureReference = text.ScriptureReference;
            story.Keyword = text.Keyword;
            story.Hint = text.Hint;
            story.Solution = text.Solution;
            story.EngagementNote = text.EngagementNote;
            story.ScriptureQuote = text.ScriptureQuote;
            story.ImageCaptions = text.ImageCaptions != null ? (string[])text.ImageCaptions.Clone() : null;
            story.Translations = new StoryTranslations();
            story.Translations.Set(sourceLanguage, BuildSourceText(story));

            AppLanguage targetLanguage = sourceLanguage == AppLanguage.Italian ? AppLanguage.English : AppLanguage.Italian;
            story.Translations.Set(targetLanguage, Translator.Translate(text, sourceLanguage, targetLanguage));
        }

        public static AppLanguage GetSourceLanguage(Story story)
        {
            return story != null ? story.SourceLanguage : AppLanguage.Italian;
        }
    }
}