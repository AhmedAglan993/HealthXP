using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class EmailAuthProvider : IAuthProvider
{
    private readonly string email;
    private readonly string password;
    private readonly bool isSignUp;
    private const string ApiKey = "AIzaSyDsrPpzKKUQyAc2p_sKxcaX2Z0r4YiBOAg";

    public EmailAuthProvider(string email, string password, bool isSignUp = false)
    {
        this.email = email;
        this.password = password;
        this.isSignUp = isSignUp;
    }

    public IEnumerator SignIn(Action<FirebaseUserData> onSuccess, Action<string> onError)
    {
        string url = isSignUp
            ? $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={ApiKey}"
            : $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={ApiKey}";

        string json = JsonUtility.ToJson(new EmailAuthPayload
        {
            email = email,
            password = password,
            returnSecureToken = true
        });

        var request = WebRequestUtils.CreateJsonPostRequest(url, json);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            onError?.Invoke(request.downloadHandler.text);
            ToastManager.Instance.ShowToast("❌ Error Signing In!", ToastData.ToastType.Error);
        }
        else
        {
            var user = JsonUtility.FromJson<FirebaseUserData>(request.downloadHandler.text);
            onSuccess?.Invoke(user);

            if (isSignUp)
            {
                // ✅ Inline verification using yield return
                yield return FirebaseEmailVerifier.SendVerificationEmail(user.idToken,
                    () => ToastManager.Instance.ShowToast("📩 Verification Sent", ToastData.ToastType.Info),
                    err => ToastManager.Instance.ShowToast("❌ Failed to Send Verification", ToastData.ToastType.Error)
                );
            }
            else
                ToastManager.Instance.ShowToast("✅ Signed In Successfully!", ToastData.ToastType.Success);
        }
    }


    [Serializable]
    private class EmailAuthPayload
    {
        public string email;
        public string password;
        public bool returnSecureToken;
    }




    [Serializable]
    private class EmailVerificationPayload
    {
        public string requestType;
        public string idToken;
    }
}
