using NetArchTest.Rules;

namespace ApprovalSchemeProcess.ArchitectureTests.Layers;

public class DependencyRuleTests
{
    [Fact]
    public void Domain_should_not_depend_on_outer_layers()
    {
        var result = Types.InAssembly(typeof(ApprovalSchemeProcess.Domain.AssemblyReference).Assembly)
            .ShouldNot()
            .HaveDependencyOnAny(
                typeof(ApprovalSchemeProcess.Application.AssemblyReference).Assembly.GetName().Name!,
                typeof(ApprovalSchemeProcess.Infrastructure.AssemblyReference).Assembly.GetName().Name!,
                "ApprovalSchemeProcess.Api")
            .GetResult();

        Assert.True(result.IsSuccessful, BuildFailureMessage(result.FailingTypeNames));
    }

    [Fact]
    public void Application_should_not_depend_on_infrastructure_or_api()
    {
        var result = Types.InAssembly(typeof(ApprovalSchemeProcess.Application.AssemblyReference).Assembly)
            .ShouldNot()
            .HaveDependencyOnAny(
                typeof(ApprovalSchemeProcess.Infrastructure.AssemblyReference).Assembly.GetName().Name!,
                "ApprovalSchemeProcess.Api")
            .GetResult();

        Assert.True(result.IsSuccessful, BuildFailureMessage(result.FailingTypeNames));
    }

    private static string BuildFailureMessage(IReadOnlyCollection<string>? failingTypeNames)
    {
        if (failingTypeNames is null || failingTypeNames.Count == 0)
        {
            return "Architecture rule failed without reported violating types.";
        }

        return string.Join(Environment.NewLine, failingTypeNames);
    }
}
