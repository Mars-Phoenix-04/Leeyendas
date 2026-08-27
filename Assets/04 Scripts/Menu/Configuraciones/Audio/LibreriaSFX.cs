using UnityEngine;

[System.Serializable]
public struct SFX
{
    public string grupoID;
    public AudioClip[] clips;
}

public class LibreriaSFX : MonoBehaviour
{
    public SFX[] efectosSonido;

    public AudioClip ObtenerSFX(string name)
    {
        foreach(var sfx in efectosSonido)
        {
            if (sfx.grupoID == name)
            {
                return sfx.clips[Random.Range(0, sfx.clips.Length)];
            }
        }
        return null;
    }
}
