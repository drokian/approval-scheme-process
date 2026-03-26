namespace ApprovalSchemeProcess.Domain.Entities;

public class User
{
    public long Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string Status { get; set; } = "active";
    public DateTime CreatedAt { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = [];
    public ICollection<Appointment> CreatedAppointments { get; set; } = [];
    public ICollection<Session> CurrentAssignedSessions { get; set; } = [];
    public ICollection<SessionAssignment> SessionAssignments { get; set; } = [];
    public ICollection<SessionAssignment> AssignedSessionActions { get; set; } = [];
    public ICollection<Query> RequestedQueries { get; set; } = [];
    public ICollection<Approval> Approvals { get; set; } = [];
    public ICollection<AuditLog> AuditLogs { get; set; } = [];
}

public class Role
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = [];
}

public class UserRole
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long RoleId { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public bool IsActive { get; set; } = true;
    public User User { get; set; } = null!;
    public Role Role { get; set; } = null!;
}

public class SecurityLevel
{
    public long Id { get; set; }
    public int LevelCode { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool RequiresApproval { get; set; }
    public int MinApprovalSteps { get; set; }
    public bool RequiresComplianceReview { get; set; }
    public bool RequiresSpecialAuthorization { get; set; }
    public ICollection<OperationType> OperationTypes { get; set; } = [];
    public ICollection<Query> Queries { get; set; } = [];
}

public class OperationType
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public long SecurityLevelId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public SecurityLevel SecurityLevel { get; set; } = null!;
    public ICollection<ApprovalScheme> ApprovalSchemes { get; set; } = [];
    public ICollection<Appointment> Appointments { get; set; } = [];
    public ICollection<Query> Queries { get; set; } = [];
}
