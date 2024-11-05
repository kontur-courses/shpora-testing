
using NUnit.Framework;
using FluentAssertions;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    private const string PrecisionErrorMessage = "precision must be a positive number";
    private const string ScaleErrorMessage = "precision must be a non-negative number less or equal than precision";

    [TestCase(-1, 0, PrecisionErrorMessage, Description = "precision < 0")]
    [TestCase(-10, 1, PrecisionErrorMessage, Description = "precision < 0")]
    [TestCase(0, 0, PrecisionErrorMessage, Description = "precision == 0")]
    [TestCase(0, 2, PrecisionErrorMessage, Description = "precision == 0")]
    [TestCase(5, -1, ScaleErrorMessage, Description = "scale < 0")]
    [TestCase(5, -6, ScaleErrorMessage, Description = "scale < 0")]
    [TestCase(1, 2, ScaleErrorMessage, Description = "scale > precision")]
    [TestCase(1, 10, ScaleErrorMessage, Description = "scale > precision")]
    [TestCase(1, 1, ScaleErrorMessage, Description = "scale == precision")]
    [TestCase(2, 2, ScaleErrorMessage, Description = "scale == precision")]
    public void NumberValidator_ThrowsArgumentException_WithInvalidParams(int precision, int scale, string msg)
    {
        Action act = () => new NumberValidator(precision, scale);
        act.Should().Throw<ArgumentException>().WithMessage(msg);
    }

    [TestCase(17, 10)]
    [TestCase(3, 2)]
    [TestCase(1, 0)]
    [TestCase(6, 5)]
    [TestCase(300, 52)]
    public void NumberValidator_DoesNotThrow_WithValidParams(int precision, int scale)
    {
        Action act = () => new NumberValidator(precision, scale);
        act.Should().NotThrow();
    }

    [TestCase("2", 1, 0, true)]
    [TestCase("52", 2, 1, true)]
    [TestCase("-300", 4, 0, false)]
    [TestCase("300", 3, 0, true)]
    [TestCase("-99", 3, 2, false)]
    [TestCase("9999999", 7, 1, false)]
    [TestCase("2147483649", 10, 1, false, Description = "Int overflow")]
    [TestCase("-999999999", 10, 1, false)]
    [TestCase("-1234567890", 11, 8, false)]
    [TestCase("0101010", 20, 12, false)]
    [TestCase("-1234567890.1234567890", 21, 10, false)]
    [TestCase("0.0", 2, 1, false)]
    [TestCase("0.000", 4, 3, true)]
    [TestCase("300.52", 5, 2, true)]
    [TestCase("+1.23", 17, 10, false)]
    [TestCase("+3.52", 15, 10, false)]
    [TestCase("300,52", 17, 10, false)]
    [TestCase("1,23", 17, 10, false)]
    [TestCase("100", 3, 1, true)]
    public void IsValidNumber_ShouldBeTrue_WithValidParams(string value, int precision, int scale, bool onlyPositive) =>
        new NumberValidator(precision, scale, onlyPositive)
            .IsValidNumber(value)
            .Should().BeTrue();

    [TestCase("", 17, 10, false)]
    [TestCase(null, 17, 10, false)]
    [TestCase("II", 17, 10, false)]
    [TestCase("III", 17, 10, false)]
    [TestCase("VI", 17, 10, false)]
    [TestCase("X", 17, 10, false)]
    [TestCase("seven.eleven", 17, 10, false)]
    [TestCase("five/two", 17, 10, false)]
    [TestCase("11.11.11", 17, 10, false)]
    [TestCase("11.22,33", 17, 10, false)]
    [TestCase("55,22,33", 17, 10, false)]
    [TestCase("1/2", 17, 10, false)]
    [TestCase("-100", 4, 0, true)]
    [TestCase("-0.00", 3, 2, true)]
    [TestCase("+1,23", 3, 2, true)]
    [TestCase("1.123", 4, 2, true)]
    [TestCase("0.000", 5, 2, true)]
    [TestCase("a.sd", 17, 10, false)]
    [TestCase("52", 1, 0, true)]
    [TestCase("-300", 3, 0, false)]
    [TestCase("300", 2, 0, false)]
    [TestCase("-1234567890", 10, 0, false)]
    [TestCase("-1.23", 3, 2, false)]
    [TestCase("-1234567890.1234567890", 20, 10, false)]
    [TestCase("100", 2, 1, true)]
    [TestCase("X,Y", 3, 2, true)]
    [TestCase("1\n.5", 3, 2, true)]
    [TestCase("+-15", 3, 2, false)]
    [TestCase("300_000", 17, 10, false)]
    [TestCase("52 000", 17, 10, false)]
    [TestCase("1/2", 17, 10, false)]
    [TestCase("12 . 22", 17, 10, false)]
    [TestCase("12 , 22", 17, 10, false)]
    public void IsValidNumber_ShouldBeFalse_WithInvalidParams(string value, int precision, int scale, 
        bool onlyPositive) =>
        new NumberValidator(precision, scale, onlyPositive)
            .IsValidNumber(value)
            .Should().BeFalse();
}