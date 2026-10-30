using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorFinPuntaje : MonoBehaviour
{
    [Header("Pantallas finales del Ending")]
    [SerializeField] private CanvasGroup CGFin;
    [SerializeField] private CanvasGroup CGPuntajeFinal;
    [SerializeField] private TextMeshProUGUI TXTComprensionTotal;
    [SerializeField] private TextMeshProUGUI TXTProgresoTotal;
    private float DuracionFadeo = 1f;
    private float TiempoPantallaFinal = 2f;
    private bool RegresaMenu = false;
    private void Start() { MusicManager.Instancia.ReproducirMusica("Final"); OcultarCanvasInicio(CGFin); OcultarCanvasInicio(CGPuntajeFinal); OcultarCanvasGameplay(); StartCoroutine(FinalCine()); }
    private void Update(){ if (RegresaMenu && Input.GetKeyDown(KeyCode.Return)) { RegresaMenu = false; StartCoroutine(RegresarAlMenu()); } }
    private void OcultarCanvasInicio(CanvasGroup CG) { if (CG != null) { CG.alpha = 0f; CG.blocksRaycasts = false; CG.interactable = false; } }
    private void OcultarCanvasGameplay() { GameObject CanvaGameplay = GameObject.Find("HUD"); if (CanvaGameplay != null) { CanvasGroup CGPantallasHud = CanvaGameplay.GetComponent<CanvasGroup>(); if (CGPantallasHud != null) { CGPantallasHud.alpha = 0f; CGPantallasHud.blocksRaycasts = false; CGPantallasHud.interactable = false; } } }
    private void CargarDatosCP()  {
        if(ProgresoComprensionNarrativa.Instancia != null){
            float CFinal = ProgresoComprensionNarrativa.Instancia.ComprensionCap;
            float PFinal = ProgresoComprensionNarrativa.Instancia.ProgresoCap;
            if (TXTProgresoTotal != null) { TXTProgresoTotal.text = $"Progreso: {PFinal:F0}%"; }
            if (TXTComprensionTotal != null) { TXTComprensionTotal.text = $"Porcentaje de Comprensión: {CFinal:F0}%"; } 
        }
    }
    private IEnumerator FinalCine() {
        yield return new WaitForSeconds(3f);
        yield return TransicionFadeo(CGFin, 1f);
        yield return new WaitForSeconds(TiempoPantallaFinal);
        yield return TransicionFadeo(CGFin, 0f);
        CargarDatosCP();
        yield return TransicionFadeo(CGPuntajeFinal, 1f);
        RegresaMenu = true;
    }
    private IEnumerator RegresarAlMenu(){
        yield return TransicionFadeo(CGPuntajeFinal, 0f);
        SceneManager.LoadScene("00_Inicio_Menu");
    }
    private IEnumerator TransicionFadeo(CanvasGroup CanvasG, float ValorAlfa)
    {
        float FadeoInicial = CanvasG.alpha;
        float Tiempo = 0f;
        CanvasG.blocksRaycasts = ValorAlfa > 0f;
        CanvasG.interactable = ValorAlfa > 0f;
        while (Tiempo < DuracionFadeo){
            Tiempo += Time.unscaledDeltaTime;
            CanvasG.alpha = Mathf.Lerp(FadeoInicial, ValorAlfa, Tiempo / DuracionFadeo);
            yield return null;
        }
        CanvasG.alpha = ValorAlfa;
    }
}
