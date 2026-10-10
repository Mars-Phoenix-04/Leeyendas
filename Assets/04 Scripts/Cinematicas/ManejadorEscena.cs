using UnityEngine;
using UnityEngine.SceneManagement;

public class ManejadorEscena : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.LoadScene("01_ACT1_DormitorioMarta");
    }
}
