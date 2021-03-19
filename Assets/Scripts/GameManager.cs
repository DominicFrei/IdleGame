using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Metal
    [SerializeField] private ResourceCounter metalCounter = default;
    [SerializeField] private BuildingUpgrade metalMineUpgrade = default;

    // Crystal
    [SerializeField] private ResourceCounter crystalCounter = default;
    [SerializeField] private BuildingUpgrade crystalMineUpgrade = default;

    // Returns wheather the upgrade can be started or not.
    public bool BuildingUpgradeButtonClicked(Resource.Type resourceType)
    {
        switch (resourceType)
        {
            case Resource.Type.Metal:
                {
                    int currentLevel = metalMineUpgrade.GetLevel();
                    int metalCostForUpgrade = Balancing.MetalMineUpgradeMetalCostPerLevel * currentLevel;
                    int crystalCostForUpgrade = Balancing.MetalMineUpgradeCrystalCostPerLevel * currentLevel;
                    if (metalCounter.Amount >= metalCostForUpgrade && crystalCounter.Amount >= crystalCostForUpgrade)
                    {
                        metalCounter.Amount -= metalCostForUpgrade;
                        crystalCounter.Amount -= crystalCostForUpgrade;
                        metalCounter.IncrementPerCycle = Balancing.MetalIncrementPerLevel * (currentLevel + 1);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            case Resource.Type.Crystal:
                {
                    int currentLevel = crystalMineUpgrade.GetLevel();
                    int metalCostForUpgrade = Balancing.CrystalMineUpgradeMetalCostPerLevel * currentLevel;
                    int crystalCostForUpgrade = Balancing.CrystalMineUpgradeCrystalCostPerLevel * currentLevel;
                    if (metalCounter.Amount >= metalCostForUpgrade && crystalCounter.Amount >= crystalCostForUpgrade)
                    {
                        metalCounter.Amount -= metalCostForUpgrade;
                        crystalCounter.Amount -= crystalCostForUpgrade;
                        crystalCounter.IncrementPerCycle = Balancing.CrystalIncrementPerLevel * (currentLevel + 1);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            default:
                Debug.Break();
                return false;
        }
    }
}
