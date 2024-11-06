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
            .Excluding(x => x.DeclaringType == typeof(Person) && x.Path.Contains("Id")));
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));
        ClassicAssert.True(AreEqual(actualTsar, expectedTsar));
    }

    // Недостатки этого подхода:
    // 1) Нечитаемость, так как логика скрыта в методе AreEqual,
    // сторонний наблюдатель из теста даже не поймет, что Id в сравнении не учитывается,
    // пока не провалится в метод
    // + сам ClassicAssert плохо читается в сравнении с FluentAssertion
    // 
    // 2) Не расширяемый: при добавлении новых полей и свойств нужно будет править этот метод,
    // тесты будут выполняться неверно 
    //
    // 3) Повторяющийся код сравнения свойств, выглядит не очень
    //
    // 4) Кода в таком подходе в несколько раз больше, чем в первом тесте
    //
    // Код с использованием FluentAssertion максимально читаемый
    // (мы сразу видим, какие свойства исключаются при сравнении),
    // гибкий и расширяемый, а также достаточно короткий.
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
