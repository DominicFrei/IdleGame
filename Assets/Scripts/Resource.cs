using Realms;

public class Resource : RealmObject
{
    public enum Type
    {
        NotSet,
        Metal,
        Crystal
    }

    [PrimaryKey]
    public string Name { get; private set; }
    public int Amount { get; set; }

    public Resource() { }

    public Resource(string name, int amount)
    {
        Name = name;
        Amount = amount;
    }
}