using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayButton : MonoBehaviour
{
    GameManager gmRef;

    private void Start()
    {
        gmRef = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void ReplayGame()
    {
        gmRef.RestartLevel();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MainGame");
    }
}
