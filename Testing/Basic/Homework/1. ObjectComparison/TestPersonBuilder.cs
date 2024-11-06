namespace HomeExercise.Tasks.ObjectComparison;

public class TestPersonBuilder
{
	private string name = string.Empty;
	private int age;
	private int height;
	private int weight;
	private Person? parent;

	private TestPersonBuilder() { }

	public static Person TestUser() => Create().Build();

	public static TestPersonBuilder Create()
	{
		return new TestPersonBuilder();
	}

	public Person Build() => new(name, age, height, weight, parent);

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
}