using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleAuthProvider : IAuthProvider
{
    private const string ApiKey = "AIzaSyDsrPpzKKUQyAc2p_sKxcaX2Z0r4YiBOAg";
    private const string Url = "https://identitytoolkit.googleapis.com/v1/accounts:signInWithIdp?key=" + ApiKey;

    private string googleIdToken;

   

    public IEnumerator SignIn(Action<FirebaseUserData> onSuccess, Action<string> onError)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
    yield return WebGLGoogleLogin.SignInWithGoogle(idToken =>
    {
        // Firebase custom token exchange using idToken
        FirebaseCustomAuth.ExchangeGoogleToken(idToken, onSuccess, onError);
    }, onError);
#else
        onError?.Invoke("Google Sign-In only supported in WebGL build.");
        yield break;
#endif
    }

}
