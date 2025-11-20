using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [Test]
    [TestCase(-1, 2, TestName = "Precision меньше 0")]
    [TestCase(0, 1, TestName = "Scale больше Precision")]
    [TestCase(5, -1, TestName = "Scale отрицательный")]
    [TestCase(5, 5, TestName = "Scale равен precision")]
    [TestCase(3, 4, TestName = "Scale больше precision")]
    public void Constructor_InvalidDefinition_Fails(int precision, int scale)
    {
        Assert.Throws<ArgumentException>(() => new NumberValidator(precision, scale));
    }

    [Test]
    [TestCase(1, 0, TestName = "Без дробной части и знака")]
    [TestCase(5, 2, TestName = "С дробной частью без знака")]
    public void Constructor_ValidDefinitionWithoutSign(int precision, int scale = 0)
    {
        Assert.DoesNotThrow(() => new NumberValidator(precision, scale));
    }

    [Test]
    [TestCase(3, false, 1, TestName = "С дробной частью и минусом")]
    [TestCase(3, true, 1, TestName = "С дробной частью и плюсом")]
    public void Constructor_ValidDefinitionWithSign(int precision, bool onlyPositive, int scale = 0)
    {
        Assert.DoesNotThrow(() => new NumberValidator(precision, scale, onlyPositive));
    }

    [Test]
    [TestCase(17, 2, true, "0", TestName = "Число 0, без дробей")]
    [TestCase(17, 2, true, "0.0", TestName = "Дробное число 0.0")]
    [TestCase(17, 2, true, "12.3", TestName = "Положительное дробное")]
    [TestCase(4, 2, true, "+1.23", TestName = "Положительное со знаком")]
    [TestCase(17, 2, true, "123", TestName = "Положительное без дробей")]
    [TestCase(5, 2, true, "12.34", TestName = "Разделитель точка")]
    [TestCase(5, 2, true, "12,34", TestName = "Разделитель запятая")]
    [TestCase(4, 2, true, "10.25", TestName = "Дробное число в пределах" +
                                              " precision и scale")]
    [TestCase(3, 0, true, "123", TestName = "Число при scale=0")]
    [TestCase(4, 2, false, "-1.23", TestName = "Отрицательное число," +
                                               " когда onlyPositive=false")]
    [TestCase(4, 2, false, "30.23", TestName = "Положительное число," +
                                               " когда onlyPositive=false")]
    public void IsValidNumber_ValidNumber(int precision, int scale, bool onlyPositive, string number)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        ClassicAssert.IsTrue(validator.IsValidNumber(number));
    }

    [Test]
    [TestCase(3, 2, true, "-1.23", TestName = "Отрицательное число," +
                                              " когда onlyPositive=true")]
    [TestCase(3, 2, true, "00.00", TestName = "Перегруз по precision без знака")]
    [TestCase(3, 2, true, "0.001", TestName = "Перегруз по scale")]
    [TestCase(3, 2, true, "+10.0", TestName = "Перегруз по precision с плюсом")]
    [TestCase(3, 2, true, "-0.1", TestName = "Перегруз по precision с минусом")]
    [TestCase(3, 0, true, "12.3", TestName = "Дробное число при scale =0")]
    [TestCase(3, 0, true, "12.0", TestName = "Дробное с нулевой дробной при scale=0")]
    [TestCase(6, 2, true, "5.234", TestName = "Перегруз по scale")]
    public void IsValidNumber_InvalidNumbers(int precision, int scale, bool onlyPositive, string number)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        ClassicAssert.IsFalse(validator.IsValidNumber(number));
    }

    [Test]
    [TestCase(null, TestName = "Null")]
    [TestCase("", TestName = "Пустая строка")]
    [TestCase("a.sd", TestName = "Буквенные символы")]
    [TestCase(".34", TestName = "Отсутствует целая часть")]
    [TestCase("12.", TestName = "Отсутствует дробная часть")]
    public void IsValidNumber_WrongFormatOrNotNumbers(string number)
    {
        var validator = new NumberValidator(4, 2);
        ClassicAssert.IsFalse(validator.IsValidNumber(number));
    }
}