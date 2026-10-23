using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(3, 2, true, "00.00", false, Description = "Превышена допустимая точность")]
    [TestCase(3, 2, true, "+1.23", false, Description = "Превышена допустимая точность с учётом знака")]
    [TestCase(17, 2, true, "0.000", false, Description = "Превышено допустимое количество дробных знаков")]
    [TestCase(4, 2, true, "-1", false, Description = "Отрицательное число при onlyPositive")]
    [TestCase(3, 2, true, "a.sd", false, Description = "Некорректный формат числа")]
    [TestCase(17, 2, true, "0.0", true, Description = "Корректное дробное число")]
    [TestCase(17, 2, true, "0", true, Description = "Корректное целое число")]
    [TestCase(3, 2, true, "+1", true, Description = "Корректное положительное число со знаком")]
    [TestCase(3, 2, false, null, false, Description = "Null вместо числа")]
    [TestCase(3, 2, false, "", false, Description = "Пустая строка")]
    [TestCase(4, 2, false, "-1.23", true, Description = "Корректное отрицательное число, когда оно разрешено")]
    [TestCase(3, 2, false, "1,23", true, Description = "Корректное дробное число с запятой")]
    [TestCase(3, 0, false, "123", true, Description = "Целое число с максимально допустимой длиной")]
    [TestCase(3, 0, false, "1234", false, Description = "Целое число превышает precision при нулевом scale")]
    public void IsValidNumber_ReturnsExpectedResult(
        int precision, int scale, bool onlyPositive, string number, bool expected)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        var result = validator.IsValidNumber(number);

        result.Should().Be(expected);
    }

    [TestCase(3, 2, false, Description = "Разрешённый режим положительных и отрицательных чисел")]
    [TestCase(3, 2, true, Description = "Разрешённый режим только положительных чисел")]
    [TestCase(int.MaxValue, int.MaxValue - 1, true, Description = "Максимальное значение precision и Scale")]
    [TestCase(1, 0, false, Description = "Минимальный precision и scale по умолчанию")]
    public void NumberValidator_WithValidArguments_DoesNotThrow(
        int precision, int scale, bool onlyPositive)
    {
        var validatorCreation = () => new NumberValidator(precision, scale, onlyPositive);

        validatorCreation.Should().NotThrow();
    }

    [TestCase(-1, 2, true, Description = "Отрицательный precision")]
    [TestCase(0, 2, true, Description = "Нулевой precision")]
    [TestCase(10, -1, true, Description = "Отрицательный scale")]
    [TestCase(10, 10, true, Description = "Scale равен precision")]
    [TestCase(10, 11, true, Description = "Scale больше precision")]
    public void NumberValidator_WithInvalidArguments_ThrowsArgumentException(
        int precision, int scale, bool onlyPositive)
    {
        var validatorCreation = () => new NumberValidator(precision, scale, onlyPositive);

        validatorCreation.Should().Throw<ArgumentException>();
    }
}