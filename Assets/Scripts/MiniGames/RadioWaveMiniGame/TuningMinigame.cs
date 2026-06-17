using UnityEngine;
using UnityEngine.UI;

public class TuningMinigame : MiniGame
{
    [Header("Referencias")]
    public GameObject minigameCanvas;
    public RadioWaveController playerWave;
    public RadioWaveController targetWave;
    public Text feedbackText;
    public Image matchIndicator;

    [Header("Dificultad")]
    [Range(0.05f, 1f)]
    public float tolerance = 0.3f;

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

    float targetFrequency;
    float targetAmplitude;
    float timer;

    bool roundActive;
    bool solved;

    public System.Action OnTuned;

    protected override void OnOpen()
    {
        if (solved)
        {
            Close();
            return;
        }
        minigameCanvas.SetActive(true);
        StartNewRound();

    }

    protected override void OnClose()
    {
        roundActive = false;
        minigameCanvas.SetActive(false);
    }

    void StartNewRound()
    {
        targetFrequency = Random.Range(minTargetFreq, maxTargetFreq);
        targetAmplitude = Random.Range(minTargetAmp, maxTargetAmp);

        if (targetWave != null)
        {
            targetWave.frequency = targetFrequency;
            targetWave.amplitude = targetAmplitude;
            targetWave.noiseAmount = 0f;
        }

        if (playerWave != null)
        {
            playerWave.noiseAmount = 0.5f;
        }

        timer = timeLimit;
        roundActive = true;

        if (feedbackText != null)
            feedbackText.text = "Sintonizá la frecuencia...";
    }

    protected override void Update()
    {
        if (!roundActive || playerWave == null) return;

        if (timeLimit > 0f)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                timer = 0f;
                roundActive = false;

                if (feedbackText != null)
                    feedbackText.text = "¡Tiempo agotado!";

                return;
            }
        }

        float freqRange = maxTargetFreq - minTargetFreq;
        float freqDiff = Mathf.Abs(playerWave.frequency - targetFrequency) / freqRange;
        float ampDiff = Mathf.Abs(playerWave.amplitude - targetAmplitude);
        float totalDiff = (freqDiff + ampDiff) * 0.5f;

        playerWave.SetNoise(Mathf.Clamp01(totalDiff * 2f));

        if (matchIndicator != null)
        {
            if (totalDiff < tolerance * 0.5f)
                matchIndicator.color = colorMatched;
            else if (totalDiff < tolerance)
                matchIndicator.color = colorClose;
            else
                matchIndicator.color = colorFar;
        }

        if (totalDiff < tolerance * 0.3f)
        {
            roundActive = false;
            solved = true;

            playerWave.SetNoise(0f);

            if (feedbackText != null)
                feedbackText.text = "¡Señal sintonizada!";

            OnTuned?.Invoke();

            GameManager.instance.MiniGameCompleted();

            Close();
        }
        else if (feedbackText != null && timeLimit > 0f)
        {
            feedbackText.text = $"Sintonizando... ({timer:F1}s)";
        }
    }

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