using UnityEngine;
using TMPro;
using Realms;

public class ResourceCounter : MonoBehaviour
{
    [Header("External Settings")]
    [SerializeField] private Resource.Type resourceType = default;
    [Header("Links to sub objects")]
    [SerializeField] private TMP_Text resourceText = default;
    [SerializeField] private ProgressBar progressBar = default;
    [SerializeField] private TMP_Text buttonText = default;

    private int amount = 42;
    public int Amount
    {
        get { return amount; }
        set
        {
            amount = value;
            var resource = realm.Find<Resource>(resourceType.ToString());
            realm.Write(() =>
            {
                resource.Amount = amount;
            });
            UpdateText();
        }
    }
    public bool IsInProgress { get; set; } = false;
    public bool ShouldContinueProgress { get; set; } = false;

    private int incrementPerCycle = default;
    public int IncrementPerCycle
    {
        get { return incrementPerCycle; }
        set { incrementPerCycle = value; UpdateText(); }
    }
    private float cycleProgress = 0f;
    private float cycleSpeed = default;
    private Realm realm;

    // Button Action
    public void StartGatheringButtonClicked()
    {
        IsInProgress = true;
    }

    private void Awake()
    {
        switch (resourceType)
        {
            case Resource.Type.Metal:
                cycleSpeed = Balancing.MetalCounterCycleSpeed;
                break;
            case Resource.Type.Crystal:
                cycleSpeed = Balancing.CrystalCounterCycleSpeed;
                break;
            default:
                Debug.Break();
                break;
        }
        realm = Realm.GetInstance();
        var resource = realm.Find<Resource>(resourceType.ToString());
        if (resource == null)
        {
            resource = new Resource(resourceType.ToString(), 500);
            realm.Write(() =>
            {
                realm.Add(resource);
            });
        }
        else
        {
            amount = resource.Amount;
        }
    }

    private void Start()
    {
        switch (resourceType)
        {
            case Resource.Type.Metal:
                incrementPerCycle = Balancing.MetalIncrementPerLevel;
                break;
            case Resource.Type.Crystal:
                incrementPerCycle = Balancing.CrystalIncrementPerLevel;
                break;
            default:
                Debug.Break();
                break;
        }
        UpdateText();
        buttonText.text = "Gather " + resourceType.ToString();
    }

    private void Update()
    {
        if (IsInProgress)
        {
            cycleProgress += cycleSpeed * Time.deltaTime;
            if (cycleProgress > 1.0f)
            {
                cycleProgress = 0.0f;
                Amount += incrementPerCycle;
                UpdateText();
                if (!ShouldContinueProgress)
                {
                    IsInProgress = false;
                }
            }
            progressBar.SetProgressPercentage(cycleProgress);
        }
    }

    private void OnDisable()
    {
        realm.Dispose();
    }

    private void UpdateText()
    {
        resourceText.text = resourceType.ToString() + ": " + Amount + " (" + incrementPerCycle + " / s)";
    }

}
