using UnityEngine;

public class BoxController : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            if (other.gameObject.transform.root == other.gameObject.transform) {
                if (other.gameObject.GetComponent<ThirdPersonCharacterController>().wasInBox) {
                    return;
                }
                other.gameObject.transform.parent = transform;
                other.gameObject.GetComponent<ThirdPersonCharacterController>().isControllingSkuf = false;
                other.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                other.gameObject.transform.position = transform.position;
                other.gameObject.GetComponent<ThirdPersonCharacterController>().InBox(true);
                other.gameObject.GetComponent<CharacterController>().enabled = true;
            }
        }
    }
}
