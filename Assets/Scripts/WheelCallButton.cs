using UnityEngine;
using UnityEngine.SceneManagement;

public class WheelCallButton : MonoBehaviour
{
    public void CallTheWheel()
    {
        SceneManager.LoadScene("WheelScene", LoadSceneMode.Additive);
        SceneManager.sceneLoaded += OnLevelSceneLoaded;
    }

    private void OnLevelSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "WheelScene")
        {
            SceneManager.SetActiveScene(scene);
            SceneManager.sceneLoaded -= OnLevelSceneLoaded;
        }
    }
}
