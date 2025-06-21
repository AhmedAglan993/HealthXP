using System;
using System.Collections;
using UnityEngine;
using System.Runtime.InteropServices;

public static class WebGLGoogleLogin
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void SignInWithGoogle();
#endif

    private static Action<string> _onSuccess;
    private static Action<string> _onError;

    public static IEnumerator SignInWithGoogle(Action<string> onSuccess, Action<string> onError)
    {
        _onSuccess = onSuccess;
        _onError = onError;

#if UNITY_WEBGL && !UNITY_EDITOR
        SignInWithGoogle();
#else
        onError?.Invoke("Google Sign-In only works in WebGL build.");
#endif
        yield return null;
    }

    public static void OnGoogleLoginSuccess(string idToken)
    {
        _onSuccess?.Invoke(idToken);
    }

    public static void OnGoogleLoginFailed(string error)
    {
        _onError?.Invoke(error);
    }
}
