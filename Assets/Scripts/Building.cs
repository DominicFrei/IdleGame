using Realms;

internal class Building : RealmObject
{
    [PrimaryKey]
    internal string Name { get; private set; } = Resource.Type.NotSet.ToString();
    internal int Level { get; set; } = 1;

    internal Building() { }

    internal Building(string name, int level)
    {
        Name = name;
        Level = level;
    }
}