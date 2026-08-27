using UnityEngine;
using static UnityEditor.PlayerSettings;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instancia;
    [SerializeField]
    private LibreriaSFX SFXLib;
    [SerializeField]
    private AudioSource sfx2DSource;


    private void Awake(){
        if (Instancia != null){
            Destroy(gameObject);
        }
        else{
            Instancia = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    //AUDIO PARA DENTRO DEL JUEGO
    public void ReproducirSonido3D (AudioClip clip, Vector3 pos){
        if (clip != null){
            AudioSource.PlayClipAtPoint(clip, pos);
        }
    }
    public void ReproducirSonido3D(string NombreSFX, Vector3 pos){
        ReproducirSonido3D(SFXLib.ObtenerSFX(NombreSFX), pos);
    }

    //AUDIO PARA MENUS Y PANTALLAS
    public void ReproducirSonido2D(string NombreSFX){
        sfx2DSource.PlayOneShot(SFXLib.ObtenerSFX(NombreSFX));
    }

}
