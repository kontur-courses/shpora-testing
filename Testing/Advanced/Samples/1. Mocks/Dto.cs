namespace Advanced.Samples.Mocks;

public record Dto(string S);
public class ComplexDto
{
    public readonly Dto? dto;
    public readonly string? s;

    public ComplexDto() { }

    public ComplexDto(Dto dto)
    {
        this.dto = dto;
        s = "Created with complex constructor";
    }
}