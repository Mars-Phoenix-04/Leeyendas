using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ManejadorDialogos : MonoBehaviour
{
    public static ManejadorDialogos Instancia { get; private set; }
    [Header("Pantallas y Componentes de la UI")]
    [SerializeField] private CanvasGroup CGDialogos;
    [SerializeField] private CanvasGroup CGHUD;
    [SerializeField] private TextMeshProUGUI TXTituloTexto;
    [SerializeField] private TextMeshProUGUI TXTDD;
    [SerializeField] private float DDesvanecimiento;
    //Eventos
    public event Action InicioDialogo;
    public event Action FinDialogo;
    //Propiedades de los dialogos
    private string[] DialogoDescripcion;
    private int IndiceTXTActual;
    private bool EstaDialogando = false;
    private Action SeCompletaDialogoDescripcion;
    public bool DialogoActivo => EstaDialogando;

    void Update(){
        if (!EstaDialogando) return;
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))   EscribirTXT();
    }
    private void Awake(){
        if (Instancia == null) { Instancia = this; DontDestroyOnLoad(gameObject); } else if (Instancia != this) { Destroy(gameObject); }
        if (CGDialogos != null) CGDialogos.alpha = 0f;  CGDialogos.blocksRaycasts = false;  CGDialogos.interactable = false;
    }

    public void IniciarDescDialg (string TituloDD, string[] Texto, Action TerminaTXT = null){
        if (Texto == null || Texto.Length == 0) {TerminaTXT?.Invoke(); return; }
        DialogoDescripcion = Texto;
        IndiceTXTActual = 0;
        SeCompletaDialogoDescripcion = TerminaTXT;
        EstaDialogando = true;

        if (TXTituloTexto != null) TXTituloTexto.text = TituloDD;
        TXTDD.text = DialogoDescripcion[IndiceTXTActual];
        InicioDialogo?.Invoke();
        StopAllCoroutines();

        StartCoroutine(TransicionFadeo(CGDialogos, 1f));
        if (CGHUD != null) StartCoroutine(TransicionFadeo(CGHUD, 0f));
    }
    private void EscribirTXT(){
        IndiceTXTActual++;
        if (IndiceTXTActual < DialogoDescripcion.Length) TXTDD.text = DialogoDescripcion[IndiceTXTActual]; else FinDescDialg();
    }
    private void FinDescDialg(){
        EstaDialogando = false;
        Action AccionPendiente = SeCompletaDialogoDescripcion;
        SeCompletaDialogoDescripcion = null;

        if (SeCompletaDialogoDescripcion == null) { StartCoroutine(TransicionFadeo(CGHUD, 1f)); }
        StartCoroutine(TransicionFadeo(CGDialogos, 0f, () =>{ FinDialogo?.Invoke();}));
        if (AccionPendiente != null){
            AccionPendiente?.Invoke();
        }
    }
    private IEnumerator TransicionFadeo(CanvasGroup CanvasG, float ValorAlfa, Action TerminoTXT = null){
        float FadeoInicial = CanvasG.alpha;
        float Tiempo = 0f;
        CanvasG.blocksRaycasts = ValorAlfa > 0f;
        CanvasG.interactable = ValorAlfa > 0f;
        while (Tiempo < DDesvanecimiento){
            Tiempo += Time.unscaledDeltaTime;
            CanvasG.alpha = Mathf.Lerp(FadeoInicial, ValorAlfa, Tiempo / DDesvanecimiento);
            yield return null;
        }
        CanvasG.alpha = ValorAlfa;
        TerminoTXT?.Invoke();
    }
}
