using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Perilla rotativa para UI que controla parámetros del shader de onda.
///
/// SETUP:
///   1. Crear una Image circular en el Canvas (sprite de perilla).
///   2. Agregar este componente.
///   3. Asignar el RadioWaveController en "waveController".
///   4. Elegir qué parámetro controla (Frequency o Amplitude).
/// </summary>
public class UIKnob : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    public enum KnobTarget { Frequency, Amplitude }

    [Header("Configuración")]
    public KnobTarget target = KnobTarget.Frequency;
    public RadioWaveController waveController;

    [Header("Rango de Valores")]
    public float minValue = 0.5f;
    public float maxValue = 20f;

    [Header("Sensibilidad")]
    [Tooltip("Grados por píxel de arrastre.")]
    public float sensitivity = 0.5f;

    [Tooltip("Recorrido total en grados.")]
    public float maxRotation = 270f;

    [Header("Audio (opcional)")]
    [Tooltip("AudioSource para sonido de perilla girando.")]
    public AudioSource knobSound;

    private float currentAngle = 0f;
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // Inicializar ángulo según valor actual del controller
        if (waveController != null)
        {
            float currentValue = target == KnobTarget.Frequency
                ? waveController.frequency
                : waveController.amplitude;

            float min = target == KnobTarget.Frequency ? minValue : 0f;
            float max = target == KnobTarget.Frequency ? maxValue : 1f;

            float normalized = Mathf.InverseLerp(min, max, currentValue);
            currentAngle = normalized * maxRotation;
            rectTransform.localRotation = Quaternion.Euler(0f, 0f, -currentAngle);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (knobSound != null && !knobSound.isPlaying)
            knobSound.Play();
    }

    public void OnDrag(PointerEventData eventData)
    {
        float delta = eventData.delta.y * sensitivity;
        currentAngle = Mathf.Clamp(currentAngle + delta, 0f, maxRotation);

        // Rotar visualmente
        rectTransform.localRotation = Quaternion.Euler(0f, 0f, -currentAngle);

        // Calcular valor normalizado
        float normalized = currentAngle / maxRotation;

        // Aplicar al controller
        if (waveController != null)
        {
            switch (target)
            {
                case KnobTarget.Frequency:
                    waveController.SetFrequency(Mathf.Lerp(minValue, maxValue, normalized));
                    break;
                case KnobTarget.Amplitude:
                    waveController.SetAmplitude(normalized);
                    break;
            }
        }
    }
}
