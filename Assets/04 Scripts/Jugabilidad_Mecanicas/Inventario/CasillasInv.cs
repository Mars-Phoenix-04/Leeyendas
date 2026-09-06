using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;

public class CasillasInv : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Referencias a la UI y elementos de las casillas")]
    [SerializeField] private Image SpriteClave;
    [SerializeField] private TextMeshProUGUI TXTNombreClave;
    [SerializeField] private TextMeshProUGUI TXTDescClave;
    [SerializeField] private CanvasGroup CasillaFrente;
    [SerializeField] private CanvasGroup CasillaDescHover;

    private float VelocidadFadeo = 10f;
    private bool HoverSINO = false;
    private bool ClaveExiste = false;

    void Update()
    {
        if (!ClaveExiste) return;
        float AlphaFrente = HoverSINO ? 0f : 1f;
        float AlphaDescripcion = HoverSINO ? 1f : 0f;
        if (CasillaFrente != null) CasillaFrente.alpha = Mathf.MoveTowards(CasillaFrente.alpha, AlphaFrente, Time.unscaledDeltaTime * VelocidadFadeo);
        if (CasillaDescHover != null) CasillaDescHover.alpha = Mathf.MoveTowards(CasillaDescHover.alpha, AlphaDescripcion, Time.unscaledDeltaTime * VelocidadFadeo);
    }

    public void DefinirClave(Clave_DATA clave) {
        if (clave != null) {
            ClaveExiste = true;
            if (SpriteClave != null) { SpriteClave.sprite = clave.IconoClave; SpriteClave.enabled = (clave.IconoClave != null); }
            if (TXTNombreClave != null) TXTNombreClave.text = clave.NombreClave;
            if (TXTDescClave != null) TXTDescClave.text = clave.descripcion;
            ResetearHover();
        }
        else {
            LimpiarCasilla();
        }
    }
    public void LimpiarCasilla() {
        HoverSINO = false;
        ClaveExiste = false;
        if (SpriteClave != null) SpriteClave.enabled = false;
        if (TXTNombreClave != null) TXTNombreClave.text = string.Empty;
        if (TXTDescClave != null) TXTDescClave.text = string.Empty;
        if (CasillaFrente != null) CasillaFrente.alpha = 0.2f;
        if (CasillaDescHover != null) CasillaDescHover.alpha = 0f;
    }
    public void ResetearHover(){
        HoverSINO = false;
        if (CasillaFrente != null) CasillaFrente.alpha = 1f;
        if (CasillaDescHover != null) CasillaDescHover.alpha = 0f;
    }

    public void OnPointerEnter(PointerEventData DataEvento){
        if(ClaveExiste) HoverSINO = true;
    }
    public void OnPointerExit(PointerEventData DataEvento){
        if (ClaveExiste) HoverSINO = false;
    }
}
