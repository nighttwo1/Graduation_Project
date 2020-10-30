using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeColorHandler : MonoBehaviour
{
    public static GameObject cube;
    // Start is called before the first frame update
    void Start()
    {
        cube = GameObject.Find("Cube");
        GetComponent< Renderer >().material.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<Renderer>().material.color == Color.red)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
    }

}
