using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{
    [Header("Scene")]
    public string mainSceneName = "MainScene";
    public float startDelay = 4.0f;

    [Header("UI")]
    public GameObject creditPanel;

    private bool isStarting = false;

    void Start()
    {
        if (creditPanel != null)
        {
            creditPanel.SetActive(false);
        }
    }

    public void StartGame()
    {
        if (isStarting) return;

        isStarting = true;

        if (StartSceneSoundManager.Instance != null)
        {
            StartSceneSoundManager.Instance.PlayStartAlarm();
        }

        StartCoroutine(LoadMainSceneAfterDelay());
    }

    IEnumerator LoadMainSceneAfterDelay()
    {
        yield return new WaitForSeconds(startDelay);
        SceneManager.LoadScene(mainSceneName);
    }

    public void ShowCredits()
    {
        if (StartSceneSoundManager.Instance != null)
        {
            StartSceneSoundManager.Instance.PlayCreditOpen();
        }

        if (creditPanel != null)
        {
            creditPanel.SetActive(true);
        }
    }

    public void HideCredits()
    {
        if (creditPanel != null)
        {
            creditPanel.SetActive(false);
        }
    }
}
