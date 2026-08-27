using UnityEngine;
using UnityEngine.Audio;

public class OpcionesManager : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer AudioMixer;
    public string ParametroMusica = "VolumenMusica";
    public string ParametroSFX = "VolumenSFX";

    [Header("Valores Globales")]
    public static float SensibilidadMouse = 50f;

    void Start(){
        float MusicaGuardada = PlayerPrefs.GetFloat("Musica", 100f);
        float SFXGuardado = PlayerPrefs.GetFloat("SFX", 100f); 
        SensibilidadMouse =  PlayerPrefs.GetFloat("Sensibilidad", 50f);
    }

    public void DefinirVolumenMusica(float valor){
        PlayerPrefs.SetFloat("Musica", valor);
        if (AudioMixer != null){
            float decibeles = valor > 0 ? Mathf.Log10(valor / 100f) * 20f : -80f; //Conversion a decibeles en los valores de 0-100
            AudioMixer.SetFloat(ParametroMusica, decibeles);
        }
    }

    public void DefinirVolumenSFX(float valor){
        PlayerPrefs.SetFloat("SFX", valor);
        if (AudioMixer != null)
        {
            float decibeles = valor > 0 ? Mathf.Log10(valor / 100f) * 20f : -80f; 
            AudioMixer.SetFloat(ParametroSFX, decibeles);
        }
    }

    public void DefinirSensibilidad(float valor){
        SensibilidadMouse = valor;
        PlayerPrefs.SetFloat("Sensibilidad", valor);
    }
}
