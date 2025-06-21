using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignUpUI : MonoBehaviour
{
    public TMP_InputField inputEmail;
    public TMP_InputField inputPassword;
    public Button buttonSignUp;
    // Start is called before the first frame update
    void Start()
    {
        buttonSignUp.onClick.AddListener(HandleSignUp);

    }

    void HandleSignUp()
    {
        string email = inputEmail.text;
        string password = inputPassword.text;
        AuthLoginManager.Instance.LoginWithProvider(new EmailAuthProvider(email, password, true));
    }
}
