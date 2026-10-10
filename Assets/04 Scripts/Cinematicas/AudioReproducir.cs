using UnityEngine;

public class AudioReproducir : MonoBehaviour
{
    void Start()
    {
        MusicManager.Instancia.ReproducirMusica("Cinematica Inicial");
    }
}
