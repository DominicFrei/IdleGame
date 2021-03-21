using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void HomeSceneButtonClicked()
    {
        SceneManager.LoadScene("00_Home");
    }

    public void BuildingsSceneButtonClicked()
    {
        SceneManager.LoadScene("01_Buildings");
    }

    public void UnitsSceneButtonClick()
    {
        SceneManager.LoadScene("02_Units");
    }
    
}
