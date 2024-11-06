using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(17, 2, true, "0.0", TestName = "Valid zero")]
    [TestCase(17, 2, true, "00.0", TestName = "Zero with leading zero")]
    [TestCase(1, 0, true, "0", TestName = "Zero with small precision")]
    public void IsValidNumber_ShouldBeTrue_WhenCorrectZero(int precision, int scale, bool onlyPositive, string value)
    {
        new NumberValidator(precision, scale, onlyPositive).IsValidNumber(value).Should().BeTrue();
    }

    [TestCase(3, 2, "1.23", TestName = "Valid number")]
    [TestCase(3, 2, "1,23", TestName = "Number with comma as point")]
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
    public void IsValidNumber_ShouldBeFalse_WhenIncorrectSigned(int precision, int scale, string value)
    {
        new NumberValidator(precision, scale, true).IsValidNumber(value).Should().BeFalse();
    }

    [TestCase(1, 0, "", TestName = "Empty string")]
    [TestCase(1, 0, " ", TestName = "Whitespace")]
    [TestCase(3, 2, "-", TestName = "Only negative sign")]
    [TestCase(3, 2,  "a.sd", TestName = "Letters instead of number")]
    [TestCase(3, 2,  null, TestName = "Null string")]
    public void IsValidNumber_ShouldBeFalse_WhenNotANumber(int precision, int scale, string value)
    {
        new NumberValidator(precision, scale).IsValidNumber(value).Should().BeFalse();
    }

    [TestCase(2, 1, true, "00.0", TestName = "Zero with leading zero")]
    [TestCase(17, 2, true, "0.000", TestName = "Zero with tailing zeroes")]
    [TestCase(1, 0, true, "+0", TestName = "Zero with plus sign")]
    public void IsValidNumber_ShouldBeFalse_WhenOverflow(int precision, int scale, bool onlyPositive,
        string value)
    {
        new NumberValidator(precision, scale, onlyPositive).IsValidNumber(value).Should().BeFalse();
    }

    [TestCase(-1, 2, TestName = "Negative precision")]
    [TestCase(1, -1, TestName = "Negative scale")]
    [TestCase(1, 2, TestName = "Scale greater than precision")]
    public void Constructor_ShouldThrowException_WhenInvalidParams(int precision, int scale)
    {
        var validator = () => new NumberValidator(precision, scale);
        validator.Should().ThrowExactly<ArgumentException>();
    }
}