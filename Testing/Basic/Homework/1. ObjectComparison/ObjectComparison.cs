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
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70, 99999999,
            new Person("Vasili III of Russia", 28, 170, 60, 99999999, null));
        
        actualTsar.Should().BeEquivalentTo(expectedTsar, config => config.Excluding(x => x.Id).Excluding(x => x.Parent.Id));
        // Я специально для теста добавил одно поле, а сам тест остался неизменен. По сути, его нужно изменять только в случае, если нужно добавить какое-то поле в игнор
        // типа того, как я добавил айдишники людей, т. е. там скорее всего окажутся только поля, которые должны быть уникальными для каждого объекта, а таких вряд ли будет прям много
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70, 99999999,
            new Person("Vasili III of Russia", 28, 170, 60, 99999999, null));

        // Какие недостатки у такого подхода? 
        // Этот код более читаем, чем был изначальный вариант в первом тесте, но его необходимо будет изменять каждый раз, когда мы будем менять класс, который тестируем
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
