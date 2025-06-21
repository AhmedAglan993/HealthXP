using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmailAuthUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField inputEmail;
    public TMP_InputField inputPassword;
    public Button buttonLogin;

    private void Start()
    {
        buttonLogin.onClick.AddListener(HandleLogin);
    }

    void HandleLogin()
    {
        string email = inputEmail.text;
        string password = inputPassword.text;
        AuthLoginManager.Instance.LoginWithProvider(new EmailAuthProvider(email, password, false));
    }

    

   
}
