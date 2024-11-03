using FluentAssertions;
using NUnit.Framework;
using System.Collections;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorShould
{
    [Test]
    public void ThrowArgumentException_AfterCreatingWithNegativePrecision()
    {
        var createNvWithNegativePrecision = () => new NumberValidator(-1);

        createNvWithNegativePrecision.Should().Throw<ArgumentException>();
    }

    [Test]
    public void ThrowArgumentException_AfterCreatingWithPrecisionEqualsToZero()
    {
        var createNvWithPrecisionEqualsToZero = () => new NumberValidator(0);

        createNvWithPrecisionEqualsToZero.Should().Throw<ArgumentException>();
    }

    [Test]
    public void ThrowArgumentException_AfterCreatingWithNegativeScale()
    {
        var createNvWithNegativeScale = () => new NumberValidator(1, -2);

        createNvWithNegativeScale.Should().Throw<ArgumentException>();
    }

    [Test]
    public void ThrowArgumentException_AfterCreatingWithScaleMoreThanPrecision()
    {
        var createNvWithScaleMoreThanPrecision = () => new NumberValidator(1, 2);

        createNvWithScaleMoreThanPrecision.Should().Throw<ArgumentException>();
    }

    [Test]
    public void ThrowArgumentException_AfterCreatingWithScaleEqualsToPrecision()
    {
        var createNvWithScaleEqualsToPrecision = () => new NumberValidator(1, 1);

        createNvWithScaleEqualsToPrecision.Should().Throw<ArgumentException>();
    }

    [Test]
    public void NotThrowArgumentException_AfterCreatingWithPositivePrecision()
    {
        var createNvWithPositivePrecision = () => new NumberValidator(1);

        createNvWithPositivePrecision.Should().NotThrow<ArgumentException>();
    }

    [Test, TestCaseSource(nameof(ValidTestCases))]
    public bool ReturnTrue_AfterCheckingValid(int precision, int scale, bool onlyPositive, string str)
    {
        return new NumberValidator(precision, scale, onlyPositive).IsValidNumber(str);
    }

    public static IEnumerable ValidTestCases
    {
        get
        {
            yield return new TestCaseData(17, 2, true, "0.0").Returns(true);
            yield return new TestCaseData(17, 2, true, "11").Returns(true);
            yield return new TestCaseData(17, 2, true, "0,0").Returns(true);
            yield return new TestCaseData(17, 2, true, "0").Returns(true);
            yield return new TestCaseData(4, 2, true, "+1.23").Returns(true);
            yield return new TestCaseData(4, 2, false, "-1.23").Returns(true);
        }
    }

    [Test, TestCaseSource(nameof(NotValidTestCases))]
    public bool ReturnFalse_AfterCheckingValid(int precision, int scale, bool onlyPositive, string str)
    {
        return new NumberValidator(precision, scale, onlyPositive).IsValidNumber(str);
    }

    public static IEnumerable NotValidTestCases
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
        }
    }
}