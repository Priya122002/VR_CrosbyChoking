using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    public void OnTryAgain()
    {
        Debug.Log("Try Again Clicked");

        RetryState.IsRetry = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}