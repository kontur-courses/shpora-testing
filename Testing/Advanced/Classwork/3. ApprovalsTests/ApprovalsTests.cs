using Newtonsoft.Json;
using NUnit.Framework;

namespace Advanced.Classwork.ApprovalsTests;

[TestFixture]
[Explicit]
public class ApprovalsTests
{
    [Test]
    public void Puzzle15_InitialState()
    {
        var puzzle15 = new Puzzle15();
        // TODO: assert
        // HINT: Approvals.Verify
    }

    #region Как это работает

    // DiffReporter - выбирает наилучший имеющийся в наличии способ сравнения
    // Approvals.Verify создает файл *.received.txt с текущим значением и сравнивает его с файлом *.approved.txt 

    #endregion

    [Test]
    public void Puzzle15_MoveRight()
    {
        var puzzle15 = new Puzzle15();
        puzzle15.MoveRight();
        
        // TODO: assert
    }

    [Test]
    public void ApproveProductData()
    {
        var product = new Product
        {
            Id = Guid.Empty,
            Name = "Name",
            Price = 3.14m,
            UnitsCode = "112"
        };
        //TODO: Verify product
        //TODO: Exclude TemporaryData
        //HINT: stateprinter.Configuration.Project.Exclude
    }

    [Test]
    public void ProductData_IsJsonSerializable()
    {
        Product original = new Product
        {
            Id = Guid.Empty,
            Name = "Name",
            Price = 3.14m,
            UnitsCode = "112",
            TemporaryData = "qwe"
        };
        string serialized = JsonConvert.SerializeObject(original);
        Product deserialized = JsonConvert.DeserializeObject<Product>(serialized);
        //TODO: Проверить, что сериализуется корректно!
        //HINT: Should().BeEquivalentTo с опциями в FluentAssertions
    }
}
