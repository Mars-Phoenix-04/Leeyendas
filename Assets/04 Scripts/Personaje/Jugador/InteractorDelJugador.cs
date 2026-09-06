using Unity.VisualScripting;
using UnityEngine;

public class InteractorDelJugador : MonoBehaviour
{
    [Header("Configuracion de Interaccion y Objetos")]
    public Transform PosJugador;
    public float RangoInteraccion;
    public LayerMask CapaInteraccion;

    void Update(){ if (Input.GetKeyDown(KeyCode.E)) Interactuar();  }

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
}
