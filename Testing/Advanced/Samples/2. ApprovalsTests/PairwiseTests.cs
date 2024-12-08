using ApprovalTests;
using ApprovalTests.Combinations;
using ApprovalTests.Reporters;
using NUnit.Framework;

namespace Advanced.Samples.ApprovalsTests;

[TestFixture]
[Explicit]
internal class PairwiseTests
{
    [Test, Combinatorial]
    public void CombinatorialConsole(
        [Values("a", "b", "c")] string a,
        [Values("+", "-")] string b,
        [Values("x", "y")] string c)
    {
        Console.WriteLine("{0} {1} {2}", a, b, c);
    }

    [Test, Pairwise]
    public void PairwiseConsole(
        [Values("a", "b", "c")] string a,
        [Values("+", "-")] string b,
        [Values("x", "y")] string c)
    {
        Console.WriteLine("{0} {1} {2}", a, b, c);
    }

    [Test, Pairwise]
    public void PairwiseApprovals(
        [Values("a", "b", "c")] string a,
        [Values("+", "-")] string b,
        [Values("x", "y")] string c)
    {
        Approvals.Verify($"{a} {b} {c}");
    }


    [Test]
    [UseReporter(typeof(DiffReporter))]
    public void CombinatorialApprovals()
    {
        CombinationApprovals.VerifyAllCombinations(
            (a, b) => a + b,
            new[] { 1, 2, 3 },
            new[] { 0, -1, -5 });
    }
}