using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPopup : MonoBehaviour
{
    public static TutorialPopup Instance;

    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text bodyText;
    [SerializeField] private Button closeButton;

    private string currentID;

    private void Awake()
    {
        Instance = this;

        root.SetActive(false);

        closeButton.onClick.AddListener(Close);
    }

    public void Show(string id, string title, string body)
    {
        if (TutorialManager.HasSeen(id))
            return;

        currentID = id;

        titleText.text = title;
        bodyText.text = body;

        root.SetActive(true);

        Time.timeScale = 0;
    }

    public void Close()
    {
        TutorialManager.MarkSeen(currentID);

        root.SetActive(false);

        Time.timeScale = 1;
    }
}