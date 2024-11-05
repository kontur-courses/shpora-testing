namespace HomeExercise.Tasks.ObjectComparison;

public class TestPersonBuilder
{
	private const string DEFAULT_NAME = "";
	private const int DEFAULT_AGE = 0;
	private const int DEFAULT_HEIGHT = 0;
	private const int DEFAULT_WEIGHT = 0;

	private string name = DEFAULT_NAME;
	private int age = DEFAULT_AGE;
	private int height = DEFAULT_HEIGHT;
	private int weight = DEFAULT_WEIGHT;
	private Person? parent = null;

	private TestPersonBuilder()
	{
	}

	public static TestPersonBuilder Create()
	{
		return new TestPersonBuilder();
	}

	public TestPersonBuilder WithName(string newName)
	{
		name = newName;
		return this;
	}

	public TestPersonBuilder WithAge(int newAge)
	{
		age = newAge;
		return this;
	}

	public TestPersonBuilder WithHeight(int newHeight)
	{
		height = newHeight;
		return this;
	}

	public TestPersonBuilder WithWeight(int newWeight)
	{
		weight = newWeight;
		return this;
	}

	public TestPersonBuilder WithParent(Person newParent)
	{
		parent = newParent;
		return this;
	}

	public Person Build()
	{
		return new Person(name, age, height, weight, parent);
	}

	public static Person TestUser() => Create().Build();
}