using System.Collections.Generic;

namespace Jw_Quiz_Development
{
    public static class StoryLibrary
    {
        public static IReadOnlyList<Story> Stories { get; } = new List<Story>
        {
            new Story
            {
                Id = 1,
                Title = "The Call of Abram",
                ScriptureReference = "Genesis 12:1-4",
                Keyword = "Faith",
                Hint = "Leave your country and follow God's promise.",
                Solution = "Abram obeyed, showing faith by going where Jehovah directed.",
                EngagementNote = "Highlight how obedience and trust in God's guidance are the core of biblical faith.",
                ImageResourceName = "Abram"
            },
            new Story
            {
                Id = 2,
                Title = "Joseph's Dreams",
                ScriptureReference = "Genesis 37:5-10",
                Keyword = "Forgiveness",
                Hint = "Dreams led to trouble, but the outcome taught mercy.",
                Solution = "Joseph forgave his brothers and saved many during famine.",
                EngagementNote = "Emphasize how forgiveness releases pain and builds God's purpose.",
                ImageResourceName = "Joseph"
            },
            new Story
            {
                Id = 3,
                Title = "Moses and the Burning Bush",
                ScriptureReference = "Exodus 3:2-10",
                Keyword = "Deliverance",
                Hint = "God appears in fire and gives a mission to lead his people out.",
                Solution = "Moses accepted the call and became Jehovah's instrument of deliverance.",
                EngagementNote = "Explore how God equips imperfect people for powerful work.",
                ImageResourceName = "Moses"
            },
            new Story
            {
                Id = 4,
                Title = "Crossing the Red Sea",
                ScriptureReference = "Exodus 14:21-22",
                Keyword = "Liberation",
                Hint = "Water stands upright when God makes a way.",
                Solution = "Jehovah protected Israel and brought them safely through the sea.",
                EngagementNote = "Draw attention to God's ability to bring victory where human effort fails.",
                ImageResourceName = "RedSea"
            },
            new Story
            {
                Id = 5,
                Title = "Ruth's Loyalty",
                ScriptureReference = "Ruth 1:16-17",
                Keyword = "Devotion",
                Hint = "A widow chooses to stay with her mother-in-law and trust in the same God.",
                Solution = "Ruth's loyalty led to blessing and a place in the line of David.",
                EngagementNote = "Show how loyalty to Jehovah and to others brings lasting reward.",
                ImageResourceName = "Ruth"
            },
            new Story
            {
                Id = 6,
                Title = "David and Goliath",
                ScriptureReference = "1 Samuel 17:45",
                Keyword = "Courage",
                Hint = "A young shepherd faces a giant with only a sling and God's help.",
                Solution = "David trusted Jehovah and toppled Goliath, showing true courage.",
                EngagementNote = "Use this story to encourage bold trust in God even in the face of fear.",
                ImageResourceName = "DavidGoliath"
            },
            new Story
            {
                Id = 7,
                Title = "Elijah and the Prophets of Baal",
                ScriptureReference = "1 Kings 18:36-39",
                Keyword = "Faithfulness",
                Hint = "Only one God answers with fire.",
                Solution = "Jehovah proved himself and the people turned their hearts back to him.",
                EngagementNote = "Teach that true worship requires loyalty to Jehovah alone.",
                ImageResourceName = "Elijah"
            },
            new Story
            {
                Id = 8,
                Title = "Daniel in the Lions' Den",
                ScriptureReference = "Daniel 6:22",
                Keyword = "Integrity",
                Hint = "A faithful man refuses to stop praying, even under penalty of death.",
                Solution = "Jehovah protected Daniel because he remained faithful.",
                EngagementNote = "Encourage standing firm in prayer and principle under pressure.",
                ImageResourceName = "Daniel"
            },
            new Story
            {
                Id = 9,
                Title = "Jonah and the Fish",
                ScriptureReference = "Jonah 1:17",
                Keyword = "Repentance",
                Hint = "Running away doesn't hide you from God's purpose.",
                Solution = "Jonah repented and learned that mercy has a purpose.",
                EngagementNote = "Focus on second chances and responding to God's call.",
                ImageResourceName = "Jonah"
            },
            new Story
            {
                Id = 10,
                Title = "Esther's Courage",
                ScriptureReference = "Esther 4:14",
                Keyword = "Purpose",
                Hint = "She may have become queen for a time like this.",
                Solution = "Esther risked everything to save her people.",
                EngagementNote = "Highlight how modest people can serve a divine purpose.",
                ImageResourceName = "Esther"
            },
            new Story
            {
                Id = 11,
                Title = "Jesus Feeds the 5,000",
                ScriptureReference = "John 6:11",
                Keyword = "Compassion",
                Hint = "There is enough when Jesus blesses what little is offered.",
                Solution = "Jesus fed the crowd, showing God's care for physical and spiritual needs.",
                EngagementNote = "Connect caring for others with living biblical compassion.",
                ImageResourceName = "FiveLoaves"
            },
            new Story
            {
                Id = 12,
                Title = "Paul and Silas in Prison",
                ScriptureReference = "Acts 16:25",
                Keyword = "Praise",
                Hint = "They sang while locked in chains.",
                Solution = "Their praise helped bring another soul to Jehovah.",
                EngagementNote = "Use this to teach that worship remains strong even in hardship.",
                ImageResourceName = "PaulSilas"
            }
        };
    }
}