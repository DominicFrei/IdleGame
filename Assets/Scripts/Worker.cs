using UnityEngine;
using TMPro;
using Realms;

public class Worker : MonoBehaviour
{
    #region Unity Editor
    [Header("Links to sub objects")]
    [SerializeField] private TMP_Text unitText = default;
    #endregion

    #region Private Properties
    private int workerCount = 0;
    private Realm realm;
    private Resource metal;
    private Resource crystal;
    #endregion

    #region Public Functions

    public void BuyWorkerButtonClicked()
    {
        realm.Write(() =>
        {
            // We need to check the amounts inside the write block to block a potential other write
            // from happening in between reading the amounts while not blocking and then writing them
            // while they might have already been update.
            if (metal.Amount >= Balancing.metalCostPerWorker && crystal.Amount >= Balancing.crystalCostPerWorker)
            {
                metal.Amount -= Balancing.metalCostPerWorker;
                crystal.Amount -= Balancing.crystalCostPerWorker;
                workerCount++;
                UpdateText();
            }
        });
    }

    #endregion

    #region Unity Messages

    private void Awake()
    {
        realm = Realm.GetInstance();
        metal = realm.Find<Resource>(Resource.Type.Metal.ToString());
        crystal = realm.Find<Resource>(Resource.Type.Crystal.ToString());
    }

    private void Start()
    {
        UpdateText();
    }

    private void OnDestroy()
    {
        realm.Dispose();
    }

    #endregion

    #region Private Functions

    private void UpdateText()
    {
        unitText.text = "Worker: " + workerCount + "\n"
            + "(" + Balancing.metalCostPerWorker + " Metal, " + Balancing.crystalCostPerWorker + " Crystal)";
    }

    #endregion
}
