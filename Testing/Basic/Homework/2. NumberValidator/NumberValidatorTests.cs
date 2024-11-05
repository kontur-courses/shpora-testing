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
            numberForCheck.IsValidNumber("").Should().BeFalse();
            numberForCheck.IsValidNumber("a.sd").Should().BeFalse();
            //Тесты для проверки данных не подходящих под регулярку

            numberForCheck.IsValidNumber("0.0").Should().BeTrue();
            numberForCheck.IsValidNumber("0").Should().BeTrue();
            numberForCheck.IsValidNumber("+1.23").Should().BeTrue();
            numberForCheck = new NumberValidator(4, 2, false);
            numberForCheck.IsValidNumber("-1.23").Should().BeTrue();
            //Тесты для проверки "нормальных" и пороговых значений
            
            numberForCheck = new NumberValidator(3, 2, true);
            numberForCheck.IsValidNumber("00.00").Should().BeFalse();
            numberForCheck.IsValidNumber("-0.0").Should().BeFalse();
            numberForCheck.IsValidNumber("+0.00").Should().BeFalse();
            //Тесты для проверки неподходящих по условию значений
        }
    }
}