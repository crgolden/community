namespace community.Models
{
    // TypeScript: user.ts
    public class UserViewModel
    {
        public string Id { get; }
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public UserViewModel(User user, string token = null)
        {
            Id = user.Id;
            Token = token;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
        }
    }
}