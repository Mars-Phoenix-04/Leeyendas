using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;

public class Manejador_Cinematicas : MonoBehaviour
{
    [System.Serializable] public struct DialogoCinematica { public string TituloPersonaje; [TextArea(2, 4)] public string TXTLinea; }
    [System.Serializable] public struct CinematicaPaso { [Header("Cámara y Duración")] public CinemachineCamera CamaraPaso; public TipoFinPaso FinalPaso; public float TiempoEspera; [Header("Componentes Animation")] public bool CambiarAnimacion; public string NombreParametro; public bool VAnimacion; [Header("Mover Personaje")] public bool MoverPersonaje; public Transform PuntoDestino; public float VelocidadMovimientoPersonaje; [Header("Dialogos")] public bool MostrarDialogo; public DialogoCinematica[] LineasDialogoPaso; }
    public enum TipoFinPaso { PorTiempo, EsperarDialogo, EsperarMovimiento}
    public static bool EnCinematica { get; private set; } = false;
    [Header("Cámara del Gameplay")][SerializeField] private CinemachineCamera CAMGameplay;
    [Header("Secuencia de Pasos")][SerializeField] private List<CinematicaPaso> PasosCinematicas;
    [Header("Componentes de la UI")]
    [SerializeField] private CanvasGroup PNegro;
    [SerializeField] private CanvasGroup PHUD;
    [SerializeField] private float DuracionFadeo = 1f;
    [SerializeField] private Transform TransformPersonaje;
    [SerializeField] private Animator AnimatorPersonaje;
    private Coroutine CRMovimientoActual;
    private void Awake(){
        if (PNegro == null) { GameObject PantallaNegra = GameObject.Find("PantallaNegro"); if (PantallaNegra != null) { PNegro = PantallaNegra.GetComponent<CanvasGroup>(); } }
        if (PHUD == null) { GameObject PantallaHUD = GameObject.Find("HUD"); if (PantallaHUD != null) { PHUD = PantallaHUD.GetComponent<CanvasGroup>(); } }
    }
    private void Start(){ StartCoroutine(SecuenciaCinematicaWAOS()); }
    private IEnumerator SecuenciaCinematicaWAOS(){
        EnCinematica = true;
        if(PNegro != null) { PNegro.alpha = 1f; PNegro.blocksRaycasts = true; }
        if(PHUD != null) { PHUD.alpha = 0f; PHUD.blocksRaycasts = false; PHUD.interactable = false; }
        if(CAMGameplay != null) { CAMGameplay.Priority = 0; }
        yield return TransicionFadeo(1f, 0f);

        for (int i = 0; i < PasosCinematicas.Count; i++){
            CinematicaPaso Paso = PasosCinematicas[i];
            CambiarPrioridadCam(Paso.CamaraPaso);
            //Animacion
            if(Paso.CambiarAnimacion && AnimatorPersonaje != null) { AnimatorPersonaje.SetBool(Paso.NombreParametro, Paso.VAnimacion); }
            if(Paso.MoverPersonaje && TransformPersonaje != null && Paso.PuntoDestino != null) { if (CRMovimientoActual != null) { StopCoroutine(CRMovimientoActual); } CRMovimientoActual = StartCoroutine(MoverPersonajeA(Paso.PuntoDestino, Paso.VelocidadMovimientoPersonaje)); }
            //Dialogos
            bool PasoCompletado = false;
            if(Paso.MostrarDialogo && ManejadorDialogos.Instancia != null && Paso.LineasDialogoPaso.Length > 0){
                string[] ARRTitulos = new string[Paso.LineasDialogoPaso.Length];
                string[] ARRTextoDialogos = new string[Paso.LineasDialogoPaso.Length];
                for (int m = 0; m < Paso.LineasDialogoPaso.Length; m++) { ARRTitulos[m] = Paso.LineasDialogoPaso[m].TituloPersonaje; ARRTextoDialogos[m] = Paso.LineasDialogoPaso[m].TXTLinea;  }
                ManejadorDialogos.Instancia.IniciarDescDialg(ARRTitulos, ARRTextoDialogos, () => { PasoCompletado = true; });
            }
            else{
                PasoCompletado = true;
            }
            //Terminar Paso
            if(Paso.FinalPaso == TipoFinPaso.PorTiempo) { yield return new WaitForSeconds(Paso.TiempoEspera); } else if(Paso.FinalPaso == TipoFinPaso.EsperarDialogo) { while (!PasoCompletado) { if (PHUD != null) { PHUD.alpha = 0f; PHUD.blocksRaycasts = false; PHUD.interactable = false; } yield return null; } PHUD.alpha = 0f; PHUD.blocksRaycasts = false; PHUD.interactable = false; } else if(Paso.FinalPaso == TipoFinPaso.EsperarMovimiento) { if(CRMovimientoActual != null) { yield return CRMovimientoActual; } }
        }
        if (PHUD != null) { PHUD.alpha = 0f; PHUD.blocksRaycasts = false; PHUD.interactable = false; }
        yield return TransicionFadeo(0f, 1f);
        if (CAMGameplay != null) { CAMGameplay.Priority = 2099; }
        yield return FadeoSincronizado(1f, 0f, 0f, 1f);
        EnCinematica = false;
        gameObject.SetActive(false);
        if(UI_Tutorial.TutorialInstancia != null) { UI_Tutorial.TutorialInstancia.MostrarTutorial(); }
    }
    private void CambiarPrioridadCam(CinemachineCamera CamaraActiva){
        if (CAMGameplay != null) CAMGameplay.Priority = 0;
        foreach (var paso in PasosCinematicas){
            if(paso.CamaraPaso != null) { paso.CamaraPaso.Priority = (paso.CamaraPaso == CamaraActiva) ? 10 : 4; }
        }
    }
    private IEnumerator TransicionFadeo(float AlfInicial, float AlfFinal){
        if (PNegro == null) yield break;
        float Tiempo = 0f;
        while (Tiempo < DuracionFadeo){
            Tiempo += Time.deltaTime;
            PNegro.alpha = Mathf.Lerp(AlfInicial, AlfFinal, Tiempo / DuracionFadeo);
            if (PHUD != null) { PHUD.alpha = 0f; PHUD.blocksRaycasts = false; PHUD.interactable = false; }
            yield return null;
        }
        PNegro.alpha = AlfFinal;
        PNegro.blocksRaycasts = AlfFinal > 0f;
    }
    private IEnumerator FadeoSincronizado(float PantallaInicio, float PantallaFinal, float HUDInicio, float HUDFinal){
        float tiempo = 0f;
        if(PHUD != null) { PHUD.blocksRaycasts = HUDFinal > 0f; PHUD.interactable = HUDFinal > 0f ; }
        while(tiempo < DuracionFadeo){
            tiempo += Time.deltaTime;
            float t = tiempo / DuracionFadeo;
            if(PNegro != null) { PNegro.alpha = Mathf.Lerp(PantallaInicio, PantallaFinal, t); }
            if(PHUD != null) { PHUD.alpha = Mathf.Lerp(HUDInicio, HUDFinal, t); }
            yield return null;
        }
        if(PNegro != null) { PNegro.alpha = PantallaFinal; PNegro.blocksRaycasts = PantallaFinal > 0f; }
        if(PHUD != null) { PHUD.alpha = HUDFinal; PHUD.blocksRaycasts = HUDFinal > 0f; PHUD.interactable = HUDFinal > 0f; }
    }
    private IEnumerator MoverPersonajeA(Transform Destino, float velocidad) {
        if (Destino == null || TransformPersonaje == null) yield break;
        if(AnimatorPersonaje != null) { AnimatorPersonaje.SetBool("Caminando", true); }
        CharacterController PersonajeCC = TransformPersonaje.GetComponent<CharacterController>();
        float VelocidadActual = 0f;
        //MOVER AL PUNTO B HASTA QUE YA ESTE CERCA
        while (Vector3.Distance(new Vector3(TransformPersonaje.position.x, 0, TransformPersonaje.position.z), new Vector3(Destino.position.x,0, Destino.position.z)) > 0.15f){
            VelocidadActual = Mathf.MoveTowards(VelocidadActual, velocidad, Time.deltaTime * (velocidad * 5f));
            Vector3 Direccion = (Destino.position - TransformPersonaje.position).normalized;
            Direccion.y = 0;
            if(Direccion.sqrMagnitude > 0.001f) { Quaternion RotacionOBjetivo = Quaternion.LookRotation(Direccion); TransformPersonaje.rotation = Quaternion.Slerp(TransformPersonaje.rotation, RotacionOBjetivo, Time.deltaTime * 10f); }
            if(PersonajeCC != null && PersonajeCC.enabled) { PersonajeCC.Move(Direccion * VelocidadActual * Time.deltaTime); } else { TransformPersonaje.position = Vector3.MoveTowards(TransformPersonaje.position, Destino.position, velocidad * Time.deltaTime); }
            yield return null;
        }
        if(AnimatorPersonaje != null) { AnimatorPersonaje.SetBool("Caminando", false); }
    }
}
//NOTA: En la cinematica del primer escenario, cuando se pasa al paso dos y llega a su posición, el modelo comienza a dar vueltas y se cambia la animación entre idle y caminar hasta que se detiene en una rotación aleatoria. 
//No se ha podido replicar este error. Sin embargo, se anota aquí que puede ser dentro de la corrutina de MoverPersonajeA debido a la rotación.
