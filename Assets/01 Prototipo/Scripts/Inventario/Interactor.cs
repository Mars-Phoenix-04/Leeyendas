using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public interface IInteractuable { void Interactuar(); }

public class Interactor : MonoBehaviour
{
    [Header("Interaccion")]
    public Transform ELInteractor;
    public float RangoInteraccion;
    public LayerMask CapaInteractuable;

    private List<IInteractuable> inventario = new List<IInteractuable>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)){   
            RevisarInteraccion();   
        }
    }

    private void RevisarInteraccion()
    {
        Collider[] colliders = Physics.OverlapSphere(ELInteractor.position, RangoInteraccion, CapaInteractuable);
        
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<IInteractuable>(out var ObjInteractuable)){
                float distancia = Vector3.Distance(ELInteractor.position, collider.transform.position);
                
                if (distancia <= RangoInteraccion && !inventario.Contains(ObjInteractuable)){
                    
                    ObjInteractuable.Interactuar();

                    inventario.Add(ObjInteractuable);
                }
            }
        }

    }
}
