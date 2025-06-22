using UnityEngine;

public class RoleSelectionUI : MonoBehaviour
{
    public void OnDoctorSelected()
    {
        RoleSelectionManager.Instance.SetRole("doctor");
        ScreenNavigator.Instance.NavigateTo(ScreenId.Welcome);
    }

    public void OnUserSelected()
    {
        RoleSelectionManager.Instance.SetRole("user");
        ScreenNavigator.Instance.NavigateTo(ScreenId.Welcome);
    }
}
