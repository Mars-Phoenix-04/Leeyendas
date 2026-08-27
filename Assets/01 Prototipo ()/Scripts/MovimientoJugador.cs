using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Jugador")]
    private CharacterController CController;
    private Animator CAnimator;
    public float velocidad = 4;
    public float gravedad = -9.8f;

    [Header("Camara")]
    public Transform camara;

    void Start()
    {
        CController = GetComponent<CharacterController>();
        CAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        float IHorizontal = Input.GetAxis("Horizontal");
        float IVertical = Input.GetAxis("Vertical");
        Vector3 movimiento = Vector3.zero;

        if (IHorizontal != 0 || IVertical != 0)
        {
            Vector3 frente = camara.forward;
            frente.y = 0;
            frente.Normalize();

            Vector3 derecha = camara.right;
            derecha.y = 0;
            derecha.Normalize();

            Vector3 direccion = frente * IVertical + derecha * IHorizontal;
            direccion.Normalize();

            movimiento = direccion * velocidad * Time.deltaTime;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direccion), 0.2f);
        }

        movimiento.y += gravedad * Time.deltaTime;

        CController.Move(movimiento);

    }
}
