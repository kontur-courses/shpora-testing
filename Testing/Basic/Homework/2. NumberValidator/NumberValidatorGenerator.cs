namespace HomeExercise.Tasks.NumberValidator;

public class ValidatorRegistry
{
    public static NumberValidator GetDefaultNumberValidator(bool onlyPositive = false)
    {
        return new NumberValidator(5,3, onlyPositive);
    }
}