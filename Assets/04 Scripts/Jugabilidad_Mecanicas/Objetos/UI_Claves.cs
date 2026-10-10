using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Claves : MonoBehaviour
{
    public static UI_Claves InstanciaUiClave { get; private set; }
    [Header("Pantalla de Clave Obtenida")]
    [SerializeField] private CanvasGroup CGClaveObtenida;
    [SerializeField] private Image SpriteClave;
    [SerializeField] private float DuracionFadeo = 0.4f;

    private void Awake(){
        if (InstanciaUiClave == null) { InstanciaUiClave = this; DontDestroyOnLoad(gameObject); }
        else if (InstanciaUiClave != this) { Destroy(gameObject); }
        OcultarPantalla();
    }
    public void MostrarPantallaClave(Clave_DATA Clave, Action CerrarPantalla = null){
        if (Clave == null) {CerrarPantalla?.Invoke(); return; }
        if (SpriteClave != null) { SpriteClave.sprite = Clave.IconoClave; SpriteClave.enabled = true; }
        StartCoroutine(SecuenciaMostrarClave(CerrarPantalla));
    }
    private IEnumerator SecuenciaMostrarClave(Action AlCerrar){
        yield return TransicionFadeo(CGClaveObtenida, 1f);
        yield return new WaitForSecondsRealtime(0.25f);
        while(!Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.Space) && !Input.GetMouseButtonDown(0)) { yield return null; }
        yield return TransicionFadeo(CGClaveObtenida, 0f);
        AlCerrar?.Invoke();
    }
    private void OcultarPantalla() { if (CGClaveObtenida == null) return; CGClaveObtenida.alpha = 0f; CGClaveObtenida.blocksRaycasts = false; CGClaveObtenida.interactable = false; }

    private IEnumerator TransicionFadeo(CanvasGroup CanvasG, float ValorAlfa){
        float FadeoInicial = CanvasG.alpha;
        float Tiempo = 0f;
        CanvasG.blocksRaycasts = ValorAlfa > 0f;
        CanvasG.interactable = ValorAlfa > 0f;
        while (Tiempo < DuracionFadeo)
        {
            Tiempo += Time.unscaledDeltaTime;
            CanvasG.alpha = Mathf.Lerp(FadeoInicial, ValorAlfa, Tiempo / DuracionFadeo);
            yield return null;
        }
        CanvasG.alpha = ValorAlfa;
    }
}
