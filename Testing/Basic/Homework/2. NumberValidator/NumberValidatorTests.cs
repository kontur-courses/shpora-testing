using NUnit.Framework;
using FluentAssertions;
using System.Collections;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-2, 2, true, TestName = "WhenPrecisionIsNegativeOnlyPositive")]
    [TestCase(-2, 2, false, TestName = "WhenPrecisionIsNegative")]
    [TestCase(1, 2, true, TestName = "WhenScaleGreaterThanPrecisionOnlyPositive")]
    [TestCase(1, 2, false, TestName = "WhenScaleGreaterThanPrecision")]
    [TestCase(2, -1, true, TestName = "WhenScaleIsLessThatZeroOnlyPositive")]
    [TestCase(2, -1, false, TestName = "WhenScaleIsLessThatZero")]
    [TestCase(2, 2, true, TestName = "WhenScaleEqualPrecisionOnlyPositive")]
    [TestCase(2, 2, false, TestName = "WhenScaleEqualPrecision")]

    public void Constructor_ShouldThrowArgumentException(int precision, int scale, bool onlyPositive)
    {
        var act = () => new NumberValidator(precision, scale, onlyPositive);

        act.Should().Throw<ArgumentException>();
    }

    [TestCaseSource(nameof(ValidNumbersCases_ReturnTrue))]
    [TestCaseSource(nameof(InalidNumberCases_ReturnFalse))]
    public void IsValidNumber_ShouldValidateCorrectly(
        string input,
        int precision,
        int scale,
        bool onlyPositive,
        bool expectedResult)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        
        var result = validator.IsValidNumber(input);
        
        result.Should().Be(expectedResult);
    }

    private static IEnumerable ValidNumbersCases_ReturnTrue
    {
        get
        {
            yield return new TestCaseData("0.0", 15, 2, true, true).SetName("Decimal");
            yield return new TestCaseData("0", 15, 2, true, true).SetName("Integer");
            yield return new TestCaseData("1,23", 4, 2, true, true).SetName("AnotherDecimalSeparator");
            yield return new TestCaseData("+1.23", 4, 2, true, true).SetName("PositiveNumberWithSignValidLenght");
        }
    }

    private static IEnumerable InalidNumberCases_ReturnFalse
    {
        get
        {
            yield return new TestCaseData("00.00", 3, 2, true, false).SetName("LeadingZeros");
            yield return new TestCaseData("-0.00", 3, 2, true, false).SetName("NegativeZero");
            yield return new TestCaseData("+0.00", 3, 2, true, false).SetName("PositiveZeroWithSignInvalidLength");
            yield return new TestCaseData("+1.23", 3, 2, true, false).SetName("PositiveNumberWithSignInvalidLength");
            yield return new TestCaseData("0.000", 15, 2, true, false).SetName("MoreFractionalDigitsThenExpected");
            yield return new TestCaseData("-1.23", 3, 2, false, false).SetName("NegativeNumberInvalidLength");
            yield return new TestCaseData("-1.23", 4, 2, true, false).SetName("NegativeNumberNotAllowed");
            yield return new TestCaseData("a.sd", 3, 2, true, false).SetName("NonnumericInput");
            yield return new TestCaseData("", 2, 1, true, false).SetName("EmptyInput");
            yield return new TestCaseData(".123", 4, 2, true, false).SetName("MissingLeadingZero");
            yield return new TestCaseData("1.", 4, 2, true, false).SetName("MissingTrailingDigits");
            yield return new TestCaseData("999.99", 4, 2, true, false).SetName("ExceedingTotalDigits");
        }
    }
}
