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
    internal string Name { get; private set; } = Type.NotSet.ToString();
    internal int Amount { get; set; } = default;
    internal int AssignedWorkers { get; set; } = default;
    internal float CycleProgress { get; set; } = default;

    internal Resource() { }

    internal Resource(string name, int amount)
    {
        Name = name;
        Amount = amount;
    }
}