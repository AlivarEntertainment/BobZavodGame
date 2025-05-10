using UnityEngine;

public class BoxController : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            if (other.gameObject.transform.root == other.gameObject.transform) {
                other.gameObject.transform.GetChild(2).position = transform.position;
                other.gameObject.transform.GetChild(2).parent = transform;
                Destroy(other.gameObject);
            }
        }
    }
}
