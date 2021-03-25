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
    internal int Amount { get; set; } = 0;

    internal int Available { get; set; } = 0;

    internal Unit() { }

    internal Unit(string name)
    {
        Name = name;
    }
}
