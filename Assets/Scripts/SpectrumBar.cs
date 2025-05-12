using UnityEngine;
using System.Collections;

public class SpectrumBar : MonoBehaviour
{
    public enum BarType { Realtime, PeakLevel, MeanLevel };

    public int index;
    public BarType barType;

    AudioSpectrum spectrum;

    void Awake()
    {
        spectrum = FindObjectOfType(typeof(AudioSpectrum)) as AudioSpectrum;
        transform.position += Vector3.right*index;
    }

    void Update ()
    {
        if (index < spectrum.Levels.Length) {
            float pos = 0.0f;

            switch (barType) {
            case BarType.Realtime:
                pos = spectrum.Levels[index];
                break;
            case BarType.PeakLevel:
                pos = spectrum.PeakLevels[index];
                break;
            case BarType.MeanLevel:
                pos = spectrum.MeanLevels[index];
                break;
            }

            var vs = transform.localPosition;
            vs.y = pos * 20.0f;
            transform.localPosition = vs;
        }
    }
}