using NUnit.Framework;

namespace Advanced.Classwork.ThingCache;


[TestFixture]
public class ThingCache_Should
{
    private IThingService thingService;
    private ThingCache thingCache;

    private const string thingId1 = "TheDress";
    private Thing thing1 = new Thing(thingId1);

    private const string thingId2 = "CoolBoots";
    private Thing thing2 = new Thing(thingId2);

    // Метод, помеченный атрибутом SetUp, выполняется перед каждым тестов
    [SetUp]
    public void SetUp()
    {
        //thingService = A...
        thingCache = new ThingCache(thingService);
    }

    // TODO: Написать простейший тест, а затем все остальные

    // Пример теста
    [Test]
    public void GiveMeAGoodNamePlease()
    {
    }

    /** Проверки в тестах
     * Assert.AreEqual(expectedValue, actualValue);
     * actualValue.Should().Be(expectedValue);
     */

    /** Синтаксис AAA
     * Arrange:
     * var fake = A.Fake<ISomeService>();
     * A.CallTo(() => fake.SomeMethod(...)).Returns(true);
     * Assert:
     * var value = "42";
     * A.CallTo(() => fake.TryRead(id, out value)).MustHaveHappened();
     */

    /** Синтаксис out
     * var value = "42";
     * string _;
     * A.CallTo(() => fake.TryRead(id, out _)).Returns(true)
     *     .AssignsOutAndRefParameters(value);
     * A.CallTo(() => fake.TryRead(id, out value)).Returns(true);
     */

    /** Синтаксис Repeat
     * var value = "42";
     * A.CallTo(() => fake.TryRead(id, out value))
     *     .MustHaveHappened(Repeated.Exactly.Twice)
     */
}