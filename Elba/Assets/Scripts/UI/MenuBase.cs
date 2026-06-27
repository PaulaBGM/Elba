using UnityEngine;

public abstract class MenuBase : MonoBehaviour, IMenu
{
    public bool IsOpen => gameObject.activeSelf;

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}