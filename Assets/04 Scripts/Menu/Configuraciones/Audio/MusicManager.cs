using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instancia;
    [SerializeField]
    private LibreriaMusical MusicaLib;
    [SerializeField]
    private AudioSource MusicaSource;

    private void Awake(){
        if (Instancia != null) { 
            Destroy(gameObject); 
        }
        else { 
            Instancia = this; 
            DontDestroyOnLoad(gameObject); 
        }
    }

    public void ReproducirMusica (string NombreCancion, float DuracionFade = 0.5f){
        StartCoroutine(FadeoEntreCanciones(MusicaLib.ObtenerRola(NombreCancion), DuracionFade));
    }

    IEnumerator FadeoEntreCanciones(AudioClip SiguienteMusica, float DuracionFade = 0.5f)
    {
        float porcentaje = 0;
        while (porcentaje < 1)
        {
            porcentaje += Time.deltaTime * 1 / DuracionFade;
            MusicaSource.volume = Mathf.Lerp(1f, 0, porcentaje);
            yield return null;
        }

        MusicaSource.clip = SiguienteMusica;
        MusicaSource.Play();
        porcentaje = 0;
        while (porcentaje < 1)
        {
            porcentaje += Time.deltaTime * 1 / DuracionFade;
            MusicaSource.volume = Mathf.Lerp(0, 1f, porcentaje);
            yield return null;
        }
    }
}
