using UnityEngine;
using TMPro;
using Realms;

public class BuildingUpgrade : MonoBehaviour
{
    #region Unity Editor
    [Header("External Settings")]
    [SerializeField] private Resource.Type resourceType = default;

    [Header("Links to sub objects")]
    [SerializeField] private TMP_Text textName = default;
    [SerializeField] private TMP_Text textCurrentLevel = default;
    [SerializeField] private TMP_Text textMaximumLevel = default;
    [SerializeField] private TMP_Text textbutton = default;
    [SerializeField] private TMP_Text textUpgradeCost = default;
    [SerializeField] private TMP_Text textAssignedWorkers = default;
    #endregion

    #region Private Properties
    private Realm realm;
    private Building building;
    private Resource metal;
    private Resource crystal;
    private Unit workers;
    #endregion

    #region Public Functions

    public void BuildingUpgradeButtonClicked()
    {
        (int nextLevelMetalCost, int nextLevelCrystalCost) = Balancing.BuldingUpgradeCost(resourceType, building.MaximumLevel + 1);
        BuyUpgrade(nextLevelMetalCost, nextLevelCrystalCost);
        UpdateData();
    }

    public void AddWorkerButtonClicked()
    {
        realm.Write(() =>
        {
            if (workers.Available > 0)
            {
                workers.Available--;
                building.WorkersAssigend++;
            }
        });
        UpdateData();
    }

    public void RemoveWorkerButtonClicked()
    {
        realm.Write(() =>
        {
            if (building.WorkersAssigend > 0)
            {
                building.WorkersAssigend--;
                workers.Available++;
            }
        });
        UpdateData();
    }

    #endregion

    #region Unity Messages

    private void Awake()
    {
        realm = Realm.GetInstance();
        building = realm.Find<Building>(resourceType.ToString());
        if (building == null)
        {
            throw new System.Exception("Game not correctly initialised.");
        }
    }

    private void Start()
    {
        metal = realm.Find<Resource>(Resource.Type.Metal.ToString());
        crystal = realm.Find<Resource>(Resource.Type.Crystal.ToString());
        workers = realm.Find<Unit>(Unit.Type.Worker.ToString());

        UpdateData();
    }

    private void OnDestroy()
    {
        realm.Dispose();
    }

    #endregion

    #region Private Functions

    private void BuyUpgrade(int nextLevelMetalCost, int nextLevelCrystalCost)
    {
        realm.Write(() =>
        {
            if (metal.Amount >= nextLevelMetalCost && crystal.Amount >= nextLevelCrystalCost)
            {
                metal.Amount -= nextLevelMetalCost;
                crystal.Amount -= nextLevelCrystalCost;
                building.MaximumLevel++;
            }
        });
        UpdateData();
    }

    private void UpdateData()
    {
        (int nextLevelMetalCost, int nextLevelCrystalCost) = Balancing.BuldingUpgradeCost(resourceType, building.MaximumLevel + 1);

        textName.text = resourceType.ToString() + " Mine";
        textCurrentLevel.text = "Current Level: " + building.CurrentLevel;
        textMaximumLevel.text = "Maximum Level: " + building.MaximumLevel;
        textbutton.text = "Unlock Level " + (building.MaximumLevel + 1);
        textUpgradeCost.text = nextLevelMetalCost + " Metal" + "\n"
            + nextLevelCrystalCost + " Crystal" + "\n";
        textAssignedWorkers.text = "Worker: " + building.WorkersAssigend;
    }

    #endregion

}
