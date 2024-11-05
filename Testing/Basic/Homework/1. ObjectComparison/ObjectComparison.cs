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

        actualTsar.Should().BeEquivalentTo(expectedTsar, options =>
            options.Excluding(o => o.Name == nameof(Person.Id) && o.DeclaringType == typeof(Person)));
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Какие недостатки у такого подхода? 
        /*
         1) Метод AreEqual проверяет только фиксированые параметры в Person, поэтому
            если класс Person будет измненён, то тесты предётся переписывать,
            добавляя новые или удаляя старые поля.
         2) Метод AreEqual возвращает True или False, поэтому даже если только одно поле
            не пройдёт проверку, всё что будет понятно, что проверка не прошла, а
            что именно было не так, узнать не получится.
         3) Даже если переписывание тестов не является проблемой (см. 1)), то при добавлении
            большего количества полей, особенно если типом новых полей будут другие классы
            со своими полями, метод AreEqual рискует разрастись до монструозных масштабов,
            из-за чего его станет очень сложно читать.
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
