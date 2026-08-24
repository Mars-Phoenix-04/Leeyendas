using UnityEngine;

public class MovimientoCamara : MonoBehaviour
{
    [Header("Jugador")]
    public Transform Aseguir;

    [Header("Camara")]
    private Camera MainCamara;
    private Vector2 TamanoPlanoCrca;
    public float MAXdistancia;
    private Vector2 angulo = new Vector2(90 * Mathf.Deg2Rad, 0);
    public Vector2 sensibilidad;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        MainCamara = GetComponent<Camera>();

        CalcularPlanosCerca();
    }

    private void CalcularPlanosCerca()
    {
        float alto = Mathf.Tan(MainCamara.fieldOfView * Mathf.Deg2Rad / 2) * MainCamara.nearClipPlane;
        float ancho = alto * MainCamara.aspect;

        TamanoPlanoCrca = new Vector2(ancho, alto);
    }

    private Vector3[] ObtenerPuntosColisionCamara(Vector3 Direccion)
    {
        Vector3 posicion = Aseguir.position;
        Vector3 centro = posicion + Direccion * (MainCamara.nearClipPlane + 0.2f);

        Vector3 derecha = transform.right * TamanoPlanoCrca.x;
        Vector3 arriba = transform.up * TamanoPlanoCrca.y;

        return new Vector3[]
        {
            centro - derecha + arriba,
            centro + derecha + arriba,
            centro - derecha - arriba,
            centro + derecha - arriba
        };
    }

    void Update()
    {
        float IHorizontal = Input.GetAxis("Mouse X");

        if (IHorizontal != 0)
        {
            angulo.x += IHorizontal * Mathf.Deg2Rad * sensibilidad.x;
        }

        float IVertical = Input.GetAxis("Mouse Y");

        if (IVertical != 0)
        {
            angulo.y += IVertical * Mathf.Deg2Rad * sensibilidad.y;
            angulo.y = Mathf.Clamp(angulo.y, -80 * Mathf.Deg2Rad, 80 * Mathf.Deg2Rad);
        }
    }
    void LateUpdate()
    {
        Vector3 DireccionOrbita = new Vector3(-Mathf.Cos(angulo.x) * -Mathf.Cos(angulo.y), Mathf.Sin(angulo.y), -Mathf.Sin(angulo.x) * Mathf.Cos(angulo.y));

        RaycastHit hit;
        float distancia = MAXdistancia;

        Vector3[] puntos = ObtenerPuntosColisionCamara(DireccionOrbita);

        foreach (Vector3 punto in puntos)
        {
            if (Physics.Raycast(punto, DireccionOrbita, out hit, MAXdistancia))
            {
                distancia = Mathf.Min((hit.point - Aseguir.position).magnitude);
            }
        }


        transform.position = Aseguir.position + DireccionOrbita * distancia;
        transform.rotation = Quaternion.LookRotation(Aseguir.position - transform.position);
    }
}
