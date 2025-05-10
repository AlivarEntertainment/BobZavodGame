using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public int SceneIn;
    public void OnChangeScene()
    {
        SceneManager.LoadScene(SceneIn);
    }
}
