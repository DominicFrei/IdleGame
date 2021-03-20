using UnityEngine;
using TMPro;
using Realms;

public class WorkerTraining : MonoBehaviour
{
    #region Unity Editor
    [Header("Links to sub objects")]
    [SerializeField] private TMP_Text unitText = default;
    #endregion

    #region Private Properties
    private Realm realm;
    private Unit workers;
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
            if (metal.Amount >= Balancing.MetalCostPerWorker && crystal.Amount >= Balancing.CrystalCostPerWorker)
            {
                metal.Amount -= Balancing.MetalCostPerWorker;
                crystal.Amount -= Balancing.CrystalCostPerWorker;
                workers.Amount++;
                workers.Available++;
                UpdateText();
            }
        });
    }

    #endregion

    #region Unity Messages

    private void Awake()
    {
        realm = Realm.GetInstance();
        workers = realm.Find<Unit>(Unit.Type.Worker.ToString());
        if (workers == null)
        {
            workers = new Unit(Unit.Type.Worker.ToString(), 0, 0);
            realm.Write(() =>
            {
                realm.Add(workers);
            });
        }
    }

    private void Start()
    {
        metal = realm.Find<Resource>(Resource.Type.Metal.ToString());
        crystal = realm.Find<Resource>(Resource.Type.Crystal.ToString());
        workers.PropertyChanged += WorkersPropertyChangedListener;
        UpdateText();
    }

    private void OnDestroy()
    {
        workers.PropertyChanged -= WorkersPropertyChangedListener;
        realm.Dispose();
    }

    #endregion

    #region Private Functions

    private void WorkersPropertyChangedListener(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        UpdateText();
    }

    private void UpdateText()
    {
        unitText.text = "Available Workers: " + workers.Available + " / " + workers.Amount + "\n"
            + "(" + Balancing.MetalCostPerWorker + " Metal, " + Balancing.CrystalCostPerWorker + " Crystal)";
    }

    #endregion
}
