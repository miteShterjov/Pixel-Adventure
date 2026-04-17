using UnityEngine;
using UnityEngine.Serialization;

public enum BackgroundType { Blue,Brown,Gray,Green,Pink,Purple,Yellow}

public class AnimatedBackground : MonoBehaviour
{
    [Header("Movement direction")]
    [SerializeField] private Vector2 movementDirection;
    [Header("Color")]
    [SerializeField] private BackgroundType backgroundType;
    [SerializeField] private Texture2D[] textures;
    
    private MeshRenderer mesh;

    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
        UpdateBackgroundTexture();
    }

    private void Update()
    {
        mesh.material.mainTextureOffset += movementDirection * Time.deltaTime;
    }

    [ContextMenu("Update background")]
    private void UpdateBackgroundTexture()
    {
        if (!mesh) mesh = GetComponent<MeshRenderer>();

        mesh.sharedMaterial.mainTexture = textures[((int)backgroundType)];
    }
}
