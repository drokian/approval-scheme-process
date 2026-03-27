using ApprovalSchemeProcess.Application.Access;
using ApprovalSchemeProcess.Application.Sessions;
using ApprovalSchemeProcess.Domain.Entities;

namespace ApprovalSchemeProcess.UnitTests.Access;

public class AccessEvaluationServiceTests
{
    [Fact]
    public async Task EvaluateAsync_returns_allowed_for_in_context_employee_request()
    {
        var service = CreateService(
            requester: CreateRequester(),
            operationType: CreateOperationType(securityLevelCode: 2, requiresApproval: true),
            sessionEvaluation: SessionContextEvaluation.Allowed(100, 200));

        var result = await service.EvaluateAsync(CreateRequest(sessionId: 100));

        Assert.Equal(AccessDecisionOutcome.Allowed, result.Outcome);
        Assert.Equal(AccessRequestClassification.InContext, result.Classification);
        Assert.False(result.RequiresApproval);
        Assert.Equal(2, result.SecurityLevelCode);
        Assert.Equal(200, result.AppointmentId);
    }

    [Fact]
    public async Task EvaluateAsync_returns_requires_approval_for_out_of_context_request_with_elevated_security()
    {
        var service = CreateService(
            requester: CreateRequester(),
            operationType: CreateOperationType(securityLevelCode: 2, requiresApproval: true),
            sessionEvaluation: SessionContextEvaluation.Denied(SessionContextFailureReason.TargetMismatch, 100, 200));

        var result = await service.EvaluateAsync(CreateRequest(sessionId: 100));

        Assert.Equal(AccessDecisionOutcome.RequiresApproval, result.Outcome);
        Assert.Equal(AccessRequestClassification.OutOfContext, result.Classification);
        Assert.True(result.RequiresApproval);
        Assert.Equal(SessionContextFailureReason.TargetMismatch, result.SessionFailureReason);
        Assert.Equal(2, result.SecurityLevelCode);
    }

    [Fact]
    public async Task EvaluateAsync_returns_allowed_for_out_of_context_request_when_security_level_does_not_require_approval()
    {
        var service = CreateService(
            requester: CreateRequester(),
            operationType: CreateOperationType(securityLevelCode: 0, requiresApproval: false),
            sessionEvaluation: SessionContextEvaluation.Denied(SessionContextFailureReason.TargetMismatch, 100, 200));

        var result = await service.EvaluateAsync(CreateRequest(sessionId: 100));

        Assert.Equal(AccessDecisionOutcome.Allowed, result.Outcome);
        Assert.Equal(AccessRequestClassification.OutOfContext, result.Classification);
        Assert.False(result.RequiresApproval);
        Assert.Equal(0, result.SecurityLevelCode);
    }

    [Fact]
    public async Task EvaluateAsync_returns_denied_when_requester_is_inactive()
    {
        var requester = CreateRequester();
        requester.Status = "inactive";
        var service = CreateService(requester, CreateOperationType(), SessionContextEvaluation.Allowed(100, 200));

        var result = await service.EvaluateAsync(CreateRequest(sessionId: 100));

        Assert.Equal(AccessDecisionOutcome.Denied, result.Outcome);
        Assert.Equal(AccessFailureReason.RequesterInactive, result.FailureReason);
    }

    [Fact]
    public async Task EvaluateAsync_returns_denied_when_requester_has_no_active_employee_role()
    {
        var requester = CreateRequester();
        requester.UserRoles.Clear();
        var service = CreateService(requester, CreateOperationType(), SessionContextEvaluation.Allowed(100, 200));

        var result = await service.EvaluateAsync(CreateRequest(sessionId: 100));

        Assert.Equal(AccessDecisionOutcome.Denied, result.Outcome);
        Assert.Equal(AccessFailureReason.RequesterNotAuthorized, result.FailureReason);
    }

