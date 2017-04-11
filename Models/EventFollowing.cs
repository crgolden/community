namespace Community.Models
{
    public class EventFollowing
    {
        public string FollowerId { get; set; }
        public int FollowerIdInt { get; set; }
        public ApplicationUser Follower { get; set; }
        public string FollowedEventId { get; set; }
        public int FollowedEventIdInt { get; set; }
        public Event FollowedEvent { get; set; }
    }
}