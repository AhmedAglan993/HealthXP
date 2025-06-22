using System;
using System.Collections;

public interface IUserProfileService
{
    IEnumerator SaveUserProfile(string uid, string token, string role, Action onSuccess, Action<string> onError);
}
