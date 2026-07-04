using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public static GameOverController Instance { get; private set; }

    [SerializeField] private GameObject root;

    private bool isGameOver;

    public bool IsGameOver => isGameOver;

    private void Awake()
    {
        Instance = this;
        root.SetActive(false);
    }

    public void Show()
    {
        if (isGameOver)
            return;

        isGameOver = true;

        root.SetActive(true);

        Time.timeScale = 0f;

        UIManager.Instance.SetGameOver(true);
    }

    public void Retry()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuInicial");
    }
}