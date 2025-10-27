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
    
        // Перепишите код на использование Fluent Assertions.
        ClassicAssert.AreEqual(actualTsar.Name, expectedTsar.Name);
        ClassicAssert.AreEqual(actualTsar.Age, expectedTsar.Age);
        ClassicAssert.AreEqual(actualTsar.Height, expectedTsar.Height);
        ClassicAssert.AreEqual(actualTsar.Weight, expectedTsar.Weight);
        
        ClassicAssert.AreEqual(expectedTsar.Parent!.Name, actualTsar.Parent!.Name);
        ClassicAssert.AreEqual(expectedTsar.Parent.Age, actualTsar.Parent.Age);
        ClassicAssert.AreEqual(expectedTsar.Parent.Height, actualTsar.Parent.Height);
        ClassicAssert.AreEqual(expectedTsar.Parent.Parent, actualTsar.Parent.Parent);
    
        actualTsar.Should().BeEquivalentTo(expectedTsar, option => option
            .Excluding(it => it.Id)
            .Excluding(it => it.Parent.Id));
    
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
    //Недостатки
    //1. Если тест падает мы просто видим, что ожидалось true, почему упало не видим
    //2. Если добавляем новые свойства в класс человека, то здесь нужно также это учитывать
    //3. Есть рекурсия без обработки случая циклов
    //(в последней строке метода AreEqual вызывается рекурсивно
    //-> появляется риск StackOverflow и из-за циклической ссылки,
    //FluentAssertions может обработать циклические ссылки, если я отмечу .IgnoringCyclicReferences())
    //4. Если бы наш тест был с большим количеством сравнений person, и блок Assert бы не выполнялся на первом,
    //то остальные бы вариант не проверялись. FluentAssertions при этом выполняет всегда все тесты,
    //и потом просто выводит, что выполнилось, а что нет
    //5. Читаемость :)
    
    
    
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