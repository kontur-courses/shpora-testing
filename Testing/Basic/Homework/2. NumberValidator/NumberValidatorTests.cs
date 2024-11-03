
using NUnit.Framework;
using NUnit.Framework.Legacy;
using FluentAssertions;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [Test]
    public void Test()
    {
        Assert.Throws<ArgumentException>(() => new NumberValidator(-1, 2, true));
        Assert.DoesNotThrow(() => new NumberValidator(1, 0, true));
        Assert.Throws<ArgumentException>(() => new NumberValidator(-1, 2, false));
        Assert.DoesNotThrow(() => new NumberValidator(1, 0, true));

        ClassicAssert.IsTrue(new NumberValidator(17, 2, true).IsValidNumber("0.0"));
        ClassicAssert.IsTrue(new NumberValidator(17, 2, true).IsValidNumber("0"));
        ClassicAssert.IsTrue(new NumberValidator(17, 2, true).IsValidNumber("0.0"));
        ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("00.00"));
        ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("-0.00"));
        ClassicAssert.IsTrue(new NumberValidator(17, 2, true).IsValidNumber("0.0"));
        ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("+0.00"));
        ClassicAssert.IsTrue(new NumberValidator(4, 2, true).IsValidNumber("+1.23"));
        ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("+1.23"));
        ClassicAssert.IsFalse(new NumberValidator(17, 2, true).IsValidNumber("0.000"));
        ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("-1.23"));
        ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("a.sd"));
    }

    [Test]
    public void NumberValidator_ExcludesNonPositivePrecision()
    {
        Action numberValidator = () => new NumberValidator(-1);

        numberValidator.Should().Throw<ArgumentException>()
            .WithMessage("precision must be a positive number");
    }

    [Test]
    public void NumberValidator_ExcludesNegativeScale()
    {
        Action numberValidator = () => new NumberValidator(2, -1);

        numberValidator.Should().Throw<ArgumentException>()
            .WithMessage("precision must be a non-negative number less or equal than precision");
        //некорректное сообщение об ошибке в конструкторе NumberValidator, должно быть
            //"scale must be a non-negative number less than precision"
    }

    [Test]
    public void NumberValidator_ExcludesScaleGreaterOrEqualThanPrecision()
    {
        Action numberValidator = () => new NumberValidator(1, 2);

        numberValidator.Should().Throw<ArgumentException>()
            .WithMessage("precision must be a non-negative number less or equal than precision");
        //некорректное сообщение аналагично предыдущему тесту
    }

    [Test]
    public void Check_IsValidNumber_ReturnsFalseIfStringIsEmpty()
    {
        var result = new NumberValidator(17, 2, true)
            .IsValidNumber(string.Empty);

        result.Should().BeFalse("Пустая строка в качестве аргумента");
    }

    [Test]
    public void Check_IsValidNumber_ReturnsFalseIfStringIsNull()
    {
        var result = new NumberValidator(17, 2, true)
            .IsValidNumber(null);

        result.Should().BeFalse("null в качестве аргумента");
    }

    [Test]
    public void Check_IsValidNumber_ReturnsFalseIfNotANumber()
    {
        var result = new NumberValidator(17, 2, true)
            .IsValidNumber("a.sd");

        result.Should().BeFalse("Не число в качестве аргумента");
    }

    public void Check_IsValidNumber_CorrectNumberOfDigits()
    {
        var result = new NumberValidator(3, 2, true)
            .IsValidNumber("-0.00");

        result.Should().BeFalse("Неправильный формат числа, неправильное количество символов");
    }

    [Test]
    public void Check_IsValidNumber_CorrectNumberOfFractionalDigits()
    {
        var result = new NumberValidator(17, 2, true)
            .IsValidNumber("0.000");

        result.Should().BeFalse("Неправильный формат чилса, неправильное количество знаков дробной части");
    }

    [Test]

    public void Check_IsValidNumber_TakesIntoAccountTheSign()
    {
        var result = new NumberValidator(4, 2, true)
            .IsValidNumber("-1.23");

        result.Should().BeFalse("Число должно быть положительным");
    }
}