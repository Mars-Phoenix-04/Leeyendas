using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Claves_PuntoInflexion : MonoBehaviour
{
    [Header("Elementos de la interfaz de Punto Inflexion")]
    [SerializeField] private Image SpriteClave;
    [SerializeField] private TextMeshProUGUI TXTNombreClave;
    [SerializeField] private RectTransform CTransformClave;
    //Valores del hover de las claves
    private float ArribaClavePixeles = 25f; //Cuanto sube
    private float VelocidadArriba = 10f;
    private Vector2 POriginal;
    private Vector2 PNueva;

    private void Awake(){ if (CTransformClave == null) CTransformClave = GetComponent<RectTransform>();   POriginal = CTransformClave.anchoredPosition;   PNueva = POriginal; }
    private void Update(){ //Aqui se interpola siempre la posicion inicial de la clave y la posicion nueva
        CTransformClave.anchoredPosition = Vector2.Lerp(CTransformClave.anchoredPosition, PNueva, Time.unscaledDeltaTime * VelocidadArriba);
    }

    public void AgarrarClaves(Clave_DATA Clave){ //Asignar su sprite y texto a las Claves que tenga ahi
        if(Clave != null){
            if(SpriteClave != null){
                SpriteClave.sprite = Clave.IconoClave;
                SpriteClave.enabled = (Clave.IconoClave != null);
            }
            if(TXTNombreClave != null) {
                TXTNombreClave.text = Clave.NombreClave;
                TXTNombreClave.enabled = true;
            }
        }
        else
        {
            LimpiarCasilla();
        }
    }

    public void LimpiarCasilla(){
        if (SpriteClave != null) SpriteClave.enabled = false;
        if (TXTNombreClave != null) TXTNombreClave.text = string.Empty;
        ClaveSeleccionada(false);
    }

    public void ClaveSeleccionada(bool SeleccionadaClave){ // Si una Clave esta seleccionada y la eleva, pero sino lo esta la deja abajo
        float PosicionY = SeleccionadaClave ? POriginal.y + ArribaClavePixeles : POriginal.y;
        PNueva = new Vector2(POriginal.x, PosicionY);
    }
}
