using NUnit.Framework;
using NUnit.Framework.Legacy;
using FluentAssertions;

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
        
        expectedTsar.Should().BeEquivalentTo(actualTsar, options => 
             options.Excluding(x => x.Id)
                    .Excluding(x => x.Parent.Id));
    }
    
    //Основные проблемы, которые я выделелил:
    //1. Расширяемость.
    //В сравнении с Fluent Assertions, который автоматически проверяет поля у класса, этот вариант использует ручную проверку каждого поля,
    //что будет ломать тест после добавления новых полей в классе.
    //В таком случае необходимо будет переписывать тест, что займет больше времени, относительно решения выше. 
    //2. Отсутствие отчетности.
    //При падении теста будет непонятно, где находится различие, т.к. будет выведено только True или False
    //3. Неуниверсальность.
    //Такое решение не подходит для написания тестов на большое количество объектов, т.к. необходимо писать метод AreEqual для каждого из объектов.
    //4. Читаемость.
    // Гораздо проще воспринимать несколько методов в одной строке, чем множество дублирующихся строк.
    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));
        
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
