using UnityEngine;

public class RotarSkybox : MonoBehaviour
{
    public float velocidadRotacion = 2.0f;

    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * velocidadRotacion);
    }
}