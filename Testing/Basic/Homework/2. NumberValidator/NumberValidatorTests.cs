using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{

    [TestCase(0, 2, Description = "Precision равен нулю")]
    [TestCase(-5, 2, Description = "Precision отрицательный")]
    public void Constructor_WithInvalidPrecision_ShouldThrowArgumentException(
        int precision, int scale)
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new NumberValidator(precision, scale, true));

        Assert.That(exception.Message, Does.Contain("precision must be a positive number"));
    }


    [TestCase(5, -1, Description = "Scale отрицательный")]
    [TestCase(2, 2, Description = "Scale равен Precision")]
    [TestCase(2, 5, Description = "Scale больше Precision")]
    public void Constructor_WithInvalidScale_ShouldThrowArgumentException(
        int precision, int scale)
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new NumberValidator(precision, scale, true));

        Assert.That(exception.Message, Does.Contain("scale must be a non-negative number"));
    }


    [TestCase("+14.23", true, Description = "Положительное число со знаком")]
    [TestCase("-14.23", false, Description = "Отрицательное число со знаком")]
    public void IsValidNumber_SignOfTheNumber_ShouldReturnTrue(
    string input,
    bool onlyPositive)
    {
        var validator = new NumberValidator(5, 2, onlyPositive);

        Assert.That(validator.IsValidNumber(input), Is.EqualTo(true));
    }

    [TestCase("-0.00", Description = "Отрицательный ноль")]
    [TestCase("+0.00", Description = "Положительный ноль со знаком")]
    [TestCase("00.00", Description = "Ведущий ноль в целой части")]
    public void IsValidNumber_IncorrectNotationOfZero_ShouldReturnFalse(string input)
    {
        var validator = new NumberValidator(3, 2, true);

        Assert.That(validator.IsValidNumber(input), Is.EqualTo(false));
    }

    [TestCase("0.0",  Description = "Простой ноль с дробной частью")]
    [TestCase("0", Description = "Простой ноль без дробной части")]
    public void IsValidNumber_CorrectNatationOfZero_ShouldReturnTrue(string input)
    {
        var validator = new NumberValidator(17, 2, true);

        Assert.That(validator.IsValidNumber(input), Is.EqualTo(true));
    }


    [Test]
    public void IsValidNumber_SoMuchScaleSymbols_ShouldReturnFalse()
    {
        var validator = new NumberValidator(17, 2, true);

        Assert.That(validator.IsValidNumber("0.000"), Is.EqualTo(false));
    }

    [Test]
    public void IsValidNumber_IncorrectNumberSign_ShouldRetrunFalse()
    {
        var validator = new NumberValidator(3, 2, true);

        Assert.That(validator.IsValidNumber("-1.23"), Is.EqualTo(false));
    }

    [Test]
    public void IsValidNumber_LettersInsteadOfNumbers_ShouldRetrunFalse()
    {
        var validator = new NumberValidator(3, 2, true);

        Assert.That(validator.IsValidNumber("a.sd"), Is.EqualTo(false));
    }

    [TestCase("+1.2", true, Description = "Ровно на границе precision (3 символа)")]
    [TestCase("124.3", false, Description = "Превышение precision на 1 символ (4 символа)")]
    [TestCase("1.2", true, Description = "Меньше границы precision (без знака, 3 символа)")]
    public void IsValidNumber_BoundaryPrecision_ShouldReturnExpected(
    string input, bool expectedResult)
    {
        var validator = new NumberValidator(3, 1, true);
        Assert.That(validator.IsValidNumber(input), Is.EqualTo(expectedResult));
    }

    [Test]
    public void Constructor_ScaleEqualsPrecision_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new NumberValidator(2, 2, true));
    }
}