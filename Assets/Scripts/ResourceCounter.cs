using UnityEngine;
using TMPro;
using Realms;

public class ResourceCounter : MonoBehaviour
{
    #region Unity Editor
    [Header("External Settings")]
    [SerializeField] private Resource.Type resourceType = default;
    [Header("Links to sub objects")]
    [SerializeField] private TMP_Text resourceText = default;
    [SerializeField] private ProgressBar progressBar = default;
    [SerializeField] private TMP_Text buttonText = default;
    #endregion

    #region Private Properties
    private Realm realm;
    private Resource resource;
    private Building building;
    private bool isInProgress = false;
    private readonly bool shouldContinueProgress = false;
    private int incrementPerCycle = 0;
    private float cycleProgress = 0f;
    private float cycleSpeed = 0f;
    #endregion

    #region Public Functions

    public void StartCollectingButtonClicked()
    {
        isInProgress = true;
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
                resource = new Resource(resourceType.ToString(), Balancing.metalStartingAmount);
                break;
            case Resource.Type.Crystal:
                resource = new Resource(resourceType.ToString(), Balancing.crystalStartingAmount);
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
        resource.PropertyChanged += ResourcePropertyChangedListener;
        building = realm.Find<Building>(resourceType.ToString());
        building.PropertyChanged += BuildingPropertyChangedListener;
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
        buttonText.text = "Collect " + resourceType.ToString();
        UpdateResourceCounterText();
    }

    private void Update()
    {
        AdvanceProgressBar();
    }

    private void OnDisable()
    {
        resource.PropertyChanged -= ResourcePropertyChangedListener;
        building.PropertyChanged -= BuildingPropertyChangedListener;
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
        switch (resourceType)
        {
            case Resource.Type.Metal:
                incrementPerCycle = Balancing.MetalIncrementPerLevel * building.Level;
                break;
            case Resource.Type.Crystal:
                incrementPerCycle = Balancing.CrystalIncrementPerLevel * building.Level;
                break;
            default:
                Debug.Break();
                break;
        }
        UpdateResourceCounterText();
    }

    private void AdvanceProgressBar()
    {
        if (isInProgress)
        {
            cycleProgress += cycleSpeed * Time.deltaTime;
            if (cycleProgress > 1.0f)
            {
                cycleProgress = 0.0f;
                realm.Write(() =>
                {
                    resource.Amount += incrementPerCycle;
                });
                UpdateResourceCounterText();
                if (!shouldContinueProgress)
                {
                    isInProgress = false;
                }
            }
            progressBar.SetProgressPercentage(cycleProgress);
        }
    }

    private void UpdateResourceCounterText()
    {
        resourceText.text = resourceType.ToString() + ": " + resource.Amount + " (" + incrementPerCycle + " / s)";
    }

    #endregion
}
