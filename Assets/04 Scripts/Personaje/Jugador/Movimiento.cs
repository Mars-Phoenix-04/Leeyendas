using UnityEngine;
using UnityEngine.InputSystem;

public class Movimiento : MonoBehaviour
{
    [Header("Jugador")]
    CharacterController CController;
    public Animator CAnimator;
    public Transform camara;

    [Header("Variables del movimiento del Jugador")]
    float VelocidadCaminar = 15f;
    float VelocidadAgachado = 5f;
    float VelocidadRotacion = 15f;
    float gravedad = -9.8f;

    [Header("Propiedades y Configs de Agacharse")]
    private float VelocidadV;
    private bool Agachado;

    void Start(){
        CController = GetComponent<CharacterController>();
        CAnimator = GetComponentInChildren<Animator>();
    }

    void Update(){ ControlAgacharse(); ControlMovimiento();}

    void ControlAgacharse (){
        Agachado = Input.GetKey(KeyCode.LeftControl);
        CAnimator.SetBool("Agachado", Agachado);
    }

    void ControlMovimiento(){
        float IHorizontal = Input.GetAxis("Horizontal");
        float IVertical = Input.GetAxis("Vertical");
        Vector3 Direccion = new Vector3(IHorizontal, 0f, IVertical);
        Vector3 movimiento = Vector3.zero;

        if (Direccion.sqrMagnitude > 0.004f){
            Vector3 frente = camara.forward;
            frente.y = 0;
            frente.Normalize();

            Vector3 derecha = camara.right;
            derecha.y = 0;
            derecha.Normalize();

            Vector3 DireccionDondeVaAIr = (frente * IVertical + derecha * IHorizontal).normalized;
            Quaternion RotacionDondeVaAIR = Quaternion.LookRotation(DireccionDondeVaAIr);
            transform.rotation = Quaternion.Slerp(transform.rotation, RotacionDondeVaAIR, Time.deltaTime * VelocidadRotacion);

            float VelocidadMovimiento = Agachado ? VelocidadAgachado : VelocidadCaminar;
            movimiento = DireccionDondeVaAIr * VelocidadMovimiento;

            CAnimator.SetBool("Caminando", !Agachado);
            CAnimator.SetBool("AgachadoCaminando", Agachado);
        }
        else
        {
            CAnimator.SetBool("Caminando", false);
            CAnimator.SetBool("AgachadoCaminando", false);
        }
        if (CController.isGrounded && VelocidadV < 0) { VelocidadV = -2f; }
        VelocidadV += gravedad * Time.deltaTime;
        movimiento.y = VelocidadV;
        CController.Move(movimiento * Time.deltaTime);
    }
}
