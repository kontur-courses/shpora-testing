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
            .Excluding(x => x.DeclaringType == typeof(Person) && x.Name == nameof(Person.Id))
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
        ClassicAssert.True(AreEqual(actualTsar, expectedTsar));
        
        // Недостаток такого подхода заключается в том, что мы добавялем метод,
        // который работает неизвестно как и его надо посмотреть,
        // чтобы понять, как именно сравниваются экзмепляры класса.
        // Также добавление новых полей в Person приведет к тому,
        // что метод проверит только ранее указанные поля,
        // новые придется вводить в ручную,
        // в моем решении проверяются все поля за исключением поля Id, что не мешает расширяемости.
        // И мое решение короче, в одну строку, и понятно что делается по названию методов
        // Такой подход не работает правильно с циклическими ссылками и не отображает сообщения об ошибке, непонятно что именно не так
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
