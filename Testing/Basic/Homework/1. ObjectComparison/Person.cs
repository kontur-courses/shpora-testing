namespace HomeExercise.Tasks.ObjectComparison;


public class Person
{
    public static int IdCounter = 0;
    public int Age, Height, Weight;
    public uint Wealth;
    public string Name;
    public Person Parent;
    public int Id;

    public Person(string name, int age, int height, int weight, uint wealth, Person parent)
    {
        Id = IdCounter++;
        Name = name;
        Age = age;
        Height = height;
        Weight = weight;
        Parent = parent;
        Wealth = wealth;
    }
}