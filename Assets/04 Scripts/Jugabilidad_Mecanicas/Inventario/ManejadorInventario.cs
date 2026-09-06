using System;
using System.Collections.Generic;
using UnityEngine;

public class ManejadorInventario : MonoBehaviour
{
    public static ManejadorInventario Instancia { get; private set; }
    public const int NoCasillas = 4;
    [SerializeField] private Clave_DATA[] casillas = new Clave_DATA[NoCasillas];

    public event Action CambiosEnElInventario;

    private void Awake(){
        if (Instancia == null) { Instancia = this; }
        else { Debug.Log("Existe instancia duplicada, no bueno"); Destroy(gameObject); }
    }

    public bool AgregarClave(Clave_DATA NuevaClave){
        
        for (int i = 0; i < NoCasillas; i++)
        {
            if (casillas[i] == null)
            {
                casillas[i] = NuevaClave;
                CambiosEnElInventario?.Invoke();
                return true;
            }
        }
        return false;
    }

    public void EliminarClave(int i){
        if (i >= 0 && i < NoCasillas && casillas[i] != null)    casillas[i] = null;    CambiosEnElInventario?.Invoke();
    }

    public Clave_DATA ObtenerClave(int i) => (i >= 0 && i < NoCasillas) ? casillas[i] : null;
    public Clave_DATA[] ObtenerTODASClaves() => casillas;
}
