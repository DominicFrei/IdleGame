using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void HomeSceneButtonClicked()
    {
        SceneManager.LoadScene("01_Home");
    }

    public void BuildingsSceneButtonClicked()
    {
        SceneManager.LoadScene("02_Buildings");
    }

    public void UnitsSceneButtonClick()
    {
        SceneManager.LoadScene("03_Units");
        Debug.Log("123");
    }

}
