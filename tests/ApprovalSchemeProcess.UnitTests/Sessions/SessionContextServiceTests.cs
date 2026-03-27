using ApprovalSchemeProcess.Application.Sessions;
using ApprovalSchemeProcess.Domain.Entities;

namespace ApprovalSchemeProcess.UnitTests.Sessions;

public class SessionContextServiceTests
{
    [Fact]
    public async Task IsInContextAsync_loads_session_from_reader_and_returns_allowed_result()
    {
        var session = new Session
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
            }
        };

        var reader = new FakeSessionContextSessionReader(session);
        var service = new SessionContextService(reader, new SessionContextEvaluator());
        var request = new SessionContextRequest(
            100,
            42,
            7,
            "citizen",
            "CIT-1001",
            new DateTime(2026, 3, 27, 10, 15, 0, DateTimeKind.Utc));

        var result = await service.IsInContextAsync(request);

        Assert.True(result.IsInContext);
        Assert.Equal(SessionContextFailureReason.None, result.FailureReason);
        Assert.Equal(100, reader.LastRequestedSessionId);
    }

    [Fact]
    public async Task IsInContextAsync_returns_session_not_found_when_reader_has_no_matching_session()
    {
        var reader = new FakeSessionContextSessionReader(null);
        var service = new SessionContextService(reader, new SessionContextEvaluator());
        var request = new SessionContextRequest(
            999,
            42,
            7,
            "citizen",
            "CIT-1001",
            new DateTime(2026, 3, 27, 10, 15, 0, DateTimeKind.Utc));

        var result = await service.IsInContextAsync(request);

        Assert.False(result.IsInContext);
        Assert.Equal(SessionContextFailureReason.SessionNotFound, result.FailureReason);
        Assert.Equal(999, reader.LastRequestedSessionId);
    }

    private sealed class FakeSessionContextSessionReader(Session? session) : ISessionContextSessionReader
    {
        public long? LastRequestedSessionId { get; private set; }

        public Task<Session?> GetByIdAsync(long sessionId, CancellationToken cancellationToken = default)
        {
            LastRequestedSessionId = sessionId;
            return Task.FromResult(session);
        }
    }
}
