using UnityEngine;

public class Objeto_Interaccion : MonoBehaviour, IInteractuable
{
    public ManejoInventario MInventario;

    public void Interactuar()
    {
        Debug.Log("Interaccion hecha");
        MInventario.AgregarItem(gameObject.name);
        Destroy(gameObject, 0.1f);
    }
}
