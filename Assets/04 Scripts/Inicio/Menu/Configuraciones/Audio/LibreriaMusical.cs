using UnityEngine;

[System.Serializable]
public struct Musica{
    public string NombreMusica;
    public AudioClip clip;
}

public class LibreriaMusical : MonoBehaviour
{
    public Musica[] rolas;

    public AudioClip ObtenerRola(string NombreMusica){
        foreach (var cancion in rolas){ if (cancion.NombreMusica == NombreMusica) return cancion.clip;}
        return null;
    }

}
