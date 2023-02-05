using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitButton : MonoBehaviour
{
    public void QuitGame()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
