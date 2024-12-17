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
        var expectedTsar = PersonBuilder.ATsar();
        var actualTsar = TsarRegistry.GetCurrentTsar();

        actualTsar.Should().BeEquivalentTo(expectedTsar, options => options
            .Excluding(x => x.Id)
            .Excluding(x => x.Parent.Id));
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        /*
         * Такой подход очевидно плох по ряду причин
         * 1. Тут есть потенциальная бесконечная рекурсия. Представим ситуацию, когда
         * actual.Parent = actual и expected.Parent = expected. Хоть случай и лишен практического
         * смысла, проблема все же есть.
         *
         * 2. Метод AreEqual проверяет ограниченное множество полей. Представим, что мы хотим добавить
         * новое поле для объекта Person. В таком случаи придется дописывать условия, для проверки нового поля
         *
         * 3. Пожалуй самое страшное. Если класс Person будет содержать любой другой объект, кроме Person, то
         * логика метода AreEqual кратно усложнится. Проверка вложенных объектов отличного от Person будет
         * невозможна без внедрения кучи if-ов на проверку типа, которые будут запускать рекурсивную проверку
         * вложенных типов, что сделает из метода if-вый ад
         *
         * 4. Сравнение с True/False не информативно. Очевидно, что при провале теста нам хотелось бы знать, что
         * конкретно пошло не так. Но из-за специфики работы метода AreEqual максимум информации, что мы узнаем -
         * равны объекты или нет. Тест не скажет, какие поля не совпали и какие значения лежали в них. Только да или нет
         */
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Какие недостатки у такого подхода? 
        ClassicAssert.True(AreEqual(actualTsar, expectedTsar));
    }

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
