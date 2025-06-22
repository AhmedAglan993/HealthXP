using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpButton : MonoBehaviour
{
    public enum ActionType { ShowPopup, HidePopup }
    [SerializeField] public popupId popup;
    [SerializeField] protected ActionType action;


    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            switch (action)
            {
                case ActionType.ShowPopup:
                    ScreenNavigator.Instance.ShowPopup(popup);
                    break;
                case ActionType.HidePopup:
                    ScreenNavigator.Instance.ClosePopup(popup);
                    break;
            }
        });
    }
}
