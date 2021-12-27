using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class LeaderBoardScene : MonoBehaviour
{
    DatabaseReference reference;
    List<Leaderboard> leaderboards;
    public Text yourname;


    // Start is called before the first frame update
    void Start()
    {
      
        leaderboards = new List<Leaderboard>(5);

        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://keepfiring-d0d46.firebaseio.com/");

        // Get the root reference location of the database.
         reference = FirebaseDatabase.DefaultInstance.RootReference;
      
        RecordData data = SaveSystem.LoadGameState();
        if (!data.loggedInWithGoogle)
        {
            Login_Anonymous();
        }
        

        
        //SubmitMyRecordToServer();
        RefleshLeaderboard();
    }

    public void RefleshLeaderboard()
    {

        FirebaseDatabase.DefaultInstance.GetReference("leader").OrderByChild("score").LimitToLast(5)
            .ValueChanged += HandleValueChanged;
        
    }


    void  HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
       
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        // Do something with the data in args.Snapshot
        //Debug.Log(args.Snapshot.GetRawJsonValue());

        //string leaderdata = args.Snapshot.GetRawJsonValue();
        //Debug.Log(leaderdata);
        int index = 0;
        foreach(var child in args.Snapshot.Children)
        {
            string s = child.GetRawJsonValue();
            Leaderboard lb = JsonUtility.FromJson<Leaderboard>(s);
            transform.GetChild(index).GetChild(2).GetComponent<Text>().text = lb.name;
            transform.GetChild(index).GetChild(3).GetComponent<Text>().text = lb.score.ToString();

            index++;
        }
       
      
        for(int i = 4; i >= index; i--)
        {
            //0 panel - 1ranking - 2name - 3score

            Transform kid = transform.GetChild(i).GetChild(0);
            //kid.GetComponent<Image>().enabled = false;

            for (int j = 1; j < 4; j++)
            {
                 kid = transform.GetChild(i).GetChild(j);
                kid.GetComponent<Text>().enabled = false;
            }
              
        }
    }


    public void SubmitMyRecordToServer()
    {
      
      
        RecordData data = SaveSystem.LoadGameState();
        if(data.name != null)
        yourname.text = data.name.ToString();
        if (data.id == null)
            return;
       // Debug.Log(data.id);
        FirebaseDatabase.DefaultInstance
      .GetReference("leader").Child(data.id)
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
              Leaderboard lb = JsonUtility.FromJson<Leaderboard>(s);
              
              int myscore = lb.score;
             
             if (data.score > myscore)
              {
                  string json = data.score.ToString();
                  reference.Child("leader").Child(data.id).Child("score").SetRawJsonValueAsync(json);
              }
              RefleshLeaderboard();
          }
      });
       
    }


    public void  Login_Anonymous()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.SignInAnonymouslyAsync().ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }

    
}
