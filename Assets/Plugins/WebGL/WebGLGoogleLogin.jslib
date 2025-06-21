mergeInto(LibraryManager.library, {
  SignInWithGoogle: function (onSuccess, onFailure) {
    // This opens Google's auth popup
    firebase.auth().signInWithPopup(new firebase.auth.GoogleAuthProvider())
      .then(function (result) {
        var idToken = result.credential.idToken;
        unityInstance.SendMessage("GoogleLoginBridge", "OnGoogleLoginSuccess", idToken);
      })
      .catch(function (error) {
        unityInstance.SendMessage("GoogleLoginBridge", "OnGoogleLoginFailed", error.message);
      });
  }
});
