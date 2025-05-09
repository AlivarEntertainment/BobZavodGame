using UnityEngine;

public class StartCaring : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            other.gameObject.GetComponent<ThirdPersonCharacterController>().StartCaring();
        }
    }
}
