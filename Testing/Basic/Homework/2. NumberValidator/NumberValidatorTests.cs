
using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(false)]
    [TestCase(true)]
    public void NumberValidator_IncorrectPrecision_Exception(bool onlyPositive)
    {
        var createAConstructor = () => new NumberValidator(-1, 2, onlyPositive);
        createAConstructor
            .Should()
            .Throw<ArgumentException>("precision должен быть положительным числом");
    }

    [TestCase(true)]
    [TestCase(false)]
    public void NumberValidator_CorrectInitialization_NoExceptions(bool onlyPositive)
    {
        var createAConstructor = () => new NumberValidator(3, 2, onlyPositive);
        createAConstructor
            .Should()
            .NotThrow();
    }

    [TestCase(1, -1)]
    [TestCase(1, 2)]
    [TestCase(1, 1)]
    public void NumberValidator_IncorrectScale_Exception(int precision, int scale)
    {
        var createAConstructor = () => new NumberValidator(precision, scale, true);
        createAConstructor
            .Should()
            .Throw<ArgumentException>("scale должно быть меньше precision, но больше или равно 0");
    }

    [Test]
    public void IsValidNumber_InvalidCharacters_False()
    {
        new NumberValidator(3, 2, true)
            .IsValidNumber("a.sd")
            .Should()
            .BeFalse("В записи числа не должны содержаться буквы");
    }

    [TestCase("00.00", 3, 2)]
    [TestCase("+1.23", 3, 2)]
    public void Should_ReturnFalse_WhenUnappropriateLength(string value, int precision, int scale)
    {
        new NumberValidator(precision, scale, true)
            .IsValidNumber(value)
            .Should()
            .BeFalse("Длина числа не соответствует шаблону");
    }

    [TestCase("+1.23", 4, 2)]
    [TestCase("0", 17, 2)]
    [TestCase("0.0", 17, 2)]
    public void IsValidNumber_CorrectValue_True(string value, int precision, int scale)
    {
        new NumberValidator(precision, scale, true)
            .IsValidNumber(value)
            .Should()
            .BeTrue();
    }

    [TestCase("-0.00", 3, 2)]
    [TestCase("+0.00", 3, 2, false)]
    public void IsValidNumber_TheWrongSign_False(string value, int precision, int scale = 0, bool onlyPositive = true)
    {
        new NumberValidator(precision, scale, onlyPositive)
            .IsValidNumber(value)
            .Should()
            .BeFalse("Знаки value и NumberValidator отличаются");
    }

    [TestCase("")]
    [TestCase(null)]
    public void IsValidNumber_NullOrEmpty_False(string value, int precision = 1, int scale = 0, bool onlyPositive = true)
    {
        new NumberValidator(precision, scale, onlyPositive)
            .IsValidNumber(value)
            .Should()
            .BeFalse("Проверяемое значение не должно равняться null или пустой строке");
    }
    
    [Test]
    public void IsValidNumber_InappropriateAccuracy_False()
    {
        new NumberValidator(17, 2, true)
            .IsValidNumber("0.000")
            .Should()
            .BeFalse("значение не может иметь точность выше указанной в NumberValidator");
    }
}