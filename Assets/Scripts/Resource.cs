using Realms;

internal class Resource : RealmObject
{
    internal enum Type
    {
        NotSet,
        Metal,
        Crystal
    }

    [PrimaryKey]
    internal string Name { get; private set; }
    internal int Amount { get; set; }

    internal Resource() { }

    internal Resource(string name, int amount)
    {
        Name = name;
        Amount = amount;
    }
}