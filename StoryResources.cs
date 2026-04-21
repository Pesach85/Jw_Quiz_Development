using System.Drawing;

namespace Jw_Quiz_Development
{
    /// <summary>
    /// Central access point for embedded PNG image resources used by the story renderer.
    /// Centralises the ResourceManager call so every form uses the same loading path.
    /// </summary>
    internal static class StoryResources
    {
        /// <summary>Resource key for the "unknown / hidden slot" placeholder (❓ 2753.png).</summary>
        public const string KeyUnknown = "2753";

        /// <summary>Resource key for the "hint / hot clue" pulsing slot (🔥 1F525.png).</summary>
        public const string KeyHint = "1F525";

        /// <summary>
        /// Loads a colored PNG from embedded resources by its resource key (filename without extension).
        /// Returns null if the key is empty or the resource does not exist.
        /// </summary>
        public static Image GetImage(string resourceKey)
        {
            if (string.IsNullOrWhiteSpace(resourceKey)) return null;
            return Properties.Resources.ResourceManager.GetObject(resourceKey) as Image;
        }
    }
}
