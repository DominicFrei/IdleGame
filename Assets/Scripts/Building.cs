using Realms;

internal class Building : RealmObject
{
    [PrimaryKey]
    internal string Name { get; private set; }
    internal int Level { get; set; }

    internal Building() { }

    internal Building(string name, int level)
    {
        Name = name;
        Level = level;
    }
}