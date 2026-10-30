using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class InteractuarObjetos : MonoBehaviour, InterfazInteraccion
{
    [System.Serializable] public struct DialogoNPC { public string TituloPersonaje; [TextArea(2, 4)] public string TXTLinea; }

    [Header("ObjetoInteraccion")]
    [SerializeField] private string NombreObjeto;
    [SerializeField] private string AccionObjeto;
    [SerializeField] private DialogoNPC[] TXTDialogoNormal;
    [SerializeField] private bool EsObjetoTutorial = false;

    [Header("INTERACCION CON CLAVE")]
    [SerializeField] private Clave_DATA ClaveEvento;
    [SerializeField] private OBJ_Script_PuntoInflexion DatosPuntoInflexion;
    [SerializeField] private DialogoNPC[] TXTDialogoConClave;

    [Header("SOLO APLICA PARA NPCS")]
    [SerializeField] private Animator NPCAnimator;
    [SerializeField] private string ParametroHablar;
    [SerializeField] private bool GiraHaciaPlayer;

    [Header("CAMARA INTERACCION")]
    [SerializeField] private CinemachineCamera CamInteracion;
    public void Interactuar(InteractorDelJugador InteractorJ){
        if (ManejadorDialogos.Instancia == null || ManejadorDialogos.Instancia.DialogoActivo) return;

        //OBJETO PARA MOSTRAR TUTORIAL
        if (EsObjetoTutorial) { if(UI_Tutorial.TutorialInstancia != null) { UI_Tutorial.TutorialInstancia.MostrarTutorial(); } return; }

        bool CuentaConUnaClave = false;
        SFXManager.Instancia.ReproducirSonido2D("ClicBTN");
        if (ClaveEvento != null && ManejadorInventario.Instancia != null) { CuentaConUnaClave = ManejadorInventario.Instancia.TieneClave(ClaveEvento); }
        // ANIMACION Y MOVIMIENTO DE NPC (SOLO SI APLICA)
        if(GiraHaciaPlayer && InteractorJ != null){
            Vector3 DireccionNPC = (InteractorJ.transform.position - transform.position);
            DireccionNPC.y = 0;
            if(DireccionNPC != Vector3.zero) { StopAllCoroutines(); StartCoroutine(RotacionPersonaje(DireccionNPC)); }
        }
        if(NPCAnimator != null) { NPCAnimator.SetBool(ParametroHablar, true); }
        // CAMBIO PRIORIDADES
        if(CamInteracion != null) { CamInteracion.Priority = 3000; }

        //SI TIENE LA CLAVE
        if (CuentaConUnaClave){
            IniciarDialogo(TXTDialogoConClave, () =>{
                VolverEstadoNormal();
                if (ClaveEvento != null){
                    if (Evento_PuntoInflexion.InstanciaPuntoInflexion != null) Evento_PuntoInflexion.InstanciaPuntoInflexion.IniciarPuntoInflexion(DatosPuntoInflexion);
                }
            });
        }
        //SI NO TIENE LA CLAVE
        else { IniciarDialogo(TXTDialogoNormal, () => { VolverEstadoNormal(); }); }
    }
    private void IniciarDialogo(DialogoNPC[] lineas, Action CBFin = null){
        if(lineas == null || lineas.Length == 0) { CBFin?.Invoke(); return; }
        string[] ArregloTitulos = new string[lineas.Length];
        string[] ArregloDialogos = new string[lineas.Length];
        for(int i = 0; i < lineas.Length; i++){
            ArregloTitulos[i] = lineas[i].TituloPersonaje;
            ArregloDialogos[i] = lineas[i].TXTLinea;
        }
        if(CBFin != null) { ManejadorDialogos.Instancia.IniciarDescDialg(ArregloTitulos, ArregloDialogos, CBFin); } else { ManejadorDialogos.Instancia.IniciarDescDialg(ArregloTitulos, ArregloDialogos); }
    }
    private IEnumerator RotacionPersonaje(Vector3 DireccionObj){
        Quaternion RotacionInicial = transform.rotation;
        Quaternion RotacionFinal = Quaternion.LookRotation(DireccionObj);
        float tiempo = 0f;
        float tiempogrio = 1f;
        while (tiempo < tiempogrio) { tiempo += Time.deltaTime; float t = tiempo / tiempogrio; transform.rotation = Quaternion.Slerp(RotacionInicial, RotacionFinal, t); yield return null; }
        transform.rotation = RotacionFinal;
    }
    private void VolverEstadoNormal(){if(NPCAnimator != null) { NPCAnimator.SetBool(ParametroHablar, false); }  if (CamInteracion != null) { CamInteracion.Priority = 0; } }
    public string TextoInteraccionUI() => $"{AccionObjeto} {NombreObjeto}";
}
