using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveButtonScript : MonoBehaviour
{
    public static GameObject button_drawing_save;

    // Start is called before the first frame update
    void Start()
    {
        button_drawing_save = GameObject.Find("SaveButton");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
