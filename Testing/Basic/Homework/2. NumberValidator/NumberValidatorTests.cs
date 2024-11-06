using System.Collections;
using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, 2, true, TestName = "WithNegativePrecision")]
    [TestCase(0, 2, false, TestName = "WithPrecisionEqualsZero")]
    [TestCase(1, -2, true, TestName = "WithNegativeScale")]
    [TestCase(1, 2, false, TestName = "WithScaleGreaterThanPrecision")]
    [TestCase(1, 1, true, TestName = "WithScaleEqualsPrecision")]
    public void NumberValidation_ConstructorThrowsArgumentException(int precision, int scale, bool onlyPositive)
    {
        var action = () => new NumberValidator(precision, scale, onlyPositive);

        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void NumberValidation_WhenCorrectParameters_NotThrowsException()
    {
        var action = () => new NumberValidator(1, 0, true);

        action.Should().NotThrow();
    }

    [TestCaseSource(nameof(ValidNumbersCases))]
    [TestCaseSource(nameof(InvalidNumbersCases))]
    [TestCaseSource(nameof(InvalidInputCases))]
    public bool IsValidNumber_WorksCorrectly(int precision, int scale, bool onlyPositive, string value)
    {
        return new NumberValidator(precision, scale, onlyPositive).IsValidNumber(value);
    }

    public static IEnumerable ValidNumbersCases
    {
        get
        {
            yield return new TestCaseData(1, 0, true, "0").SetName("ForZero_WithCorrectParams_ReturnTrue").Returns(true);
            yield return new TestCaseData(2, 1, true, "0.0").SetName("ForZero_WithZeroFractionalPartAndDot_ReturnTrueOnCorrectParams").Returns(true);
            yield return new TestCaseData(2, 1, true, "0.1").SetName("ForZero_WithNonZeroFractionalPartAndDot_ReturnTrueOnCorrectParams").Returns(true);
            yield return new TestCaseData(2, 1, true, "0,1").SetName("ForZero_WithNonZeroDecimalPartAndComma_ReturnTrueOnCorrectParams").Returns(true);
            yield return new TestCaseData(2, 0, true, "+1").SetName("ForPositiveInteger_WithSign_ReturnTrueOnCorrectParams").Returns(true);
            yield return new TestCaseData(3, 1, true, "+1.1").SetName("ForPositiveDecimal_WithDotAndSign_ReturnTrueOnCorrectParams").Returns(true);
            yield return new TestCaseData(3, 1, true, "+1,1").SetName("ForPositiveDecimal_WithCommaAndSign_ReturnTrueOnCorrectParams").Returns(true);
            yield return new TestCaseData(2, 0, false, "-1").SetName("ForNegativeInteger_WithSign_ReturnTrueOnCorrectParams").Returns(true);
            yield return new TestCaseData(3, 1, false, "-1.1").SetName("ForNegativeDecimal_WithDotAndSign_ReturnTrueOnCorrectParams").Returns(true);
            yield return new TestCaseData(3, 1, false, "-1,1").SetName("ForNegativeDecimal_WithCommaAndSign_ReturnTrueOnCorrectParams").Returns(true);
            yield return new TestCaseData(7, 2, true, "671,23").SetName("ForRealDecimal_WithCorrectParameters_ReturnTrue").Returns(true);
        }
    }

    public static IEnumerable InvalidNumbersCases
    {
        get
        {
            yield return new TestCaseData(2, 0, true, "-1").SetName("ForNegativeInteger_WhenOnlyPositiveConfigured_ReturnFalse").Returns(false);
            yield return new TestCaseData(10, 5, true, "-23.15").SetName("ForNegativeDecimal_WhenOnlyPositiveConfigured_ReturnFalse").Returns(false);
            yield return new TestCaseData(3, 2, true, "4321.5").SetName("ForDecimal_WithIncorrectPrecision_ReturnFalse").Returns(false);
            yield return new TestCaseData(5, 0, true, "132568").SetName("ForInteger_WithIncorrectPrecision_ReturnFalse").Returns(false);
            yield return new TestCaseData(5, 2, true, "0.123").SetName("ForPositiveDecimal_WithIncorrectScale_ReturnFalse").Returns(false);
            yield return new TestCaseData(7, 2, false, "-0.432").SetName("ForNegativeDecimal_WithIncorrectScale_ReturnFalse").Returns(false);
            yield return new TestCaseData(3, 0, true, "+145").SetName("ForPositiveInteger_WithSign_PrecisionCalculationAccountPlusSign").Returns(false);
            yield return new TestCaseData(3, 1, true, "+645.4").SetName("ForPositiveDecimal_WithSign_PrecisionCalculationAccountPlusSign").Returns(false);
            yield return new TestCaseData(3, 0, false, "-124").SetName("ForNegativeInteger_WithSign_PrecisionCalculationAccountMinusSign").Returns(false);
            yield return new TestCaseData(3, 1, false, "-325.7").SetName("ForNegativeDecimal_WithSign_PrecisionCalculationAccountMinusSign").Returns(false);
        }
    }

    public static IEnumerable InvalidInputCases
    {
        get
        {
            yield return new TestCaseData(10, 9, true, "127.0.0.1").SetName("ForNumber_WithNonRealNumberFormat_ReturnFalse").Returns(false);
            yield return new TestCaseData(2, 1, true, ".0").SetName("ForNumber_WithMissingIntegerPart_ReturnFalse").Returns(false);
            yield return new TestCaseData(2, 0, true, "0.").SetName("ForNumber_WithMissingFractionalPart_ReturnFalse").Returns(false);
            yield return new TestCaseData(10, 2, true, " abcde.f").SetName("ForNotNumberInput_ReturnFalse").Returns(false);

        }
    }
}