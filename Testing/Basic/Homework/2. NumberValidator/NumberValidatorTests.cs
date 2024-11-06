using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(17, 2, "0.0", TestName = "Valid zero")]
    [TestCase(17, 2, "00.0", TestName = "Zero with leading zero")]
    [TestCase(1, 0, "0", TestName = "Zero with small precision")]
    public void IsValidNumber_ShouldBeTrue_WhenCorrectZero(int precision, int scale, string value)
    {
        new NumberValidator(precision, scale, true).IsValidNumber(value).Should().BeTrue();
    }

    [TestCase(3, 2, "1.23", TestName = "Valid number")]
    [TestCase(5, 4, "1,23", TestName = "Number with comma as point")]
    public void IsValidNumber_ShouldBeTrue_WhenCorrectNumber(int precision, int scale, string value)
    {
        new NumberValidator(precision, scale, true).IsValidNumber(value).Should().BeTrue();
    }

    [TestCase(4, 2, true, "+1.23", TestName = "Positive number")]
    [TestCase(4, 3, false, "-1.23", TestName = "Negative number")]
    public void IsValidNumber_ShouldBeTrue_WhenCorrectSigned(int precision, int scale, bool onlyPositive, string value)
    {
        new NumberValidator(precision, scale, onlyPositive).IsValidNumber(value).Should().BeTrue();
    }

    [TestCase(4, 3, "-1.23", TestName = "Negative number when only positive flag is set")]
    [TestCase(4, 3, "-0", TestName = "Negative zero when only positive flag is set")]
    public void IsValidNumber_ShouldBeFalse_WhenIncorrectSigned(int precision, int scale, string value)
    {
        new NumberValidator(precision, scale, true).IsValidNumber(value).Should().BeFalse();
    }

    [TestCase(1, 0, "", TestName = "Empty string")]
    [TestCase(3, 2, "-", TestName = "Only negative sign")]
    [TestCase(3, 2,  "a.sd", TestName = "Letters instead of number")]
    [TestCase(3, 2,  null, TestName = "Null string")]
    public void IsValidNumber_ShouldBeFalse_WhenNotANumber(int precision, int scale, string value)
    {
        new NumberValidator(precision, scale).IsValidNumber(value).Should().BeFalse();
    }

    [TestCase(2, 1, true, "00.0", TestName = "Overflow with leading zero")]
    [TestCase(17, 2, true, "0.000", TestName = "Overflow with long fraction")]
    [TestCase(1, 0, true, "+0", TestName = "Number with plus sign")]
    public void IsValidNumber_ShouldBeFalse_WhenOverflow(int precision, int scale, bool onlyPositive,
        string value)
    {
        new NumberValidator(precision, scale, onlyPositive).IsValidNumber(value).Should().BeFalse();
    }

    [TestCase(-1, 2, TestName = "Negative precision")]
    [TestCase(1, -1, TestName = "Negative scale")]
    [TestCase(1, 1, TestName = "Scale equal to precision")]
    public void Constructor_ShouldThrowException_WhenInvalidParams(int precision, int scale)
    {
        var validator = () => new NumberValidator(precision, scale);
        validator.Should().ThrowExactly<ArgumentException>();
    }

    [TestCase(true, TestName = "Only positive")]
    [TestCase(false, TestName = "Not only positive")]
    public void Constructor_ShouldNotThrowException_WhenValidParams(bool onlyPositive)
    {
        var validator = () => new NumberValidator(1, 0, onlyPositive);
        validator.Should().NotThrow();
    }
}