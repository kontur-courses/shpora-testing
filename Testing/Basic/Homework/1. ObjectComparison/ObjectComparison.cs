using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.ObjectComparison;
public class ObjectComparison
{
    [Test]
    [Description("Проверка текущего царя")]
    [Category("ToRefactor")]
    public void CheckCurrentTsar()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();

        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60,
                new Person("Ivan III", 65, 170, 80, null)));


		actualTsar.Should().BeEquivalentTo(expectedTsar, 
            options => options
            .Excluding(p => p.Path.EndsWith("Id")));
	}
	/*
	 * Преимущества подхода:
	 * 1)   Хорошая информативность: при непрохождении теста ясно показывается, какие поля не совпали.
	 * 2)   Хорошая расширяемость: при добавлении или удалении полей в классе, нужно внести минимум изменений в тесте.
	 * 3)   Хорошая читаемость: из-за меньшего объема кода и понятного названия методов улучшается читаемость кода.
	 */

	[Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
			new Person("Vasili III of Russia", 28, 170, 60,
				new Person("Ivan III", 65, 170, 80, null)));

        // Какие недостатки у такого подхода? 
        ClassicAssert.True(AreEqual(actualTsar, expectedTsar));
    }
	/*
     * Недостатки:
     * 1)   При измнении класса Person необходимо будет изменять метод AreEqual.
     * 2)   ClassicAssert.True() принимает на вход булевое выражение, за счет чего сравнение происходит с True и если тест не проходит,
     *      то в сообщении будет написано что результат не совпал, без дополнительной информации что именно пошло не так.
     * 3)   Функция сравнения классов не должна определятся внутри класса с тестами.
     */

	private bool AreEqual(Person? actual, Person? expected)
    {
        if (actual == expected) return true;
        if (actual == null || expected == null) return false;
        return
            actual.Name == expected.Name
            && actual.Age == expected.Age
            && actual.Height == expected.Height
            && actual.Weight == expected.Weight
            && AreEqual(actual.Parent, expected.Parent);
    }
}
