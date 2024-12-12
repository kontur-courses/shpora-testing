
using NUnit.Framework;
using FluentAssertions;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(0,0, TestName = "PrecisionIsZero")]
    [TestCase(-1, 0, TestName = "PrecisionIsNegative")]
    [TestCase(1, -1, TestName = "ScaleIsNegative")]
    [TestCase(5, 5, TestName = "ScaleEqualsPrecision")]
    [TestCase(1, 2, TestName = "ScaleIsGreaterThanPrecision")]
    public void Constructor_ThrowException_When(int precision, int scale)
    {
var createValidator = () => new NumberValidator(precision, scale);

createValidator.Should().Throw<ArgumentException>();
    }

    [TestCase(1, 0, TestName = "PrecisionIsGreaterThanScaleAndScaleIsZero")]
    [TestCase(5, 2, TestName = "PrecisionGreaterThanScaleAndScaleGreaterThanZero")]
    public void Constructor_NotThrowException_When(int precision, int scale)
    {
        var createValidator = () => new NumberValidator(precision, scale);

        createValidator.Should().NotThrow();
    }

    [TestCase(1, 0, false, "0", true, TestName = "IntegerWithoutSignAndLengthEqualsPrecision")]
    [TestCase(5, 0, false, "40", true, TestName = "IntegerWithoutSignAndLengthLessThanPrecision")]
    [TestCase(2, 0, false, "+0", true, TestName = "PositiveZeroWithSign")]
    [TestCase(2, 0, false, "-0", true, TestName = "NegativeZeroWithSign")]
    [TestCase(3, 0, false, "+21", true, TestName = "IntegerWithSignAndLengthEqualsPrecision")]
    [TestCase(5, 0, false, "-40", true, TestName = "IntegerWithSignAndLengthLessThanPrecision")]
    [TestCase(3, 1, false, "0", true, TestName = "IntegerWithoutSignAndScaleIsGreaterThanZero")]
    [TestCase(5, 2, false, "+21", true, TestName = "IntegerWithSignAndScaleIsGreaterThanZero")]
    [TestCase(5, 2, false,"123.45", true, TestName = "DecimalWithoutSignAndFractionalPartLengthEqualsScale")]
    [TestCase(7, 4, false, "537.23", true, TestName = "DecimalWithoutSignAndFractionalPartLengthLessThanScale")]
    [TestCase(3, 2, false, "0.0", true, TestName = "DecimalZeroWithinPrecisionAndScale")]
    [TestCase(6, 2, false, "+237.89", true, TestName = "DecimalWithSignAndFractionalPartLengthEqualsScale")]
    [TestCase(7, 4, false, "-445.12", true, TestName = "DecimalWithoutSignAndFractionalPartLengthLessThanScale")]
    [TestCase(5, 2, false, "371,47", true, TestName = "DecimalWithoutSignWithCommaSeparator")]
    [TestCase(6, 2, false, "+111,11", true, TestName = "DecimalWithSignWithCommaSeparator")]
    [TestCase(1, 0, true, "11", false, TestName = "IntegerWithoutSignExceedsPrecision")]
    [TestCase(3, 1, true, "112.3", false, TestName = "DecimalWithoutSignExceedsPrecision")]
    [TestCase(2, 0, true, "+42", false, TestName = "IntegerWithSignExceedsPrecision")]
    [TestCase(4, 2, true, "+42.75", false, TestName = "DecimalWithSignExceedsPrecision")]
    [TestCase(2, 0, true, "5.0", false, TestName = "DecimalProvidedAndScaleIsZero")]
    [TestCase(5, 0, true, "1.23", false, TestName = "DecimalProvidedAndScaleIsZero")]
    [TestCase(3, 1, true, "5.67", false, TestName = "DecimalExceedsScale")]
    [TestCase(7, 3, true, "-0", false, TestName = "NegativeZeroAndIsOnlyPositiveIsTrue")]
    [TestCase(7, 3, true, "-10", false, TestName = "NegativeIntegerAndIsOnlyPositiveIsTrue")]
    [TestCase(7, 3, true, "-1.23", false, TestName = "NegativeDecimalAndIsOnlyPositiveIsTrue")]
    [TestCase(5, 2, false, null, false, TestName = "ValueIsNull")]
    [TestCase(5, 2, false, "", false, TestName = "ValueIsEmpty")]
    [TestCase(5, 2, false, "    ", false, TestName = "ValueIsWhitespace")]
    [TestCase(5, 2, false, "\t", false, TestName = "ValueIsEscapeSequences")]
    [TestCase(5, 2, false, "a.sd", false, TestName = "ValueContainsLetters")]
    [TestCase(5, 2, false, "10O", false, TestName = "ValueContainsLetters")]
    [TestCase(7, 2, false, "12_34", false, TestName = "ValueContainsIncorrectSeparator")]
    [TestCase(5, 0, false, "871.", false, TestName = "DecimalWithoutFractionAndScaleZero")]
    [TestCase(5, 2, false, "636.", false, TestName = "DecimalWithoutFractionAndScaleIsPositive")]
    public void IsValidNumber_ReturnExpectedResult_When(int precision, int scale, bool onlyPositive, string valueForValidation, bool expectedResult)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        var isValid = validator.IsValidNumber(valueForValidation);

        isValid.Should().Be(expectedResult);
    }
}