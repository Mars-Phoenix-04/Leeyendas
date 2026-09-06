using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDInventario : MonoBehaviour
{
    [System.Serializable]
    public struct CasillasInv { public Image iconoImagen; public CanvasGroup CGCasilla; public TextMeshProUGUI ClaveTXT; }
    [SerializeField] private CasillasInv[] ArregloCasillas = new CasillasInv[ManejadorInventario.NoCasillas];

    private void OnEnable(){
        if(ManejadorInventario.Instancia != null)
        ManejadorInventario.Instancia.CambiosEnElInventario += ActualizarHUD;
    }
    private void OnDisable(){
        if (ManejadorInventario.Instancia != null) 
        ManejadorInventario.Instancia.CambiosEnElInventario -= ActualizarHUD;
    }

    void Start(){
        ActualizarHUD();
    }
    public void ActualizarHUD(){
        if (ManejadorInventario.Instancia == null) return;
        Clave_DATA[] claves = ManejadorInventario.Instancia.ObtenerTODASClaves();

        for (int i = 0; i < ArregloCasillas.Length; i++){
            if (i >= claves.Length) break;
            Clave_DATA clave = claves[i];

            if (clave != null){
                if (ArregloCasillas[i].iconoImagen != null){
                    ArregloCasillas[i].iconoImagen.sprite = clave.IconoClave;
                    ArregloCasillas[i].iconoImagen.enabled = true;
                }
                if (ArregloCasillas[i].ClaveTXT != null){
                    ArregloCasillas[i].ClaveTXT.text = clave.NombreClave;
                    ArregloCasillas[i].ClaveTXT.enabled = true;
                }
                if (ArregloCasillas[i].CGCasilla != null){
                    ArregloCasillas[i].CGCasilla.alpha = 1f;
                }
            }
            else {
                if (ArregloCasillas[i].iconoImagen != null){
                    ArregloCasillas[i].iconoImagen.sprite = null;
                    ArregloCasillas[i].iconoImagen.enabled = false;
                }
                if (ArregloCasillas[i].ClaveTXT != null){
                    ArregloCasillas[i].ClaveTXT.text = string.Empty;
                    ArregloCasillas[i].ClaveTXT.enabled = false;
                }
                if (ArregloCasillas[i].CGCasilla != null){
                    ArregloCasillas[i].CGCasilla.alpha = 0f;
                }
            }
        }
    }
}
