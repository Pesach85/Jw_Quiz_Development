namespace Jw_Quiz_Development
{
    public static class AppFeatureFlags
    {
        // Step 3 rollout: legacy stories 1-12 now route to DynamicStoryForm when data checks pass.
        // Rollback strategy: set this to false to immediately restore static forms.
        public static bool UseDynamicRendererForLegacyStories => true;
    }
}