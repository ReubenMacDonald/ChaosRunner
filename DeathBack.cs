using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathBack : MonoBehaviour
{
    public void Back()
    {
        SceneManager.LoadScene("Menu");
    }
}