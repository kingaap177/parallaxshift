using System.Numerics;
using UnityEngine;

[System.Serializable]

public class BackgroundHandler : MonoBehaviour
{
    public float speed;

    [SerializeField]
    private Renderer bgRenderer;

    void Update()
    {
        bgRenderer.material.mainTextureOffset += new UnityEngine.Vector2(speed * Time.deltaTime, 0);
    }
}