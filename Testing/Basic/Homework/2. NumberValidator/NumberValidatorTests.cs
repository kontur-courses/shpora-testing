using FluentAssertions;
using NUnit.Framework;


namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    
    [TestCase(-1, 2, true, TestName = "Отрицательная precision")]
    [TestCase(5, -1, true, TestName = "Отрицательная scale")]
    [TestCase(5, 5, true, TestName = "Scale равен precision")]
    [TestCase(5, 6, true, TestName = "Scale больше precision")]
    [TestCase(0, 0, true, TestName = "Precision ноль")]
    public void Constructor_ThrowExceptions_InvalidParameter(int precision, int scale, bool onlyPositive)
    {
        Action act = () => new NumberValidator(precision, scale, onlyPositive);
        
        act.Should().Throw<ArgumentException>();
    }
    
    
    [TestCase(1, 0, true, TestName = "Минимальные валидные параметры")]
    [TestCase(1, 0, false, TestName = "OnlyPositive false")]
    [TestCase(5, 2, true, TestName = "Scale меньше precision")]
    [TestCase(3, 0, true, TestName = "Scale ноль")]
    public void Constructor_DoesNotThrowExceptions_CorrectParameter(int precision, int scale, bool onlyPositive)
    {
        Action act = () => new NumberValidator(precision, scale, onlyPositive);
        
        act.Should().NotThrow();
    }
    
    
    [TestCase(17, 2, true, "0", TestName = "Целое число ноль должно быть валидным")]
    [TestCase(17, 2, true, "0.0", TestName = "Дробное число ноль должно быть валидным")]
    [TestCase(4, 2, true, "+1.23", TestName = "Положительное число со знаком плюс должно быть валидным")]
    [TestCase(17, 2, false, "-1.23", TestName = "Отрицательное число должно быть валидным когда onlyPositive=false")]
    [TestCase(3, 0, true, "123", TestName = "Целое число без дробной части должно быть валидным")]
    public void IsValidNumber_ReturnTrue_ValidCases(int precision, int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        
        var result = validator.IsValidNumber(value);

        result.Should().BeTrue();
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
        
        var result = validator.IsValidNumber(value);
        
        result.Should().BeFalse();
    }


    [TestCase(1, 0, true, "0", TestName = "Минимальная precision с нулем")]
    [TestCase(2, 1, true, "0.1", TestName = "Граничные значения precision и scale")]
    [TestCase(3, 2, true, "1.23", TestName = "Максимальное количество цифр в дробной части")]
    public void IsValidNumber_BoundaryCases_ReturnsTrue(int precision, int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        
        var result = validator.IsValidNumber(value);
        
        result.Should().BeTrue();
    }
}