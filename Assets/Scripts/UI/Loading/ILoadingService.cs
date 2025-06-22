public interface ILoadingService
{
    void Show(string message = "Loading...");
    void Hide();
    bool IsVisible { get; }
}
