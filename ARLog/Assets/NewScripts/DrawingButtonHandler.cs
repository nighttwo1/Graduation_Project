using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingButtonHandler : MonoBehaviour
{

    public static int button_count = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickB()
    {
        button_count += 1;
        SwipeTrail.swipmanager.SetActive(true);
        RecogButtonScript.recogBtn.SetActive(false);
        SaveButtonScript.button_drawing_save.SetActive(true);

    }
}
