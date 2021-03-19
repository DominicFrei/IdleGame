using UnityEngine;
using TMPro;

public class BuildingUpgrade : MonoBehaviour
{
    [Header("External Settings")]
    [SerializeField] private Resource.Type resourceType = default;
    [SerializeField] private GameManager gameManager = default;
    [Header("Links to sub objects")]
    [SerializeField] private TMP_Text buildingLevelText = default;
    [SerializeField] private TMP_Text buildingUpgradeButtonText = default;
    [SerializeField] private TMP_Text buildingUpgradeCostText = default;

    private int level = 1;

    public void BuildingUpgradeButtonClicked()
    {
        bool canUpgradeBuilding = gameManager.BuildingUpgradeButtonClicked(resourceType);
        if (canUpgradeBuilding)
        {
            level++;
            UpdateData();
        }
    }

    public int GetLevel()
    {
        return level;
    }

    private void Start()
    {
        UpdateData();
    }

    private void UpdateData()
    {
        int nextLevel = level + 1;
        int nextLevelMetalCost = 0;
        int nextLevelCrystalCost = 0;
        switch (resourceType)
        {
            case Resource.Type.Metal:
                nextLevelMetalCost = Balancing.MetalMineUpgradeMetalCostPerLevel * level;
                nextLevelCrystalCost = Balancing.MetalMineUpgradeCrystalCostPerLevel * level;
                break;
            case Resource.Type.Crystal:
                nextLevelMetalCost = Balancing.CrystalMineUpgradeMetalCostPerLevel * level;
                nextLevelCrystalCost = Balancing.CrystalMineUpgradeCrystalCostPerLevel * level;
                break;
            default:
                Debug.Break();
                break;
        }

        buildingLevelText.text = resourceType.ToString() + " Mine" + "\n" + "Level " + level;
        buildingUpgradeButtonText.text = "Build Level " + nextLevel;
        buildingUpgradeCostText.text = "Level " + nextLevel + ":" + "\n"
            + nextLevelMetalCost + " Metal" + "\n"
            + nextLevelCrystalCost + " Crystal" + "\n"
            + "(+" + Balancing.MetalIncrementPerLevel + " " + resourceType.ToString() + " / s)";
    }
}
