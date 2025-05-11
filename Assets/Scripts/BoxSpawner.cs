using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public GameObject BoxPrefub;
    private float TimeDelay;
    private float TimeNow = 0f;
    void Start() {
        TimeDelay = 10f;
    }
    void FixedUpdate() {
        TimeNow += Time.deltaTime;
        if (TimeNow >= TimeDelay) {
            Instantiate(BoxPrefub, transform.position, transform.rotation);
            TimeNow = 0f;
        }
    }
    void OnTriggerEnter(Collider other) {
        if (other.tag != "Box") {
            return;
        }
        if (other.gameObject.transform.childCount == 0) {
            Destroy(other.gameObject);
        } /*else {
            GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonCharacterController>().Die();
        }*/
    }
}
