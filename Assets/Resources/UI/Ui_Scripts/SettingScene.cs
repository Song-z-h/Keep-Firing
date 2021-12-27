using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using Firebase;
using System.Collections;


public class SettingScene : MonoBehaviour
{
    public Text statusText;
    Firebase.Auth.FirebaseAuth auth;
    public string webClientId = "1071309782203-hbcnp1t6h8hccrmtismfdimk964grla5.apps.googleusercontent.com";
    public GameObject googleSignInButton;
    DatabaseReference reference;
    bool isCompletedTrue = false;

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
        RecordData data = SaveSystem.LoadGameState();
       
        if (auth.CurrentUser != null && data.loggedInWithGoogle)
        {
            statusText.text = auth.CurrentUser.UserId;
            SubmitMyDataToServer();
            googleSignInButton.SetActive(false);
        }

    }

    void Update()
    {
        /*RecordData data = SaveSystem.LoadGameState();
        Debug.Log("google id ====================== " + FirebaseAuth.DefaultInstance.CurrentUser.UserId);
        Debug.Log("data.id ======================== " + data.id);
        Debug.Log("data.googleID ========================" + data.googleId);*/
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
               
                

                Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(((Task<GoogleSignInUser>)task).Result.IdToken, null);
                auth.SignInWithCredentialAsync(credential).ContinueWith(async authTask => {
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
                        Debug.Log("_11111111111111111111111111111111111111111111111111");
                         await Task.Run(() => signInCompleted.SetResult(((Task<FirebaseUser>)authTask).Result));
                        Debug.Log("_222222222222222222222222222222222222222222222");
                        /*RecordData data = SaveSystem.LoadGameState();
                        Debug.Log("google id ====================== " + FirebaseAuth.DefaultInstance.CurrentUser.UserId);
                        Debug.Log("data.id ======================== " + data.id);
                        Debug.Log("data.googleID ========================" + data.googleId);
                        //sign in successfully
                        //RecordData data = SaveSystem.LoadGameState();

                       // await Task.Run(() => CheckIfExsistAccount());

                        Debug.Log("isCompletedTrue --> " + isCompletedTrue);
                        if (isCompletedTrue)
                        {
                            //retrive my data from existing account
                             LoadMyDataFromServer();
                            Debug.Log("_load! load load load load load load load vload load vbloadload load");
                        }
                        else
                        {
                            //register an account and link with my google
                             RegisterMyDataToGoogleAccount();
                            Debug.Log("_Register Register Register Register Register Register Register Register");
                        }
                        */

                        StartCoroutine(DealWithAccount());


                        googleSignInButton.SetActive(false);
                        auth = FirebaseAuth.DefaultInstance;
                        FirebaseUser user = auth.CurrentUser;
                        statusText.text = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
                    }
                });
            }
        });

       
    }

    public void RegisterMyDataToGoogleAccount()
    {
        Debug.Log("_333333333333333333333333333333333333333333333333333");
        RecordData data = SaveSystem.LoadGameState();
            if (data.loggedInWithGoogle == true)
                return;
        //string googleId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        Debug.Log("_4444444444444444444444444444444444444444444444444444");
        if (FirebaseAuth.DefaultInstance.CurrentUser.UserId == null)
                return;

        //save reference id to this user's google account tree
        data.googleId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        if (data.id == null)
            data.id = reference.Child("user").Push().Key;

        if (FirebaseAuth.DefaultInstance.CurrentUser.UserId != null)
            reference.Child("googleAccounts").Child(FirebaseAuth.DefaultInstance.CurrentUser.UserId).Child("account1").SetValueAsync(data.id);

        //save all data of this user to user tree 
       
        data.loggedInWithGoogle = true;
       
        string json = JsonUtility.ToJson(data);
        reference.Child("user").Child(data.id).SetRawJsonValueAsync(json);

        
        SaveSystem.SaveGameState(data);
       
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

    public void OnSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddStatusText("Calling SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
          OnAuthenticationFinished);
    }

    public void OnSignOut()
    {
        //AddStatusText("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
    }

    public void OnDisconnect()
    {
        AddStatusText("Calling Disconnect");
        GoogleSignIn.DefaultInstance.Disconnect();
    }

    public void CheckIfExsistAccount()
    {
        Debug.Log("_666666666666666666666666666666666666666666666666666666");
        if (FirebaseAuth.DefaultInstance.CurrentUser.UserId == null)
        {
            Debug.Log("_ userID null!!!!!");
            return;
        }
        Debug.Log("_77777777777777777777777777777777777777777777777777777");


         FirebaseDatabase.DefaultInstance
     .GetReference("googleAccounts").Child(FirebaseAuth.DefaultInstance.CurrentUser.UserId).Child("account1")
     .GetValueAsync().ContinueWith(task => {
         //Debug.Log(task.Result);
         if (task.IsFaulted)
         {
             Debug.Log("_8888888888888888888888888888888888888888888888888888");
             Debug.Log("not been able to get that data");
             isCompletedTrue = false;       
         }
          else if (task.IsCompleted)
         { 
             //set data id and data googleid

             DataSnapshot snapshot = task.Result;
             string s = snapshot.GetRawJsonValue();
             
             RecordData data = SaveSystem.LoadGameState();
             data.googleId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
             //data.id = s.Substring(1, s.Length - 1);
             string c = "\"";
             data.id = s.Replace(c, "");
             data.loggedInWithGoogle = true;
             Debug.Log("data.googleID :" +  data.googleId);
             Debug.Log("data.id :" +  data.id);


            
              isCompletedTrue = true;

             if (data.id == null || data.googleId == null)
             {
                 isCompletedTrue = false;
                 data.loggedInWithGoogle = false;
             }
             Debug.Log("_99999999999999999999999999999999999999999999999999999");
             SaveSystem.SaveGameState(data);
             return;
         }
         
         Debug.Log("_1010101010101010101010101010101010101010101010");
         isCompletedTrue = false;
         return;
     });
        Debug.Log("_12121212121212121212121212121212121212121212121212121212");
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<System.Exception> enumerator =
                    task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error =
                            (GoogleSignIn.SignInException)enumerator.Current;
                    AddStatusText("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    AddStatusText("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            AddStatusText("Canceled");
        }
        else
        {
            AddStatusText("Welcome: " + task.Result.DisplayName + "!");
            
        }
    }
    
    public void OnSignInSilently()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddStatusText("Calling SignIn Silently");

        GoogleSignIn.DefaultInstance.SignInSilently()
              .ContinueWith(OnAuthenticationFinished);
    }


    public void OnGamesSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = true;
        GoogleSignIn.Configuration.RequestIdToken = false;

        AddStatusText("Calling Games SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
          OnAuthenticationFinished);
    }

    IEnumerator DealWithAccount()
    {
        CheckIfExsistAccount();
        yield return new WaitForSeconds(3);
        if (isCompletedTrue)
        {
            //retrive my data from existing account
            LoadMyDataFromServer();
            Debug.Log("_load! load load load load load load load vload load vbloadload load");
        }
        else
        {
            //register an account and link with my google
            RegisterMyDataToGoogleAccount();
            Debug.Log("_Register Register Register Register Register Register Register Register");
        }
    }

    // private List<string> messages = new List<string>();
    void AddStatusText(string text)
    {
       /* if (messages.Count == 5)
        {
            messages.RemoveAt(0);
        }
        messages.Add(text);
        string txt = "";
        foreach (string s in messages)
        {
            txt += "\n" + s;
        }*/
        statusText.text = text;
    }

    void OnApplicationQuit()
    {
        OnSignOut();
        auth.SignOut();
        Debug.Log("Application ending after " + Time.time + " seconds");
    }

    /*[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        Debug.Log("Before scene loaded");
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://keepfiring-d0d46.firebaseio.com/");

        // Get the root reference location of the database.
        DatabaseReference reference;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        

    }*/
}



