using UnityEngine;

public class RoleSelectionManager : MonoBehaviour
{
    public static RoleSelectionManager Instance { get; private set; }
    public string SelectedRole { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetRole(string role)
    {
        SelectedRole = role;
        Debug.Log("🎯 Role selected: " + role);
    }

    public bool HasSelectedRole() => !string.IsNullOrEmpty(SelectedRole);
}
