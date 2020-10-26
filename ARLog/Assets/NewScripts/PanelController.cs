using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    public static GameObject panel;
    private GameObject button_drawing;
    // Start is called before the first frame update
    void Start()
    {
        panel = GameObject.Find("Panel");
        button_drawing = GameObject.Find("Button_drawing");
        SwipeTrail.swipmanager.SetActive(false);   //그리기 비활성화
    }

    // Update is called once per frame
    void Update()
    {
        
        if (CloudRecoEventHandler.IsTargetRecognized)
        {
            //DrawPanelController.panel_draw.SetActive(true);
            //panel_draw.GetComponent<Renderer>().enabled = true;
            button_drawing.SetActive(true);
            button_drawing.GetComponent<Renderer>().enabled = true;

        }
        else
        {
            //DrawPanelController.panel_draw.SetActive(false);
            //panel_draw.GetComponent<Renderer>().enabled = false;
            button_drawing.SetActive(false);
        }
        
    }
}
