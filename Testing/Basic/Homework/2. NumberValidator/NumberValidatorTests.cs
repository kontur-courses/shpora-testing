using FluentAssertions;
using NUnit.Framework;
using System.Collections;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorShould
{
    [TestCaseSource(nameof(InvalidPrecisionAndScaleData))]
    public void ThrowArgumentException_AfterCreatingWith(int precision, int scale)
    {
        var createNumberValidator = () => new NumberValidator(precision, scale);

        createNumberValidator.Should().Throw<ArgumentException>();
    }

    private static IEnumerable InvalidPrecisionAndScaleData
    {
        get
        {
            yield return new TestCaseData(-1, 0);
            yield return new TestCaseData(0, 0);
            yield return new TestCaseData(1, -2);
            yield return new TestCaseData(1, 2);
            yield return new TestCaseData(1, 1);
        }
    }

    [TestCaseSource(nameof(ValidPrecisionAndScaleData))]
    public void NotThrowArgumentException_AfterCreating(int precision, int scale)
    {
        var createNumberValidator = () => new NumberValidator(precision, scale);

        createNumberValidator.Should().NotThrow<ArgumentException>();
    }

    private static IEnumerable ValidPrecisionAndScaleData
    {
        get
        {
            yield return new TestCaseData(3, 2);
            yield return new TestCaseData(int.MaxValue, int.MaxValue - 1);
        }
    }

    [TestCaseSource(nameof(IsValidTestCases))]
    public bool IsValid_ReturnResult_AfterExecuting(int precision, int scale, bool onlyPositive, string str)
    {
        return new NumberValidator(precision, scale, onlyPositive).IsValidNumber(str);
    }

    public static IEnumerable IsValidTestCases
    {
        get
        {
            yield return new TestCaseData(3, 2, true, "00.00").Returns(false);
            yield return new TestCaseData(17, 2, true, "0.0a").Returns(false);
            yield return new TestCaseData(17, 2, true, "a0.0").Returns(false);
            yield return new TestCaseData(3, 2, true, "-0.00").Returns(false);
            yield return new TestCaseData(3, 2, true, "+0.00").Returns(false);
            yield return new TestCaseData(3, 2, true, "+1.23").Returns(false);
            yield return new TestCaseData(3, 2, true, "-1.23").Returns(false);
            yield return new TestCaseData(3, 2, true, "-1.2").Returns(false);
            yield return new TestCaseData(3, 2, true, "a.sd").Returns(false);
            yield return new TestCaseData(17, 2, true, "0.000").Returns(false);
            yield return new TestCaseData(17, 2, true, "").Returns(false);
            yield return new TestCaseData(17, 2, true, null).Returns(false);
            yield return new TestCaseData(17, 2, true, "0.0").Returns(true);
            yield return new TestCaseData(17, 2, true, "11").Returns(true);
            yield return new TestCaseData(17, 2, true, "0,0").Returns(true);
            yield return new TestCaseData(17, 2, true, "0").Returns(true);
            yield return new TestCaseData(4, 2, true, "+1.23").Returns(true);
            yield return new TestCaseData(4, 2, false, "-1.23").Returns(true);
        }
    }
}