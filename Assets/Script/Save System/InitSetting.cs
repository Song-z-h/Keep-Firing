using System.Threading.Tasks;
using Google;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using Firebase;
using UnityEngine.SceneManagement;

public class InitSetting : MonoBehaviour
{
    
    Firebase.Auth.FirebaseAuth auth;
    public string webClientId = "1071309782203-hbcnp1t6h8hccrmtismfdimk964grla5.apps.googleusercontent.com";
   
    DatabaseReference reference;

    private GoogleSignInConfiguration configuration;
    void Awake()
    {
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = webClientId,
            RequestIdToken = true
        };
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://keepfiring-d0d46.firebaseio.com/");

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        if (PlayerPrefs.GetInt("FIRSTTIMEOPENING", 1) == 0)
        {
            RecordData data = SaveSystem.LoadGameState();
            if (data != null)
                if (data.loggedInWithGoogle)
                    SignInWithGoogle(false);
        }
           
        SceneManager.LoadScene("StartScene");
    }

    public void SignInWithGoogle(bool linkWithCurrentAnonUser)
    {

        GoogleSignIn.Configuration = new GoogleSignInConfiguration
        {
            RequestIdToken = true,
            // Copy this value from the google-service.json file.
            // oauth_client with type == 3
            WebClientId = webClientId
        };

        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;

        Task<GoogleSignInUser> signIn = GoogleSignIn.DefaultInstance.SignIn();

        TaskCompletionSource<FirebaseUser> signInCompleted = new TaskCompletionSource<FirebaseUser>();
        signIn.ContinueWith(task => {
            if (task.IsCanceled)
            {
                signInCompleted.SetCanceled();
            }
            else if (task.IsFaulted)
            {
                signInCompleted.SetException(task.Exception);

            }
            else
            {
               
                auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                Firebase.Auth.FirebaseUser user = auth.CurrentUser;
                //LoadMyDataFromServer();
                Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(((Task<GoogleSignInUser>)task).Result.IdToken, null);
                auth.SignInWithCredentialAsync(credential).ContinueWith(authTask => {
                    if (authTask.IsCanceled)
                    {
                        signInCompleted.SetCanceled();
                    }
                    else if (authTask.IsFaulted)
                    {
                        signInCompleted.SetException(authTask.Exception);
                    }
                    else
                    {
                        signInCompleted.SetResult(((Task<FirebaseUser>)authTask).Result);
                    }
                });
            }
        });


    }
    

    public void SubmitMyDataToServer()
    {

        RecordData data = SaveSystem.LoadGameState();
        if (data.googleId == null || data.id == null)
            return;
        string json = JsonUtility.ToJson(data);
        reference.Child("user").Child(data.id).SetRawJsonValueAsync(json);
    }

    public void LoadMyDataFromServer()
    {
        RecordData data = SaveSystem.LoadGameState();
        if (data.googleId == null || data.id == null)
            return;
        FirebaseDatabase.DefaultInstance
     .GetReference("user").Child(data.id)
     .GetValueAsync().ContinueWith(task => {
         //Debug.Log(task.Result);
         if (task.IsFaulted)
         {
             Debug.Log("not been able to get that data");
         }
         else if (task.IsCompleted)
         {
             DataSnapshot snapshot = task.Result;
             string s = snapshot.GetRawJsonValue();
             RecordData dataFromServer = JsonUtility.FromJson<RecordData>(s);

             SaveSystem.SaveGameState(dataFromServer);
         }
     });

    }
    
   
}
