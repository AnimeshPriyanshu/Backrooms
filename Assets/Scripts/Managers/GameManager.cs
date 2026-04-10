using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Game State")]
    public int currentLevel = 0; // 0 = menu, 1-3 = levels, 4 = boss
    public int maxLevel = 3;
    public bool isGameOver = false;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        currentLevel = 1;
        isGameOver = false;
        // Assumes scenes are added to Build Settings in order: Menu (0), Lvl 1 (1), Lvl 2 (2), Lvl 3 (3), Boss (4)
        SceneController.Instance.LoadSceneByIndex(currentLevel);
    }

    public void NextLevel()
    {
        currentLevel++;
        if (currentLevel > maxLevel + 1) // +1 accommodates the boss room
        {
            // Win game sequence
            Debug.Log("You Escaped the Backrooms!");
            SceneController.Instance.LoadSceneByName("MainMenu");
            currentLevel = 0;
            return;
        }
        
        SceneController.Instance.LoadSceneByIndex(currentLevel);
    }

    public void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over!");
        // Return to main menu on death
        SceneController.Instance.LoadSceneByName("MainMenu");
        currentLevel = 0;
    }
}
