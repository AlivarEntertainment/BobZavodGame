using UnityEngine;

public class GameLaunching : MonoBehaviour
{
    public GameObject PlayerObj;
    public GameObject CameraNorm;
    public GameObject PlayerTimeLine;
    public GameObject CameraTimeline;
    void Start()
    {
        PlayerObj.SetActive(true);
        CameraNorm.SetActive(true);
        PlayerTimeLine.SetActive(false);
        CameraTimeline.SetActive(false);
    }

}
