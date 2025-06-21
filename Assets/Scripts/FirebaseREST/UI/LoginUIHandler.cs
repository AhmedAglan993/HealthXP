using UnityEngine;

public class LoginUIHandler : MonoBehaviour
{
    public void OnClickLoginAnonymous()
    {
        AuthLoginManager.Instance.LoginWithProvider(new AnonymousAuthProvider());
    }

    public void OnClickLoginGoogle()
    {
        AuthLoginManager.Instance.LoginWithProvider(new GoogleAuthProvider());
    }

    private void OnEnable()
    {
        AuthLoginManager.Instance.OnLoginSuccess += OnLoginSuccess;
        AuthLoginManager.Instance.OnLoginError += OnLoginError;
    }

    private void OnDisable()
    {
        AuthLoginManager.Instance.OnLoginSuccess -= OnLoginSuccess;
        AuthLoginManager.Instance.OnLoginError -= OnLoginError;
    }

    private void OnLoginSuccess(FirebaseUserData user)
    {
        Debug.Log("🎉 Welcome, " + user.localId);
        // Proceed to game or dashboard
    }

    private void OnLoginError(string error)
    {
        Debug.LogError("⚠️ Login Failed: " + error);
    }
}
