using Realms;

internal class Unit : RealmObject
{
    internal enum Type
    {
        NotSet,
        Worker
    }

    [PrimaryKey]
    internal string Name { get; private set; } = Type.NotSet.ToString();
    internal int Amount { get; set; } = default;

    internal int Available { get; set; } = default;

    internal Unit() { }

    internal Unit(string name, int amount, int available)
    {
        Name = name;
        Amount = amount;
        Available = available;
    }
}
