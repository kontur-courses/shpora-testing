using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCaseSource(nameof(GetCaseValidators))]
    public bool NumberValidator_CheckValidator(NumberValidator validator, string number)
    {
        return validator.IsValidNumber(number);
    }

    [TestCase(0, 2, true, TestName = "PrecisionShouldBeMoreThan 0")]
    [TestCase(3, -1, false, TestName = "ScaleShouldBeMoreThan -1")]
    [TestCase(3, 5, false, TestName = "ScaleShouldBeMoreThanPrecision")]
    public void NumberValidator_CheckArgumentException(int precision, int scale, bool onlyPositive)
    {
        Action action = () => new NumberValidator(precision, scale, onlyPositive);
        action.Should().Throw<ArgumentException>();
    }

    [TestCase(1, 0, true)]
    public void NumberValidator_CheckDoesNotThrow(int precision, int scale, bool onlyPositive)
    {
        Action action = () => new NumberValidator(precision, scale, onlyPositive);
        action.Should().NotThrow();
    }

    private static IEnumerable<TestCaseData> GetCaseValidators()
    {
        yield return new TestCaseData(new NumberValidator(17, 2, true), "0").Returns(true).SetName("zero");
        yield return new TestCaseData(new NumberValidator(3, 2, true), "00.00").Returns(false)
            .SetName("PrecisionShouldBeMoreThanCountOfNumber");
        yield return new TestCaseData(new NumberValidator(3, 2, true), "-0.00").Returns(false)
            .SetName("NumberShouldBePositive");
        yield return new TestCaseData(new NumberValidator(3, 2, true), "+0.00").Returns(false)
            .SetName("ShouldBeWithoutSign");
        yield return new TestCaseData(new NumberValidator(4, 2, true), "+1.23").Returns(true).SetName("WithSignPlus");
        yield return new TestCaseData(new NumberValidator(17, 2, true), "0.000").Returns(false)
            .SetName("ScaleShouldBeMoreThanCountOfNumberAfterDot");
        yield return new TestCaseData(new NumberValidator(3, 2, true), "a.sd").Returns(false)
            .SetName("ShouldBeWithoutLetters");
        // Новые тесты
        yield return new TestCaseData(new NumberValidator(4, 2), "-1.23").Returns(true).SetName("NegativeNumber");
        yield return new TestCaseData(new NumberValidator(1), "-").Returns(false).SetName("onlySign");
        yield return new TestCaseData(new NumberValidator(2, 0, true), "").Returns(false).SetName("EmptyNumber");
        yield return new TestCaseData(new NumberValidator(4, 1), "+-13.0").Returns(false).SetName("TwoSign");
        yield return new TestCaseData(new NumberValidator(9, 5, true), "+133.00001").Returns(true).SetName("");
        yield return new TestCaseData(new NumberValidator(3, 1, true), "+1,0").Returns(true)
            .SetName("WithDecimalPoint");
        yield return new TestCaseData(new NumberValidator(3, 1, true), "+,1").Returns(false)
            .SetName("WithoutIntegerPartWithSign");
        yield return new TestCaseData(new NumberValidator(3, 1, true), ",1").Returns(false)
            .SetName("WithoutIntegerAndSign");
        yield return new TestCaseData(new NumberValidator(3, 1, true), ".").Returns(false).SetName("OnlyDot");
    }
}