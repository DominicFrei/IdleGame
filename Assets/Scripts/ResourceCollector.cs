using UnityEngine;
using TMPro;

public class ResourceCollector : MonoBehaviour
{
    [Header("External Settings")]
    [SerializeField] private Resource.Type resourceType = default;

    [Header("Links to sub objects")]
    [SerializeField] private TMP_Text unitText = default;

    public void BuyUnitButtonClicked()
    {

    }

    private void Awake()
    {
        switch (resourceType)
        {
            case Resource.Type.Metal:
                {
                    unitText.text = "Metal Collector: ";
                    break;
                }
        }
    }
}
