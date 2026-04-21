using System.Collections.Generic;
using System.Linq;

namespace Jw_Quiz_Development
{
    public static class StoryEngine
    {
        public static IReadOnlyList<Story> GetAllStories()
        {
            return StoryLibrary.Stories;
        }

        public static Story GetStory(int id)
        {
            return StoryLibrary.Stories.FirstOrDefault(s => s.Id == id);
        }

        public static Story GetNextStory(int currentId)
        {
            return StoryLibrary.Stories.FirstOrDefault(s => s.Id == currentId + 1);
        }

        public static Story GetFirstStory()
        {
            return StoryLibrary.Stories.FirstOrDefault();
        }

        public static int TotalStories => StoryLibrary.Stories.Count;
    }
}