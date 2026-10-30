using System.Collections;
using UnityEngine;

public class UI_Tutorial : MonoBehaviour
{
    public static UI_Tutorial TutorialInstancia { get; private set; }
    public static bool TutorialActivo { get; private set; } = false;
    [SerializeField] private CanvasGroup CGTutorial;
    [SerializeField] private float DuracionFadeo = 0.5f;
    private void Awake(){ if (TutorialInstancia == null) { TutorialInstancia = this; } else { Destroy(gameObject); } NoMoreTutorial(); }
    private void Update(){ if (TutorialActivo && Input.GetKeyDown(KeyCode.Escape)) { StartCoroutine(CerrarTutorial()); } }
    public void MostrarTutorial() { if (TutorialActivo) return; StartCoroutine(MuestraTutorial()); }
    private IEnumerator MuestraTutorial() {
        TutorialActivo = true;
        Time.timeScale = 0f;
        yield return TransicionFadeo(CGTutorial, 1f);
    }
    private IEnumerator CerrarTutorial(){
        yield return TransicionFadeo(CGTutorial, 0f);
        Time.timeScale = 1f;
        TutorialActivo = false;
    }
    private void NoMoreTutorial(){
        if (CGTutorial == null) return;
        CGTutorial.alpha = 0f;
        CGTutorial.blocksRaycasts = false;
        CGTutorial.interactable = false;
        TutorialActivo = false;
    }
    private IEnumerator TransicionFadeo(CanvasGroup CanvasG, float ValorAlfa)
    {
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
