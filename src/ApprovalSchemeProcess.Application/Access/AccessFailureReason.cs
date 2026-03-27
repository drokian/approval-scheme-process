namespace ApprovalSchemeProcess.Application.Access;

public enum AccessFailureReason
{
    None = 0,
    RequesterNotFound = 1,
    RequesterInactive = 2,
    RequesterNotAuthorized = 3,
    OperationTypeNotFound = 4,
    OperationTypeInactive = 5,
    InvalidSessionContext = 6
}
