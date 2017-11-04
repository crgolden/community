using System;

namespace community.Models
{
    public class EventFollower
    {
        public Guid FollowedEventId { get; set; }
        public Event FollowedEvent { get; set; }
        public string FollowerId { get; set; }
        public ApplicationUser Follower { get; set; }
    }
}