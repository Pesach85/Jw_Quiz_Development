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

        public static IReadOnlyList<Story> GetDynamicStories()
        {
            return StoryLibrary.Stories.Where(s => s.IsDynamic).ToList();
        }

        public static IReadOnlyList<Story> GetAllStoriesIncludingUser()
        {
            var all = new List<Story>(StoryLibrary.Stories);
            all.AddRange(UserStoryLibrary.GetUserStories());
            return all;
        }

        public static Story GetStory(int id)
        {
            return GetAllStoriesIncludingUser().FirstOrDefault(s => s.Id == id);
        }

        public static Story GetNextStory(int currentId)
        {
            return GetAllStoriesIncludingUser().FirstOrDefault(s => s.Id == currentId + 1);
        }

        public static Story GetFirstStory()
        {
            return StoryLibrary.Stories.FirstOrDefault();
        }

        public static int TotalStories => StoryLibrary.Stories.Count;
        public static int TotalStoriesIncludingUser => GetAllStoriesIncludingUser().Count;
    }
}