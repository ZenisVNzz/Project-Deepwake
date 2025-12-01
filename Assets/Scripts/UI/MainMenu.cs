using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header(" UI Panels")]
    public GameObject optionPanel;
    private SFXData ClickSFX = ResourceManager.Instance.GetAsset<SFXData>("UIButtonSFX");

    public void Play()
    {
        SFXManager.Instance.Play(ClickSFX, transform.position);
        SceneManager.LoadScene("GameScene");
    }
    public void OnPlayOnlineButton()
    {
        SFXManager.Instance.Play(ClickSFX, transform.position);
        SceneManager.LoadScene("Login");
    }
    public void BackToTitle()
    {
        SFXManager.Instance.Play(ClickSFX, transform.position);
        SceneManager.LoadScene("TitleScene");
    }

    public void QuitGame()
    {
        SFXManager.Instance.Play(ClickSFX, transform.position);
        Debug.Log("Quit!");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OptionPanel()
    {
        SFXManager.Instance.Play(ClickSFX, transform.position);
        optionPanel.SetActive(true);
    }

    public void CloseOptionPanel()
    {
        SFXManager.Instance.Play(ClickSFX, transform.position);
        optionPanel.SetActive(false);
    }
    public void Credit()
    {
        SFXManager.Instance.Play(ClickSFX, transform.position);
        SceneManager.LoadScene("CreditScene");
    }    
}
