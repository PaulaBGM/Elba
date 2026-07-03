using UnityEngine;

public class GameEndUI : MonoBehaviour
{
    public static GameEndUI Instance { get; private set; }

    [SerializeField] private GameObject root;

    private void Awake()
    {
        Instance = this;

        if (root != null)
            root.SetActive(false);
    }

    public void Show()
    {
        Time.timeScale = 0f;
        root.SetActive(true);
    }

    public void ContinueGame()
    {
        root.SetActive(false);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}