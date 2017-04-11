namespace Community.Models
{
    public class ApplicationUserFollowing
    {
        public int FollowerIdInt { get; set; }
        public ApplicationUser Follower { get; set; }
        public int FollowedUserIdInt { get; set; }
        public ApplicationUser FollowedUser { get; set; }
    }
}