using UnityEngine;
using UnityEngine.UI;

public class TuningMinigame : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Controller de la onda del jugador (la que se manipula).")]
    public RadioWaveController playerWave;

    [Tooltip("Controller de la onda objetivo (referencia visual). Puede ser null si no querés mostrarla.")]
    public RadioWaveController targetWave;

    [Tooltip("Texto para feedback al jugador.")]
    public Text feedbackText;

    [Tooltip("Imagen que cambia de color según cercanía.")]
    public Image matchIndicator;

    [Header("Dificultad")]
    [Tooltip("Tolerancia para considerar sintonizado. Menor = más difícil.")]
    [Range(0.05f, 1f)]
    public float tolerance = 0.3f;

    [Tooltip("Tiempo límite en segundos. 0 = sin límite.")]
    public float timeLimit = 30f;

    [Header("Rango de Frecuencia Objetivo")]
    public float minTargetFreq = 2f;
    public float maxTargetFreq = 15f;

    [Header("Rango de Amplitud Objetivo")]
    public float minTargetAmp = 0.2f;
    public float maxTargetAmp = 0.9f;

    [Header("Colores del Indicador")]
    public Color colorFar = Color.red;
    public Color colorClose = Color.yellow;
    public Color colorMatched = Color.green;

    // Estado interno
    private float targetFrequency;
    private float targetAmplitude;
    private float timer;
    private bool roundActive = false;

    /// <summary>
    /// Evento que se dispara cuando el jugador sintoniza correctamente.
    /// Suscribite desde otros scripts para dar puntos, avanzar nivel, etc.
    /// </summary>
    public System.Action OnTuned;

    /// <summary>
    /// Evento que se dispara cuando se acaba el tiempo.
    /// </summary>
    public System.Action OnTimeUp;

    private void Start()
    {
        StartNewRound();
    }

    public void StartNewRound()
    {
        // Generar objetivo aleatorio
        targetFrequency = Random.Range(minTargetFreq, maxTargetFreq);
        targetAmplitude = Random.Range(minTargetAmp, maxTargetAmp);

        // Configurar onda objetivo si existe
        if (targetWave != null)
        {
            targetWave.frequency = targetFrequency;
            targetWave.amplitude = targetAmplitude;
            targetWave.noiseAmount = 0f; // Señal pura, sin ruido
        }

        // Resetear ruido del jugador (empieza con estática)
        if (playerWave != null)
        {
            playerWave.noiseAmount = 0.5f;
        }

        timer = timeLimit;
        roundActive = true;

        if (feedbackText != null)
            feedbackText.text = "Sintonizá la frecuencia...";
    }

    private void Update()
    {
        if (!roundActive || playerWave == null) return;

        // Timer
        if (timeLimit > 0f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = 0f;
                roundActive = false;

                if (feedbackText != null)
                    feedbackText.text = "¡Tiempo agotado!";

                OnTimeUp?.Invoke();
                return;
            }
        }

        // Calcular diferencia normalizada
        float freqRange = maxTargetFreq - minTargetFreq;
        float freqDiff = Mathf.Abs(playerWave.frequency - targetFrequency) / freqRange;

        float ampDiff = Mathf.Abs(playerWave.amplitude - targetAmplitude);

        float totalDiff = (freqDiff + ampDiff) * 0.5f;

        // Ajustar ruido: más cerca = menos estática
        playerWave.SetNoise(Mathf.Clamp01(totalDiff * 2f));

        // Actualizar indicador
        if (matchIndicator != null)
        {
            if (totalDiff < tolerance * 0.5f)
                matchIndicator.color = colorMatched;
            else if (totalDiff < tolerance)
                matchIndicator.color = colorClose;
            else
                matchIndicator.color = colorFar;
        }

        // ¡Sintonizado!
        if (totalDiff < tolerance * 0.3f)
        {
            roundActive = false;
            playerWave.SetNoise(0f);

            if (feedbackText != null)
                feedbackText.text = "¡Señal sintonizada!";

            OnTuned?.Invoke();
        }
        else if (feedbackText != null && timeLimit > 0f)
        {
            feedbackText.text = string.Format("Sintonizando... ({0:F1}s)", timer);
        }
    }

    /// <summary>
    /// Devuelve qué tan cerca está el jugador (0 = perfecto, 1 = lejos).
    /// Útil para UI adicional como barras de progreso.
    /// </summary>
    public float GetMatchProgress()
    {
        if (playerWave == null) return 1f;

        float freqRange = maxTargetFreq - minTargetFreq;
        float freqDiff = Mathf.Abs(playerWave.frequency - targetFrequency) / freqRange;
        float ampDiff = Mathf.Abs(playerWave.amplitude - targetAmplitude);

        return Mathf.Clamp01((freqDiff + ampDiff) * 0.5f);
    }

    public bool IsRoundActive() => roundActive;
    public float GetTimeRemaining() => timer;
}
