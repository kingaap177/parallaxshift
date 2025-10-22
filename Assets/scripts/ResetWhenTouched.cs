using UnityEngine;


public class ResetWhenTouched : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
            Application.LoadLevel(Application.loadedLevel);
    }
}
