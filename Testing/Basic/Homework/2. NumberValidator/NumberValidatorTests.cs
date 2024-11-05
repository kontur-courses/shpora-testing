using NUnit.Framework;
using NUnit.Framework.Legacy;
using FluentAssertions;
using FluentAssertions.Execution;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [Test]
    public void Test()
    {
        using (new AssertionScope()) //Позволяет выполнить все проверки даже если какая-то упала
        //Информация об ошибках выдастся по заверщению всех тестов
        {
            Action act = () => new NumberValidator(5, 2, true);
            act.Should().NotThrow(because: "Целая и дробная часть больше нуля, цифры целой части больше чем в дробной");

            act = () => new NumberValidator(-1, 2, true);
            act.Should().Throw<ArgumentException>(because: "Целая часть меньше нуля");

            act = () => new NumberValidator(1, -1, true);
            act.Should().Throw<ArgumentException>(because: "Дробная часть меньше нуля");
        
            act = () => new NumberValidator(1, 1, false);
            act.Should().Throw<ArgumentException>(because: "Дробная часть равна целой части");
            //Тесты для проверки корректности инициализации
            
            NumberValidator numberForCheck = new NumberValidator(4, 2, true);

            numberForCheck.IsValidNumber("").Should().BeFalse(because: "Ввод пуст");
            numberForCheck.IsValidNumber(null).Should().BeFalse(because: "Ввод пуст");

            numberForCheck.IsValidNumber("a.sd").Should().BeFalse(because: "Используются не арабские цифры");
            numberForCheck.IsValidNumber("సున్న.నాలుగు").Should().BeFalse(because: "Используются не арабские цифры");
            numberForCheck.IsValidNumber("IV.III").Should().BeFalse(because: "Используются не арабские цифры");
            numberForCheck.IsValidNumber("девять.ноль").Should().BeFalse(because: "Используются не арабские цифры");

            numberForCheck.IsValidNumber("9dot8").Should().BeFalse(because: "Некорректный разделитель");
            numberForCheck.IsValidNumber("3..2").Should().BeFalse(because: "Некорректный разделитель");
            numberForCheck.IsValidNumber("1/2").Should().BeFalse(because: "Некорректный разделитель");

            numberForCheck.IsValidNumber("plus9").Should().BeFalse(because: "Некорректный знак");
            numberForCheck.IsValidNumber("++1").Should().BeFalse(because: "Некорректный знак");
            //Тесты для проверки данных не подходящих под регулярку

            numberForCheck.IsValidNumber("0.0").Should().BeTrue();
            numberForCheck.IsValidNumber("0").Should().BeTrue();
            numberForCheck.IsValidNumber("+1.23").Should().BeTrue();
            numberForCheck.IsValidNumber("+1,23").Should().BeTrue(because: "Знак запятой допустим");
            numberForCheck = new NumberValidator(4, 2, false);
            numberForCheck.IsValidNumber("-1.23").Should().BeTrue();
            //Тесты для проверки "нормальных" и пороговых значений
            
            numberForCheck = new NumberValidator(3, 2, true);
            numberForCheck.IsValidNumber("00.00").Should().BeFalse(because: "Используется больше символов чем положено");
            numberForCheck.IsValidNumber("-0.0").Should().BeFalse(because: "Число отрицательное, хотя флаг не выставлен");
            numberForCheck.IsValidNumber("+0.00").Should().BeFalse(because: "Используется больше символов чем положено");
            //Тесты для проверки неподходящих по условию значений
        }
    }
}