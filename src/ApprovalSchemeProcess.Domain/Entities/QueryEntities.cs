namespace ApprovalSchemeProcess.Domain.Entities;

public class Query
{
    public long Id { get; set; }
    public long? SessionId { get; set; }
    public long RequestedByUserId { get; set; }
    public long OperationTypeId { get; set; }
    public long SecurityLevelId { get; set; }
    public string TargetType { get; set; } = string.Empty;
    public string TargetIdentifier { get; set; } = string.Empty;
    public string RequestClassification { get; set; } = string.Empty;
    public string? Justification { get; set; }
    public bool IsEmergency { get; set; }
    public bool IsOverride { get; set; }
    public string Status { get; set; } = "pending";
    public DateTime RequestedAt { get; set; }
    public DateTime? DecidedAt { get; set; }
    public DateTime? ExecutedAt { get; set; }
    public Session? Session { get; set; }
    public User RequestedByUser { get; set; } = null!;
    public OperationType OperationType { get; set; } = null!;
    public SecurityLevel SecurityLevel { get; set; } = null!;
    public ApprovalRequest? ApprovalRequest { get; set; }
}

public class AuditLog
{
    public long Id { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public long EntityId { get; set; }
    public long? ActorUserId { get; set; }
    public DateTime OccurredAt { get; set; }
    public string? IpAddress { get; set; }
    public string? DeviceIdentifier { get; set; }
    public string? Details { get; set; }
    public User? ActorUser { get; set; }
}
