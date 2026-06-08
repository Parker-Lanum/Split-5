using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public List<GameObject> levels; 
    public int currLevelIndex = 0;
    private int levelsCompleted = 1;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI loseText;

    public float timer = 10f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            loseText.text = string.Format("Out of Time!\nPress R to Restart");
            levels[currLevelIndex].transform.eulerAngles = new Vector3(0, 0, 180);
        }
        else
        {
            timer -= Time.deltaTime;
            timerText.text = string.Format("{0:F0}", timer);
        }

    }

    public void NextLevel()
    {
        if (levels.Count == 1)
        {            
            loseText.text = string.Format("You're insane!\nGo Rest");
            timer += 95;
        }
        else
        {
            float decelerate = 1f + (3f * Mathf.Exp(-0.05f * levelsCompleted));
            timer += decelerate;
            levels[currLevelIndex].SetActive(false);
            levels.RemoveAt(currLevelIndex);
            currLevelIndex = Random.Range(0, levels.Count);
            print(currLevelIndex);
            levels[currLevelIndex].SetActive(true);
            levelsCompleted ++;
            AudioController.Instance.SetLevelAudio(levelsCompleted - 1);  
            levelText.text = string.Format("Level: {0}", levelsCompleted);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        NextLevel();
    }
}
