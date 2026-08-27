using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TXT_Hover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TMP_Text textoBoton;
    void Awake(){ textoBoton = GetComponentInChildren<TMP_Text>(); }

    public void OnPointerEnter (PointerEventData eventData){ if (textoBoton != null) { textoBoton.fontStyle |= FontStyles.Underline;} }

    public void OnPointerExit (PointerEventData eventData) { if(textoBoton !=null) { textoBoton.fontStyle &= ~FontStyles.Underline; } }
}
