using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadNextScene : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.transform.childCount == 2) {
            SceneManager.LoadScene(1);
        }
    }
}
