using UnityEngine;
using UnityEngine.Playables;

public class FinalPlay : MonoBehaviour
{
    public PlayableDirector playable;
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playable.Play();
        }
    }
}
