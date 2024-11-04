using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, 2, true, TestName = "negative precision")]
    [TestCase(0, 2, false, TestName = "precision == 0")]
    [TestCase(1, -2, true, TestName = "negative scale")]
    [TestCase(1, 2, false, TestName = "scale > precision")]
    [TestCase(1, 1, true, TestName = "scale == precision")]
    public void NumberValidation_ConstructorHasArgumentException(int precision, int scale, bool onlyPositive)
    {
        var action = () => new NumberValidator(precision, scale, onlyPositive);
        action.Should().Throw<ArgumentException>();
    }

    [TestCase(1, 0, true, TestName = "default")]
    public void NumberValidation_ConstructorDoesNotHaveArgumentException(int precision, int scale, bool onlyPositive)
    {
        var action = () => new NumberValidator(precision, scale, onlyPositive);
        action.Should().NotThrow();
    }

    [TestCase(1, 0, true, "0",ExpectedResult = true)]
    [TestCase(2, 1, true, "0.0", ExpectedResult = true)]
    [TestCase(2, 1, true, "0.1", ExpectedResult = true)]
    [TestCase(2, 1, true, "0,1", ExpectedResult = true)]
    [TestCase(2, 0, true, "+1", ExpectedResult = true)]
    [TestCase(3, 1, true, "+1.1", ExpectedResult = true)]
    [TestCase(3, 1, true, "+1,1", ExpectedResult = true)]
    [TestCase(2, 0, false, "-1", ExpectedResult = true)]
    [TestCase(3, 1, false, "-1.1", ExpectedResult = true)]
    [TestCase(3, 1, false, "-1,1", ExpectedResult = true)]
    [TestCase(7, 2, true, "671,23", ExpectedResult = true)]

    [TestCase(2, 0, true, "", ExpectedResult = false)]
    [TestCase(2, 1, true, ".0", ExpectedResult = false)]
    [TestCase(2, 0, true, "0.", ExpectedResult = false)]
    [TestCase(2, 0, true, "-1", ExpectedResult = false)]
    [TestCase(10, 2, true, "abcde.f", ExpectedResult = false)]
    [TestCase(10, 9, true, "127.0.0.1", ExpectedResult = false)]
    [TestCase(3, 2, true, "4321.5", ExpectedResult = false)]
    [TestCase(3, 0, true, "+145", ExpectedResult = false)]
    [TestCase(3, 0, false, "-124", ExpectedResult = false)]
    public bool IsValidNumber(int precision, int scale, bool onlyPositive, string value)
    {
        return new NumberValidator(precision, scale, onlyPositive).IsValidNumber(value);
    }
}