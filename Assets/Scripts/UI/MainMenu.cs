using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header(" UI Panels")]
    public GameObject optionPanel;  
    
    public void Play()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void PlayOnline()
    {
        SceneManager.LoadScene("OnlineScene");
    }
    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OptionPanel()
    {
        optionPanel.SetActive(true);
    }

    public void CloseOptionPanel()
    {
        optionPanel.SetActive(false);
    }
    public void Credit()
    {
        SceneManager.LoadScene("CreditScene");
    }    
}
