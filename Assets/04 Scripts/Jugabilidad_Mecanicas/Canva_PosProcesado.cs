using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(Canvas))]
public class Canva_PosProcesado : MonoBehaviour
{
    private Canvas CanvaPantallasETC;
    private void Awake() {
        CanvaPantallasETC = GetComponent<Canvas>();
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(BuscarCamara());
    }
    private void OnDestroy(){   SceneManager.sceneLoaded -= OnSceneLoaded;  }
    void OnSceneLoaded(Scene Escena, LoadSceneMode Modo) { StartCoroutine(BuscarCamara()); }
    IEnumerator BuscarCamara(){
        while(CanvaPantallasETC.worldCamera == null){
            GameObject OBJCam = GameObject.FindWithTag("MainCamera");
            if(OBJCam != null){
                Camera CCamara = OBJCam.GetComponent<Camera>();
                if(CCamara != null) { CanvaPantallasETC.worldCamera = CCamara; yield break; }
            }
            yield return null;
        }
    }

}
