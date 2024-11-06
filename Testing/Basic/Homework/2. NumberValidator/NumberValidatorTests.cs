using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, 2)]
    [TestCase(1, -1)]
    [TestCase(2, 3)]
    public void WhenInvalidParameters_ShouldThrowArgumentException(int precision, int scale)
    {
        var func = () => new NumberValidator(precision, scale);
        func.Should().Throw<ArgumentException>();
    }

    [Test]
    public void WhenValid_ShouldNotThrow()
    {
        var action = () => new NumberValidator(1, 0, true);
        action.Should().NotThrow();
    }

    [TestCase("", false, true)]
    [TestCase(null, false, true)]
    [TestCase("12345", true, true)]
    [TestCase("123.45", true, true)]
    [TestCase("-123.45", false, true)]
    [TestCase("001.23", true, true)]
    [TestCase("0", true, true)]
    [TestCase("0.0", true, true)]
    [TestCase("+123.45", false, true)]
    [TestCase("123456", false, true)]
    [TestCase("123.456", false, true)]
    [TestCase("0000.00", false, true)]
    [TestCase("0.000", false, true)]
    [TestCase("ab.c", false, true)]
    [TestCase("123.45.67", false, true)]
    [TestCase("123,67", true, true)]
    [TestCase("123 45", false, true)]
    [TestCase("++1", false, true)]
    [TestCase("1.", false, true)]

    [TestCase("12345", true, false)]
    [TestCase("123.45", true, false)]
    [TestCase("123.45a", false, false)]
    [TestCase("12.3.4", false, false)]
    [TestCase("-0", true, false)]
    [TestCase("-12.45", true, false)]
    [TestCase("-123.45", false, false)]
    [TestCase("--1", false, false)]
    [TestCase("-1.", false, false)]
    public void WhenInputIsVarious_ShouldReturnExpected(string input, bool expected, bool onlyPositive)
    {
        var validator = new NumberValidator(5, 2, onlyPositive);
        ValidateNumber(validator, input, expected);
    }

    private void ValidateNumber(NumberValidator validator, string input, bool expected)
    {
        var result = validator.IsValidNumber(input);
        result.Should().Be(expected, $"Input '{input}' did not return expected result '{expected}'.");
    }
}