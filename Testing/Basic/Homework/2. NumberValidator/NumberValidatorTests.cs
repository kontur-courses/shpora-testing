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

    [TestCaseSource(nameof(ValidNumbersCases))]
    [TestCaseSource(nameof(InalidNumberCases))]
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

    private static IEnumerable ValidNumbersCases
    {
        get
        {
            yield return new TestCaseData("0.0", 15, 2, true, true).SetName("ForDecimal_ReturnTrue");
            yield return new TestCaseData("0", 15, 2, true, true).SetName("ForInteger_ReturnTrue");
            yield return new TestCaseData("1,23", 4, 2, true, true).SetName("ForAnotherDecimalSeparator_ReturnTrue");
            yield return new TestCaseData("+1.23", 4, 2, true, true).SetName("ForPositiveNumberWithSignValidLenght_ReturnTrue");
            yield return new TestCaseData("-12.23", 10, 2, false, true).SetName("ForNegativeNumberNotOnlyPositive_ReturnTrue");
        }
    }

    private static IEnumerable InalidNumberCases
    {
        get
        {
            yield return new TestCaseData("00.00", 3, 2, true, false).SetName("ForLeadingZeros_ReturnFalse");
            yield return new TestCaseData("-0.00", 3, 2, true, false).SetName("ForNegativeZero_ReturnFalse");
            yield return new TestCaseData("+0.00", 3, 2, true, false).SetName("ForPositiveZeroWithSignInvalidLength_ReturnFalse");
            yield return new TestCaseData("+1.23", 3, 2, true, false).SetName("ForPositiveNumberWithSignInvalidLength_ReturnFalse");
            yield return new TestCaseData("0.000", 15, 2, true, false).SetName("ForMoreFractionalDigitsThenExpected_ReturnFalse");
            yield return new TestCaseData("-1.23", 3, 2, false, false).SetName("ForNegativeNumberInvalidLength_ReturnFalse");
            yield return new TestCaseData("-1.23", 4, 2, true, false).SetName("ForNegativeNumberOnlyPositive_ReturnFalse");
            yield return new TestCaseData("a.sd", 3, 2, true, false).SetName("ForNonnumericInput_ReturnFalse");
            yield return new TestCaseData("", 2, 1, true, false).SetName("ForEmptyInput_ReturnFalse");
            yield return new TestCaseData(".123", 4, 2, true, false).SetName("ForMissingLeadingZero_ReturnFalse");
            yield return new TestCaseData("1.", 4, 2, true, false).SetName("ForMissingTrailingDigits_ReturnFalse");
            yield return new TestCaseData("999.99", 4, 2, true, false).SetName("ForExceedingTotalDigits_ReturnFalse");
        }
    }
}
