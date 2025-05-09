using UnityEngine;

public class Water : MonoBehaviour
{
    private bool dropped = false;
    void Update() {
        if (dropped) {
            return;
        }
        if (transform.position.y < 4) {
            dropped = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonCharacterController>().StopCaring();
        }
    }
}
