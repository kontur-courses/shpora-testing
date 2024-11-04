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
            new Person("Vasili III of Russia", 28, 170, 60, null));
        
        actualTsar.Should().BeEquivalentTo(expectedTsar, options => options
                .Excluding(x => x.Name == nameof(Person.Id) && 
                                x.DeclaringType == typeof(Person))
                .IgnoringCyclicReferences());
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Какие недостатки у такого подхода? 
        /* 1. Ограниченная расширяемость:
         * Когда мы добавим что-то в класс Person, придется метод AreEqual обновлять вручную.
         * Тем самым будет усложняться поддержка.
         *2. Недостаточная читаемость и ясность:
         *  Загроможденный код: AreEqual содержит явные проверки каждого свойства,
         * тем самым делая код громоздким и менее понятным.
         * Также логика сравнения скрыта внутри метода, тем самым затрудняя понимания сравнения объектов.
         * 3. Отсутствие подробных сообщений об ошибках:
         *  True или False не дает никакой детальной информации об ошибке, что усложнит исправления кода.
         */
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
