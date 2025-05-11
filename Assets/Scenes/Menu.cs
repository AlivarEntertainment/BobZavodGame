using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public int SceneIn;
    public GameObject Launcher;
    public GameObject TimeObj;
    public void OnChangeScene()
    {
        SceneManager.LoadScene(SceneIn);
    }
    public void OnSkip()
    {
        TimeObj.SetActive(false);
        Launcher.SetActive(true);
        Destroy(this.gameObject);
    }
}
