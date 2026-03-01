using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LandingLoader : MonoBehaviour
{
    public Image fillImage;
    public float loadDuration = 10f;
    public string nextSceneName = "MenuScene";

    private float timer = 0f;

    void Start()
    {
        fillImage.fillAmount = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        float progress = timer / loadDuration;
        fillImage.fillAmount = progress;

        if (progress >= 1f)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}