using UnityEngine;

[CreateAssetMenu(fileName = "OBJ_Script_PuntoInflexion", menuName = "Leeyenda/PuntoInflexion")]
public class OBJ_Script_PuntoInflexion : ScriptableObject{
    [Header("Diseño en el Canvas")]
    public Sprite ImagenCap;
    public string TXTCap;
    [Header("Historia")]
    //Texto de la historia y donde se va a cambiar el texto, para esto debemos poner dentro del texto '{0}'
    //Esto porque cuando se cambia dentro de la pantalla de UI, se usa un String.Format y no una interpolación
    //porque el texto lo estamos mandando desde aquí y no dentro del script del evento.
    [TextArea(4, 8)] [Tooltip("Al escribir texto se tiene que poner un {0} en donde va a ir la clave")] public string TXTHistoria;
    [TextArea(4, 8)] public string[] TXTResultadoPuntoInflexion;
    public float ProgresoHistoria;
    public float SubirProgreso => ProgresoHistoria;
    [Header("CLAVE Correcta")]
    [SerializeField] private Clave_DATA ClaveCorrectaUso;
    public Clave_DATA ClaveEventoUso => ClaveCorrectaUso;

    [Header("Cambios de Escena")]
    [SerializeField] private bool VaACambiarDeEscena;
    [SerializeField] private string NombreEscenaCambiar;
    public bool CambiarDeEscena => VaACambiarDeEscena;
    public string NombreSiguienteEscena => NombreEscenaCambiar;

}
