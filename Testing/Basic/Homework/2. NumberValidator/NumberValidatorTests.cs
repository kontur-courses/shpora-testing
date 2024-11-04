
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    #region InitializationTests
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-100)]
    [TestCase(-9999)]
    [Category("InitializationTests")]
    public void Constructor_PrecisionIsNotPositive_ThrowArgumentException(int precision)
    {
        TestDelegate action = () => new NumberValidator(precision);

        var ex = Assert.Throws<ArgumentException>(action,
            $"Failed: Expected ArgumentException for precision = {precision}");

        Assert.That(ex.Message, Is.EqualTo("precision must be a positive number"),
            "Failed: Expected specific message for precision is not positive");
    }

    [TestCase(-1)]
    [TestCase(-100)]
    [TestCase(-9999)]
    [Category("InitializationTests")]
    public void Constructor_ScaleIsNegative_ThrowArgumentException(int scale)
    {
        TestDelegate action = () => new NumberValidator(1, scale);

        var ex = Assert.Throws<ArgumentException>(action,
            $"Failed: Expected ArgumentException for scale = {scale}");

        Assert.That(ex.Message, Is.EqualTo("precision must be a non-negative number less or equal than precision"),
            "Failed: Expected specific message for scale is negative");
    }

    [TestCase(5, 5)]
    [TestCase(1, 2)]
    [TestCase(5, 10)]
    [Category("InitializationTests")]
    public void Constructor_ScaleNotLessThanPrecision_ThrowArgumentException(int precision, int scale)
    {
        TestDelegate action = () => new NumberValidator(precision, scale);

        var ex = Assert.Throws<ArgumentException>(action,
            $"Failed: Expected ArgumentException for scale >= precision (precision = {precision}, scale = {scale})");

        Assert.That(ex.Message, Is.EqualTo("precision must be a non-negative number less or equal than precision"),
            "Failed: Expected specific message for scale >= precision");
    }

    [TestCase(1, 0)]
    [TestCase(2, 1)]
    [TestCase(10, 5)]
    [Category("InitializationTests")]
    public void Constructor_PrecisionAndScaleAreValid_NotThrowException(int precision, int scale)
    {
        TestDelegate action = () => new NumberValidator(precision, scale);

        Assert.DoesNotThrow(action,
            $"Failed: Unexpected ArgumentException for precision = {precision}, scale = {scale}");
    }
    #endregion

    #region IntegersTests
    [TestCase(1, "0")]
    [TestCase(2, "21")]
    [TestCase(5, "40")]
    [Category("IntegersTests")]
    public void IsValidNumber_IntegerWithoutSignWithinPrecision_ReturnTrue(int precision, string numberToValidator)
    {
        var validator = new NumberValidator(precision, 0, true);
        var isValid = validator.IsValidNumber(numberToValidator);

        Assert.That(isValid, Is.True,
            $"Failed: Expected valid for '{numberToValidator}', precision = {precision}, scale=0, onlyPositive=true");
    }

    [TestCase(2, "+0")]
    [TestCase(2, "-0")]
    [TestCase(3, "+21")]
    [TestCase(5, "-40")]
    [Category("IntegersTests")]
    public void IsValidNumber_IntegerWithSignWithinPrecision_ReturnTrue(int precision, string numberToValidator)
    {
        var validator = new NumberValidator(precision, 0, false);
        var isValid = validator.IsValidNumber(numberToValidator);

        Assert.That(isValid, Is.True,
            $"Failed: Expected valid for '{numberToValidator}', precision = {precision}, scale=0, onlyPositive=false");
    }

    [TestCase(2, 1, "0")]
    [TestCase(3, 1, "5")]
    [TestCase(5, 2, "-1")]
    [TestCase(3, 2, "+21")]
    [Category("IntegersTests")]
    public void IsValidNumber_IntegerProvidedAndScaleIsGreaterThanZero_ReturnTrue(int precision, int scale, string numberToValidator)
    {
        var validator = new NumberValidator(precision, scale, false);
        var isValid = validator.IsValidNumber(numberToValidator);

        Assert.That(isValid, Is.True,
            $"Failed: Expected valid for '{numberToValidator}', precision = {precision}, scale={scale}, onlyPositive=false");
    }
    #endregion

    #region DecimalsTests
    [TestCase(5, 2, "123.45")]
    [TestCase(5, 2, "123.45")]
    [TestCase(7, 2, "123.45")]
    [TestCase(5, 4, "123.45")]
    [TestCase(3, 2, "0.0")]
    [Category("DecimalsTests")]
    public void IsValidNumber_DecimalsWithoutSignWithinPrecisionAndScale_ReturnsTrue(int precision, int scale, string numberToValidator)
    {
        var validator = new NumberValidator(precision, scale, true);
        var isValid = validator.IsValidNumber(numberToValidator);

        Assert.That(isValid, Is.True,
            $"Failed: Expected valid for '{numberToValidator}', precision = {precision}, scale={scale}, onlyPositive=true");
    }

    [TestCase(6, 2, "+123.45")]
    [TestCase(7, 2, "-123.45")]
    [TestCase(3, 2, "+0.0")]
    [TestCase(3, 2, "-0.0")]
    [Category("DecimalsTests")]
    public void IsValidNumber_DecimalsWithSignWithinPrecisionAndScale_ReturnsTrue(int precision, int scale, string numberToValidator)
    {
        var validator = new NumberValidator(precision, scale, false);
        var isValid = validator.IsValidNumber(numberToValidator);

        Assert.That(isValid, Is.True,
            $"Failed: Expected valid for '{numberToValidator}', precision = {precision}, scale={scale}, onlyPositive=false");
    }

    [TestCase("123.45")]
    [TestCase("123,45")]
    [TestCase("+123,45")]
    [TestCase("-123,45")]
    [TestCase("+123.45")]
    [TestCase("-123.45")]
    [Category("DecimalsTests")]
    public void IsValidNumber_DecimalsWithCommaOrDotSeparator_ReturnTrue(string numberToValidator)
    {
        var validator = new NumberValidator(6, 3, false);
        var isValid = validator.IsValidNumber(numberToValidator);

        Assert.That(isValid, Is.True,
            $"Failed: Expected valid for '{numberToValidator}', precision = 6, scale=3, onlyPositive=false");
    }
    #endregion

    #region InvalidConfigurationTests
    [TestCase(1, 0, "11")]
    [TestCase(3, 1, "112.3")]
    [TestCase(2, 0, "+42")]
    [TestCase(4, 2, "+42.75")]
    [Category("InvalidConfigurationTests")]
    public void IsValidNumber_NumberExceedsPrecision_ReturnFalse(int precision, int scale, string numberToValidator)
    {
        var validator = new NumberValidator(precision, scale, false);
        var isValid = validator.IsValidNumber(numberToValidator);

        Assert.That(isValid, Is.False,
            $"Failed: Expected invalid for '{numberToValidator}', precision = {precision}, scale={scale}, onlyPositive=false");
    }

    [TestCase(2, "0.0")]
    [TestCase(3, "1.23")]
    [TestCase(5, "-12.34")]
    [TestCase(7, "+123.45")]
    [Category("InvalidConfigurationTests")]
    public void IsValidNumber_DecimalsProvidedAndScaleIsZero_ReturnFalse(int precision, string numberToValidator)
    {
        var validator = new NumberValidator(precision, 0, false);
        var isValid = validator.IsValidNumber(numberToValidator);

        Assert.That(isValid, Is.False,
            $"Failed: Expected invalid for '{numberToValidator}', precision={precision}, scale=0, onlyPositive=false");
    }

    [TestCase(3, 1, "1.23")]
    [TestCase(4, 1, "+1.32")]
    [TestCase(7, 2, "+1.321")]
    [Category("InvalidConfigurationTests")]
    public void IsValidNumber_DecimalExceedsScale_ReturnFalse(int precision, int scale, string numberToValidator)
    {
        var validator = new NumberValidator(precision, scale, false);
        var isValid = validator.IsValidNumber(numberToValidator);

        Assert.That(isValid, Is.False,
            $"Failed: Expected invalid for '{numberToValidator}', precision={precision}, scale={scale}, onlyPositive=false");
    }

    [TestCase("-0")]
    [TestCase("-10")]
    [TestCase("-1.23")]
    [Category("InvalidConfigurationTests")]
    public void IsValidNumber_NegativeNumberIsOnlyPositive_ReturnFalse(string numberToValidator)
    {
        var validator = new NumberValidator(7, 3, true);
        var isValid = validator.IsValidNumber(numberToValidator);

        Assert.That(isValid, Is.False,
            $"Failed: Expected invalid for '{numberToValidator}', precision=7, scale=3, onlyPositive=false");
    }
    #endregion

    #region InvalidInputsTests
    [TestCase(null)]
    [TestCase("")]
    [TestCase("    ")]
    [TestCase("\t")]
    [Category("InvalidInputsTests")]
    public void IsValidNumber_NumberNullOrEmpty_ReturnFalse(string numberToValidator)
    {
        var validator = new NumberValidator(5);
        var isValid = validator.IsValidNumber(numberToValidator);

        Assert.That(isValid, Is.False,
            $"Failed: Expected invalid for '{numberToValidator}', precision = 5, scale=0, onlyPositive=false");
    }

    [TestCase("a.sd")]
    [TestCase("10O")]
    [TestCase("12.34#")]
    [TestCase("$12.34")]
    [Category("InvalidInputsTests")]
    public void IsValidNumber_InputContainsLetters_ReturnFalse(string numberToValidator)
    {
        var validator = new NumberValidator(5, 3);
        var isValid = validator.IsValidNumber(numberToValidator);

        Assert.That(isValid, Is.False,
            $"Failed: Expected invalid for '{numberToValidator}', precision = 5, scale=3, onlyPositive=false");
    }

    [TestCase("12_34")]
    [TestCase("12/34")]
    [TestCase("$12;34")]
    [Category("InvalidInputsTests")]
    public void IsValidNumber_DecimalsContainsIncorrectSeparator_ReturnFalse(string numberToValidator)
    {
        var validator = new NumberValidator(7, 3);
        var isValid = validator.IsValidNumber(numberToValidator);

        Assert.That(isValid, Is.False,
            $"Failed: Expected invalid for '{numberToValidator}', precision = 5, scale=3, onlyPositive=false");
    }

    [TestCase(0, "123.")]
    [TestCase(1, "123.")]
    [TestCase(0, "12,")]
    [TestCase(2, "123,")]
    [Category("InvalidInputsTests")]
    public void IsValidNumber_DecimalsMissingFractionalPart_ReturnFalse(int scale, string numberToValidator)
    {
        var validator = new NumberValidator(5, scale);
        var isValid = validator.IsValidNumber(numberToValidator);

        Assert.That(isValid, Is.False,
            $"Failed: Expected invalid for '{numberToValidator}', precision = 5, scale={scale}, onlyPositive=false");
    }
    #endregion
}