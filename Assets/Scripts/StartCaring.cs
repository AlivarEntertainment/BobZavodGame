using UnityEngine;

public class StartCaring : MonoBehaviour
{
    public GameObject Joystick;
    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            Joystick.SetActive(true);
            other.gameObject.GetComponent<ThirdPersonCharacterController>().StartCaring();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Joystick.SetActive(false);
    }
}
