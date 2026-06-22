using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    public static InteractionPromptUI Instance { get; private set; }

    [SerializeField] private RectTransform root;

    private Transform target;
    private Camera mainCamera;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        mainCamera = Camera.main;

        Hide();
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        root.position =  mainCamera.WorldToScreenPoint(  target.position);
    }

    public void Show(Transform anchor)
    {
        target = anchor;
        root.gameObject.SetActive(true);
    }

    public void Hide()
    {
        target = null;
        root.gameObject.SetActive(false);
    }
}