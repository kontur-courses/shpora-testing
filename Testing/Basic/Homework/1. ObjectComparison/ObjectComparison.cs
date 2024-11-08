using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.ObjectComparison;

public class ObjectComparison
{
    [Test]
    [Description("Проверка текущего царя")]
    [Category("ToRefactor")]
    public void TsarRegistry_ShouldBeEqual_WithCurrentTsar()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();

        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        actualTsar.Should().BeEquivalentTo(expectedTsar,
            options => options
                .Excluding(x => x.Path.EndsWith("Id"))
                .AllowingInfiniteRecursion()
                .IncludingProperties());
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
        /*
         * Недостатки:
         *  1. Такая реализация теста не даёт никакой конкретики при обвале теста,
         *      он скажет только Success или Failed, но не причину, а точнее мы не узнаем
         *      какое конкретно поле класса Person показало несовпадение, в отличие от моего решения.
         *  2. Масштабируемость. Метод AreEqual работает только для текущей реализации Person,
         *      и при его изменении (добавление или удаление полей), нужно будет добавлять/убирать строки
         *      и в AreEqual. Также можно и вовсе обойтись без него, что заметно повысит читаемость кода.
         * 3. Ликвидность. Если нам потребуется сравнить каких-нибудь двух Person без определённого поля,
         *      тогда в текущем классе потребуется несколько реализаций AreEqual, что очень плохо.
         */
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