namespace Api.ComponentTests.Setup;

public class IntegrationTestAttribute : FactAttribute
{
    public IntegrationTestAttribute()
    {
        var envVarValue = Environment.GetEnvironmentVariable("INTEGRATIONTESTS") ?? "";
        if (!envVarValue.Equals("true", StringComparison.OrdinalIgnoreCase))
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            Skip = "Component tests should only run in docker-compose, by setting COMPONENTTESTS to true";
        }
    }
}
