using System.Collections;

public interface IAuthProvider
{
    IEnumerator SignIn(System.Action<FirebaseUserData> onSuccess, System.Action<string> onError);
}
