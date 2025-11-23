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
        
        actualTsar.Should().BeEquivalentTo(expectedTsar, options => options.ExcludingMembersNamed(nameof(Person.Id)).AllowingInfiniteRecursion());
        
        // Комментарий от студента: 
        // Можно было написать вот так (ниже код), но тут есть проблемы.
        // Я не уверен в строке 36, так как сравнение скорее всего идёт по ссылке, и тогда оно будет верным если оба null.
        // И в идеале мы должны сравнивать вглубь. А что если там много родителей? У отца есть отец и т.д.
        // Тогда такое решение нам не подойдёт. В решении выше таких проблем нет.
        
        // actualTsar.Should().Satisfy<Person>(tsar =>
        // {
        //     tsar.Name.Should().Be(expectedTsar.Name);
        //     tsar.Age.Should().Be(expectedTsar.Age);
        //     tsar.Height.Should().Be(expectedTsar.Height);
        //     tsar.Weight.Should().Be(expectedTsar.Weight);
        //     
        //     tsar.Parent.Name.Should().Be(expectedTsar.Parent.Name);
        //     tsar.Parent.Age.Should().Be(expectedTsar.Parent.Age);
        //     tsar.Parent.Height.Should().Be(expectedTsar.Parent.Height);
        //     tsar.Parent.Parent.Should().Be(expectedTsar.Parent.Parent);
        // });
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
        
        // Ответ от студента: Если тест упадёт, не будет конкретно понятно, в чём проблема, ведь мы просто получим false.
        // Прошлый подход точно покажет, где проблема, например если возраст не совпал, он об этом скажет.
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
