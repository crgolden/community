using System.Collections.Generic;

namespace Community.Data.Seed
{
    public static class Events
    {
        public static List<Dictionary<string, string>> JimEvents = new List<Dictionary<string, string>>
        {
            new Dictionary<string, string>
            {
                {"Name", "Drinks"},
                {"Details", "Let's have some drinks at the Yard Bar."}
            }
        };

        public static List<Dictionary<string, string>> MarleneEvents = new List<Dictionary<string, string>>
        {
            new Dictionary<string, string>
            {
                {"Name", "Softball"},
                {"Details", "Join in for a game of softball at Bull Creek!"}
            }
        };

        public static List<Dictionary<string, string>> TimEvents = new List<Dictionary<string, string>>
        {
            new Dictionary<string, string>
            {
                {"Name", "Movies"},
                {"Details", "We're going to watch some movies at the Alamo Drafthouse."}
            }
        };
    }
}