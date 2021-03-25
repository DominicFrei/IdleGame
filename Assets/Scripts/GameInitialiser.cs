using Realms;

internal sealed class GameState
{
    internal static void Initialise()
    {
        Realm realm = Realm.GetInstance();
        realm.Write(() =>
        {
            realm.RemoveAll();
            realm.Add(new Resource(Resource.Type.Metal.ToString(), Balancing.MetalStartingAmount));
            realm.Add(new Resource(Resource.Type.Crystal.ToString(), Balancing.CrystalStartingAmount));
            realm.Add(new Building(Resource.Type.Metal.ToString()));
            realm.Add(new Building(Resource.Type.Crystal.ToString()));
            Unit workers = new Unit(Unit.Type.Worker.ToString())
            {
                Amount = Balancing.CnitialWorkerCount,
                Available = Balancing.CnitialWorkerCount
            };
            realm.Add(workers);
        });
    }
}