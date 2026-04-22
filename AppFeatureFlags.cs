namespace Jw_Quiz_Development
{
    public static class AppFeatureFlags
    {
        // Step 1 migration guard: keep legacy stories on static forms until their data is fully ready.
        public static bool UseDynamicRendererForLegacyStories => false;
    }
}