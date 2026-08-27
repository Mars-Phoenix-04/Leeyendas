using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Navegacion : MonoBehaviour
{
    [Header("Pantallas UI")]
    public CanvasGroup Menu;
    public CanvasGroup SeleccionarCapitulo;
    public CanvasGroup Opciones;
    public CanvasGroup Creditos;
    private CanvasGroup PantallaActual;

    [Header("Fade In y Out")]
    public float VelocidadFadePantalla;


    void Awake(){
        EstadoPantalla(SeleccionarCapitulo, false);
        EstadoPantalla(Opciones, false);
        EstadoPantalla(Creditos, false);
        EstadoPantalla(Menu, false);
        PantallaActual = Menu;
    }
    
    public void ApareceMenu(){
        StartCoroutine(ActivarMenu(Menu));
    }

    private IEnumerator ActivarMenu(CanvasGroup PantallaMenu){
        while (PantallaMenu.alpha < 1f){
            PantallaMenu.alpha += Time.deltaTime * VelocidadFadePantalla;
            yield return null;
        }

        PantallaMenu.alpha = 1f;
        PantallaMenu.interactable = true;
        PantallaMenu.blocksRaycasts = true;
    }

    private void EstadoPantalla(CanvasGroup pantalla, bool seve){
        if (pantalla == null){ return; }

        pantalla.alpha = seve ? 1f : 0f;
        pantalla.interactable = seve;
        pantalla.blocksRaycasts = seve;
    }

    public void BTNJugar(){
        SFXManager.Instancia.ReproducirSonido2D("ClicBTN");
        SceneManager.LoadScene("01_ACT1_DormitorioMarta");
    }

    public void BTNSeleccionarCapitulo(){
        SFXManager.Instancia.ReproducirSonido2D("ClicBTN");
        StartCoroutine(CambioPantalla(PantallaActual, SeleccionarCapitulo));
    }

    public void BTNOpciones(){
        SFXManager.Instancia.ReproducirSonido2D("ClicBTN");
        StartCoroutine(CambioPantalla(PantallaActual, Opciones));
    }

    public void BTNCreditos(){
        SFXManager.Instancia.ReproducirSonido2D("ClicBTN");
        StartCoroutine(CambioPantalla(PantallaActual, Creditos));
    }
    public void BTNSalir(){
        SFXManager.Instancia.ReproducirSonido2D("ClicBTN");
        Application.Quit();
    }

    public void BTNVolverMenu(){
        SFXManager.Instancia.ReproducirSonido2D("ClicBTN");
        StartCoroutine(CambioPantalla(PantallaActual, Menu));
    }

    private IEnumerator CambioPantalla(CanvasGroup SalidaPantalla, CanvasGroup EntradaPantalla){
        SalidaPantalla.interactable = false;
        SalidaPantalla.blocksRaycasts = false;

        while (SalidaPantalla.alpha > 0f){
            SalidaPantalla.alpha -= Time.deltaTime * VelocidadFadePantalla;
            yield return null;
        }
        SalidaPantalla.alpha = 0f;
        PantallaActual = EntradaPantalla;

        while (EntradaPantalla.alpha < 1f){
            EntradaPantalla.alpha += Time.deltaTime * VelocidadFadePantalla;
            yield return null;
        }
        EntradaPantalla.alpha = 1f;
        EntradaPantalla.interactable = true;
        EntradaPantalla.blocksRaycasts = true;
    }
}
