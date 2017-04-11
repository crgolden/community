namespace Community.Models
{
    public class EventFollowing
    {
        public int FollowerId { get; set; }
        public ApplicationUser Follower { get; set; }
        public int FollowedEventId { get; set; }
        public Event FollowedEvent { get; set; }
    }
}