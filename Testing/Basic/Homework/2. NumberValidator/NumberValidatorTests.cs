using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(int.MinValue)]
    [Description("Конструктор бросает исключение, если precision меньше или равен 0")]
    public void Constructor_ThrowsException_WhenPrecisionIsNotPositive(
        int precision)
    {
        var constructor = () => new NumberValidator(precision);
        constructor.Should().Throw<ArgumentException>();
    }

    [TestCase(-1)]
    [TestCase(int.MinValue)]
    [Description("Конструктор бросает исключение, если scale отрицателен")]
    public void Constructor_ThrowsException_WhenScaleIsNegative(
        int scale)
    {
        var constructor = () => new NumberValidator(1, scale);
        constructor.Should().Throw<ArgumentException>();
    }

    [TestCase(1, 2)]
    [TestCase(2, 2)]
    [Description("Конструктор бросает исключение, если scale больше или равен precision")]
    public void Constructor_ThrowsException_WhenScaleIsGreaterThanOrEqualToPrecision(
        int precision,
        int scale)
    {
        var constructor = () => new NumberValidator(precision, scale);
        constructor.Should().Throw<ArgumentException>();
    }

    [TestCase(10, 5)]
    [TestCase(10, 0)]
    [TestCase(int.MaxValue, int.MaxValue - 1)]
    [Description("Нет исключений в конструкторе, если параметры правильные")]
    public void Constructor_DoesNotThrowException_WithCorrectParams(
        int precision,
        int scale)
    {
        var constructor = () => new NumberValidator(precision, scale);
        constructor.Should().NotThrow();
    }

    [Test]
    [Description("Scale равен 0, если не указан явно")]
    public void Constructor_SetsFractionalPartLengthToZero_ByDefault()
    {
        var numberValidator = new NumberValidator(10);

        numberValidator.IsValidNumber("1").Should().BeTrue();
        numberValidator.IsValidNumber("1.1").Should().BeFalse();
    }

    [Test]
    [Description("Работает с отрицательными числами, если в конструкторе не указано обратное")]
    public void IsValidNumber_ValidatesNegativeNumbers_IfNotDisabledInConstructor()
    {
        var numberValidator = new NumberValidator(20, 10);

        numberValidator.IsValidNumber("-1").Should().BeTrue();
        numberValidator.IsValidNumber("-1.1").Should().BeTrue();
    }

    [Test, Combinatorial]
    [Description("Возвращает true, если строка - удовлетворяющее ограничениям целое число")]
    public void IsValidNumber_ReturnsTrue_IfIntegerNumberIsValid(
        [Values("", "-", "+")] string sign,
        [Values("0", "1", "1234567890", "00", "01")] string number)
    {
        number = sign + number;
        var numberValidator = new NumberValidator(number.Length, 0, false);

        numberValidator.IsValidNumber(number).Should().BeTrue();
    }

    [Test, Combinatorial]
    [Description("Возвращает true, если строка - удовлетворяющее ограничениям число с дробной частью")]
    public void IsValidNumber_ReturnsTrue_IfNumberWithFractionalPartIsValid(
        [Values("", "-", "+")] string sign,
        [Values("0", "1", "1234567890", "00", "01")] string intPart,
        [Values(".", ",")] string decimalPoint,
        [Values("0", "1", "1234567890", "00", "01")] string fracPart)
    {
        var number = sign + intPart + decimalPoint + fracPart;
        var precision = sign.Length + intPart.Length + fracPart.Length;
        var numberValidator = new NumberValidator(precision, fracPart.Length, false);

        numberValidator.IsValidNumber(number).Should().BeTrue();
    }

    [TestCase(1, 0, "00")]
    [TestCase(3, 2, "00.00")]
    [TestCase(1, 0, "11")]
    [TestCase(2, 0, "-11")]
    [TestCase(2, 1, "-1.1")]
    [TestCase(2, 1, "11.1")]
    [TestCase(3, 1, "-11.1")]
    [Description("Возвращает false, если у числа много знаков в целой части")]
    public void IsValidNumber_ReturnsFalse_IfTooManyDigits(
        int precision,
        int scale,
        string number)
    {
        var numberValidator = new NumberValidator(precision, scale, false);

        numberValidator.IsValidNumber(number).Should().BeFalse();
    }

    [TestCase(2, 0, "0.0")]
    [TestCase(3, 1, "0.00")]
    [TestCase(2, 0, "1.1")]
    [TestCase(3, 1, "1.11")]
    [Description("Возвращает false, если у числа много знаков в дробной части")]
    public void IsValidNumber_ReturnsFalse_IfTooManyDigitsInFractionalPart(
        int precision,
        int scale,
        string number)
    {
        var numberValidator = new NumberValidator(precision, scale, false);

        numberValidator.IsValidNumber(number).Should().BeFalse();
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("   ")]
    [Description("Возвращает false, если строка null или состоит из пробелов")]
    public void IsValidNumber_ReturnsFalse_IfStringIsNullOrWhitespace(
        string number)
    {
        var numberValidator = new NumberValidator(10, 9, false);

        numberValidator.IsValidNumber(number).Should().BeFalse();
    }

    [TestCaseSource(nameof(GetNotNumbers))]
    [Description("Возвращает false, если строка не является числом")]
    public void IsValidNumber_ReturnsFalse_IfNotNumber(
        string number)
    {
        var numberValidator = new NumberValidator(number.Length, number.Length - 1, false);

        numberValidator.IsValidNumber(number).Should().BeFalse();
    }

    [TestCase("-0")]
    [TestCase("-1")]
    [TestCase("-1.1")]
    [Description("Не работает с отрицательными числами, если они отключены")]
    public void IsValidNumber_ReturnsFalse_IfNumberIsNegativeAndNegativeNumbersDisabled(
        string number)
    {
        var numberValidator = new NumberValidator(number.Length, number.Length - 1, true);

        numberValidator.IsValidNumber(number).Should().BeFalse();
    }

    [TestCase("0")]
    [TestCase("+0")]
    [TestCase("1")]
    [TestCase("+1")]
    [TestCase("1.1")]
    [TestCase("+1.1")]
    [Description("Работает с положительными числами, если отрицательные отключены")]
    public void IsValidNumber_ReturnsTrue_IfNumberIsPositiveAndNegativeNumbersAreDisabled(
        string number)
    {
        var numberValidator = new NumberValidator(number.Length, number.Length - 1, true);

        numberValidator.IsValidNumber(number).Should().BeTrue();
    }

    [Test]
    [Description("Работает с большими числами")]
    public void IsValidNumber_ReturnsTrue_IfBigIntegerNumberIsValid()
    {
        var length = 10_000;
        var number = new string('1', length);
        var numberValidator = new NumberValidator(length, 0, false);

        numberValidator.IsValidNumber(number).Should().BeTrue();
    }

    [Test]
    [Description("Работает с числами с большой дробной частью")]
    public void IsValidNumber_ReturnsTrue_IfBigNumberWithFractionalPartIsValid()
    {
        var length = 10_000;
        var number = $"0.{new string('1', length - 1)}";

        var numberValidator = new NumberValidator(length, length - 1, false);

        numberValidator.IsValidNumber(number).Should().BeTrue();
    }

    [Test]
    [Description("Проверка на нескольких валидаторах")]
    public void IsValidNumber_Works_WithMultipleNumberValidatorInstances()
    {
        var number = "111.11";

        var numberValidator1 = new NumberValidator(5, 2, false);
        var numberValidator2 = new NumberValidator(3, 1, false);

        numberValidator1.IsValidNumber(number).Should().BeTrue();
        numberValidator2.IsValidNumber(number).Should().BeFalse();
    }

    private static IEnumerable<TestCaseData> GetNotNumbers()
    {
        yield return new TestCaseData("abc");
        yield return new TestCaseData("123abc");
        yield return new TestCaseData("123.abc");
        yield return new TestCaseData("abc.123");
        yield return new TestCaseData(".");
        yield return new TestCaseData(",");
        yield return new TestCaseData("1.");
        yield return new TestCaseData(".1");
        yield return new TestCaseData("--1");
        yield return new TestCaseData("++1");
        yield return new TestCaseData("1.-1");
        yield return new TestCaseData("1.+1");
        yield return new TestCaseData("-.1");
        yield return new TestCaseData("+.1");
        yield return new TestCaseData(" 1");
        yield return new TestCaseData("1 ");
        yield return new TestCaseData("1. ");
    }
}
