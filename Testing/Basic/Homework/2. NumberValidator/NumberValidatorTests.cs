
using NUnit.Framework;
using FluentAssertions;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    #region ConstructorTests
    [TestCase(0,0, "precision must be a positive number", 
        TestName = "Constructor_PrecisionIsZero_ThrowArgumentException (precision = 0, scale = 0)")]
    [TestCase(-1, 0, "precision must be a positive number", 
        TestName = "Constructor_PrecisionIsNegative_ThrowArgumentException (precision = -1, scale = 0)")]

    [TestCase(1, -1, "scale must be a non-negative number less or equal than precision", 
        TestName = "Constructor_ScaleIsNegative_ThrowArgumentException (precision = 1, scale = -1)")]
    [TestCase(5, 5, "scale must be a non-negative number less or equal than precision", 
        TestName = "Constructor_PrecisionEqualsScale_ThrowArgumentException (precision = 5, scale = 5)")]
    [TestCase(1, 2, "scale must be a non-negative number less or equal than precision", 
        TestName = "Constructor_PrecisionIsGreaterThanPrecision_ThrowArgumentException (precision = 1, scale = 2)")]
    [Category("InitializationTests")]
    public void Constructor_InvalidArguments_ThrowArgumentException(int precision, int scale, string expectedMessage)
    {
        Action action = () => new NumberValidator(precision, scale);

        action.Should().Throw<ArgumentException>().WithMessage(expectedMessage);
    }

    [TestCase(1, 0, 
        TestName = "Constructor_PrecisionIsGreaterThanScaleAndScaleIsZero_DoesNotThrowException (precision = 1, scale = 0)")]
    [TestCase(5, 2, 
        TestName = "Constructor_PrecisionGreaterThanScaleAndScaleGreaterThanZero_DoesNotThrowException (precision = 5, scale = 2)")]
    [Category("InitializationTests")]
    public void Constructor_ValidArguments_NotThrowException(int precision, int scale)
    {
        Action action = () => new NumberValidator(precision, scale);

        action.Should().NotThrow();
    }
    #endregion

    #region IntegersTests
    [TestCase(1, 0, "0", 
        TestName = "IsValidNumber_IntegerWithoutSign_LengthEqualsPrecision_ReturnsTrue (precision = 1, scale = 0, onlyPositive = false, valueForValidation = \"0\")")]
    [TestCase(5, 0, "40", 
        TestName = "IsValidNumber_IntegerWithoutSign_LengthLessThanPrecision_ReturnsTrue (precision = 5, scale = 0, onlyPositive = false, valueForValidation = \"40\")")]
    [TestCase(2, 0, "+0",
        TestName = "IsValidNumber_PositiveZeroWithSign_ReturnsTrue (precision = 2, scale = 0, onlyPositive = false, valueForValidation = \"+0\")")]

    [TestCase(2, 0, "-0",
        TestName = "IsValidNumber_NegativeZeroWithSign_ReturnsTrue (precision = 2, scale = 0, onlyPositive = false, valueForValidation = \"-0\")")]
    [TestCase(3, 0, "+21",
        TestName = "IsValidNumber_IntegerWithSign_LengthEqualsPrecision_ReturnsTrue (precision = 3, scale = 0, onlyPositive = false, valueForValidation = \"+21\")")]
    [TestCase(5, 0, "-40",
        TestName = "IsValidNumber_IntegerWithSign_LengthLessThanPrecision_ReturnsTrue (precision = 5, scale = 0, onlyPositive = false, valueForValidation = \"-40\")")]

    [TestCase(3, 1, "0",
        TestName = "IsValidNumber_IntegerWithoutSign_ScaleIsGreaterThanZero_ReturnsTrue (precision = 3, scale = 1, onlyPositive = false, valueForValidation = \"6\")")]
    [TestCase(5, 2, "+21",
        TestName = "IsValidNumber_IntegerWithSign_ScaleIsGreaterThanZero_ReturnsTrue (precision = 5, scale = 2, onlyPositive = false, valueForValidation = \"+37\")")]
    [Category("IntegersTests")]
    public void IsValidNumber_IntegerWithinPrecisionAndScale_ReturnTrue(int precision, int scale, string valueForValidation)
    {
        var validator = new NumberValidator(precision, scale, false);
        var isValid = validator.IsValidNumber(valueForValidation);

        isValid.Should().BeTrue();
    }
    #endregion

    #region DecimalsTests
    [TestCase(5, 2, "123.45", 
        TestName = "IsValidNumber_DecimalWithoutSign_FractionalPartLengthEqualsScale_ReturnsTrue (precision = 5, scale = 2, onlyPositive = false, valueForValidation = \"123.45\")")]
    [TestCase(7, 4, "537.23", 
        TestName = "IsValidNumber_DecimalWithoutSign_FractionalPartLengthLessThanScale_ReturnsTrue (precision = 7, scale = 4, onlyPositive = false, valueForValidation = \"537.23\")")]

    [TestCase(3, 2, "0.0", 
        TestName = "IsValidNumber_DecimalZeroWithinPrecisionAndScale_ReturnsTrue (precision = 3, scale = 2, valueForValidation = \"0.0\")")]

    [TestCase(6, 2, "+237.89",
        TestName = "IsValidNumber_DecimalWithSign_FractionalPartLengthEqualsScale_ReturnsTrue (precision = 6, scale = 2, onlyPositive = false, valueForValidation = \"+237.89\")")]
    [TestCase(7, 4, "-445.12",
        TestName = "IsValidNumber_DecimalWithoutSign_FractionalPartLengthLessThanScale_ReturnsTrue (precision = 7, scale = 4, onlyPositive = false, valueForValidation = \"-445.12\")")]

    [TestCase(5, 2, "371,47",
        TestName = "IsValidNumber_DecimalWithoutSign_WithCommaSeparator_ReturnsTrue (precision = 5, scale = 2, valueForValidation = \"371,47\")")]
    [TestCase(6, 2, "+111,11",
        TestName = "IsValidNumber_DecimalWithSign_WithCommaSeparator_ReturnsTrue (precision = 6, scale = 2, valueForValidation = \"111,11\")")]
    [TestCase(6, 2, "-984,75",
        TestName = "IsValidNumber_DecimalWithSign_WithCommaSeparator_ReturnsTrue (precision = 6, scale = 2, valueForValidation = \"-984,75\")")]
    [Category("DecimalsTests")]
    public void IsValidNumber_DecimalWithinPrecisionAndScale_ReturnsTrue(int precision, int scale, string valueForValidation)
    {
        var validator = new NumberValidator(precision, scale, false);
        var isValid = validator.IsValidNumber(valueForValidation);

        isValid.Should().BeTrue();
    }
    #endregion

    #region InvalidConfigurationTests

    [TestCase(1, 0, "11",
        TestName = "IsValidNumber_IntegerWithoutSign_ExceedsPrecision_ReturnsFalse (precision = 1, scale = 0, onlyPositive = true, valueForValidation = \"11\")")]
    [TestCase(3, 1, "112.3",
        TestName = "IsValidNumber_DecimalWithoutSign_ExceedsPrecision_ReturnsFalse (precision = 3, scale = 1, onlyPositive = true, valueForValidation = \"112.3\")")]

    [TestCase(2, 0, "+42",
        TestName = "IsValidNumber_IntegerWithSign_ExceedsPrecision_ReturnFalse (precision = 2, scale = 0, onlyPositive = true, valueForValidation = \"+42\")")]
    [TestCase(4, 2, "+42.75",
        TestName = "IsValidNumber_DecimalWithSign_ExceedsPrecision_ReturnFalse (precision = 4, scale = 2, onlyPositive = true, valueForValidation = \"+42.75\")")]

    [TestCase(2, 0,"5.0",
        TestName = "IsValidNumber_DecimalProvided_ScaleIsZero_ReturnFalse (precision = 2, scale = 0, onlyPositive = true, valueForValidation = \"5.0\")")]
    [TestCase(5, 0, "1.23",
        TestName = "IsValidNumber_DecimalProvided_ScaleIsZero_ReturnFalse (precision = 5, scale = 0, onlyPositive = true, valueForValidation = \"1.23\")")]

    [TestCase(3, 1, "5.67",
        TestName = "IsValidNumber_DecimalExceedsScale_ReturnFalse (precision = 3, scale = 1, onlyPositive = true, valueForValidation = \"5.67\")")]

    [TestCase(7, 3, "-0",
        TestName = "IsValidNumber_NegativeZero_IsOnlyPositive_ReturnFalse (precision = 7, scale = 3, onlyPositive = true, valueForValidation = \"-0\")")]
    [TestCase(7, 3, "-10",
        TestName = "IsValidNumber_NegativeInteger_IsOnlyPositive_ReturnFalse (precision = 7, scale = 3, onlyPositive = true, valueForValidation = \"-10\")")]
    [TestCase(7, 3, "-1.23",
        TestName = "IsValidNumber_NegativeDecimal_IsOnlyPositive_ReturnFalse (precision = 7, scale = 3, onlyPositive = true, valueForValidation = \"-1.23\")")]
    [Category("InvalidConfigurationTests")]
    public void IsValidNumber_InvalidConfigurations_ReturnFalse(int precision, int scale, string valueForValidation)
    {
        var validator = new NumberValidator(precision, scale, true);
        var isValid = validator.IsValidNumber(valueForValidation);

        isValid.Should().BeFalse();
    }
    #endregion

    #region InvalidInputsTests
    [TestCase(5, 2, null, 
        TestName = "IsValidNumber_InputIsNull_ReturnFalse (precision = 5, scale = 2, onlyPositive = false, valueForValidation = null)")]
    [TestCase(5, 2, "", 
        TestName = "IsValidNumber_InputIsEmpty_ReturnFalse (precision = 5, scale = 2, onlyPositive = false, valueForValidation = \"\")")]
    [TestCase(5, 2, "    ", 
        TestName = "IsValidNumber_InputIsWhitespace_ReturnFalse (precision = 5, scale = 2, onlyPositive = false, valueForValidation = \"    \")")]
    [TestCase(5, 2, "\t", 
        TestName = "IsValidNumber_InputIsEscapeSequences_ReturnFalse (precision = 5, scale = 2, onlyPositive = false, valueForValidation = \"\\t\")")]

    [TestCase(5,2, "a.sd", 
        TestName = "IsValidNumber_InputContainsLetters_ReturnsFalse (precision = 5, scale = 2, onlyPositive = false, valueForValidation = \"a.sd\")")]
    [TestCase(5, 2, "10O", 
        TestName = "IsValidNumber_InputContainsLetters_ReturnsFalse (precision = 5, scale = 2, onlyPositive = false, valueForValidation = \"10O\")")]

    [TestCase(7, 2, "12_34", 
        TestName = "IsValidNumber_InputContainsIncorrectSeparator_ReturnsFalse (precision = 5, scale = 2, onlyPositive = false, valueForValidation = \"12_34\")")]

    [TestCase(5, 0, "871.", 
        TestName = "IsValidNumber_DecimalWithoutFraction_ScaleZero_ReturnsFalse (precision = 5, scale = 0, onlyPositive = false, valueForValidation = \"871.\")")]
    [TestCase(5, 2, "636.", 
        TestName = "IsValidNumber_DecimalWithoutFraction_ScaleIsPositive_ReturnsFalse (precision = 5, scale = 2, onlyPositive = false, valueForValidation = \"636.\")")]

    [Category("InvalidInputsTests")]
    public void IsValidNumber_InvalidInputs_ReturnFalse(int precision, int scale, string valueToValidator)
    {
        var validator = new NumberValidator(precision, scale);
        var isValid = validator.IsValidNumber(valueToValidator);

        isValid.Should().BeFalse();
    }
    #endregion
}