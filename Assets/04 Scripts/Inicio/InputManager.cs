using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //public Animator LAnimator;
    public TransicionLibro TransLibro;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)){ TransLibro.ComienzoTransicion(); } 
        
        else if (Input.GetMouseButtonDown(0))
        {
            Ray rayo = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayo, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("LibroMenu"))
                {
                    TransLibro.ComienzoTransicion();
                }
            }
        }
        /*
         * 
        if (Input.GetMouseButtonDown(0))
        {
            Ray rayo = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayo, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("LibroMenu"))
                {
                    Debug.Log("Toco el libro");
                    LAnimator.SetTrigger("AbrirLibro");
                }
            }
        }
        */
    }
}
