using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UI_Pausa : MonoBehaviour
{
    [Header("Pantallas UI")]
    public CanvasGroup PantallaPausa;
    public CanvasGroup PausaMenu;
    public CanvasGroup OpcionesMenu;
    public string NombreEscenaInicio = "00_Inicio_Menu";
    public float DuracionDesvanecimiento = 0.4f;
    public static bool JuegoPausado { get; private set; }
    private CanvasGroup PantallaActiva;
    private bool Transicion = false;

    void Awake(){
        EstadoPantalla(PantallaPausa, false);
        EstadoPantalla(PausaMenu, false);
        EstadoPantalla(OpcionesMenu, false);
        JuegoPausado = false;
    }
    private void Update(){
        if (Transicion) return;
        if (UI_Claves.MostrandoClave || UI_Tutorial.TutorialActivo) return;
        if ( Input.GetKeyDown(KeyCode.P)) { if (!JuegoPausado) { SFXManager.Instancia.ReproducirSonido2D("ClicBTN"); PausarJuego(); } else { if (PantallaActiva == OpcionesMenu) { SFXManager.Instancia.ReproducirSonido2D("ClicBTN"); BTNVolverMenuPausa(); } else { BTNContinuar(); } } }
    }
    public void PausarJuego(){
        JuegoPausado = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartCoroutine(ActivarPausa());
    }
    private void EstadoPantalla(CanvasGroup Canvas, bool Seve){
        if (Canvas == null) return;
        Canvas.alpha = Seve ? 1f : 0f;
        Canvas.interactable = Seve;
        Canvas.blocksRaycasts = Seve;
    }

    //BOTONES FUNCIONES
    public void BTNContinuar() { if(SFXManager.Instancia != null) SFXManager.Instancia.ReproducirSonido2D("ClicBTN"); StartCoroutine(DesactivarPausa()); }
    public void BTNOpciones() { if (SFXManager.Instancia != null) SFXManager.Instancia.ReproducirSonido2D("ClicBTN"); StartCoroutine(CambioPantallas(PantallaActiva, OpcionesMenu)); }
    public void BTNVolverMenuPausa() { if (SFXManager.Instancia != null) SFXManager.Instancia.ReproducirSonido2D("ClicBTN"); StartCoroutine(CambioPantallas(PantallaActiva, PausaMenu)); }
    public void BTNVolverAlInicio() { if (SFXManager.Instancia != null) SFXManager.Instancia.ReproducirSonido2D("ClicBTN"); Time.timeScale = 1f; JuegoPausado = false; SceneManager.LoadScene(NombreEscenaInicio); }

    //Corrutinas aqui, venga por sus corrutinas
    private IEnumerator ActivarPausa(){
        Transicion = true;
        PantallaActiva = PausaMenu;
        StartCoroutine(TransicionFadeo(PausaMenu, 1f));
        yield return TransicionFadeo(PantallaPausa, 1f);
        Transicion = false;
    }
    private IEnumerator DesactivarPausa(){
        Transicion = true;
        if(PantallaActiva != null) { StartCoroutine(TransicionFadeo(PantallaActiva, 0f)); }
        yield return TransicionFadeo(PantallaPausa, 0f);
        Time.timeScale = 1f;
        JuegoPausado = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Transicion = false;
    }
    private IEnumerator CambioPantallas(CanvasGroup SalidaPantalla, CanvasGroup EntradaPantalla){
        Transicion = true;
        yield return TransicionFadeo(SalidaPantalla, 0f);
        PantallaActiva = EntradaPantalla;
        yield return TransicionFadeo(EntradaPantalla, 1f);
        Transicion = false;
    }
    private IEnumerator TransicionFadeo(CanvasGroup CanvasG, float ValorAlfa){
        float FadeoInicial = CanvasG.alpha;
        float Tiempo = 0f;
        CanvasG.blocksRaycasts = ValorAlfa > 0f;
        CanvasG.interactable = ValorAlfa > 0f;
        while (Tiempo < DuracionDesvanecimiento)
        {
            Tiempo += Time.unscaledDeltaTime;
            CanvasG.alpha = Mathf.Lerp(FadeoInicial, ValorAlfa, Tiempo / DuracionDesvanecimiento);
            yield return null;
        }
        CanvasG.alpha = ValorAlfa;
    }
    //Hacer que si se sale de la aplicación, el jugador pueda seguir usando pausa.
    private void OnApplicationFocus(bool focus){ //Este es para cuando vuelvo al juego
        if(focus && JuegoPausado){
            RefrescarES();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    private void RefrescarES(){
        EventSystem ES = EventSystem.current;
        if(ES != null) { ES.SetSelectedGameObject(null); ES.enabled = false; ES.enabled = true; var ModuloInput = ES.GetComponent<UnityEngine.EventSystems.StandaloneInputModule>(); if(ModuloInput != null) { ModuloInput.enabled = false; ModuloInput.enabled = true; } }
    }
}
