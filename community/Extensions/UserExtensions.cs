using community.Models;

namespace community.Extensions
{
    public static class UserExtensions
    {
        public static UserViewModel ToViewModel(this User user, string token = null)
        {
            return new UserViewModel(user, token);
        }
    }
}