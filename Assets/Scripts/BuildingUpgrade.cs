using UnityEngine;
using TMPro;
using Realms;

public class BuildingUpgrade : MonoBehaviour
{
    #region Unity Editor
    [Header("External Settings")]
    [SerializeField] private Resource.Type resourceType = default;

    [Header("Links to sub objects")]
    [SerializeField] private TMP_Text buildingLevelText = default;
    [SerializeField] private TMP_Text buildingUpgradeButtonText = default;
    [SerializeField] private TMP_Text buildingUpgradeCostText = default;
    #endregion

    #region Private Properties
    private Realm realm;
    private Building building;
    private Resource metal;
    private Resource crystal;
    #endregion

    #region Public Functions

    public void BuildingUpgradeButtonClicked()
    {
        switch (resourceType)
        {
            case Resource.Type.Metal:
                {
                    int metalCostForUpgrade = Balancing.MetalMineUpgradeMetalCostPerLevel * building.Level;
                    int crystalCostForUpgrade = Balancing.MetalMineUpgradeCrystalCostPerLevel * building.Level;
                    BuyUpgrade(metalCostForUpgrade, crystalCostForUpgrade);
                    break;
                }

            case Resource.Type.Crystal:
                {
                    int metalCostForUpgrade = Balancing.CrystalMineUpgradeMetalCostPerLevel * building.Level;
                    int crystalCostForUpgrade = Balancing.CrystalMineUpgradeCrystalCostPerLevel * building.Level;
                    BuyUpgrade(metalCostForUpgrade, crystalCostForUpgrade);
                    break;
                }
            case Resource.Type.NotSet:
                Debug.Break();
                break;
        }
        UpdateData();
    }

    #endregion

    #region Unity Messages

    private void Awake()
    {
        realm = Realm.GetInstance();
        building = realm.Find<Building>(resourceType.ToString());
        if (building != null)
        {
            return;
        }

        building = new Building(resourceType.ToString(), 1);
        realm.Write(() =>
        {
            realm.Add(building);
        });

        metal = realm.Find<Resource>(Resource.Type.Metal.ToString());
        crystal = realm.Find<Resource>(Resource.Type.Crystal.ToString());
    }

    private void Start()
    {
        UpdateData();
    }

    private void OnDestroy()
    {
        realm.Dispose();
    }

    #endregion

    #region Private Functions

    private void BuyUpgrade(int metalCostForUpgrade, int crystalCostForUpgrade)
    {
        realm.Write(() =>
        {
            // We need to check the amounts inside the write block to block a potential other write
            // from happening in between reading the amounts while not blocking and then writing them
            // while they might have already been update.
            if (metal.Amount >= metalCostForUpgrade && crystal.Amount >= crystalCostForUpgrade)
            {
                metal.Amount -= metalCostForUpgrade;
                crystal.Amount -= crystalCostForUpgrade;
                building.Level++;
            }
        });
    }

    private void UpdateData()
    {
        int nextLevel = building.Level + 1;
        int nextLevelMetalCost = 0;
        int nextLevelCrystalCost = 0;
        switch (resourceType)
        {
            case Resource.Type.Metal:
                nextLevelMetalCost = Balancing.MetalMineUpgradeMetalCostPerLevel * building.Level;
                nextLevelCrystalCost = Balancing.MetalMineUpgradeCrystalCostPerLevel * building.Level;
                break;
            case Resource.Type.Crystal:
                nextLevelMetalCost = Balancing.CrystalMineUpgradeMetalCostPerLevel * building.Level;
                nextLevelCrystalCost = Balancing.CrystalMineUpgradeCrystalCostPerLevel * building.Level;
                break;
            default:
                Debug.Break();
                break;
        }

        buildingLevelText.text = resourceType.ToString() + " Mine" + "\n" + "Level " + building.Level;
        buildingUpgradeButtonText.text = "Build Level " + nextLevel;
        buildingUpgradeCostText.text = "Level " + nextLevel + ":" + "\n"
            + nextLevelMetalCost + " Metal" + "\n"
            + nextLevelCrystalCost + " Crystal" + "\n"
            + "(+" + Balancing.MetalIncrementPerLevel + " " + resourceType.ToString() + " / s)";
    }

    #endregion

}
