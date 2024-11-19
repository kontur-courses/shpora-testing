
using NUnit.Framework;
using NUnit.Framework.Legacy;
using FluentAssertions;
using Newtonsoft.Json.Linq;

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

    [TestCase(2, -1, Description ="Negative Scale")]
    [TestCase(1, 2, Description ="Scale value is greater than precision value")]
    public void NumberValidator_ExcludesNegativeOrWrongValueScale(int precision, int scale)
    {
        Action numberValidator = () => new NumberValidator(precision, scale);

        numberValidator.Should().Throw<ArgumentException>()
            .WithMessage("scale must be a non-negative number less than precision");
    }
    
    [TestCase( "", Description = "String.Empty as argument")]
    [TestCase( null, Description = "null as argument")]
    public void Check_IsValidNumber_ReturnsFalseIfStringIsNullOrEmpty(string number)
    {
        var result = new NumberValidator(17, 2, true)
            .IsValidNumber(number);

        result.Should().BeFalse("Пустая строка в качестве аргумента");
    }
    
    [TestCase( "-0.00")]
    [TestCase("0.000")]
    [TestCase( "a.sd")]
    public void Check_IsValidNumber_TheCorrectNumberFormat( string number)
    {
        var result = new NumberValidator(17, 2, true)
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