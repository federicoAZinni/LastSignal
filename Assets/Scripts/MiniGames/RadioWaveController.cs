using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controlador que pasa parámetros al shader RadioWave_Canvas.
///
/// SETUP:
///   1. Crear un Material con shader "UI/RadioWave_Canvas".
///   2. En el Canvas, crear una RawImage (o Image).
///   3. Asignar el Material a la RawImage.
///   4. Agregar este componente al mismo GameObject.
///   5. Las perillas llaman a SetFrequency() y SetAmplitude().
/// </summary>
[RequireComponent(typeof(Graphic))]
public class RadioWaveController : MonoBehaviour
{
    [Header("Parámetros de la Onda")]
    [Range(0.5f, 20f)] public float frequency = 3f;
    [Range(0f, 1f)]    public float amplitude = 0.5f;
    [Range(0f, 10f)]   public float speed = 2f;

    [Header("Ruido / Estática")]
    [Range(0f, 1f)]    public float noiseAmount = 0.3f;
    [Range(0f, 20f)]   public float noiseSpeed = 10f;

    [Header("Apariencia")]
    [Range(0.001f, 0.05f)] public float thickness = 0.01f;
    [Range(0f, 0.1f)]      public float glow = 0.02f;

    private Material mat;

    // IDs cacheados (evita string lookup cada frame)
    static readonly int ID_Freq      = Shader.PropertyToID("_Frequency");
    static readonly int ID_Amp       = Shader.PropertyToID("_Amplitude");
    static readonly int ID_Speed     = Shader.PropertyToID("_Speed");
    static readonly int ID_Noise     = Shader.PropertyToID("_NoiseAmount");
    static readonly int ID_NSpeed    = Shader.PropertyToID("_NoiseSpeed");
    static readonly int ID_Thickness = Shader.PropertyToID("_Thickness");
    static readonly int ID_Glow      = Shader.PropertyToID("_Glow");

    void Start()
    {
        // Instanciar material para no modificar el asset original
        var graphic = GetComponent<Graphic>();
        mat = Instantiate(graphic.material);
        graphic.material = mat;
    }

    void Update()
    {
        if (mat == null) return;

        mat.SetFloat(ID_Freq, frequency);
        mat.SetFloat(ID_Amp, amplitude);
        mat.SetFloat(ID_Speed, speed);
        mat.SetFloat(ID_Noise, noiseAmount);
        mat.SetFloat(ID_NSpeed, noiseSpeed);
        mat.SetFloat(ID_Thickness, thickness);
        mat.SetFloat(ID_Glow, glow);
    }

    // === Métodos para las perillas ===

    public void SetFrequency(float value) => frequency = value;
    public void SetAmplitude(float value) => amplitude = value;
    public void SetNoise(float value) => noiseAmount = value;
    public void SetSpeed(float value) => speed = value;

    void OnDestroy()
    {
        if (mat != null) Destroy(mat);
    }
}
