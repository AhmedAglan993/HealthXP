using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ScreenButton : MonoBehaviour
{

    [SerializeField] private ScreenId screen;

    protected virtual void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            ScreenNavigator.Instance.NavigateTo(screen);
        });
    }
}
