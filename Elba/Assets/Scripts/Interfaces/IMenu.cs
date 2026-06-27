public interface IMenu
{
    bool IsOpen { get; }

    void Show();
    void Hide();
}