using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnonymousLoginUI : MonoBehaviour
{
    public void OnClickLoginAsGuest()
    {
        var provider = new AnonymousAuthProvider();
        AuthLoginManager.Instance.LoginWithProvider(provider);
    }
}
