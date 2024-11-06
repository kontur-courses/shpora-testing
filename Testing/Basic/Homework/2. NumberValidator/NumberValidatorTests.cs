using NUnit.Framework;
using NUnit.Framework.Legacy;
using static System.Formats.Asn1.AsnWriter;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(0)]
    [TestCase(-1)]
    public void NumberValidator_ThrowsArgumentException_WhenPrecisionLessOrEqualThanZero(int precision)
    {
        Assert.Throws<ArgumentException>(() => new NumberValidator(precision));
    }

    [TestCase(1, -1)]
    public void NumberValidator_ThrowsArgumentException_WhenScaleLessThanZero(int precision,
        int scale)
    {
        Assert.Throws<ArgumentException>(() => new NumberValidator(precision, scale));
    }

    [TestCase(1, 1)]
    [TestCase(1, 2)]
    public void NumberValidator_ThrowsArgumentException_WhenScaleMoreOrEqualThanPrecision(int precision,
        int scale)
    {
        Assert.Throws<ArgumentException>(() => new NumberValidator(precision, scale));
    }

    [TestCase(17, 2, true, null, ExpectedResult = false)]
    [TestCase(17, 2, true, "", ExpectedResult = false)]
    [TestCase(17, 2, true, " ", ExpectedResult = false)]
    public bool IsValidNumber_ReturnFalse_WhenGivenNullOrEmptyString(int precision,
        int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        return validator.IsValidNumber(value);
    }

    [TestCase(17, 2, true, "0.0", ExpectedResult = true)]
    [TestCase(17, 2, true, "0", ExpectedResult = true)]
    [TestCase(4, 2, true, "+1.23", ExpectedResult = true)]
    public bool IsValidNumber_ReturnTrue_WhenGivenCorrectValues(int precision,
        int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        return validator.IsValidNumber(value);
    }

    [TestCase(17, 2, true, "0,0", ExpectedResult = true)]
    public bool IsValidNumber_ReturnTrue_WhenGivenCorrectValuesWithComma(int precision,
        int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        return validator.IsValidNumber(value);
    }

    [TestCase(17, 2, true, "a.sd", ExpectedResult = false)]
    [TestCase(17, 2, true, "1..2", ExpectedResult = false)]
    [TestCase(17, 2, true, "1.2.3", ExpectedResult = false)]
    [TestCase(17, 2, true, "1.a", ExpectedResult = false)]
    [TestCase(17, 2, true, "a.1", ExpectedResult = false)]
    [TestCase(17, 2, true, "0.,0", ExpectedResult = false)]
    public bool IsValidNumber_ReturnFalse_WhenGivenNotANumberValues(int precision,
        int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        return validator.IsValidNumber(value);
    }

    [TestCase(3, 2, true, "00.00", ExpectedResult = false)]
    [TestCase(3, 0, true, "0.000", ExpectedResult = false)]
    [TestCase(3, 2, false, "-1.23", ExpectedResult = false)]
    public bool IsValidNumber_ReturnFalse_WhenPrecisionIsExceeded(int precision,
        int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        return validator.IsValidNumber(value);
    }

    [TestCase(17, 2, true, "0.000", ExpectedResult = false)]
    public bool IsValidNumber_ReturnFalse_WhenScaleIsExceeded(int precision,
        int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        return validator.IsValidNumber(value);
    }

    [TestCase(3, 2, true, "-0.00", ExpectedResult = false)]
    public bool IsValidNumber_ReturnFalse_WhenInvalidNumberSign(int precision,
        int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        return validator.IsValidNumber(value);
    }

    [TestCase(3, 2, true, "+1.23", ExpectedResult = true)]
    [TestCase(3, 2, true, "1.23", ExpectedResult = true)]
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
    public bool IsValidNumber_ReturnTrue_WhenGivenCorrectPositiveValue(int precision,
        int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        return validator.IsValidNumber(value);
    }
}