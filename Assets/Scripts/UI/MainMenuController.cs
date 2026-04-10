using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject optionsPanel;

    private void Start()
    {
        if (optionsPanel != null)
            optionsPanel.SetActive(false);
    }

    public void PlayGame()
    {
        Debug.Log("Play Button Clicked");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame();
        }
        else
        {
            Debug.LogWarning("GameManager is missing! Ensure it is in the scene.");
        }
    }

    public void ToggleOptions()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(!optionsPanel.activeSelf);
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game...");
        Application.Quit();
    }
}
