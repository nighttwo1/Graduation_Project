using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawPanelController : MonoBehaviour
{
    public static GameObject panel_draw;
    // Start is called before the first frame update
    void Start()
    {
        panel_draw = GameObject.Find("Panel_Draw");
        panel_draw.GetComponent<Renderer>().enabled = true;

    }

    // Update is called once per frame
    void Update()
    {

    }
}