using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTest2 : MonoBehaviour
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
    }
}
