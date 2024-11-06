namespace HomeExercise.Tasks.ObjectComparison;


public class Person
{
    public static int IdCounter = 0;
    public int Age, Height, Weight;
    public string Name;
    public Person Parent;
    public int Id;

    public Person(string name, int age, int height, int weight, Person parent)
    {
        Id = IdCounter++;
        Name = name;
        Age = age;
        Height = height;
        Weight = weight;
        Parent = parent;
    }

    public override bool Equals(object obj)
    {
        if (obj is Person person) return this.GetHashCode() == person.GetHashCode();
        return false;
    }

    public int GetHashCode()
    {
        var result = 0;
        var degree = 0;
        foreach (var field in GetType().GetFields())
        {
            if (field.IsStatic || field.Name == "Id") continue;
            result += (int)Math.Pow(field.GetValue(this).GetHashCode(), ++degree);
        }
    
        return result;
    }
}