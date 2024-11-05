using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(17, 2, true, "0.0", ExpectedResult = true, TestName = "Valid zero")]
    [TestCase(17, 2, true, "0", ExpectedResult = true, TestName = "Valid zero without fraction")]
    [TestCase(3, 2, true, "00.00", ExpectedResult = false, TestName = "Invalid zero with leading zero")]
    [TestCase(3, 2, true, "-0.00", ExpectedResult = false, TestName = "Invalid zero with negative sign")]
    [TestCase(3, 2, true, "+0.00", ExpectedResult = false, TestName = "Invalid zero with positive sign")]
    [TestCase(17, 2, true, "0.000", ExpectedResult = false, TestName = "Invalid zero with tailing zeroes")]
    [TestCase(1, 0, true, "0", ExpectedResult = true, TestName = "Valid zero with precision = 1 and scale = 0")]
    [TestCase(2, 0, true, "+0", ExpectedResult = true, TestName = "Valid zero with plus sign, precision = 2, scale = 0")]
    [TestCase(1, 0, true, "+0", ExpectedResult = false, TestName = "Invalid zero with plus sign, precision = 2, scale = 0")]
    [TestCase(1, 0, true, "-0", ExpectedResult = false, TestName = "Invalid zero with minus sign, precision = 2, scale = 0")]

    // Positive number cases
    [TestCase(4, 2, true, "+1.23", ExpectedResult = true, TestName = "Valid positive number with positive sign")]
    [TestCase(3, 2, true, "1.23", ExpectedResult = true, TestName = "Valid positive number without sign")]
    [TestCase(3, 2, false, "1.23", ExpectedResult = true, TestName = "Valid positive number without sign")]
    [TestCase(3, 2, true, "+1.23", ExpectedResult = false, TestName = "Invalid number with positive sign")]

    // Negative number cases
    [TestCase(3, 2, false, "-1.23", ExpectedResult = false, TestName = "Invalid negative number")]
    [TestCase(4, 3, false, "-1.23", ExpectedResult = true, TestName = "Valid negative number")]
    [TestCase(4, 3, true, "-1.23", ExpectedResult = false, TestName = "Invalid negative number with onlyPositive = true")]
    [TestCase(2, 1, false, "-1", ExpectedResult = true, TestName = "Valid negative number with precision = 2 and scale = 1")]

    // Other cases
    [TestCase(1, 0, true, "", ExpectedResult = false, TestName = "Empty string")]
    [TestCase(3, 2, true, "a.sd", ExpectedResult = false, TestName = "Letters instead of number")]
    [TestCase(3, 2, true, null, ExpectedResult = false, TestName = "Null string")]
    [TestCase(3, 2, true, "0,0", ExpectedResult = true, TestName = "Valid with comma as point")]

    public bool NumberValidator_IsValidNumber(int precision, int scale, bool onlyPositive, string value)
    {
        return new NumberValidator(precision, scale, onlyPositive).IsValidNumber(value);
    }

    [TestCase(-1, 2, true, true)]
    [TestCase(1, -1, true, true)]
    [TestCase(1, 2, true, true)]

    public void NumberValidator_ThrowsException(int precision, int scale, bool onlyPositive, bool shouldThrow)
    {
        var validator = () => new NumberValidator(precision, scale, onlyPositive);
        if (shouldThrow) validator.Should().ThrowExactly<ArgumentException>();
        else validator.Should().NotThrow();
    }
}