using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject player;
    public float xAxis, yAxis, zAxis;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Update()
    {
        transform.position = new Vector3(player.transform.position.x + xAxis, player.transform.position.y + yAxis, player.transform.position.z + zAxis);
    }
}
