using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Slider : MonoBehaviour, IDragHandler
{
    [Header("Textos")]
    public TMP_Text TextoDisplay;
    public string Etiqueta; //Cambiar por Musica, SFX y Sensibilidad
    public string Porcentaje = "%";

    [Header("Rango de Valores de Opciones")]
    public float ValorMinimo = 0f;
    public float ValorMaximo = 100f;
    public float ValorInicial = 100f;
    public float Sensibilidad = 0.5f;

    [Header("Evento Unity")]
    public UnityEvent<float> CambioDeValor;

    void Start(){ CambiarTexto(); }
    public void CambiarTexto(){
        if (TextoDisplay != null){
            TextoDisplay.text = $"{Etiqueta}{Mathf.RoundToInt(ValorInicial)}{Porcentaje}";
        }
    }

    public void OnDrag(PointerEventData eventData){
        ValorInicial += eventData.delta.x * Sensibilidad;
        ValorInicial = Mathf.Clamp(ValorInicial, ValorMinimo, ValorMaximo);
        CambiarTexto();
        CambioDeValor?.Invoke(ValorInicial);
    }
}
