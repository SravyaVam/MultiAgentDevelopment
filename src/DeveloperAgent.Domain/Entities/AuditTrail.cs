namespace DeveloperAgent.Domain.Entities
{
    public class AuditTrail
    {
        public long Id { get; set; }
        public string EntityType { get; set; } = string.Empty;
        public string? EntityId { get; set; }
        public string Action { get; set; } = string.Empty;
        public int? ActorUserId { get; set; }
        public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
        public string? Changes { get; set; } // JSON string describing changes
        public string? IpAddress { get; set; }
    }
}
