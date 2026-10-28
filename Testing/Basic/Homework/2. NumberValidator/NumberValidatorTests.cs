using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, 2, true, TestName = "Throws exception when precision is negative")]
    [TestCase(5, 6, false, TestName = "Throws exception when scale is greater than precision")]
    [TestCase(0, 2, false, TestName = "Throws exception when precision is zero")]
    [TestCase(5, -1, false, TestName = "Throws exception when scale is negative")]
    [TestCase(5, 5, false, TestName = "Throws exception when scale equals precision")]
    public void NumberValidatorConstructor_ThrowsException_WhenInvalidData(int presicion,
                                                                           int scale,
                                                                           bool onlyPositive)
    {
        var act = () => new NumberValidator(presicion, scale, onlyPositive);

        act.Should().Throw<ArgumentException>($"При параметрах presicion = {presicion} и scale = {scale}");

    }

    [TestCase("0", 17, 2, true, TestName = "Is valid number when input is zero")]
    [TestCase("0.0", 17, 2, true, TestName = "Is valid number when zero has decimal part")]
    [TestCase("+1.23", 4, 2, true, TestName = "Is valid number with positive sign")]
    [TestCase("-1.23", 4, 2, false, TestName = "Is valid number with negative sign when onlyPositive is false")]
    [TestCase("12.34", 5, 2, false, TestName = "Is valid number that fits exactly into precision")]
    [TestCase("12,34", 5, 2, false, TestName = "Is valid number with comma as decimal separator")]
    public void IsValidNumber_ReturnTrue_WhenValidData(string input,int precision, int scale,
                                                       bool onlyPositive)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        var act = validator.IsValidNumber(input);

        act.Should().BeTrue($"значение '{input}' должно быть валидным при precision={precision}, scale={scale}");
    }

    [TestCase("00.00", 3, 2, true, TestName = "Is not valid number when total length exceeds precision")]
    [TestCase("+0.00", 3, 2, true, TestName = "Is not valid number when sign causes precision overflow")]
    [TestCase("0.000", 17, 2, true, TestName = "Is not valid number when decimal part exceeds scale")]
    [TestCase("-1.23", 3, 2, true, TestName = "Is not valid number when onlyPositive is true and number is negative")]
    [TestCase("a.sd", 3, 2, true, TestName = "Is not valid number when contains letters")]
    [TestCase("", 3, 2, true, TestName = "Is not valid number when input is empty")]
    [TestCase(null, 3, 2, true, TestName = "Is not valid number when input is null")]
    [TestCase("1.2.3", 5, 2, false, TestName = "Is not valid number when has multiple decimal points")]
    [TestCase("1;23", 5, 2, false, TestName = "Is not valid number when has invalid separator semicolon")]
    public void IsValidNumber_ReturnFalse_WhenInvalidData(string input, int precision, int scale,
                                                          bool onlyPositive)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        var act = validator.IsValidNumber(input);

        act.Should().BeFalse($"значение '{input}' должно быть НЕвалидным при precision={precision}, scale={scale}");
    }
}