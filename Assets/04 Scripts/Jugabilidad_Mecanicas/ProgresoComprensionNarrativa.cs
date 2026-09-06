using System;
using UnityEngine;

public class ProgresoComprensionNarrativa : MonoBehaviour
{
    public static ProgresoComprensionNarrativa Instancia { get; private set; }
    [Header("Config de Progreso de la Historia")]
    [SerializeField] private int CapituloActual = 1;
    [Range(0f, 100f)][SerializeField] private float Caprogreso = 0f;

    [Header("Config de Nivel de Comprension")]
    [Range(0f, 100f)][SerializeField] private float CapComprension = 0f;

    //Eventos de Cambio en Valores y Propiedades
    public event Action<int> CambioDeCapitulo;
    public event Action<float> CambioEnProgreso;
    public event Action<float> CambioEnComprension;

    public int Capitulo => CapituloActual;
    public float ProgresoCap => Caprogreso;
    public float ComprensionCap => CapComprension;

    private void Awake(){
        if (Instancia == null) Instancia = this;
        else if (Instancia != this) Destroy(gameObject);
    }
    public void ProgresoCapituloCambio(float progreso){
        Caprogreso = Mathf.Clamp(Caprogreso + progreso, 0f, 100f);
        CambioEnProgreso?.Invoke(Caprogreso);
    }
    public void ComprensionCapituloCambio(float comprension){
        CapComprension = Mathf.Clamp(CapComprension + comprension, 0f, 100f);
        CambioEnComprension?.Invoke(CapComprension);
    }
    public void CapituloEnCurso(int capitulo){
        CapituloActual = capitulo;
        Caprogreso = 0f;
        CambioDeCapitulo?.Invoke(capitulo);
        CambioEnProgreso?.Invoke(Caprogreso);
    }
}
