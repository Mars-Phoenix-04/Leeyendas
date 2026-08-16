using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_Interaccion : MonoBehaviour
{
    public ManejoInventario MInventario;

    [Header("Elementos UI")]
    [SerializeField] private RawImage[] ImagenesItems;
    [SerializeField] private string[] NombresItems;

    void Update()
    {
        for (int i = 0; i < ImagenesItems.Length; i++)
        {
            ActualizarImagen(ImagenesItems[i], NombresItems[i]);
        }
    }

    private void ActualizarImagen(RawImage ImagenInv, string NombreItem)
    {
        bool tieneItem = MInventario.TieneItem(NombreItem);
        ImagenInv.enabled = tieneItem;
    }
}
