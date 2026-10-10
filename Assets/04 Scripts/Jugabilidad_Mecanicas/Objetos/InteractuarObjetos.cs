using UnityEngine;

public class InteractuarObjetos : MonoBehaviour, InterfazInteraccion
{
    [Header("ObjetoInteraccion")]
    [SerializeField] private string NombreObjeto;
    [SerializeField] private string AccionObjeto;
    [TextArea(3, 5)][SerializeField] private string[] TXTSinClave;

    [Header("INTERACCION CON CLAVE")]
    [SerializeField] private Clave_DATA ClaveEvento;
    [SerializeField] private OBJ_Script_PuntoInflexion DatosPuntoInflexion;
    [TextArea(3, 5)][SerializeField] private string[] TXTMensajeObj;
    public void Interactuar(InteractorDelJugador InteractorJ){
        if (ManejadorDialogos.Instancia == null || ManejadorDialogos.Instancia.DialogoActivo) return;
        bool CuentaConUnaClave = false;
        SFXManager.Instancia.ReproducirSonido2D("ClicBTN");
        if (ClaveEvento != null && ManejadorInventario.Instancia != null) { CuentaConUnaClave = ManejadorInventario.Instancia.TieneClave(ClaveEvento); }
       
        //SI TIENE LA CLAVE
        if (CuentaConUnaClave){
            ManejadorDialogos.Instancia.IniciarDescDialg("Marta", TXTMensajeObj, () =>{
                if (ClaveEvento != null && CuentaConUnaClave){
                    if (Evento_PuntoInflexion.InstanciaPuntoInflexion != null) Evento_PuntoInflexion.InstanciaPuntoInflexion.IniciarPuntoInflexion(DatosPuntoInflexion);
                }
            });
        }
        //SI NO TIENE LA CLAVE
        else { ManejadorDialogos.Instancia.IniciarDescDialg("Marta", TXTSinClave);}
    }

    public string TextoInteraccionUI() => $"{AccionObjeto} {NombreObjeto}";
}
