namespace HomeExercise.Tasks.NumberValidator;

public class TestNumberValidatorBuilder
{
    public const int DEFAULT_PRECISION = 5;
    public const int DEFAULT_SCALE = 3;
    public const bool DEFAULT_ONLY_POSITIVE = true;
    private int precision = DEFAULT_PRECISION;
    private int scale = DEFAULT_SCALE;
    private bool onlyPositive = DEFAULT_ONLY_POSITIVE;

    public TestNumberValidatorBuilder WithPrecision(int precision)
    {
        this.precision = precision;
        return this;
    }
    
    public TestNumberValidatorBuilder WithScale(int scale)
    {
        this.scale = scale;
        return this;
    }
    
    public TestNumberValidatorBuilder WithOnlyPositive(bool onlyPositive)
    {
        this.onlyPositive = onlyPositive;
        return this;
    }
    
    public NumberValidator Build()
    {
        return new NumberValidator(precision, scale, onlyPositive);
    }
}