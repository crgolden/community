using System.Collections.Generic;

namespace Community.Data.Seed
{
    public static class RoleClaims
    {
        public static List<string> ValuesUser = new List<string>
        {
            "Edit User",
            "Create Address",
            "Create Event",
            "Edit Event",
            "Delete Event",
            "Create EventAttendance",
            "Delete EventAttendance",
            "Create UserFollowing",
            "Delete UserFollowing",
            "Create EventFollowing",
            "Delete EventFollowing"
        };

        public static List<string> ValuesAdmin = new List<string>
        {
            "Create User",
            "Delete User",
            "Edit Address",
            "Delete Address"
        };
    }
}