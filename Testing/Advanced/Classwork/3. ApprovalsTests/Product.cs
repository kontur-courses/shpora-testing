using System.Text.Json.Serialization;

namespace Advanced.Classwork.ApprovalsTests;

public class Product
{
    public Guid Id { get; set; }
    [JsonIgnore]
    public string TemporaryData { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string UnitsCode { get; set; }
}