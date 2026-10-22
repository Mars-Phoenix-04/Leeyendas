using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Persistencia : MonoBehaviour
{
    private static UI_Persistencia InstanciaDeCanvaUi;
    private void Awake(){ if (InstanciaDeCanvaUi == null) { InstanciaDeCanvaUi = this; DontDestroyOnLoad(gameObject); } else { Destroy(gameObject); } }
    private void OnEnable(){ SceneManager.sceneLoaded += VerificarEscena; }
    private void OnDisable() { SceneManager.sceneLoaded -= VerificarEscena; }
    private void VerificarEscena(Scene Escena, LoadSceneMode Modo) { if(Escena.name == "00_Inicio_Menu") { Destroy(gameObject); } }
}
