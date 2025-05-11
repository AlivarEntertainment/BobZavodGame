using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadNextScene : MonoBehaviour
{
    public void OnTriggerEnter(Collider other) {
        Debug.Log(other.gameObject.transform.childCount);
        if (other.gameObject.transform.childCount == 2) {
            SceneManager.LoadScene(2);
        }
    }
}
