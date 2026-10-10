using UnityEngine;

public class InteractuarClaveObtencion : MonoBehaviour, InterfazInteraccion
{
    [SerializeField] private Clave_DATA ClaveDt;
    [TextArea(3, 5)][SerializeField] private string[] TXTPrimeraInteraccion;
    [TextArea(3, 5)][SerializeField] private string[] TXTClaveRecogida;
    [SerializeField] private string NombreObjeto;
    [SerializeField] private string AccionObjeto;
    private bool YaRecolectado = false;

    public void Interactuar(InteractorDelJugador InteractorJ){
        if (ManejadorDialogos.Instancia == null || ManejadorDialogos.Instancia.DialogoActivo) return;
        if (YaRecolectado){
            if(TXTClaveRecogida != null && TXTClaveRecogida.Length > 0){ManejadorDialogos.Instancia.IniciarDescDialg("Marta", TXTClaveRecogida);}
            return;
        }

        ManejadorDialogos.Instancia.IniciarDescDialg("Marta", TXTPrimeraInteraccion, () =>{
            if(UI_Claves.InstanciaUiClave != null) { UI_Claves.InstanciaUiClave.MostrarPantallaClave(ClaveDt, () => { GuardarClaveInventario(); }); } else { GuardarClaveInventario(); }
        });
    }
    private void GuardarClaveInventario(){
        if (ManejadorDialogos.Instancia == null) return;
        bool YaEstaEnInv = ManejadorInventario.Instancia.AgregarClave(ClaveDt);
        if (YaEstaEnInv) { YaRecolectado = true; } else { Debug.LogWarning("Se lleno el inventario, esto no deberia pasar, aiudaaa"); }
    }
    public string TextoInteraccionUI() => $"{AccionObjeto} {NombreObjeto}";
}
