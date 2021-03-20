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
    private float cycleProgress = default;
    private float cycleSpeed = default;
    #endregion

    #region Public Functions

    public void AddWorkerButtonClicked()
    {
        realm.Write(() =>
        {
            if (workers.Available > 0 && resource.AssignedWorkers < building.Level)
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
        if (resource != null)
        {
            return;
        }

        switch (resourceType)
        {
            case Resource.Type.Metal:
                resource = new Resource(resourceType.ToString(), Balancing.metalStartingAmount, 0);
                break;
            case Resource.Type.Crystal:
                resource = new Resource(resourceType.ToString(), Balancing.crystalStartingAmount, 0);
                break;
            default:
                Debug.Break();
                break;
        }
        realm.Write(() =>
        {
            realm.Add(resource);
        });
    }

    private void Start()
    {
        building = realm.Find<Building>(resourceType.ToString());
        resource.PropertyChanged += ResourcePropertyChangedListener;
        building.PropertyChanged += BuildingPropertyChangedListener;
        workers = realm.Find<Unit>(Unit.Type.Worker.ToString());
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
            cycleProgress += cycleSpeed * resource.AssignedWorkers * Time.deltaTime;
            if (cycleProgress > 1f)
            {
                cycleProgress = 0f;
                realm.Write(() =>
                {
                    resource.Amount += incrementPerCycle;
                });
                UpdateResourceCounterText();
            }
            progressBar.SetProgressPercentage(cycleProgress);
        }
    }

    private void RecalculateIncrementPerCycle()
    {
        switch (resourceType)
        {
            case Resource.Type.Metal:
                cycleSpeed = Balancing.MetalCounterCycleSpeed;
                incrementPerCycle = Balancing.MetalIncrementPerLevel * building.Level;
                break;
            case Resource.Type.Crystal:
                cycleSpeed = Balancing.CrystalCounterCycleSpeed;
                incrementPerCycle = Balancing.CrystalIncrementPerLevel * building.Level;
                break;
            default:
                Debug.Break();
                break;
        }
        UpdateResourceCounterText();
    }

    private void UpdateResourceCounterText()
    {
        resourceAmountText.text = resourceType.ToString() + ": " + resource.Amount;
        workerAmountText.text = "Workers: " + resource.AssignedWorkers + " / " + building.Level;
    }

    #endregion
}
