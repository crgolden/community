using System.Collections.Generic;

namespace Community.Data.Seed
{
    public static class Addresses
    {
        public static List<Dictionary<string, string>> JimAddresses = new List<Dictionary<string, string>>
        {
            new Dictionary<string, string>
            {
                {"Street", "4600 W Guadalupe St"},
                {"Street2", "806"},
                {"City", "Austin"},
                {"State", "TX"},
                {"ZipCode", "78751"},
                {"Latitude", "30.314223"},
                {"Longitude", "-97.732929"},
                {"Home", "true"}
            },
            new Dictionary<string, string>
            {
                {"Street", "6700 Burnet Rd"},
                {"Street2", string.Empty},
                {"City", "Austin"},
                {"State", "TX"},
                {"ZipCode", "78757"},
                {"Latitude", "30.343087"},
                {"Longitude", "-97.739128"},
                {"Home", "false"}
            }
        };

        public static List<Dictionary<string, string>> MarleneAddresses = new List<Dictionary<string, string>>
        {
            new Dictionary<string, string>
            {
                {"Street", "6804 N Capital of Texas Hwy"},
                {"Street2", "1123"},
                {"City", "Austin"},
                {"State", "TX"},
                {"ZipCode", "78731"},
                {"Latitude", "30.369964"},
                {"Longitude", "-97.789853"},
                {"Home", "true"}
            },
            new Dictionary<string, string>
            {
                {"Street", "6701 Lakewood Dr"},
                {"Street2", string.Empty},
                {"City", "Austin"},
                {"State", "TX"},
                {"ZipCode", "78731"},
                {"Latitude", "30.368693"},
                {"Longitude", "-97.784469"},
                {"Home", "false"}
            }
        };

        public static List<Dictionary<string, string>> TimAddresses = new List<Dictionary<string, string>>
        {
            new Dictionary<string, string>
            {
                {"Street", "2819 Foster Ln"},
                {"Street2", "601"},
                {"City", "Austin"},
                {"State", "TX"},
                {"ZipCode", "78757"},
                {"Latitude", "30.356405"},
                {"Longitude", "-97.738210"},
                {"Home", "true"}
            },
            new Dictionary<string, string>
            {
                {"Street", "2700 W Anderson Ln"},
                {"Street2", string.Empty},
                {"City", "Austin"},
                {"State", "TX"},
                {"ZipCode", "78757"},
                {"Latitude", "30.360028"},
                {"Longitude", "-97.734848"},
                {"Home", "false"}
            }
        };
    }
}