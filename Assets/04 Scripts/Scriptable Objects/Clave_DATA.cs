using UnityEngine;
[CreateAssetMenu(fileName = "NuevaClave", menuName = "Leeyenda/Clave")]

public class Clave_DATA : ScriptableObject{
    public string IDClave;
    public string NombreClave;
    public Sprite IconoClave;
    public int PuntosComprension;
    [TextArea] public string descripcion;
}
