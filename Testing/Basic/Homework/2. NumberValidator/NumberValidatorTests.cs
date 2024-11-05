using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.ComponentModel.DataAnnotations;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [Test]
    public void WhenPrecisionIsNegative_ShouldReturnArgumentException()
    {
        Action action = () => new NumberValidator(-1, 2, true);
        action.Should().Throw<ArgumentException>().WithMessage("precision must be a positive number");
    }

    [Test]
    public void WhenScaleIsNegative_ShouldThrowArgumentException()
    {
        Action act = () => new NumberValidator(1, -1, false);
        act.Should().Throw<ArgumentException>().WithMessage("precision must be a non-negative number less or equal than precision");
    }

    [Test]
    public void WhenScaleIsMoreThanPrecision_ShouldThrowArgumentException()
    {
        Action act = () => new NumberValidator(2, 3, false);
        act.Should().Throw<ArgumentException>().WithMessage("precision must be a non-negative number less or equal than precision");
    }

    [Test]
    public void WhenValid_ShouldNotThrow()
    {
        Action act = () => new NumberValidator(1, 0, true);
        act.Should().NotThrow();
    }

    [Test]
    public void WhenInputIsEmpty_ShouldReturnFalse()
    {
        NumberValidator validator = new NumberValidator(5, 2, true);
        ValidateNumber(validator, "", false);
    }

    [Test]
    public void WhenInputIsNull_ShouldReturnFalse()
    {
        NumberValidator validator = new NumberValidator(5, 2, true);
        ValidateNumber(validator, null, false);
    }


    [Test]
    [TestCase("12345", true)]
    [TestCase("123.45", true)]
    [TestCase("-123.45", false)]
    [TestCase("001.23", true)]
    [TestCase("0", true)]
    [TestCase("0.0", true)]
    [TestCase("+123.45", false)]
    [TestCase("123456", false)]
    [TestCase("123.456", false)]
    [TestCase("0000.00", false)]
    [TestCase("0.000", false)]
    [TestCase("ab.c", false)]
    [TestCase("123.45.67", false)]
    [TestCase("123,67", true)]
    [TestCase("123 45", false)]
    [TestCase("++1", false)]
    [TestCase("1.", false)]
    public void WhenInputIsVarious_ShouldReturnExpected_PositiveValidator(string input, bool expected)
    {
        NumberValidator validator = new NumberValidator(5, 2, true);
        ValidateNumber(validator, input, expected);
    }

    [Test]
    [TestCase("12345", true)]
    [TestCase("123.45", true)]
    [TestCase("123.45a", false)]
    [TestCase("12.3.4", false)]
    [TestCase("-0", true)]
    [TestCase("-12.45", true)]
    [TestCase("-123.45", false)]
    [TestCase("--1", false)]
    [TestCase("-1.", false)]
    public void WhenInputIsVarious_ShouldReturnExpected_NegativeValidator(string input, bool expected)
    {
        NumberValidator validator = new NumberValidator(5, 2, false);
        ValidateNumber(validator, input, expected);
    }

    private void ValidateNumber(NumberValidator validator, string input, bool expected)
    {
        bool result = validator.IsValidNumber(input);
        result.Should().Be(expected, $"Input '{input}' did not return expected result '{expected}'.");
    }
}