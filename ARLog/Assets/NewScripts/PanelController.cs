using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using System;

public class PanelController : MonoBehaviour
{
    public static GameObject panel;

    public GameObject trailPrefab;
    private GameObject thisTrail;

    public GameObject point;

    private GameObject p;

    public static List<GameObject> p_list;

    public static List<Vector3> stored_drawings;


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

    void Awake()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                //   app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.

                //InitializeFirebase();
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    private void InitializeFirebase()
    {
        FirebaseApp app = FirebaseApp.DefaultInstance;
        app.SetEditorDatabaseUrl("https://arlog-2c813.firebaseio.com/");

        Debug.Log("Fault3");
        FirebaseDatabase.DefaultInstance.GetReference("target/165592bee2684516b2fcbc8c9c5af96c").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.Log("Fault");
                // Handle the error...
                //DrawingButtonScript.button_drawing.SetActive(true);

            }
            else if (task.IsCompleted)
            {
                Debug.Log("Fault5");
                DataSnapshot snapshot = task.Result;
                Debug.Log("SNAPSHOT: " + snapshot.ChildrenCount);

                if(snapshot.ChildrenCount == 0)
                {
                    DrawingButtonScript.button_drawing.SetActive(true);
                }
                //thisTrail = (GameObject)Instantiate(trailPrefab);
                //thisTrail.transform.rotation = Quaternion.identity;
                // Do something with snapshot...
                foreach (var rules in snapshot.Children)
                {   
                    
                    Debug.Log(rules.Child(CloudRecoEventHandler.target_id));
                    Debug.Log("Faul2t");
                    Debug.Log(rules.Child("target_id").Value);
                    Debug.Log("165592bee2684516b2fcbc8c9c5af96c");
                    
                    string position = (string)rules.Child("temp_id").Value;
                    string[] positions = position.Split(' ');

                    Vector3 v = new Vector3();
                    v.x = float.Parse(positions[0]);
                    v.y = float.Parse(positions[1]);
                    v.z = float.Parse(positions[2]);
                    Debug.Log("Vector: " + v.x + " " + v.y + " " + v.z);

                    //thisTrail.transform.position = v;
                }
            }
            //Debug.Log("Fault4");
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        panel = GameObject.Find("Panel");

        SwipeTrail.swipmanager.SetActive(false);   //그리기 비활성화
        SaveButtonScript.button_drawing_save.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        stored_drawings = new List<Vector3>();
        if (CloudRecoEventHandler.IsTargetRecognized)
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            app.SetEditorDatabaseUrl("https://arlog-2c813.firebaseio.com/");


            FirebaseDatabase.DefaultInstance.GetReference("target/"+ CloudRecoEventHandler.target_id).GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted)
                {
                    // Handle the error...
                    DrawingButtonScript.button_drawing.SetActive(true);
                    //Debug.Log("ABS");

                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    if (snapshot.ChildrenCount == 0)
                    {
                        DrawingButtonScript.button_drawing.SetActive(true);
                        //Debug.Log("ABS2222");
                    }
                    else if(CloudRecoEventHandler.first)
                    {
                        p_list = new List<GameObject>();

                        CloudRecoEventHandler.first = false;
                        //Debug.Log("ABS33333");
                        // Do something with snapshot...
                        foreach (var rules in snapshot.Children)
                        {
                            string position = (string)rules.Child("temp_id").Value;
                            string[] positions = position.Split(' ');

                            Vector3 v = new Vector3();
                            v.x = float.Parse(positions[0]);
                            v.y = float.Parse(positions[1]);
                            v.z = float.Parse(positions[2]);
                            //Debug.Log("Vector: " + v.x + " " + v.y + " " + v.z);


                            stored_drawings.Add(v);
                        }


                        thisTrail = (GameObject)Instantiate(trailPrefab, stored_drawings[0], Quaternion.identity);
                        thisTrail.transform.position = stored_drawings[0];

                        //p = (GameObject)Instantiate(point, 4 * stored_drawings[0], Quaternion.identity);

                        for (int i = 0; i < stored_drawings.Count; i++)
                        {
                            p = (GameObject)Instantiate(point, 4 * stored_drawings[i], Quaternion.identity);
                            //p_list.Add(p);

                            //thisTrail.transform.position = stored_drawings[i];
                            Debug.Log("Vector : " + 4 * stored_drawings[i].x + " " + 4 * stored_drawings[i].y + " " + 4 * stored_drawings[i].z);
                        }

                    }

                    
                }
            });

            //DrawPanelController.panel_draw.SetActive(true);
            //panel_draw.GetComponent<Renderer>().enabled = true;
            //DrawingButtonScript.button_drawing.SetActive(true);
            //DrawingButtonScript.button_drawing.GetComponent<Renderer>().enabled = true;

        }
        else
        {
            //DrawPanelController.panel_draw.SetActive(false);
            //panel_draw.GetComponent<Renderer>().enabled = false;
            DrawingButtonScript.button_drawing.SetActive(false);
            
        }
        
    }
}
