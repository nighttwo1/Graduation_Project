using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;

public class textscript : MonoBehaviour
{
    public Text ScriptTxt;

    // Start is called before the first frame update
    void Start()
    {
        ScriptTxt.text = "탐색된 타겟이 없습니다";

        /*
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://arlog-2c813.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        User user = new User("TEST", "TEST2");
        string json = JsonUtility.ToJson(user);
        reference.Child("users").SetRawJsonValueAsync(json);
        */

    }

    public class User
    {
        public string username;
        public string email;

        public User()
        {
        }

        public User(string username, string email)
        {
            this.username = username;
            this.email = email;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ScriptTxt.text = CloudRecoEventHandler.targetname;
        //ScriptTxt.text = ButtonTest2.button_count + "";
    }
}
