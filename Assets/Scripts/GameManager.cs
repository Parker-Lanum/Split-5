using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public GameObject[] levels; 
    public int currLevelIndex = 0;
    public int levelCount = 3;
    public Rigidbody2D player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextLevel()
    {
        if (currLevelIndex < levelCount-1)
        {
            levels[currLevelIndex].SetActive(false);
            currLevelIndex ++;
            levels[currLevelIndex].SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("WinScreen");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        NextLevel();
    }
}
