public class LoadingService : ILoadingService
{
    private readonly LoadingView view;

    public LoadingService(LoadingView view)
    {
        this.view = view;
    }

    public void Show(string message = "Loading...")
    {
        view.SetMessage(message);
        view.FadeIn();
    }

    public void Hide()
    {
        view.FadeOut();
    }

    public bool IsVisible => view.IsVisible;
}
