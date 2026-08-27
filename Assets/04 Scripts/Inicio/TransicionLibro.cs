using System.Collections;
using UnityEngine;

public class TransicionLibro : MonoBehaviour
{
    [Header("Componentes")]
    public Animator LAnimator;
    private string TriggerAnim = "AbrirLibro";
    private bool Abriendo = false;
    private Quaternion rotacionInicial;

    [Header("UI Controller")]
    public UI_Navegacion UI_Nav;

    [Header("Transform del Libro")]
    public Transform PosicionSalida;
    public Transform PosicionMenu;
    public Transform PosicionUI;
    public float VelocidadMov;
    public float VelocidadRot;

    [Header("Efectos Libro")]
    public float MAXrotacionLibro;
    public float VelocidadRotLibro;

    //Movimiento del libro poseido (INICIO)
    void Start(){
        MusicManager.Instancia.ReproducirMusica("Menu Principal");
        rotacionInicial = transform.rotation;
    }
    void Update(){
        if (!Abriendo){
            float anguloY = Mathf.Sin(Time.time * VelocidadRotLibro) * MAXrotacionLibro;
            transform.rotation = rotacionInicial * Quaternion.Euler(anguloY, 0, 0);
        }
    }

    public void ComienzoTransicion(){
        if (Abriendo) return;
        Abriendo = true;
        StartCoroutine(MovimientoLibro());
    }
    
    private IEnumerator MovimientoLibro(){
        //Movimiento position
        while (Vector3.Distance(transform.position, PosicionSalida.position) > 0.004f){
            transform.position = Vector3.MoveTowards(transform.position, PosicionSalida.position, VelocidadMov * Time.deltaTime);
            yield return null;
        }
        transform.position = PosicionSalida.position;
        SFXManager.Instancia.ReproducirSonido2D("AbriendoLibro");

        //Movimiento rotation
        while (Quaternion.Angle(transform.rotation, PosicionSalida.rotation) > 0.004f){
            transform.rotation = Quaternion.RotateTowards(transform.rotation, PosicionSalida.rotation, VelocidadRot * 100f * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, PosicionMenu.position, VelocidadMov * Time.deltaTime);
            //Animación
            if (LAnimator != null)
            {
                LAnimator.SetTrigger(TriggerAnim);
            }
            yield return null;
        }
        transform.position = PosicionMenu.position;
        transform.rotation = PosicionSalida.rotation;

        while (Vector3.Distance(transform.position, PosicionUI.position) > 0.004f) {
            transform.position = Vector3.MoveTowards(transform.position, PosicionUI.position, 0.15f * Time.deltaTime);
            yield return null;
        }

        UI_Nav.ApareceMenu();
    }
}
