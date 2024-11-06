using System.Collections;
using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, 2, true, TestName = "NumberValidator_WithNegativePrecision_ThrowsException")]
    [TestCase(0, 2, false, TestName = "NumberValidator_WithPrecisionEqualsZero_ThrowsException")]
    [TestCase(1, -2, true, TestName = "NumberValidator_WithNegativeScale_ThrowsException")]
    [TestCase(1, 2, false, TestName = "NumberValidator_WithScaleGreaterThanPrecision_ThrowsException")]
    [TestCase(1, 1, true, TestName = "NumberValidator_WithScaleEqualsPrecision_ThrowsException")]
    public void NumberValidation_ConstructorThrowsArgumentException(int precision, int scale, bool onlyPositive)
    {
        var action = () => new NumberValidator(precision, scale, onlyPositive);

        action.Should().Throw<ArgumentException>();
    }

    [Test(Description = "NumberValidator_WithCorrectParameters_NotThrowException")]
    public void NumberValidation_ConstructorDoesNotHaveArgumentException()
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
            yield return new TestCaseData(2, 0, true, "-1").SetName("ForNegativeInteger_WithMinusSign_OnlyPositiveIsFalse").Returns(false);
            yield return new TestCaseData(3, 2, true, "4321.5").SetName("ForDecimal_WithIncorrectPrecision_ReturnFalse").Returns(false);
            yield return new TestCaseData(3, 0, true, "+145").SetName("ForPositiveDecimal_WithPlusSign_PrecisionCalculationAccountPlusSign").Returns(false);
            yield return new TestCaseData(3, 0, false, "-124").SetName("ForNegativeDecimal_WithMinusSign_PrecisionCalculationAccountMinusSign").Returns(false);
        }
    }

    public static IEnumerable InvalidInputCases
    {
        get
        {
            yield return new TestCaseData(10, 9, true, "127.0.0.1").SetName("ForNumber_WithNonRealNumberFormat_ReturnFalse").Returns(false);
            yield return new TestCaseData(2, 0, true, "").SetName("ForNumber_IsEmptyString_ReturnFalse").Returns(false);
            yield return new TestCaseData(2, 1, true, ".0").SetName("ForNumber_WithMissingIntegerPart_ReturnFalse").Returns(false);
            yield return new TestCaseData(2, 0, true, "0.").SetName("ForNumber_WithMissingFractionalPart_ReturnFalse").Returns(false);
            yield return new TestCaseData(10, 2, true, "abcde.f").SetName("ForNumber_WithNonDigitCharacters_ReturnFalse").Returns(false);

        }
    }
}