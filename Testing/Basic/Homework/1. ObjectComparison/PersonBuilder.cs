namespace HomeExercise.Tasks.ObjectComparison;

public class PersonBuilder
{
    public const int DEFAULT_AGE = 52;
    public const int DEFAULT_WEIGHT = 70;
    public const int DEFAULT_HEIGHT = 170;
    public const string DEFAULT_NAME = "John Doe";

    private Person? parent;
    private int age = DEFAULT_AGE;
    private string name = DEFAULT_NAME;
    private int height = DEFAULT_HEIGHT;
    private int weight = DEFAULT_WEIGHT;

    private PersonBuilder() { }

    public static PersonBuilder APerson() => new();

    public Person Build() => new(name, age, height, weight, parent);

    public static Person ATsarParent() => APerson()
        .WithName("Vasili III of Russia")
        .WithAge(28)
        .WithHeight(170)
        .WithWeight(60)
        .Build();
    
    public static Person ATsar() => APerson()
        .WithName("Ivan IV The Terrible")
        .WithAge(54)
        .WithHeight(170)
        .WithWeight(70)
        .WithParent(ATsarParent())
        .Build();
    
    # region Property Setters
    public PersonBuilder WithName(string inputName)
    {
        name = inputName;
        return this;
    }

    public PersonBuilder WithAge(int inputAge)
    {
        age = inputAge;
        return this;
    }

    public PersonBuilder WithHeight(int inputHeight)
    {
        height = inputHeight;
        return this;
    }

    public PersonBuilder WithWeight(int inputWeight)
    {
        weight = inputWeight;
        return this;
    }

    public PersonBuilder WithParent(Person inputParent)
    {
        parent = inputParent;
        return this;
    }
    # endregion Property Setters
}