using UnityEngine;

public class LoadingManager : MonoBehaviour
{
    public static ILoadingService Service { get; private set; }

    [SerializeField] private LoadingView loadingView;

    private void Awake()
    {
        if (Service == null)
        {
            DontDestroyOnLoad(gameObject);
            Service = new LoadingService(loadingView);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
