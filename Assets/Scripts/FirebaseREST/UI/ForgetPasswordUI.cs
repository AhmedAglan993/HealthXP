using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ForgetPasswordUI : MonoBehaviour
{
    public TMP_InputField inputEmail;
    public Button buttonForgotPassword;
    // Start is called before the first frame update
    void Start()
    {
        buttonForgotPassword.onClick.AddListener(HandleResetPassword);

    }

    // Update is called once per frame
    void HandleResetPassword()
    {
        string email = inputEmail.text;
        StartCoroutine(FirebasePasswordReset.SendResetEmail(email,
            () => ToastManager.Instance.ShowToast("🔐 Reset email sent.", ToastData.ToastType.Success),
            err => ToastManager.Instance.ShowToast($"⚠️ Error: {err}", ToastData.ToastType.Error)));
    }
}
