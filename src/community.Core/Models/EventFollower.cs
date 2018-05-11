using System;

namespace community.Core.Models
{
    public class EventFollower
    {
        public Guid FollowedEventId { get; set; }
        public Event FollowedEvent { get; set; }
        public string FollowerId { get; set; }
        public User Follower { get; set; }
    }
}