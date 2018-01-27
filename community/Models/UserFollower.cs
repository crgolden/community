namespace community.Models
{
    public class UserFollower
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public string FollowerId { get; set; }
        public User Follower { get; set; }
    }
}