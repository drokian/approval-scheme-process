namespace ApprovalSchemeProcess.Domain.Entities;

public class Appointment
{
    public long Id { get; set; }
    public long OperationTypeId { get; set; }
    public string CitizenIdentifier { get; set; } = string.Empty;
    public string? CitizenDisplayName { get; set; }
    public DateTime ScheduledStartAt { get; set; }
    public DateTime ScheduledEndAt { get; set; }
    public string Status { get; set; } = "scheduled";
    public long? CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public OperationType OperationType { get; set; } = null!;
    public User? CreatedByUser { get; set; }
    public ICollection<AppointmentTarget> Targets { get; set; } = [];
    public Session? Session { get; set; }
}

public class AppointmentTarget
{
    public long Id { get; set; }
    public long AppointmentId { get; set; }
    public string TargetType { get; set; } = string.Empty;
    public string TargetIdentifier { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
    public DateTime CreatedAt { get; set; }
    public Appointment Appointment { get; set; } = null!;
}

public class Session
{
    public long Id { get; set; }
    public long AppointmentId { get; set; }
    public long? CurrentAssignedUserId { get; set; }
    public string Status { get; set; } = "active";
    public DateTime ActivatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime LastActivityAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public DateTime? InvalidatedAt { get; set; }
    public string? InvalidationReason { get; set; }
    public DateTime CreatedAt { get; set; }
    public Appointment Appointment { get; set; } = null!;
    public User? CurrentAssignedUser { get; set; }
    public ICollection<SessionAssignment> Assignments { get; set; } = [];
    public ICollection<Query> Queries { get; set; } = [];
}

public class SessionAssignment
{
    public long Id { get; set; }
    public long SessionId { get; set; }
    public long UserId { get; set; }
    public string AssignmentType { get; set; } = "primary";
    public string? AssignmentReason { get; set; }
    public long? AssignedByUserId { get; set; }
    public DateTime AssignedAt { get; set; }
    public DateTime? ReleasedAt { get; set; }
    public bool IsCurrent { get; set; } = true;
    public Session Session { get; set; } = null!;
    public User User { get; set; } = null!;
    public User? AssignedByUser { get; set; }
}
