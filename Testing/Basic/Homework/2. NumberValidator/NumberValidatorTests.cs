using NUnit.Framework;
using NUnit.Framework.Legacy;
using static System.Formats.Asn1.AsnWriter;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(int.MinValue)]
    public void NumberValidator_ThrowsArgumentException_WhenPrecisionLessOrEqualThanZero(int precision)
    {
        Assert.Throws<ArgumentException>(() => new NumberValidator(0));
    }

    [Test]
    [TestCase(1, -1)]
    [TestCase(1, int.MinValue)]
    public void NumberValidator_ThrowsArgumentException_WhenScaleLessThanZero(int precision,
        int scale)
    {
        Assert.Throws<ArgumentException>(() => new NumberValidator(precision, scale));
    }

    [Test]
    [TestCase(1, 1)]
    [TestCase(1, 2)]
    public void NumberValidator_ThrowsArgumentException_WhenScaleMoreOrEqualThanPrecision(int precision,
        int scale)
    {
        Assert.Throws<ArgumentException>(() => new NumberValidator(precision, scale));
    }

    [Test]
    [TestCase(17, 2, true, null)]
    [TestCase(17, 2, true, "")]
    [TestCase(17, 2, true, " ")]
    public void IsValidNumber_ReturnFalse_WhenGivenNullOrEmptyString(int precision,
        int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        Assert.That(validator.IsValidNumber(value) == false);
    }

    [Test]
    [TestCase(17, 2, true, "0.0")]
    [TestCase(17, 2, true, "0")]
    [TestCase(4, 2, true, "+1.23")]
    public void IsValidNumber_ReturnTrue_WhenGivenCorrectValues(int precision,
        int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        Assert.That(validator.IsValidNumber(value) == true);
    }

    [Test]
    [TestCase(17, 2, true, "0,0")]
    public void IsValidNumber_ReturnTrue_WhenGivenCorrectValuesWithComma(int precision,
        int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        Assert.That(validator.IsValidNumber(value) == true);
    }

    [Test]
    [TestCase(17, 2, true, "a.sd")]
    [TestCase(17, 2, true, "1..2")]
    [TestCase(17, 2, true, "1.2.3")]
    [TestCase(17, 2, true, "1.a")]
    [TestCase(17, 2, true, "a.1")]
    [TestCase(17, 2, true, "0.,0")]
    public void IsValidNumber_ReturnFalse_WhenGivenNotANumberValues(int precision,
        int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        Assert.That(validator.IsValidNumber(value) == false);
    }

    [Test]
    [TestCase(3, 2, true, "00.00")]
    [TestCase(3, 0, true, "0.000")]
    [TestCase(3, 2, false, "-1.23")]
    public void IsValidNumber_ReturnFalse_WhenPrecisionIsExceeded(int precision,
        int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        Assert.That(validator.IsValidNumber(value) == false);
    }

    [Test]
    [TestCase(17, 2, true, "0.000")]
    public void IsValidNumber_ReturnFalse_WhenScaleIsExceeded(int precision,
        int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        Assert.That(validator.IsValidNumber(value) == false);
    }

    [Test]
    [TestCase(3, 2, true, "-0.00")]
    [TestCase(3, 2, true, "-1.23")]
    public void IsValidNumber_ReturnFalse_WhenInvalidNumberSign(int precision,
        int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        Assert.That(validator.IsValidNumber(value) == false);
    }

    [Test]
    [TestCase(3, 2, true, "+1.23")]
    [TestCase(3, 2, true, "1.23")]
    //  Я думаю, что в данном задании подразумивается,
    //  что NumberValidator корректен.
    //  Тогда цитирую:
    //  "Формат числового значения указывается в виде N(m.к),
    //  где m – максимальное количество знаков в числе,
    //  включая знак (для отрицательного числа),".
    //  В моём понимании,исходя из документации в IsValidNumber,
    //  тест со значением "+1.23" должен
    //  возвращать true, так как +1.23 это положительное число,
    //  а знак включаться в длину должен только у отрицательного числа.
    //  Я не так понял?
    public void IsValidNumber_ReturnTrue_WhenGivenCorrectPositiveValue(int precision,
        int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        Assert.That(validator.IsValidNumber(value) == true);
    }
}