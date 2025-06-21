public interface ITokenCacheService
{
    bool HasCachedUser();
    FirebaseUserData LoadCachedUser();
    void SaveUser(FirebaseUserData user);
    void ClearCache();
}
