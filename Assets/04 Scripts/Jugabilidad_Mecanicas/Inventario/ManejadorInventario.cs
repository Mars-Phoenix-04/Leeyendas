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
        if (Instancia == null) { Instancia = this; DontDestroyOnLoad(gameObject); }
        else if (Instancia != this) { Destroy(gameObject); }
    }

    public bool AgregarClave(Clave_DATA NuevaClave){  
        for (int i = 0; i < NoCasillas; i++){
            if (casillas[i] == null){
                casillas[i] = NuevaClave;
                CambiosEnElInventario?.Invoke();
                return true;
            }
        }
        return false;
    }
    public void EliminarClave(int i){
        if (i < 0 || i >= casillas.Length) return;
        casillas[i] = null;
        for(int j = i; j < casillas.Length - 1; j++){ //Esto es para recorrer los objetos en el inventario una vez se destruyan
            casillas[j] = casillas[j + 1];
        }
        casillas[casillas.Length - 1] = null;    
        CambiosEnElInventario?.Invoke();
    }
    public bool TieneClave(Clave_DATA ClaveEncontrada) {
        if (ClaveEncontrada == null) return false;
        foreach (var clave in casillas){
            if(clave != null){
                if (clave == ClaveEncontrada || clave.name == ClaveEncontrada.name) return true;
            }
        }
        return false;
    }
    public Clave_DATA ObtenerClave(int i) => (i >= 0 && i < NoCasillas) ? casillas[i] : null;
    public Clave_DATA[] ObtenerTODASClaves() => casillas;
}
