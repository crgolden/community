namespace Community.Models
{
    public class ApplicationUserFollowing
    {
        public int FollowerId { get; set; }
        public ApplicationUser Follower { get; set; }
        public int FollowedUserId { get; set; }
        public ApplicationUser FollowedUser { get; set; }
    }
}