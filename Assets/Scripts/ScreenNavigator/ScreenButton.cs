using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ScreenButton : MonoBehaviour
{
    public enum ActionType { ShowScreen, ShowPopup, HidePopup }

    [SerializeField] private ActionType action;
    [SerializeField] private UIScreen screen;
    [SerializeField] private UIPopup popup;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            switch (action)
            {
                case ActionType.ShowScreen:
                    UIScreenNavigator.Instance.ShowScreen(screen);
                    break;
                case ActionType.ShowPopup:
                    UIScreenNavigator.Instance.ShowPopup(popup);
                    break;
                case ActionType.HidePopup:
                    UIScreenNavigator.Instance.HidePopup(popup);
                    break;
            }
        });
    }
}
