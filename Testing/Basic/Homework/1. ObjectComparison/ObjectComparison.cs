using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.ObjectComparison;

/*
 * Изучив документацию FluentAssertions я обнаружил метод BeEquivalentTo(),
 * который умеет сравнивать объекты по их содержимому, то есть не нужно
 * вручную проверять эквивалентность каждого свойства по отдельности
 * 
 * BeEquivalentTo() вторым аргументом принимает Func<EquivalencyOptions<TExpectation>, EquivalencyOptions<TExpectation>>
 * с помощью него можно настроить правила сравнения, например: в данном случае не нужно
 * учитывать поле Id, чтобы исключить его из сравнения есть options.Excluding(x => x.Property))
 * и ExcludingMembersNamed("Id"). Их различие в том, что Excluding(x => x.Property) исключает только сравнение
 * в корневом элементе, у остальных вложенных Person он будет учитывать поле Id, что не является корректным поведеним.
 * 
 * Так как ExcludingMembersNamed() не был доступен в текущей версии FluentAssertions, я уставновил последний релиз
 * 
 * Предложенное решение в CheckCurrentTsar_WithCustomEquality() значительно хуже, потому что при изменении Person 
 * необходимо менять AreEqual() в тестовом проекте, появляется неудобная связанность. BeEquivalentTo() находит новые 
 * свойства автоматически. Так же при неудачном сравнении в тесте CheckCurrentTsar_WithCustomEquality() мы просто получим
 * Expected: True But was: False, а BeEquivalentTo() дает понятное сообщение о том, что именно отличается в объектах.
*/
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
            options.ExcludingMembersNamed("Id"));
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
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