    [Fact]
    public async Task EvaluateAsync_returns_denied_when_operation_type_is_inactive()
    {
        var operationType = CreateOperationType();
        operationType.IsActive = false;
        var service = CreateService(CreateRequester(), operationType, SessionContextEvaluation.Allowed(100, 200));

        var result = await service.EvaluateAsync(CreateRequest(sessionId: 100));

        Assert.Equal(AccessDecisionOutcome.Denied, result.Outcome);
        Assert.Equal(AccessFailureReason.OperationTypeInactive, result.FailureReason);
    }

    [Fact]
    public async Task EvaluateAsync_returns_denied_when_session_context_is_terminally_invalid()
    {
        var service = CreateService(
            requester: CreateRequester(),
            operationType: CreateOperationType(),
            sessionEvaluation: SessionContextEvaluation.Denied(SessionContextFailureReason.SessionExpired, 100, 200));

        var result = await service.EvaluateAsync(CreateRequest(sessionId: 100));

        Assert.Equal(AccessDecisionOutcome.Denied, result.Outcome);
        Assert.Equal(AccessFailureReason.InvalidSessionContext, result.FailureReason);
        Assert.Equal(SessionContextFailureReason.SessionExpired, result.SessionFailureReason);
    }

    [Fact]
    public async Task EvaluateAsync_classifies_request_without_session_as_out_of_context()
    {
        var service = CreateService(
            requester: CreateRequester(),
            operationType: CreateOperationType(securityLevelCode: 1, requiresApproval: true),
            sessionEvaluation: SessionContextEvaluation.Denied(SessionContextFailureReason.None));

        var result = await service.EvaluateAsync(CreateRequest(sessionId: null));

        Assert.Equal(AccessDecisionOutcome.RequiresApproval, result.Outcome);
        Assert.Equal(AccessRequestClassification.OutOfContext, result.Classification);
        Assert.Equal(SessionContextFailureReason.None, result.SessionFailureReason);
        Assert.Null(result.SessionId);
    }

    private static AccessEvaluationService CreateService(
        User? requester,
        OperationType? operationType,
        SessionContextEvaluation sessionEvaluation) =>
        new(
            new FakeSessionContextService(sessionEvaluation),
            new FakeAccessEvaluationReader(requester, operationType));

    private static AccessEvaluationRequest CreateRequest(long? sessionId) =>
        new(
            sessionId,
            42,
            7,
            "citizen",
            "CIT-1001",
            null,
            false,
            false,
            new DateTime(2026, 3, 27, 10, 15, 0, DateTimeKind.Utc));

    private static User CreateRequester() =>
        new()
        {
            Id = 42,
            Username = "ayse.yilmaz",
            EmployeeNumber = "EMP-1001",
            FullName = "Ayse Yilmaz",
            Status = "active",
            UserRoles =
            [
                new UserRole
                {
                    Id = 1,
                    UserId = 42,
                    IsActive = true,
                    ValidFrom = new DateTime(2026, 3, 27, 9, 0, 0, DateTimeKind.Utc),
                    Role = new Role
                    {
                        Id = 10,
                        Code = "EMPLOYEE",
                        Name = "Employee"
                    }
                }
            ]
        };

    private static OperationType CreateOperationType(int securityLevelCode = 2, bool requiresApproval = true) =>
        new()
        {
            Id = 7,
            Code = "LAND_SALE",
            Name = "Land Sale",
            IsActive = true,
            SecurityLevelId = 5,
            SecurityLevel = new SecurityLevel
            {
                Id = 5,
                LevelCode = securityLevelCode,
                Name = $"Level {securityLevelCode}",
                RequiresApproval = requiresApproval
            }
        };

    private sealed class FakeSessionContextService(SessionContextEvaluation sessionEvaluation) : ISessionContextService
    {
        public Task<SessionContextEvaluation> IsInContextAsync(
            SessionContextRequest request,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(sessionEvaluation);
    }

    private sealed class FakeAccessEvaluationReader(User? requester, OperationType? operationType) : IAccessEvaluationReader
    {
        public Task<User?> GetRequesterAsync(long userId, CancellationToken cancellationToken = default) =>
            Task.FromResult(requester);

        public Task<OperationType?> GetOperationTypeAsync(long operationTypeId, CancellationToken cancellationToken = default) =>
            Task.FromResult(operationType);
    }
}
