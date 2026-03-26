namespace ApprovalSchemeProcess.Domain.Entities;

public class ApprovalScheme
{
    public long Id { get; set; }
    public long OperationTypeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int VersionNo { get; set; } = 1;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime? RetiredAt { get; set; }
    public OperationType OperationType { get; set; } = null!;
    public ICollection<ApprovalSchemeStep> Steps { get; set; } = [];
    public ICollection<ApprovalRequest> ApprovalRequests { get; set; } = [];
}

public class ApprovalSchemeStep
{
    public long Id { get; set; }
    public long ApprovalSchemeId { get; set; }
    public int StepOrder { get; set; }
    public string RoleCode { get; set; } = string.Empty;
    public string ReviewType { get; set; } = "approval";
    public bool IsMandatory { get; set; } = true;
    public int? TimeoutMinutes { get; set; }
    public DateTime CreatedAt { get; set; }
    public ApprovalScheme ApprovalScheme { get; set; } = null!;
    public ICollection<Approval> Approvals { get; set; } = [];
}

public class ApprovalRequest
{
    public long Id { get; set; }
    public long QueryId { get; set; }
    public long ApprovalSchemeId { get; set; }
    public string Status { get; set; } = "pending";
    public DateTime RequestedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Query Query { get; set; } = null!;
    public ApprovalScheme ApprovalScheme { get; set; } = null!;
    public ICollection<Approval> Approvals { get; set; } = [];
}

public class Approval
{
    public long Id { get; set; }
    public long ApprovalRequestId { get; set; }
    public long ApprovalSchemeStepId { get; set; }
    public long ApproverUserId { get; set; }
    public string Decision { get; set; } = string.Empty;
    public string? Reason { get; set; }
    public DateTime DecidedAt { get; set; }
    public ApprovalRequest ApprovalRequest { get; set; } = null!;
    public ApprovalSchemeStep ApprovalSchemeStep { get; set; } = null!;
    public User ApproverUser { get; set; } = null!;
}
