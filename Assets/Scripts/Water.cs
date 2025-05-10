using UnityEngine;

public class Water : MonoBehaviour
{
    private bool dropped = false;
    public GameObject ToKill;
    void Update() {
        if (dropped) {
            return;
        }
        if (transform.position.y < 3) {
            dropped = true;
            ToKill.SetActive(false);
            gameObject.SetActive(false);
            GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonCharacterController>().StopCaring();
        }
    }
}
