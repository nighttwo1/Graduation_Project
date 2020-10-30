using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using UnityEngine.UI;

public class SaveButtonHandler : MonoBehaviour
{
    public static Boolean isSaveBtnClicked2 = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isSaveBtnClicked2)
        {

        }
    }

    public class Drawings
    {
        public string target_id;
        public List<GameObject> trail_objects;

        public string temp_id;
        public List<Vector3> trail_objects_pos;

        public Drawings()
        {

        }

        public Drawings(string target_id, List<GameObject> trail_objects)
        {
            this.target_id = target_id;
            this.trail_objects = trail_objects;
        }

        public Drawings(string target_id, string temp_id)
        {
            this.target_id = target_id;
            this.temp_id = temp_id;
        }

        public Drawings(string target_id, List<Vector3> trail_objects_pos)
        {
            this.target_id = target_id;
            this.trail_objects_pos = trail_objects_pos;
        }
    }

    public void SaveOnClickB()
    {
        isSaveBtnClicked2 = true;

        Drawings d = new Drawings(CloudRecoEventHandler.target_id, SwipeTrail.trail_objectList);
        //사용자가 그린 그림 setactive(false)로 비활성화하여 그림 안보이게
        for (int i = 0; i < SwipeTrail.trail_objectList.Count; i++)
        {
            SwipeTrail.trail_objectList[i].SetActive(false);
        }

        SwipeTrail.swipmanager.SetActive(false);    //그림 그리기 비활성화
        RecogButtonScript.recogBtn.SetActive(true);     //이미지 인식 버튼 활성화
        SaveButtonScript.button_drawing_save.SetActive(false);      //그림 저장 버튼 비활성화
        DrawingButtonScript.button_drawing.SetActive(false);

        /** Firebase에 trail_object들과 target_id를 저장한다
        */


        List<string> t = new List<string>();
        t.Add("A");
        t.Add("B");
        t.Add("C");



        FirebaseApp app = FirebaseApp.DefaultInstance;
        app.SetEditorDatabaseUrl("https://arlog-2c813.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        //List<Vector3> temp_vector3 = SwipeTrail.objects_list[0];

        for(int i = 0; i < SwipeTrail.objects_list.Count; i++)
        {
            List<Vector3> temp_vector3 = SwipeTrail.objects_list[i];
            for(int j = 0; j < temp_vector3.Count; j++)
            {
                Drawings user = new Drawings(d.target_id, temp_vector3[j].x + " " + temp_vector3[j].y + " " + temp_vector3[j].z);
                string json = JsonUtility.ToJson(user);
                reference.Child("target").Child(d.target_id).Push().SetRawJsonValueAsync(json);
            }
            
        }
        /*
        Drawings user = new Drawings(d.target_id, temp_vector3[0].x+" "+temp_vector3[0].y+ " " +temp_vector3[0].z);
        string json = JsonUtility.ToJson(user);
        reference.Child("target").Child(d.target_id).Push().SetRawJsonValueAsync(json);
        */
        //string json = JsonUtility.ToJson(t);
        //reference.Child("target").Push().Child("Object").SetRawJsonValueAsync(json);

    }
}
