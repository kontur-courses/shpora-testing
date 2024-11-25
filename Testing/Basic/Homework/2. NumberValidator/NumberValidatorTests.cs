
using System.Collections;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [Test]
    [Description("Проверяем, что конструктор NumberValidator не кидает исключения если входные параметры" +
                 "заданы верно: precision > 0 и 0 <= scale < precision")]
    [TestCase(1, 0, true)]
    [TestCase(1, 0, false)]
    [TestCase(12, 5, true)]
    [TestCase(12, 5, false)]
    public void NumberValidator_DoesNotThrowOnCreation_WithCorrectArguments(
        int precision,
        int scale,
        bool onlyPositive)
    {
        new Action(() => new NumberValidator(precision, scale, onlyPositive))
            .Should()
            .NotThrow();
    }

    [Test]
    [Description("Проверяем, что конструктор NumberValidator кидает ArgumentException, " +
                 "если аргумент scale имеет значение больше или равное precision")]
    [TestCase(1, 2, false)]
    [TestCase(1, 2, true)]
    [TestCase(7, 9, true)]
    [TestCase(7, 9, false)]
    [TestCase(7, 7, true)]
    [TestCase(7, 7, false)]
    public void NumberValidator_ThrowsOnCreation_WithScale_BeingMoreOrEqualTo_Precision(
        int precision,
        int scale,
        bool onlyPositive)
    {
        new Action(() => new NumberValidator(precision, scale, onlyPositive))
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("precision must be a non-negative number less or equal than precision");
    }

    [Test]
    [Description("Проверяем, что конструктор NumberValidator кидает ArgumentException, " +
                 "если аргумент precision имеет значение меньше или равное нулю")]
    [TestCase(-1, 2, false)]
    [TestCase(-1, 2, true)]
    [TestCase(0, 0, false)]
    [TestCase(0, 0, true)]
    public void NumberValidator_ThrowsOnCreation_WithPrecision_BeingLessOrEqualTo_Zero(
        int precision,
        int scale,
        bool onlyPositive)
    {
        new Action(() => new NumberValidator(precision, scale, onlyPositive))
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("precision must be a positive number");
    }

    [Test]
    [Description("Проверяем, что конструктор NumberValidator кидает ArgumentException, " +
                 "если аргумент scale имеет значение меньше нуля")]
    [TestCase(1, -2, true)]
    [TestCase(1, -2, false)]
    [TestCase(7, -1, true)]
    [TestCase(7, -1, false)]
    public void NumberValidator_ThrowsOnCreation_WithScale_BeingLessThan_Zero(
        int precision,
        int scale,
        bool onlyPositive)
    {
        new Action(() => new NumberValidator(precision, scale, onlyPositive))
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("precision must be a non-negative number less or equal than precision");
    }

    [Test]
    [Description("Проверяем, что метод IsValidNumber возвращает: " +
                 "1) true для корректных чисел если они положительны (параметр onlyPositive == true)" +
                 "2) false для некорректных и любых отрицательных чисел (параметр onlyPositive == true)" +
                 "3) true для корректных чисел с любым знаком (параметр onlyPositive == false)" +
                 "4) false для некорректных чисел с любым знаком (параметр onlyPositive == false)")]
    [TestCaseSource(nameof(OnlyPositiveValidNumberCases))]
    [TestCaseSource(nameof(OnlyPositiveInvalidNumberCases))]
    [TestCaseSource(nameof(GeneralValidNumberCases))]
    [TestCaseSource(nameof(GeneralInvalidNumberCases))]
    public void IsValidNumber_Returns_ExpectedResult(
        int precision,
        int scale,
        bool onlyPositive,
        string number,
        bool expectedResult)
    {
        new NumberValidator(precision, scale, onlyPositive)
            .IsValidNumber(number)
            .Should()
            .Be(expectedResult);
    }

    public static object[] OnlyPositiveValidNumberCases()
    {
        var onlyPositiveCasesWithScaleOfZero = new[]
        {
            "1", "+0", "01", "00"
        }.Select(number => new object[]
        {
            2, 0, true, number, true
        });
            
        var onlyPositiveCases = new[]
        {
            "0.0", "0,0", "0", "1.23", "+0.00", "01.10", "+0,00", "1,0"
        }.Select(number => new object[]
        {
            4, 2, true, number, true
        });

        return onlyPositiveCasesWithScaleOfZero
            .Concat(onlyPositiveCases)
            .ToArray<object>();
    }

    public static object[] OnlyPositiveInvalidNumberCases()
    {
        var numbers = new[]
        {
            "000.0", "-0.00", "+0,00", "+1.23", "-1,23", "a.sd",
            "0.000", "0.", ",00", "", null, "    ", " ", "-", "+",
            "   1.5", "0,2   ", "0  . 11"
        };

        var onlyPositiveCasesWithScaleOfZero =
            new[]
                {
                    "-1", "+000", "0.0", "3.14", "-0.1"
                }
                .Concat(numbers)
                .Select(number => new object[]
                {
                    3, 0, true, number, false
                });

        var onlyPositiveCases =
            numbers.Select(number => new object[]
            {
                3, 2, true, number, false
            });

        return onlyPositiveCasesWithScaleOfZero
            .Concat(onlyPositiveCases)
            .ToArray<object>();
    }

    public static object[] GeneralValidNumberCases()
    {
        var onlyPositiveCasesWithScaleOfZero = new[]
        {
            "1", "+0", "01", "00", "-0", "+2", "-2"
        }.Select(number => new object[]
        {
            2, 0, false, number, true
        });
            
        var onlyPositiveCases = new[]
        {
            "0.0", "0", "-0", "-1.23", "+0.00", "01.10", "-1,10", "+10.0"
        }.Select(number => new object[]
        {
            4, 2, false, number, true
        });

        return onlyPositiveCasesWithScaleOfZero
            .Concat(onlyPositiveCases)
            .ToArray<object>();
    }

    public static object[] GeneralInvalidNumberCases()
    {
        var numbers = new[]
        {
            "000.0", "-0.00", "+0,00", "+1.23", "-1,23", "a.sd",
            "0.000", "0.", ",00", "", null, "    ", " ", "-,0",
            "+0,", "   1.5", "0,2   ", "0  . 11"
        };

        var onlyPositiveCasesWithScaleOfZero =
            new[]
                {
                    "-1.0", "+000", "0.0", "3.14", "-0.1", "1111"
                }
                .Concat(numbers)
                .Select(number => new object[]
                {
                    3, 0, false, number, false
                });

        var onlyPositiveCases =
            numbers.Select(number => new object[]
            {
                3, 2, false, number, false
            });

        return onlyPositiveCasesWithScaleOfZero
            .Concat(onlyPositiveCases)
            .ToArray<object>();
    }
}