using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [Test]
    public void Constructor_ShouldThrowArgumentException_WhenPrecisionIsNegative()
    {
        Action action = () => new NumberValidator(-1, 2, true);

        action.Should().Throw<ArgumentException>()
            .WithMessage("precision must be a positive number");
    }

    [Test]
    public void Constructor_ShouldThrowArgumentException_WhenScaleIsNegative()
    {
        Action act = () => new NumberValidator(3, -1, true);

        act.Should().Throw<ArgumentException>()
            .WithMessage("precision must be a non-negative number less or equal than precision");
    }

    [Test]
    public void Constructor_ShouldThrowArgumentException_WhenScaleExceedsThePrecision()
    {
        Action act = () => new NumberValidator(1, 4, true);
        
        act.Should().Throw<ArgumentException>()
            .WithMessage("precision must be a non-negative number less or equal than precision");
    }

    [Test]
    public void Constructor_ShouldNotThrowException_WhenInputValuesAreValid()
    {
        Assert.DoesNotThrow(() => new NumberValidator(1, 0, true));
    }

    [TestCase(4, 3, true, "")]
    [TestCase(4, 3, true, null)]
    [TestCase(4, 3, true, "a")]
    [TestCase(4, 3, true, "a.sd")]
    [TestCase(4, 3, true, "a.0")]
    [TestCase(10, 7, true, "1.23a4")]
    [TestCase(10, 7, true, "123.")]
    public void IsValidNumber_ReturnFalse_WhenInputValueIsInvalidFormat(int precision, int scale, bool onlyPositive, string value)
    {
        var numberValidator = new NumberValidator(precision, scale, onlyPositive);

        var isValidNumber = numberValidator.IsValidNumber(value);

        isValidNumber.Should().Be(false);
    }
    
    [TestCase(5, 2, "-123")]
    [TestCase(5, 4, "-1.23")]
    public void IsValidNumber_ReturnFalse_WhenValueIsNegativeAndOnlyPositiveIsTrue(int precision, int scale,
        string value)
    {
        var numberValidator = new NumberValidator(precision, scale, true);

        var isValidNumber = numberValidator.IsValidNumber(value);

        isValidNumber.Should().Be(false);
    }
    
    [TestCase(2, 0, "12", true)]
    [TestCase(2, 1, "123", false)]
    public void IsValidNumber_ReturnExpectedResult_WhenInputValueIsPositiveInteger(int precision, int scale,
        string value, bool expectedResult)
    {
        var numberValidator = new NumberValidator(precision, scale, true);

        var isValidNumber = numberValidator.IsValidNumber(value);

        isValidNumber.Should().Be(expectedResult);
    }
    
    [TestCase(3, 0, "-12", true)]
    [TestCase(3, 1, "-123", false)]
    public void IsValidNumber_ReturnExpectedResult_WhenInputValueIsNegativeInteger(int precision, int scale, string value, 
        bool expectedResult)
    {
        var numberValidator = new NumberValidator(precision, scale);

        var isValidNumber = numberValidator.IsValidNumber(value);

        isValidNumber.Should().Be(expectedResult);
    }
    
    [TestCase(8, 4, "12.34", true)]
    [TestCase(5, 4, "11.234", true)]
    [TestCase(7, 3, "1.234", true)]
    [TestCase(8, 4, "+123,4567", true)]
    [TestCase(4, 2, "+12.34", false)]
    [TestCase(6, 2, "1.234", false)]
    public void IsValidNumber_ReturnExpectedResult_WhenInputValueIsPositiveDouble(int precision, int scale, string value, 
        bool expectedResult)
    {
        var numberValidator = new NumberValidator(precision, scale, true);

        var isValidNumber = numberValidator.IsValidNumber(value);

        isValidNumber.Should().Be(expectedResult);
    }
    
    [TestCase(5, 4, "-12.3", true)]
    [TestCase(8, 4, "-1234.567", true)]
    [TestCase(12, 4, "-123.4567", true)]
    [TestCase(6, 3, "-12,345", true)]
    [TestCase(4, 1, "-123,4", false)]
    [TestCase(10, 2, "-123.456", false)]
    public void IsValidNumber_ReturnExpectedResult_WhenInputValueIsNegativeDouble(int precision, int scale,
        string value, bool expectedResult)
    {
        var numberValidator = new NumberValidator(precision, scale);

        var isValidNumber = numberValidator.IsValidNumber(value);

        isValidNumber.Should().Be(expectedResult);
    }
}