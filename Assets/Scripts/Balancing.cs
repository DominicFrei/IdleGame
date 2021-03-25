internal sealed class Balancing
{
    // Starting Resources
    internal static readonly int MetalStartingAmount = 100000;
    internal static readonly int CrystalStartingAmount = 100000;
    internal static readonly int CnitialWorkerCount = 3;

    private static readonly int MetalIncrementPerLevel = 5;
    private static readonly int CrystalIncrementPerLevel = 5;

    private static readonly float MetalCounterCycleSpeed = 0.2f;
    private static readonly float CrystalCounterCycleSpeed = 0.085f;

    private static readonly int MetalMineUpgradeMetalCostPerLevel = 100;
    private static readonly int MetalMineUpgradeCrystalCostPerLevel = 50;
    private static readonly int CrystalMineUpgradeMetalCostPerLevel = 100;
    private static readonly int CrystalMineUpgradeCrystalCostPerLevel = 50;

    private static readonly int MetalCostPerWorker = 100;
    private static readonly int CrystalCostPerWorker = 100;

    private static readonly string UnhandledCaseExceptionMessage = "Unhandled case.";

    public static int MetalCostForNextWorker(int totalWorkerCount) => MetalCostPerWorker * (totalWorkerCount + 1);
    public static int CrystalCostForNextWorker(int totalWorkerCount) => CrystalCostPerWorker * (totalWorkerCount + 1);

    public static (int metal, int crystal) BuldingUpgradeCost(Resource.Type buildingType, int nextLevel)
    {
        int nextLevelMetalCost;
        int nextLevelCrystalCost;
        switch (buildingType)
        {
            case Resource.Type.Metal:
                nextLevelMetalCost = MetalMineUpgradeMetalCostPerLevel * nextLevel;
                nextLevelCrystalCost = MetalMineUpgradeCrystalCostPerLevel * nextLevel;
                break;
            case Resource.Type.Crystal:
                nextLevelMetalCost = CrystalMineUpgradeMetalCostPerLevel * nextLevel;
                nextLevelCrystalCost = CrystalMineUpgradeCrystalCostPerLevel * nextLevel;
                break;
            default:
                throw new System.Exception(UnhandledCaseExceptionMessage);
        }
        return (nextLevelMetalCost, nextLevelCrystalCost);
    }

    public static int MaximumWorkersPossible(int buildingLevel)
    {
        return buildingLevel + 4;
    }

    public static float CycleSpeed(Resource.Type resourceType)
    {
        switch (resourceType)
        {
            case Resource.Type.Metal:
                return MetalCounterCycleSpeed;
            case Resource.Type.Crystal:
                return CrystalCounterCycleSpeed;
            default:
                throw new System.Exception(UnhandledCaseExceptionMessage);
        }
    }

    public static int IncrementPerCycle(Resource.Type resourceType, int buildingLevel)
    {
        switch (resourceType)
        {
            case Resource.Type.Metal:
                return MetalIncrementPerLevel * buildingLevel;
            case Resource.Type.Crystal:
                return CrystalIncrementPerLevel * buildingLevel;
            default:
                throw new System.Exception(UnhandledCaseExceptionMessage);
        }
    }
}
