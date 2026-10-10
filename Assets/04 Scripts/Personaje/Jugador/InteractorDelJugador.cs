using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using System.Collections;
using System;

public class InteractorDelJugador : MonoBehaviour
{
    [Header("Configuracion de Interaccion y Objetos")]
    [SerializeField] private Transform PosJugador;
    [SerializeField] private float RangoInteraccion;
    [SerializeField] private LayerMask CapaInteraccion;

    [Header("Mensaje de Interaccion")]
    [SerializeField] private CanvasGroup CGMensajeInt;
    [SerializeField] private TextMeshProUGUI TXTMensajeInt;
    [SerializeField] private float DDesvanecimiento;
    private Coroutine CRDesvanecimiento;
    private float ValorAlfa = -1f;
    private InterfazInteraccion InteraccionCercana;

    void Start(){   if (CGMensajeInt != null) CGMensajeInt.alpha = 0f;  }

    void Update(){
        if (ManejadorDialogos.Instancia != null && ManejadorDialogos.Instancia.DialogoActivo) {CambiarAlfa(0f);   return; }
        DetectarInteraccion();
        if (InteraccionCercana != null && Input.GetKeyDown(KeyCode.E)){
            InteraccionCercana.Interactuar(this);
            CambiarAlfa(0f);
        }
    }

    private void DetectarInteraccion(){
        Vector3 PuntoBusqeuda = PosJugador != null ? PosJugador.position : transform.position;
        Collider[] colliders = Physics.OverlapSphere(PuntoBusqeuda, RangoInteraccion, CapaInteraccion);
        InterfazInteraccion ObjCercano = null;
        float DObjCercano = float.MaxValue;

        foreach (Collider collider in colliders){
            InterfazInteraccion ObjInteractuable = collider.GetComponent<InterfazInteraccion>() ?? collider.GetComponentInParent<InterfazInteraccion>();
            if (ObjInteractuable != null){
                float distancia = Vector3.Distance(PuntoBusqeuda, collider.transform.position);
                if(distancia < DObjCercano) { DObjCercano = distancia; ObjCercano = ObjInteractuable; }
            }
        }
        InteraccionCercana = ObjCercano;
        if(InteraccionCercana != null) {
            if (TXTMensajeInt != null) TXTMensajeInt.text = InteraccionCercana.TextoInteraccionUI(); CambiarAlfa(1f);
        }
        else { CambiarAlfa(0f); }
    }

    private void CambiarAlfa(float AlfaFinal){
        if (CGMensajeInt == null) return;
        if (Mathf.Approximately(ValorAlfa, AlfaFinal)) return;
        ValorAlfa = AlfaFinal;
        if (CRDesvanecimiento != null) { StopCoroutine(CRDesvanecimiento); }
        CRDesvanecimiento = StartCoroutine(DesvanecimientoPantalla(CGMensajeInt, ValorAlfa));
    }
    private IEnumerator DesvanecimientoPantalla(CanvasGroup CanvasG, float ValorAlfa, Action TerminoFadeo = null){
        float FadeoInicial = CanvasG.alpha;
        float Tiempo = 0f;

        while (Tiempo < DDesvanecimiento){
            Tiempo += Time.unscaledDeltaTime;
            CanvasG.alpha = Mathf.Lerp(FadeoInicial, ValorAlfa, Tiempo / DDesvanecimiento);
            yield return null;
        }
        CanvasG.alpha = ValorAlfa;
        TerminoFadeo?.Invoke();
    }


    /*
     * Mostrar Texto Prompt
    private void MostrarMensaje(string TXTPrompt){
        if (CGMensajeInt != null) CGMensajeInt.alpha = 1f;
        if (TXTMensajeInt != null) TXTMensajeInt.text = TXTPrompt;
    }
    private void OcultarMensaje(){
        if (CGMensajeInt != null) CGMensajeInt.alpha = 0f;
    }
    */

    /*
     * Funcion de Interaccion anterior que nomas se usaba para recoger las claves
    private void Interactuar()
    {
        Collider[] colliders = Physics.OverlapSphere(PosJugador.position, RangoInteraccion, CapaInteraccion);
        InterfazInteraccion InteractuableCerca = null;
        float DistanciaCercana = float.MaxValue;

        foreach (Collider collider in colliders){
            InterfazInteraccion interactuable = collider.GetComponent<InterfazInteraccion>() ?? collider.GetComponentInParent<InterfazInteraccion>();
            if (interactuable != null){
                float distancia = Vector3.Distance(PosJugador.position, collider.transform.position);
                if (distancia < DistanciaCercana)   DistanciaCercana = distancia;   InteractuableCerca = interactuable;
            }
        }
        InteractuableCerca?.InteractuarClave(this);
    }
    */
}
