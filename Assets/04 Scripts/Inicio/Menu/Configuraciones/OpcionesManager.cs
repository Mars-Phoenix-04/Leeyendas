using UnityEngine;
using UnityEngine.Audio;

public class OpcionesManager : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer AudioMixer;
    public string ParametroMaster = "VolumenMaster";
    public string ParametroMusica = "VolumenMusica";
    public string ParametroSFX = "VolumenSFX";

    [Header("Valores Globales")]
    public static float SensibilidadMouse = 50f;

    void Start(){
        float MasterGuardado = PlayerPrefs.GetFloat("Master", 100f);
        float MusicaGuardada = PlayerPrefs.GetFloat("Musica", 100f);
        float SFXGuardado = PlayerPrefs.GetFloat("SFX", 100f);

        DefinirVolumenMaster(MasterGuardado);
        DefinirVolumenMusica(MusicaGuardada);
        DefinirVolumenSFX(SFXGuardado);
        //SensibilidadMouse =  PlayerPrefs.GetFloat("Sensibilidad", 50f);
    }
    public void DefinirVolumenMaster(float valor){
        PlayerPrefs.SetFloat("Master", valor);
        AplicarVolumenMixer(ParametroMaster, valor);
    }
    public void DefinirVolumenMusica(float valor){
        PlayerPrefs.SetFloat("Musica", valor);
        AplicarVolumenMixer(ParametroMusica, valor);
    }
    public void DefinirVolumenSFX(float valor){
        PlayerPrefs.SetFloat("SFX", valor);
        AplicarVolumenMixer(ParametroSFX, valor);
    }
    public void AplicarVolumenMixer(string Parametro, float valor) { if(AudioMixer != null) { float decibeles = valor > 0 ? Mathf.Log10(valor / 100f) * 20f : -80f; AudioMixer.SetFloat(Parametro, decibeles); } }
    //Se elimino la configuracion de sensibilidad ya que esto no afectaba realmente a la jugabilidad del usuario
    //public void DefinirSensibilidad(float valor){SensibilidadMouse = valor;PlayerPrefs.SetFloat("Sensibilidad", valor);}
}
