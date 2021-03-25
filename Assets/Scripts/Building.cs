using Realms;

internal class Building : RealmObject
{
    [PrimaryKey]
    internal string Name { get; private set; }
    internal int CurrentLevel { get; set; } = 1;
    internal int MaximumLevel { get; set; } = 1;
    internal int WorkersAssigend { get; set; } = 0;

    internal Building() { }

    internal Building(string name)
    {
        Name = name;
    }
}