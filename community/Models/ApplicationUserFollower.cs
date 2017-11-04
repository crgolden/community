namespace community.Models
{
    public class ApplicationUserFollower
    {
        public string FollowedUserId { get; set; }
        public ApplicationUser FollowedUser { get; set; }
        public string FollowerId { get; set; }
        public ApplicationUser Follower { get; set; }
    }
}