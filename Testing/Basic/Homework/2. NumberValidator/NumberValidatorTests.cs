
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

    [TestCase(2, -1)]
    [TestCase(1, 2)]
    public void NumberValidator_ExcludesNegativeOrWrongValueScale(int precision, int scale)
    {
        Action numberValidator = () => new NumberValidator(precision, scale);

        numberValidator.Should().Throw<ArgumentException>()
            .WithMessage("precision must be a non-negative number less or equal than precision");
        //некорректное сообщение об ошибке в конструкторе NumberValidator, должно быть
            //"scale must be a non-negative number less than precision"
    }
    
    [TestCase(17, 2, true, "")]
    [TestCase(17, 2, true, null)]
    public void Check_IsValidNumber_ReturnsFalseIfStringIsNullOrEmpty(int precision, int scale, bool onlyPositive, string number)
    {
        var result = new NumberValidator(17, 2, true)
            .IsValidNumber(number);

        result.Should().BeFalse("Пустая строка в качестве аргумента");
    }
    
    [TestCase(3, 2, true, "-0.00")]
    [TestCase(17, 2, true, "0.000")]
    [TestCase(17, 2, true, "a.sd")]
    public void Check_IsValidNumber_TheCorrectNumberFormat(int precision, int scale, bool onlyPositive, string number)
    {
        var result = new NumberValidator(precision, scale, onlyPositive)
            .IsValidNumber(number);

        result.Should().BeFalse("Неправильный формат числа");
    }

    [Test]
    public void Check_IsValidNumber_TakesIntoAccountTheSign()
    {
        var result = new NumberValidator(4, 2, true)
            .IsValidNumber("-1.23");

        result.Should().BeFalse("Число должно быть положительным");
    }
}