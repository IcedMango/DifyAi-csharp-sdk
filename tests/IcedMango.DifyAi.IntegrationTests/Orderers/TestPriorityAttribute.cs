namespace IcedMango.DifyAi.IntegrationTests.Orderers;

/// <summary>
/// Attribute to specify test execution priority.
/// Lower values execute first.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class TestPriorityAttribute : Attribute
{
    public int Priority { get; }

    public TestPriorityAttribute(int priority)
    {
        Priority = priority;
    }
}
