using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textscript : MonoBehaviour
{
    public Text ScriptTxt;

    // Start is called before the first frame update
    void Start()
    {
        ScriptTxt.text = "탐색된 타겟이 없습니다";    
    }

    // Update is called once per frame
    void Update()
    {
        ScriptTxt.text = CloudRecoEventHandler.targetname;
    }
}
