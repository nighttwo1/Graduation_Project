using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendering : MonoBehaviour
{
    LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetColors(Color.yellow, Color.white);
        lineRenderer.SetWidth(0.2F, 0.2F);
        lineRenderer.SetVertexCount(200);

        for(int i =0;i<PanelController.stored_drawings.Count;i++)
            lineRenderer.SetPosition(0, 4 * PanelController.stored_drawings[i]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
