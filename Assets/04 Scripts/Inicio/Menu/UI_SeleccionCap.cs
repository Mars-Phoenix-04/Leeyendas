using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_SeleccionCap : MonoBehaviour
{
    [System.Serializable] public struct DatosCapitulo { public int NoCapitulo; public string TituloCapitulo; public Sprite IlustracionCapitulo; public string NombreEscena; public bool ExisteCapituloEs; }
    [Header("Datos de los capitulos")]
    [SerializeField] private DatosCapitulo[] Capitulos;
    [Header("Cosas de la UI para cambiar")]
    [SerializeField] private CanvasGroup CGPantallaSeleccionCap;
    [SerializeField] private TMP_Text TXTNoCap;
    [SerializeField] private TMP_Text TXTNombreCap;
    [SerializeField] private Image IllCap;
    [Header("Alerta de no capitulo")]
    [SerializeField] private CanvasGroup CGNoCapituloAqui;
    [SerializeField] private float DuracionDesvanecimiento = 0.4f;
    private int IndiceCapituloActual = 0;
    private bool MostrarAlertaNoCap = false;

    private void Awake() { if (CGNoCapituloAqui != null) { CGNoCapituloAqui.alpha = 0f; CGNoCapituloAqui.blocksRaycasts = false; CGNoCapituloAqui.interactable = false; } }
    private void Start() { ActualizarUI(); }
    private void Update() {
        if (MostrarAlertaNoCap) return;
        if (CGPantallaSeleccionCap != null && (!CGPantallaSeleccionCap.interactable || CGPantallaSeleccionCap.alpha < 0.9999f)) { return; }
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) { SFXManager.Instancia.ReproducirSonido2D("ClicBTN"); ConfirmarSeleccion(); }
    }
    public void BTNSiguienteCap() => CambiarCapitulo(1);
    public void BTNAnteriorCap() => CambiarCapitulo(-1);

    public void CambiarCapitulo(int Direccion) {
        if (CGPantallaSeleccionCap != null && (!CGPantallaSeleccionCap.interactable || CGPantallaSeleccionCap.alpha < 0.9999f)) { return; }
        SFXManager.Instancia.ReproducirSonido2D("ClicBTN");
        IndiceCapituloActual += Direccion;
        if (IndiceCapituloActual >= Capitulos.Length) IndiceCapituloActual = 0;
        else if (IndiceCapituloActual < 0) IndiceCapituloActual = Capitulos.Length - 1;
        if(EventSystem.current != null) { EventSystem.current.SetSelectedGameObject(null); }
        ActualizarUI();
    }
    private void ActualizarUI() {
        if (Capitulos == null) return;
        DatosCapitulo Capitulo = Capitulos[IndiceCapituloActual];
        TXTNoCap.text = Capitulo.NoCapitulo.ToString();
        TXTNombreCap.text = Capitulo.TituloCapitulo;
        IllCap.sprite = Capitulo.IlustracionCapitulo;
    }
    public void ConfirmarSeleccion() {
        if (MostrarAlertaNoCap || Capitulos == null || Capitulos.Length == 0) return;
        if (CGPantallaSeleccionCap != null && (!CGPantallaSeleccionCap.interactable || CGPantallaSeleccionCap.alpha < 0.9999f)) { return; }
        DatosCapitulo CapituloSeleccionado = Capitulos[IndiceCapituloActual];
        if (CapituloSeleccionado.ExisteCapituloEs && !string.IsNullOrEmpty(CapituloSeleccionado.NombreEscena)) { SceneManager.LoadScene(CapituloSeleccionado.NombreEscena); }
        else { StartCoroutine(SecuenciaNoCapitulo()); }
    }
    private IEnumerator SecuenciaNoCapitulo(){
        MostrarAlertaNoCap = true;
        yield return TransicionFadeo(CGNoCapituloAqui, 1f);
        yield return new WaitForSecondsRealtime(0.25f);
        while (!Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.Space) && !Input.GetMouseButtonDown(0)) { yield return null; }
        yield return TransicionFadeo(CGNoCapituloAqui, 0f);
        MostrarAlertaNoCap = false;
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
