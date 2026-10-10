using System.Collections;
using UnityEngine;

public class TransicionLibro : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private Animator LAnimator;
    [SerializeField] private CanvasGroup CGAlertaPresionEnter;
    [SerializeField] private float DuracionDesvanecimiento = 0.8f;
    private string TriggerAnim = "AbrirLibro";
    private bool Abriendo = false;
    private Quaternion rotacionInicial;

    [Header("UI Controller")]
    [SerializeField] private UI_Navegacion UI_Nav;

    [Header("Transform del Libro")]
    [SerializeField] private Transform PosicionSalida;
    [SerializeField] private Transform PosicionMenu;
    [SerializeField] private Transform PosicionUI;
    [SerializeField] private float VelocidadMov;
    [SerializeField] private float VelocidadRot;

    [Header("Efectos Libro")]
    [SerializeField] private float MAXrotacionLibro;
    [SerializeField] private float VelocidadRotLibro;

    //Movimiento del libro poseido (INICIO)
    void Start(){
        MusicManager.Instancia.ReproducirMusica("Menu Principal");
        rotacionInicial = transform.rotation;
        StartCoroutine(TransicionFadeo(CGAlertaPresionEnter, 1f));
    }
    void Update(){
        if (!Abriendo){
            float anguloY = Mathf.Sin(Time.time * VelocidadRotLibro) * MAXrotacionLibro;
            transform.rotation = rotacionInicial * Quaternion.Euler(anguloY, 0, 0);
        }
    }

    public void ComienzoTransicion(){
        if (Abriendo) return;
        Abriendo = true;
        StartCoroutine(TransicionFadeo(CGAlertaPresionEnter, 0f));
        StartCoroutine(MovimientoLibro());
    }
    
    private IEnumerator MovimientoLibro(){
        //Movimiento position
        while (Vector3.Distance(transform.position, PosicionSalida.position) > 0.004f){
            transform.position = Vector3.MoveTowards(transform.position, PosicionSalida.position, VelocidadMov * Time.deltaTime);
            yield return null;
        }
        transform.position = PosicionSalida.position;
        SFXManager.Instancia.ReproducirSonido2D("AbriendoLibro");

        //Movimiento rotation
        while (Quaternion.Angle(transform.rotation, PosicionSalida.rotation) > 0.004f){
            transform.rotation = Quaternion.RotateTowards(transform.rotation, PosicionSalida.rotation, VelocidadRot * 100f * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, PosicionMenu.position, VelocidadMov * Time.deltaTime);
            //Animación
            if (LAnimator != null){ LAnimator.SetTrigger(TriggerAnim); }
            yield return null;
        }
        transform.position = PosicionMenu.position;
        transform.rotation = PosicionSalida.rotation;

        while (Vector3.Distance(transform.position, PosicionUI.position) > 0.004f) {
            transform.position = Vector3.MoveTowards(transform.position, PosicionUI.position, 0.15f * Time.deltaTime);
            yield return null;
        }
        UI_Nav.ApareceMenu();
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
}
