using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingButtonScript : MonoBehaviour
{
    public static GameObject button_drawing;
    // Start is called before the first frame update
    void Start()
    {
        button_drawing = GameObject.Find("Button_drawing");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
