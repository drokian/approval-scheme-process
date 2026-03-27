using ApprovalSchemeProcess.Application.Sessions;
using ApprovalSchemeProcess.Domain.Entities;

namespace ApprovalSchemeProcess.UnitTests.Sessions;

public class SessionContextEvaluatorTests
{
    private readonly SessionContextEvaluator _sut = new();

    [Fact]
    public void IsInContext_returns_allowed_when_session_is_active_and_target_matches()
    {
        var session = CreateValidSession();
        var request = CreateRequest();

        var result = _sut.IsInContext(session, request);

        Assert.True(result.IsInContext);
        Assert.Equal(SessionContextFailureReason.None, result.FailureReason);
        Assert.Equal(session.Id, result.SessionId);
        Assert.Equal(session.AppointmentId, result.AppointmentId);
    }

    [Fact]
    public void IsInContext_returns_session_not_found_when_session_is_missing()
    {
        var result = _sut.IsInContext(null, CreateRequest());

        Assert.False(result.IsInContext);
        Assert.Equal(SessionContextFailureReason.SessionNotFound, result.FailureReason);
    }

    [Fact]
    public void IsInContext_returns_session_expired_when_expiry_has_passed()
    {
        var session = CreateValidSession();
        var request = CreateRequest(requestedAtUtc: new DateTime(2026, 3, 27, 10, 45, 0, DateTimeKind.Utc));

        session.ExpiresAt = new DateTime(2026, 3, 27, 10, 30, 0, DateTimeKind.Utc);

        var result = _sut.IsInContext(session, request);

        Assert.False(result.IsInContext);
        Assert.Equal(SessionContextFailureReason.SessionExpired, result.FailureReason);
    }

    [Fact]
    public void IsInContext_returns_session_closed_when_session_was_closed()
    {
        var session = CreateValidSession();
        session.ClosedAt = new DateTime(2026, 3, 27, 10, 20, 0, DateTimeKind.Utc);

        var result = _sut.IsInContext(session, CreateRequest());

        Assert.False(result.IsInContext);
        Assert.Equal(SessionContextFailureReason.SessionClosed, result.FailureReason);
    }

    [Fact]
    public void IsInContext_returns_session_invalidated_when_session_was_invalidated()
    {
        var session = CreateValidSession();
        session.InvalidatedAt = new DateTime(2026, 3, 27, 10, 10, 0, DateTimeKind.Utc);
        session.InvalidationReason = "manual_override";

        var result = _sut.IsInContext(session, CreateRequest());

        Assert.False(result.IsInContext);
        Assert.Equal(SessionContextFailureReason.SessionInvalidated, result.FailureReason);
    }

    [Fact]
    public void IsInContext_returns_appointment_not_active_when_appointment_status_is_not_active()
    {
        var session = CreateValidSession();
        session.Appointment.Status = "scheduled";

        var result = _sut.IsInContext(session, CreateRequest());

        Assert.False(result.IsInContext);
        Assert.Equal(SessionContextFailureReason.AppointmentNotActive, result.FailureReason);
    }

    [Fact]
    public void IsInContext_returns_user_not_assigned_when_requester_does_not_own_the_session()
    {
        var session = CreateValidSession();
        var request = CreateRequest(requestedByUserId: 404);

        var result = _sut.IsInContext(session, request);

        Assert.False(result.IsInContext);
        Assert.Equal(SessionContextFailureReason.UserNotAssigned, result.FailureReason);
    }

    [Fact]
    public void IsInContext_returns_operation_type_mismatch_when_request_operation_differs()
    {
        var session = CreateValidSession();
        var request = CreateRequest(operationTypeId: 999);

        var result = _sut.IsInContext(session, request);

        Assert.False(result.IsInContext);
        Assert.Equal(SessionContextFailureReason.OperationTypeMismatch, result.FailureReason);
    }

    [Fact]
    public void IsInContext_returns_target_mismatch_when_request_target_is_outside_session_context()
    {
        var session = CreateValidSession();
        var request = CreateRequest(targetIdentifier: "CIT-9999");

        var result = _sut.IsInContext(session, request);

        Assert.False(result.IsInContext);
        Assert.Equal(SessionContextFailureReason.TargetMismatch, result.FailureReason);
    }

    [Fact]
    public void IsInContext_uses_current_assignment_history_when_current_assigned_user_is_not_set()
    {
        var session = CreateValidSession();
        session.CurrentAssignedUserId = null;

        var result = _sut.IsInContext(session, CreateRequest());

        Assert.True(result.IsInContext);
        Assert.Equal(SessionContextFailureReason.None, result.FailureReason);
    }

    private static SessionContextRequest CreateRequest(
        long sessionId = 100,
        long requestedByUserId = 42,
        long operationTypeId = 7,
        string targetType = "citizen",
        string targetIdentifier = "CIT-1001",
        DateTime? requestedAtUtc = null) =>
        new(
            sessionId,
            requestedByUserId,
            operationTypeId,
            targetType,
            targetIdentifier,
            requestedAtUtc ?? new DateTime(2026, 3, 27, 10, 15, 0, DateTimeKind.Utc));

    private static Session CreateValidSession() =>
        new()
        {
            Id = 100,
            AppointmentId = 200,
            CurrentAssignedUserId = 42,
            Status = "active",
            ActivatedAt = new DateTime(2026, 3, 27, 10, 0, 0, DateTimeKind.Utc),
            ExpiresAt = new DateTime(2026, 3, 27, 10, 30, 0, DateTimeKind.Utc),
            LastActivityAt = new DateTime(2026, 3, 27, 10, 5, 0, DateTimeKind.Utc),
            Appointment = new Appointment
            {
                Id = 200,
                OperationTypeId = 7,
                CitizenIdentifier = "CIT-1001",
                ScheduledStartAt = new DateTime(2026, 3, 27, 10, 0, 0, DateTimeKind.Utc),
                ScheduledEndAt = new DateTime(2026, 3, 27, 10, 30, 0, DateTimeKind.Utc),
                Status = "active",
                Targets =
                [
                    new AppointmentTarget
                    {
                        Id = 1,
                        AppointmentId = 200,
                        TargetType = "citizen",
                        TargetIdentifier = "CIT-1001",
                        IsPrimary = true,
                        CreatedAt = new DateTime(2026, 3, 27, 9, 55, 0, DateTimeKind.Utc)
                    }
                ]
            },
            Assignments =
            [
                new SessionAssignment
                {
                    Id = 1,
                    SessionId = 100,
                    UserId = 42,
                    AssignmentType = "primary",
                    AssignedAt = new DateTime(2026, 3, 27, 10, 0, 0, DateTimeKind.Utc),
                    IsCurrent = true
                }
            ]
        };
}
