using Xunit.Abstractions;
using Xunit.Sdk;

namespace IcedMango.DifyAi.IntegrationTests.Orderers;

/// <summary>
/// Custom test case orderer that orders tests by their TestPriority attribute.
/// Tests without the attribute default to priority int.MaxValue (run last).
/// </summary>
public class PriorityOrderer : ITestCaseOrderer
{
    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases)
        where TTestCase : ITestCase
    {
        var sortedMethods = new SortedDictionary<int, List<TTestCase>>();

        foreach (var testCase in testCases)
        {
            var priority = GetPriority(testCase);

            if (!sortedMethods.TryGetValue(priority, out var list))
            {
                list = new List<TTestCase>();
                sortedMethods.Add(priority, list);
            }

            list.Add(testCase);
        }

        foreach (var list in sortedMethods.Values)
        {
            // Sort by method name within the same priority for consistent ordering
            list.Sort((x, y) => StringComparer.OrdinalIgnoreCase.Compare(
                x.TestMethod.Method.Name,
                y.TestMethod.Method.Name));

            foreach (var testCase in list)
            {
                yield return testCase;
            }
        }
    }

    private static int GetPriority<TTestCase>(TTestCase testCase) where TTestCase : ITestCase
    {
        var attributeInfo = testCase.TestMethod.Method
            .GetCustomAttributes(typeof(TestPriorityAttribute).AssemblyQualifiedName)
            .FirstOrDefault();

        if (attributeInfo == null)
        {
            return int.MaxValue; // No priority attribute, run last
        }

        // Get the constructor argument (the priority value)
        var args = attributeInfo.GetConstructorArguments().ToArray();
        return args.Length > 0 && args[0] is int priority ? priority : int.MaxValue;
    }
}
