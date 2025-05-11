using UnityEngine;
using System.Collections;
public class Water : MonoBehaviour
{
    private bool dropped = false;
    public Bots ToKill;
    public GameObject CameraCar;
    public BoxCollider collider;
    void Update() {
        if (dropped) {
            return;
        }
        if (transform.position.y < 3.5) {
            dropped = true;
            ToKill.RobotAnimator.SetTrigger("DieSelf");
            ToKill.enabled = false;
            StartCoroutine("CameraChange");
            GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonCharacterController>().StopCaring();
            collider.enabled = false;
        }
    }
    IEnumerator CameraChange()
    {
        
        yield return new WaitForSeconds(4f);
        CameraCar.SetActive(false);
        gameObject.SetActive(false);
    }
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Car")
        {
            CameraCar.SetActive(true);
        }
    }
}
