using ApprovalSchemeProcess.Domain.Entities;

namespace ApprovalSchemeProcess.Application.Sessions;

public interface ISessionContextEvaluator
{
    SessionContextEvaluation IsInContext(Session? session, SessionContextRequest request);
}
