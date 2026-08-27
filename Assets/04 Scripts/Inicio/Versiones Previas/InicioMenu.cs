/*
    ESTE SCRIPT YA NO ESTA EN USO
    Descripcion: Previamente se usaba para hacer
    la animación fade in del menu dentro del libro.
    Sin embargo, ahora esto esta integrado dentro del
    script de UI_Navegacion en la carpeta de menu.
*/

using System.Collections;
using UnityEngine;

public class InicioMenu : MonoBehaviour
{
    public CanvasGroup Menu;
    public float VelocidadFade;

    void Awake(){
        if (Menu != null){
            Menu.alpha = 0f;
            Menu.interactable = false;
            Menu.blocksRaycasts = false;
        }
    }

    public void ActivarMenu(){
        StartCoroutine(DesvanecimientoMenu());
    }

    private IEnumerator DesvanecimientoMenu(){
        float alpha = 0f;
        while (alpha < 1f){
            alpha += Time.deltaTime + VelocidadFade;
            Menu.alpha = alpha;
            yield return null;
        }

        Menu.alpha = 1f;
        Menu.interactable = true;
        Menu.blocksRaycasts = true;
    }
}
