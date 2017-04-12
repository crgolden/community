namespace Community.Models
{
    public class EventFollower
    {
        public string EventId { get; set; }
        public int EventIndex { get; set; }
        public Event Event { get; set; }
        public string FollowerId { get; set; }
        public int FollowerIndex { get; set; }
        public ApplicationUser Follower { get; set; }
    }
}