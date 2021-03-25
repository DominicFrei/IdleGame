using UnityEngine;
using TMPro;
using Realms;

public class ResourceCounter : MonoBehaviour
{
    #region Unity Editor
    [Header("External Settings")]
    [SerializeField] private Resource.Type resourceType = default;
    [Header("Links to sub objects")]
    [SerializeField] private TMP_Text resourceAmountText = default;
    [SerializeField] private TMP_Text workerAmountText = default;
    [SerializeField] private ProgressBar progressBar = default;
    #endregion

    #region Private Properties
    private Realm realm;
    private Resource resource;
    private Building building;
    private Unit workers;
    private int incrementPerCycle = default;
    private float cycleSpeed = default;
    #endregion

    #region Public Functions

    public void AddWorkerButtonClicked()
    {
        realm.Write(() =>
        {
            if (workers.Available > 0 && resource.AssignedWorkers < Balancing.MaximumWorkersPossible(building.CurrentLevel))
            {
                workers.Available--;
                resource.AssignedWorkers++;
                UpdateResourceCounterText();
            }
        });
    }

    public void RemoveWorkerButtonClicked()
    {
        realm.Write(() =>
        {
            if (resource.AssignedWorkers > 0)
            {
                resource.AssignedWorkers--;
                workers.Available++;
                UpdateResourceCounterText();
            }
        });
    }

    #endregion

    #region Unity Messages

    private void Awake()
    {
        realm = Realm.GetInstance();
        resource = realm.Find<Resource>(resourceType.ToString());
        if (resource == null)
        {
            throw new System.Exception("Game not correctly initialised.");
        }
    }

    private void Start()
    {
        building = realm.Find<Building>(resourceType.ToString());
        workers = realm.Find<Unit>(Unit.Type.Worker.ToString());

        resource.PropertyChanged += ResourcePropertyChangedListener;
        building.PropertyChanged += BuildingPropertyChangedListener;

        RecalculateIncrementPerCycle();
    }

    private void Update()
    {
        AdvanceProgressBar();
    }

    private void OnDisable()
    {
        resource.PropertyChanged -= ResourcePropertyChangedListener;
        building.PropertyChanged -= BuildingPropertyChangedListener;
    }

    private void OnDestroy()
    {
        realm.Dispose();
    }

    #endregion

    #region Private Functions

    private void ResourcePropertyChangedListener(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        UpdateResourceCounterText();
    }

    private void BuildingPropertyChangedListener(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        RecalculateIncrementPerCycle();
    }

    private void AdvanceProgressBar()
    {
        if (resource.AssignedWorkers > 0)
        {
            realm.Write(() =>
            {
                resource.CycleProgress += cycleSpeed * resource.AssignedWorkers * Time.deltaTime;
                if (resource.CycleProgress > 1f)
                {
                    resource.CycleProgress = 0f;
                    resource.Amount += incrementPerCycle;
                }
            });
            UpdateResourceCounterText();
            progressBar.SetProgressPercentage(resource.CycleProgress);
        }
    }

    private void RecalculateIncrementPerCycle()
    {
        cycleSpeed = Balancing.CycleSpeed(resourceType);
        incrementPerCycle = Balancing.IncrementPerCycle(resourceType, building.CurrentLevel);
        UpdateResourceCounterText();
    }

    private void UpdateResourceCounterText()
    {
        resourceAmountText.text = resourceType.ToString() + ": " + resource.Amount;
        workerAmountText.text = "Workers: " + resource.AssignedWorkers + " / " + Balancing.MaximumWorkersPossible(building.CurrentLevel);
    }

    #endregion
}
