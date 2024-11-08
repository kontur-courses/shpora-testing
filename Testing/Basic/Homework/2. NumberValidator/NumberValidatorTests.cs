using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, 2, true, TestName = "Negative precision")]
    [TestCase(2, 5, true, TestName = "Precision less then scale")]
    [TestCase(2, -3, true, TestName = "Negative scale")]
    [TestCase(-5, -3, true, TestName = "Negative scale and Precision less then scale ")]
    [TestCase(null, null, null, TestName = "Null input data")]
    public void NumberValidator_ShouldThrowArgumentException_WithIncorrectData(int precision, int scale,
        bool onlyPositive)
    {
        Action act = () => new NumberValidator(precision, scale, onlyPositive);
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void NumberValidator_ShouldNotThrowArgumentException_WithCorrectData()
    {
        Action act = () => new NumberValidator(1, 0, true);
        act.Should().NotThrow<ArgumentException>();
    }

    [TestCase(8, 2, true, "0.0", ExpectedResult = true)]
    [TestCase(3, 0, true, "0", ExpectedResult = true)]
    [TestCase(4, 2, false, "-1.24", ExpectedResult = true)]
    [TestCase(4, 2, false, "-1.24", ExpectedResult = true)]
    public bool NumberValidator_ShouldReturnTrue_WithCorrectData(int precision, int scale, bool onlyPositive,
        String number)
    {
        NumberValidator numberValidator = new NumberValidator(precision, scale, onlyPositive);
        return numberValidator.IsValidNumber(number);
    }

    [TestCase(4, 2, true, "-1.23", ExpectedResult = false)]
    [TestCase(5, 2, true, "1.253", ExpectedResult = false)]
    [TestCase(3, 0, false, "a.ds", ExpectedResult = false)]
    [TestCase(4, 2, false, "", ExpectedResult = false)]
    [TestCase(5, 2, true, "+-1.25", ExpectedResult = false)]
    [TestCase(5, 2, true, null, ExpectedResult = false)]
    [TestCase(3, 0, true, "1234", ExpectedResult = false)]
    [TestCase(5, 2, true, "\r", ExpectedResult = false)]
    [TestCase(3, 0, true, "1 23", ExpectedResult = false)]
    [TestCase(3, 0, true, " 2.3", ExpectedResult = false)]
    public bool NumberValidator_ShouldReturnFalse_WithInCorrectData(int precision, int scale, bool onlyPositive,
        String number)
    {
        NumberValidator numberValidator = new NumberValidator(precision, scale, onlyPositive);
        return numberValidator.IsValidNumber(number);
    }
}