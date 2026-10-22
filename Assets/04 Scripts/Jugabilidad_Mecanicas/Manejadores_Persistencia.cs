using UnityEngine;
using UnityEngine.SceneManagement;

public class Manejadores_Persistencia : MonoBehaviour
{
    public static Manejadores_Persistencia Instancia_Manejadores { get; private set; }
    private void Awake() { if (Instancia_Manejadores == null) { Instancia_Manejadores = this; DontDestroyOnLoad(gameObject); } else { Destroy(gameObject); } }
    private void OnEnable() { SceneManager.sceneLoaded += VerificarEscena; }
    private void OnDisable() { SceneManager.sceneLoaded -= VerificarEscena; }
    private void VerificarEscena(Scene Escena, LoadSceneMode Modo) { if (Escena.name == "00_Inicio_Menu") { Destroy(gameObject); Instancia_Manejadores = null; } }
}
