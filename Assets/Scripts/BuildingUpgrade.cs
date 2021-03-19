using UnityEngine;
using TMPro;
using Realms;

public class BuildingUpgrade : MonoBehaviour
{
    [Header("External Settings")]
    [SerializeField] private Resource.Type resourceType = default;
    [SerializeField] private ResourceCounter metalCounter = default;
    [SerializeField] private ResourceCounter crystalCounter = default;

    [Header("Links to sub objects")]
    [SerializeField] private TMP_Text buildingLevelText = default;
    [SerializeField] private TMP_Text buildingUpgradeButtonText = default;
    [SerializeField] private TMP_Text buildingUpgradeCostText = default;

    public int Level
    {
        get
        {
            var building = realm.Find<Building>(resourceType.ToString());
            if (building == null)
            {
                building = new Building(resourceType.ToString(), 1);
                realm.Write(() =>
                {
                    realm.Add(building);
                });
            }
            return building.Level;
        }
        set
        {
            var building = realm.Find<Building>(resourceType.ToString());
            realm.Write(() =>
            {
                building.Level = value;
            });
            UpdateData();
        }
    }

    private Realm realm;

    public void BuildingUpgradeButtonClicked()
    {
        switch (resourceType)
        {
            case Resource.Type.Metal:
                {
                    int metalCostForUpgrade = Balancing.MetalMineUpgradeMetalCostPerLevel * Level;
                    int crystalCostForUpgrade = Balancing.MetalMineUpgradeCrystalCostPerLevel * Level;
                    BuyUpgrade(metalCostForUpgrade, crystalCostForUpgrade);
                    metalCounter.IncrementPerCycle = Balancing.MetalIncrementPerLevel * (Level + 1);
                    break;
                }

            case Resource.Type.Crystal:
                {
                    int metalCostForUpgrade = Balancing.CrystalMineUpgradeMetalCostPerLevel * Level;
                    int crystalCostForUpgrade = Balancing.CrystalMineUpgradeCrystalCostPerLevel * Level;
                    BuyUpgrade(metalCostForUpgrade, crystalCostForUpgrade);
                    crystalCounter.IncrementPerCycle = Balancing.CrystalIncrementPerLevel * (Level + 1);
                    break;
                }
            case Resource.Type.NotSet:
                Debug.Break();
                break;
        }
        UpdateData();
    }

    private bool BuyUpgrade(int metalCostForUpgrade, int crystalCostForUpgrade)
    {
        if (metalCounter.Amount < metalCostForUpgrade || crystalCounter.Amount < crystalCostForUpgrade)
        {
            return false;
        }

        metalCounter.Amount -= metalCostForUpgrade;
        crystalCounter.Amount -= crystalCostForUpgrade;
        Level++;

        return true;
    }

    private void Awake()
    {
        realm = Realm.GetInstance();
    }

    private void Start()
    {
        UpdateData();
    }

    private void OnDisable()
    {
        realm.Dispose();
    }

    private void UpdateData()
    {
        int nextLevel = Level + 1;
        int nextLevelMetalCost = 0;
        int nextLevelCrystalCost = 0;
        switch (resourceType)
        {
            case Resource.Type.Metal:
                nextLevelMetalCost = Balancing.MetalMineUpgradeMetalCostPerLevel * Level;
                nextLevelCrystalCost = Balancing.MetalMineUpgradeCrystalCostPerLevel * Level;
                break;
            case Resource.Type.Crystal:
                nextLevelMetalCost = Balancing.CrystalMineUpgradeMetalCostPerLevel * Level;
                nextLevelCrystalCost = Balancing.CrystalMineUpgradeCrystalCostPerLevel * Level;
                break;
            default:
                Debug.Break();
                break;
        }

        buildingLevelText.text = resourceType.ToString() + " Mine" + "\n" + "Level " + Level;
        buildingUpgradeButtonText.text = "Build Level " + nextLevel;
        buildingUpgradeCostText.text = "Level " + nextLevel + ":" + "\n"
            + nextLevelMetalCost + " Metal" + "\n"
            + nextLevelCrystalCost + " Crystal" + "\n"
            + "(+" + Balancing.MetalIncrementPerLevel + " " + resourceType.ToString() + " / s)";
    }
}
