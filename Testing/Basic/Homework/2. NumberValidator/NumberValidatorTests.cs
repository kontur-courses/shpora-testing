
using NUnit.Framework;
using NUnit.Framework.Legacy;

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
    public void Constructor_ThrowExceptions_InvalidParameter()
    {
        Assert.Throws<ArgumentException>(() => new NumberValidator(-1, 2, true));
        Assert.Throws<ArgumentException>(() => new NumberValidator(5, -1));
        Assert.Throws<ArgumentException>(() => new NumberValidator(5, 5)); 
        Assert.Throws<ArgumentException>(() => new NumberValidator(5, 6));
        Assert.Throws<ArgumentException>(() => new NumberValidator(0, 0));
    }
    
    [Test]
    public void Constructor_DoesNotThrowExceptions_CorrectParameter()
    {
        Assert.DoesNotThrow(() => new NumberValidator(1, 0, true));
        Assert.DoesNotThrow(() => new NumberValidator(1));
        Assert.DoesNotThrow(() => new NumberValidator(5, 2));
        Assert.DoesNotThrow(() => new NumberValidator(5, 2, true));
    }
    
    [TestCase(17, 2, true, "0", TestName = "Целое число ноль должно быть валидным")]
    [TestCase(17, 2, true, "0.0", TestName = "Дробное число ноль должно быть валидным")]
    [TestCase(4, 2, true, "+1.23", TestName = "Положительное число со знаком плюс должно быть валидным")]
    [TestCase(17, 2, false, "-1.23", TestName = "Отрицательное число должно быть валидным когда onlyPositive=false")]
    [TestCase(3, 0, true, "123", TestName = "Целое число без дробной части должно быть валидным")]
    public void IsValidNumber_ReturnTrue_ValidCases(int precision, int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        
        ClassicAssert.IsTrue(validator.IsValidNumber(value));
    }

    [TestCase(3, 2, true, "00.00", TestName = "Превышение общего лимита цифр")]
    [TestCase(3, 2, true, "-0.00", TestName = "Отрицательное число при запрете отрицательных")]
    [TestCase(3, 2, true, "+0.00", TestName = "Знак плюс с нулем превышает лимит цифр")]
    [TestCase(3, 2, true, "+1.23", TestName = "Превышение лимита цифр в целом числе")]
    [TestCase(17, 2, true, "0.000", TestName = "Превышение лимита цифр в дробной части")]
    [TestCase(3, 2, true, "a.sd", TestName = "Символы не являющиеся цифрами")]
    [TestCase(3, 2, true, "", TestName = "Пустая строка как входное значение")]
    [TestCase(3, 2, true, null, TestName = "Null как входное значение")]
    public void IsValidNumber_ReturnFalse_InvalidCases(int precision, int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        
        ClassicAssert.IsFalse(validator.IsValidNumber(value));
    }


    [TestCase(1, 0, true, "0", TestName = "Минимальная precision с нулем")]
    [TestCase(2, 1, true, "0.1", TestName = "Граничные значения precision и scale")]
    [TestCase(3, 2, true, "1.23", TestName = "Максимальное количество цифр в дробной части")]
    public void IsValidNumber_BoundaryCases_ReturnsTrue(int precision, int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        ClassicAssert.IsTrue(validator.IsValidNumber(value));
    }
}