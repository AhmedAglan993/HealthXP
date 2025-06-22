using System;
using System.Collections;
using UnityEngine;

public class UserSessionManager : MonoBehaviour
{
    public static UserSessionManager Instance { get; private set; }

    private IUserProfileService profileService;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            profileService = new FirestoreUserProfileService();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnLoginSuccess(FirebaseUserData user)
    {
        AuthLoginManager.Instance.CurrentUser = user;

        if (IsNewUserFlow())
            HandleNewUser(user);
        else
            HandleReturningUser(user);
    }

    private bool IsNewUserFlow()
    {
        return RoleSelectionManager.Instance != null &&
               !string.IsNullOrEmpty(RoleSelectionManager.Instance.SelectedRole);
    }

    private void HandleNewUser(FirebaseUserData user)
    {
        string selectedRole = RoleSelectionManager.Instance.SelectedRole;

        StartCoroutine(profileService.SaveUserProfile(
            user.localId,
            user.idToken,
            selectedRole,
            onSuccess: () =>
            {
                Debug.Log($"✅ New role saved: {selectedRole}");
                NavigateToHome(selectedRole);
            },
            onError: error =>
            {
                Debug.LogError($"❌ Failed to save role: {error}");
                NavigateToHome("user"); // fallback
            }
        ));
    }

    private void HandleReturningUser(FirebaseUserData user)
    {
        string docPath = $"users/{user.localId}";

        StartCoroutine(FirestoreRESTManager.GetDocument(
            docPath,
            user.idToken,
            onSuccess: resultJson =>
            {
                string role = ParseRoleFromJson(resultJson);
                NavigateToHome(role);
            },
            onError: error =>
            {
                Debug.LogError($"❌ Failed to fetch role: {error}");
                NavigateToHome("user"); // fallback
            }
        ));
    }

    private void NavigateToHome(string role)
    {
        var screen = role == "doctor" ? ScreenId.DoctorDashboard : ScreenId.UserDashboard;
        ScreenNavigator.Instance.NavigateTo(screen);
    }

    private string ParseRoleFromJson(string json)
    {
        try
        {
            var parsed = JsonUtility.FromJson<FirestoreRoleWrapper>(json);
            return parsed?.fields?.role?.stringValue ?? "user";
        }
        catch (Exception ex)
        {
            Debug.LogError($"⚠️ Failed to parse role JSON: {ex.Message}");
            return "user";
        }
    }

    // JSON Mapping Classes
    [Serializable] public class StringValue { public string stringValue; }
    [Serializable] public class RoleFields { public StringValue role; }
    [Serializable] public class FirestoreRoleWrapper { public RoleFields fields; }
}
