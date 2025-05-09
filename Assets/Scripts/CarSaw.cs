using UnityEngine;

public class CarSaw : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            PlayerPrefs.SetInt("SawCar", 1);
        }
    }
}
