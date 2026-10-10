using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class Evento_PuntoInflexion : MonoBehaviour
{
    public static Evento_PuntoInflexion InstanciaPuntoInflexion { get; private set; }
    [Header("Elementos de la UI")]
    [SerializeField] private CanvasGroup CGHUD;
    [SerializeField] private CanvasGroup CGPuntoInflexion;
    [SerializeField] private float DuracionDesvanecimiento = 0.8f;
    [SerializeField] private Image IlustraciónCapitulo;
    [SerializeField] private TextMeshProUGUI TXTCapitulo;
    [SerializeField] private TextMeshProUGUI TXTHistoria;
    //Inventario
    [SerializeField] private Claves_PuntoInflexion[] ArregloCasillas = new Claves_PuntoInflexion[ManejadorInventario.NoCasillas];

    [Header("Alerta de Confirmacion de uso de Clave")]
    [SerializeField] private CanvasGroup CGAlertaConfirmar;
    [SerializeField] private TextMeshProUGUI TXTConfirmar;
    [Header("Alerta de Comprension y subir")]
    [SerializeField] private CanvasGroup CGAlertaComprension;
    [SerializeField] private TextMeshProUGUI TXTComprensionObtenida;
    [Header("Alerta de CLAVE incorrecta")]
    [SerializeField] private CanvasGroup CGEsaNoEsLaClave;

    //Instancia del evento activa, eventos, listas de inventarios.
    public bool EstaActivo { get; private set; }
    private OBJ_Script_PuntoInflexion EventoPI;

    private List<Clave_DATA> ClavesDisponibles = new List<Clave_DATA>();
    private List<int> IndiceEnInventario = new List<int>();
    private int IndiceClaveSelec = -1;
    private bool EsperandoInput = false;
    private bool PITerminado = false;

    private void Awake(){
        if (InstanciaPuntoInflexion == null) InstanciaPuntoInflexion = this; else Destroy(gameObject);
        if(CGPuntoInflexion != null){
            CGPuntoInflexion.alpha = 0f;
            CGPuntoInflexion.blocksRaycasts = false;
            CGPuntoInflexion.interactable = false;
        }
        if (CGAlertaConfirmar != null) CGAlertaConfirmar.alpha = 0f;
        if (CGAlertaComprension != null) CGAlertaComprension.alpha = 0f;
    }
    private void Update(){
        if (!EstaActivo || PITerminado) return;
        if (EsperandoInput) { ConfirmarUsoClave(); } else { NavegacionEntreClaves(); }
    }

    public void IniciarPuntoInflexion(OBJ_Script_PuntoInflexion EPI){
        StartCoroutine(TransicionFadeo(CGHUD, 0f));
        EventoPI = EPI;
        EstaActivo = true;
        EsperandoInput = false;
        PITerminado = false;
        CargarElementosPuntoInflexion();
        CargarInventarioClaves();
        ActualizarTexto();
        StartCoroutine(TransicionFadeo(CGPuntoInflexion, 1f));
    }
    private void CargarElementosPuntoInflexion(){
        if (EventoPI == null) return;
        if (IlustraciónCapitulo != null) IlustraciónCapitulo.sprite = EventoPI.ImagenCap;
        if (TXTCapitulo != null) TXTCapitulo.text = EventoPI.TXTCap;
    }
    private void CargarInventarioClaves(){
        ClavesDisponibles.Clear();
        IndiceEnInventario.Clear();

        //Obtener las claves que el usuario ha obtenido
        if (ManejadorInventario.Instancia != null){
            Clave_DATA[] ClavesCargadas = ManejadorInventario.Instancia.ObtenerTODASClaves();
            for (int i = 0; i < ClavesCargadas.Length; i++){
                if (ClavesCargadas[i] != null){
                    ClavesDisponibles.Add(ClavesCargadas[i]);
                    IndiceEnInventario.Add(i);
                }
            }
        }
        //Asignar los datos a las casillas
        for (int i = 0; i < ArregloCasillas.Length; i++){
            if (ArregloCasillas[i] == null) continue;
            Clave_DATA Clave = (i < ClavesDisponibles.Count) ? ClavesDisponibles[i] : null;
            ArregloCasillas[i].AgarrarClaves(Clave);
        }
        IndiceClaveSelec = ClavesDisponibles.Count > 0 ? 0 : -1;
        ActualizarSeleccionCasillas();
    }

    private void NavegacionEntreClaves() {
        if (ClavesDisponibles.Count == 0) return;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)){
            IndiceClaveSelec--;
            if (IndiceClaveSelec < 0) IndiceClaveSelec = ClavesDisponibles.Count - 1;
            EstadoSeleccionado();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)){
            IndiceClaveSelec++;
            if (IndiceClaveSelec >= ClavesDisponibles.Count) IndiceClaveSelec = 0;
            EstadoSeleccionado();
        }

        if (Input.GetKeyDown(KeyCode.Return) && IndiceClaveSelec >= 0) { AlertaConfirmacion(); }
    }
    private void EstadoSeleccionado() { ActualizarSeleccionCasillas(); ActualizarTexto(); }
    private void ActualizarSeleccionCasillas(){
        for (int i = 0; i < ArregloCasillas.Length; i++){
            if (ArregloCasillas[i] == null) continue;
            bool EstaSeleccionada = (i == IndiceClaveSelec && i < ClavesDisponibles.Count);
            ArregloCasillas[i].ClaveSeleccionada(EstaSeleccionada);
        }
    }
    private void ActualizarTexto(){
        if (EventoPI == null || TXTHistoria == null) return;
        if(ClavesDisponibles.Count > 0 && IndiceClaveSelec >= 0 && IndiceClaveSelec < ClavesDisponibles.Count){
            string PalabraSeleccionada = $"{ClavesDisponibles[IndiceClaveSelec].NombreClave}";
            TXTHistoria.text = string.Format(EventoPI.TXTHistoria, PalabraSeleccionada);
        }
        else{
            TXTHistoria.text = string.Format(EventoPI.TXTHistoria, "________");
        }
    }
    private void AlertaConfirmacion(){
        EsperandoInput = true;
        if (TXTConfirmar != null){
            TXTConfirmar.text = $"¿Desea usar la clave '{ClavesDisponibles[IndiceClaveSelec].NombreClave}'?";
        }
        StartCoroutine(TransicionFadeo(CGAlertaConfirmar, 1f));
    }
    private void ConfirmarUsoClave(){
        if (Input.GetKeyDown(KeyCode.Return)) { StartCoroutine(UsoClave()); }
        else if (Input.GetKeyDown(KeyCode.Escape)) { EsperandoInput = false; StartCoroutine(TransicionFadeo(CGAlertaConfirmar, 0f)); }
    }

    //Corutinas, en pocas palabras, mi pesadilla
    private IEnumerator UsoClave(){
        PITerminado = true;
        yield return TransicionFadeo(CGAlertaConfirmar, 0f);
        Clave_DATA ClaveQueSeUso = ClavesDisponibles[IndiceClaveSelec];
        int CasillaInventario = IndiceEnInventario[IndiceClaveSelec];

        //Revisar que si se este usando la clave correcta, sino lo regresa
        bool SiEsLaClaveCorrecta = (EventoPI.ClaveEventoUso == null) || (ClaveQueSeUso == EventoPI.ClaveEventoUso) || (ClaveQueSeUso.NombreClave == EventoPI.ClaveEventoUso.NombreClave);
        if (!SiEsLaClaveCorrecta) { yield return ClaveIncorrecta(); yield break; }
        //Esto sigue igual y sucede cuando la clave si es la que viene en los datos
        TXTHistoria.text = string.Format(EventoPI.TXTHistoria, $"<b>{ClaveQueSeUso.NombreClave}</b>");
        int ComprensionPuntos = ClaveQueSeUso.PuntosComprension;
        float ProgresoPuntos = (EventoPI != null) ? EventoPI.ProgresoHistoria : 0f;
        ProgresoComprensionNarrativa.Instancia.ComprensionCapituloCambio(ComprensionPuntos);
        ProgresoComprensionNarrativa.Instancia.ProgresoCapituloCambio(ProgresoPuntos);
        if (TXTComprensionObtenida != null) { TXTComprensionObtenida.text = $"+{ComprensionPuntos} Comprensión"; }
        yield return TransicionFadeo(CGAlertaComprension, 1f);
        yield return new WaitForSecondsRealtime(5f);
        yield return TransicionFadeo(CGAlertaComprension, 0f);

        if(ManejadorInventario.Instancia != null) { ManejadorInventario.Instancia.EliminarClave(CasillaInventario); }
        yield return TransicionFadeo(CGPuntoInflexion, 0f);
        EstaActivo = false;
        Action AccionPosDialogo = null;
        if (EventoPI != null && EventoPI.CambiarDeEscena && !string.IsNullOrEmpty(EventoPI.NombreSiguienteEscena)) { 
            Debug.Log($" Cambiar escena = {EventoPI.CambiarDeEscena} y el nombre es {EventoPI.NombreSiguienteEscena}"); 
            string NuevaEscena = EventoPI.NombreSiguienteEscena;
            AccionPosDialogo = () => { Debug.Log($" Intento cargar la escena con nombre: {EventoPI.NombreSiguienteEscena}"); SceneManager.LoadScene(NuevaEscena); }; 
        } else { Debug.LogError("Esta vacio el evento pi"); }
        if (ManejadorDialogos.Instancia != null && EventoPI != null && EventoPI.TXTResultadoPuntoInflexion.Length > 0) { ManejadorDialogos.Instancia.IniciarDescDialg("Marta", EventoPI.TXTResultadoPuntoInflexion, AccionPosDialogo); } else { AccionPosDialogo?.Invoke(); }

        /*
        if(ManejadorDialogos.Instancia != null && EventoPI.TXTResultadoPuntoInflexion.Length > 0){
            ManejadorDialogos.Instancia.IniciarDescDialg("Marta", EventoPI.TXTResultadoPuntoInflexion);
        } else if (EventoPI.CambiarDeEscena && !string.IsNullOrEmpty(EventoPI.NombreSiguienteEscena)){
            //ManejadorDialogos.Instancia.IniciarDescDialg("Marta", EventoPI.TXTResultadoPuntoInflexion);
            SceneManager.LoadScene(EventoPI.NombreSiguienteEscena);
        }
        */
    }
    private IEnumerator TransicionFadeo(CanvasGroup CanvasG, float ValorAlfa) {
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
    private IEnumerator ClaveIncorrecta(){
        yield return TransicionFadeo(CGEsaNoEsLaClave, 1f);
        yield return new WaitForSecondsRealtime(0.25f);
        while (!Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.Space) && !Input.GetMouseButtonDown(0)) { yield return null; }
        yield return TransicionFadeo(CGEsaNoEsLaClave, 0f);
        EsperandoInput = false;
        PITerminado = false;
    }
}
