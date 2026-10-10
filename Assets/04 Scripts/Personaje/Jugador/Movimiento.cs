using UnityEngine;
using UnityEngine.InputSystem;

public class Movimiento : MonoBehaviour
{
    [Header("Jugador")]
    private CharacterController CController;
    [SerializeField] private Animator CAnimator;
    [SerializeField] private Transform camara;

    [Header("Variables del movimiento del Jugador")]
    [SerializeField] private float VelocidadCaminar;
    [SerializeField] private float VelocidadAgachado;
    [SerializeField] private float VelocidadRotacion;
    [SerializeField] private float gravedad;

    [Header("Propiedades y Configs de Agacharse")]
    [SerializeField] private float VelocidadV;
    [SerializeField] private bool Agachado;

    private void Awake(){
        camara = Camera.main.transform;
        CController = GetComponent<CharacterController>();
        if(CAnimator == null) CAnimator = GetComponentInChildren<Animator>();
    }

    void Update(){
        if ((ManejadorDialogos.Instancia != null && ManejadorDialogos.Instancia.DialogoActivo) || (Evento_PuntoInflexion.InstanciaPuntoInflexion != null && Evento_PuntoInflexion.InstanciaPuntoInflexion.EstaActivo)) {
            AnimacionIdle();
            return; 
        }
        ControlAgacharse(); ControlMovimiento();
    }
    private void AnimacionIdle(){
        if (CAnimator == null) return;
        CAnimator.SetBool("Caminando", false);
        CAnimator.SetBool("AgachadoCaminando", false);
    }
    private void ControlAgacharse (){
        Agachado = Input.GetKey(KeyCode.LeftControl);
        CAnimator.SetBool("Agachado", Agachado);
    }
    private void ControlMovimiento(){
        float IHorizontal = Input.GetAxis("Horizontal");
        float IVertical = Input.GetAxis("Vertical");

        //Hacer que solo camine hacia enfrente
        float IVerticalAtras = Mathf.Max(0f, IVertical);

        Vector3 frente = camara.forward;
        frente.y = 0;
        frente.Normalize();

        Vector3 derecha = camara.right;
        derecha.y = 0;
        derecha.Normalize();

        Vector3 DireccionDondeVaAIr = (frente * IVerticalAtras + derecha * IHorizontal).normalized;
        Vector3 movimiento = Vector3.zero;

        if (DireccionDondeVaAIr.sqrMagnitude > 0.004f){
            DireccionDondeVaAIr.Normalize();

            Quaternion RotacionDondeVaAIR = Quaternion.LookRotation(DireccionDondeVaAIr);
            transform.rotation = Quaternion.Slerp(transform.rotation, RotacionDondeVaAIR, Time.deltaTime * VelocidadRotacion);

            float VelocidadMovimiento = Agachado ? VelocidadAgachado : VelocidadCaminar;
            movimiento = DireccionDondeVaAIr * VelocidadMovimiento;

            CAnimator.SetBool("Caminando", !Agachado);
            CAnimator.SetBool("AgachadoCaminando", Agachado);
        }
        else {
            CAnimator.SetBool("Caminando", false);
            CAnimator.SetBool("AgachadoCaminando", false);
        }
        if (CController.isGrounded && VelocidadV < 0) { VelocidadV = -2f; }
        VelocidadV += gravedad * Time.deltaTime;
        movimiento.y = VelocidadV;
        CController.Move(movimiento * Time.deltaTime);
    }
}
