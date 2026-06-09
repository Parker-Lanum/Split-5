using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public List<GameObject> levels;
    public int currLevelIndex = 0;

    private int levelsCompleted = 1;
    private bool gameOver = false;
    private bool gameEnded = false;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI loseText;

    public float timer = 10f;

    void Start()
    {
        // Only keep the current level active at start.
        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].SetActive(i == currLevelIndex);
        }

        if (levelText != null)
        {
            levelText.text = "Level: " + levelsCompleted;
        }

        if (loseText != null)
        {
            loseText.text = "";
        }

        if (AudioController.Instance != null)
        {
            AudioController.Instance.SetLevelAudio(0);
        }
    }

    void Update()
    {
        if (gameOver || gameEnded) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            GameOver();
            return;
        }

        if (timerText != null)
        {
            timerText.text = string.Format("{0:F0}", timer);
        }
    }

    public void NextLevel()
    {
        if (gameOver || gameEnded) return;
        if (levels == null || levels.Count == 0) return;

        // If this is the final remaining level, finish the game.
        if (levels.Count <= 1)
        {
            EndGame();
            return;
        }

        float bonusTime = 1f + (3f * Mathf.Exp(-0.05f * levelsCompleted));
        timer += bonusTime;

        levels[currLevelIndex].SetActive(false);
        levels.RemoveAt(currLevelIndex);

        currLevelIndex = Random.Range(0, levels.Count);
        levels[currLevelIndex].SetActive(true);

        levelsCompleted++;

        if (AudioController.Instance != null)
        {
            AudioController.Instance.SetLevelAudio(levelsCompleted - 1);
        }

        if (levelText != null)
        {
            levelText.text = "Level: " + levelsCompleted;
        }
    }

    void GameOver()
    {
        gameOver = true;
        timer = 0f;

        if (timerText != null)
        {
            timerText.text = "0";
        }

        if (loseText != null)
        {
            loseText.text = "Out of Time!\nPress R to Restart";
        }

        if (levels.Count > 0 && currLevelIndex >= 0 && currLevelIndex < levels.Count)
        {
            levels[currLevelIndex].transform.eulerAngles = new Vector3(0, 0, 180);
        }
    }

    void EndGame()
    {
        gameEnded = true;

        if (loseText != null)
        {
            loseText.text = "You're insane!\nGo Rest";
        }

        if (levelText != null)
        {
            levelText.text = "Completed";
        }

        if (AudioController.Instance != null)
        {
            AudioController.Instance.SetLevelAudio(10);
        }
    }
}