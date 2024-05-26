using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseButton : MonoBehaviour
{
    public void CloseTheWheel()
    {
        Scene _curentScene = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(_curentScene);
    }
}
