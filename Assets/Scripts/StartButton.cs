using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void StartButtonClicked()
    {
        GameState.Initialise();
        SceneManager.LoadScene("01_Home");
    }
}
