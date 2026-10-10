using UnityEngine;

public class RecogerClave : MonoBehaviour, InterfazInteraccion
{
    [Header("ObjetoScriptiable")]
    [SerializeField] private Clave_DATA DataDeClave;

    public void Interactuar(InteractorDelJugador InteractorJ){
        bool AgregadoAInv = ManejadorInventario.Instancia.AgregarClave(DataDeClave);
        if (AgregadoAInv) gameObject.SetActive(false);
        else Debug.Log("Ya no hay espacio");
    }
    public string TextoInteraccionUI() => $"Recoger {DataDeClave.NombreClave}";
}
