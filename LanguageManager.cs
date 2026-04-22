using System;
using System.IO;

namespace Jw_Quiz_Development
{
    public static class LanguageManager
    {
        private const string LanguageFileName = "AppLanguage.dat";

        static LanguageManager()
        {
            CurrentLanguage = Load();
        }

        public static event EventHandler LanguageChanged;

        public static AppLanguage CurrentLanguage { get; private set; }

        public static void SetLanguage(AppLanguage language)
        {
            if (CurrentLanguage == language)
                return;

            CurrentLanguage = language;
            Save(language);

            var handler = LanguageChanged;
            if (handler != null)
                handler(null, EventArgs.Empty);
        }

        private static AppLanguage Load()
        {
            try
            {
                if (!File.Exists(LanguageFileName))
                    return AppLanguage.Italian;

                string raw = File.ReadAllText(LanguageFileName).Trim();
                AppLanguage parsed;
                return Enum.TryParse(raw, true, out parsed) ? parsed : AppLanguage.Italian;
            }
            catch
            {
                return AppLanguage.Italian;
            }
        }

        private static void Save(AppLanguage language)
        {
            try
            {
                File.WriteAllText(LanguageFileName, language.ToString());
            }
            catch
            {
                // Best-effort persistence only.
            }
        }
    }
}