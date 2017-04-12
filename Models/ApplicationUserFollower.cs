namespace Community.Models
{
    public class ApplicationUserFollower
    {
        public int UserIndex { get; set; }
        public ApplicationUser User { get; set; }
        public int FollowerIndex { get; set; }
        public ApplicationUser Follower { get; set; }
    }
}