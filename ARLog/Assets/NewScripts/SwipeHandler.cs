using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeHandler : MonoBehaviour
{
    private Camera ARCamera;

    // Start is called before the first frame update
    void Start()
    {
        ARCamera = GameObject.Find("ARCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0)))
        {
            Plane objPlane = new Plane(ARCamera.transform.forward * -1, this.transform.position);
            Ray mRay = ARCamera.ScreenPointToRay(Input.mousePosition);
            float rayDistance;
            if (objPlane.Raycast(mRay, out rayDistance))
            {
                this.transform.position = mRay.GetPoint(rayDistance);
            }
        }
    }
}
