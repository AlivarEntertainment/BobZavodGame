using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public int my = 0;
    public void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            PlayerPrefs.SetInt("LastCheckpoit", my);
        }
    }
}
