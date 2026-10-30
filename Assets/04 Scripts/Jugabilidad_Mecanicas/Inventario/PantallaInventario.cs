using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class PantallaInventario : MonoBehaviour
{
    [Header("Pantalla Inventario y Textos")]
    [SerializeField] private CanvasGroup CGPantallaInv;
    [SerializeField] private float DuracionDesvanecimiento = 0.4f;
    [SerializeField] private GameObject PantallaInv;
    [SerializeField] private TextMeshProUGUI TXTCapitulo;
    [SerializeField] private TextMeshProUGUI TXTProgreso;
    [SerializeField] private TextMeshProUGUI TXTComprension;

    [SerializeField] private CasillasInv[] ArregloCasillas = new CasillasInv[ManejadorInventario.NoCasillas];
    private bool InventarioAbierto = false;
    void Start(){ 
        //CerrarInventario(); 
        if(CGPantallaInv != null) { CGPantallaInv.alpha = 0f; CGPantallaInv.interactable = false; CGPantallaInv.blocksRaycasts = false; }
        InventarioAbierto = false;
    }

    void Update(){
        if ((ManejadorDialogos.Instancia != null && ManejadorDialogos.Instancia.DialogoActivo) || UI_Pausa.JuegoPausado || UI_Claves.MostrandoClave || UI_Tutorial.TutorialActivo) return;
        if (Input.GetKeyDown(KeyCode.B)) CambioPantallaINV(); 
    }

    public void CambioPantallaINV(){
        InventarioAbierto = !InventarioAbierto;
        if (InventarioAbierto) { AbrirInventario();  }
        else { CerrarInventario(); }
    }
    private void CerrarInventario(){
        //PantallaInv.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StopAllCoroutines();
        StartCoroutine(TransicionFadeo(CGPantallaInv, 0f));
    }
    private void AbrirInventario(){
        //PantallaInv.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        CargarCPC();
        StopAllCoroutines();
        StartCoroutine(TransicionFadeo(CGPantallaInv, 1f));
    }

    private void CargarCPC(){
        CargarInventario();
        if (ProgresoComprensionNarrativa.Instancia != null){
            CargarCapitulo(ProgresoComprensionNarrativa.Instancia.Capitulo);
            CargarProgreso(ProgresoComprensionNarrativa.Instancia.ProgresoCap);
            CargarComprension(ProgresoComprensionNarrativa.Instancia.ComprensionCap);
        }
    }
    private void CargarComprension(float comprension) { if (TXTComprension != null) TXTComprension.text = $"Comprensión : {Mathf.RoundToInt(comprension)}%"; }
    private void CargarProgreso(float progreso) { if (TXTProgreso != null) TXTProgreso.text = $"Progreso: {Mathf.RoundToInt(progreso)}%"; }
    private void CargarCapitulo(int capitulo){  if (TXTCapitulo != null) TXTCapitulo.text = $"Capítulo {capitulo}"; }

    private void CargarInventario(){
        if (ManejadorInventario.Instancia == null) return;
        Clave_DATA[] Claves = ManejadorInventario.Instancia.ObtenerTODASClaves();

        for (int i = 0; i < ArregloCasillas.Length; i++){
            if (ArregloCasillas[i] == null) continue;
            Clave_DATA clave = (i < Claves.Length) ? Claves[i] : null;
            ArregloCasillas[i].DefinirClave(clave);
        }
    }

    private void OnEnable(){
        if (ManejadorInventario.Instancia != null) ManejadorInventario.Instancia.CambiosEnElInventario += CargarInventario;
        if (ProgresoComprensionNarrativa.Instancia != null){
            ProgresoComprensionNarrativa.Instancia.CambioDeCapitulo += CargarCapitulo;
            ProgresoComprensionNarrativa.Instancia.CambioEnProgreso += CargarProgreso;
            ProgresoComprensionNarrativa.Instancia.CambioEnComprension += CargarComprension;
        }
    }
    private void OnDisable()
    {
        if (ManejadorInventario.Instancia != null) ManejadorInventario.Instancia.CambiosEnElInventario -= CargarInventario;
        if (ProgresoComprensionNarrativa.Instancia != null){
            ProgresoComprensionNarrativa.Instancia.CambioDeCapitulo -= CargarCapitulo;
            ProgresoComprensionNarrativa.Instancia.CambioEnProgreso -= CargarProgreso;
            ProgresoComprensionNarrativa.Instancia.CambioEnComprension -= CargarComprension;
        }
    }
    private IEnumerator TransicionFadeo(CanvasGroup CanvasG, float ValorAlfa)
    {
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
}
