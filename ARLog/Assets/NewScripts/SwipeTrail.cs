using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeTrail : MonoBehaviour
{
    public static GameObject swipmanager;

    public GameObject trailPrefab;
    GameObject thisTrail;
    Vector3 startPos;
    private Camera ARCamera;

    // Start is called before the first frame update
    void Start()
    {
        swipmanager = GameObject.Find("SwipeManager");
    }

    // Update is called once per frame
    void Update()
    {
        ARCamera = GameObject.Find("ARCamera").GetComponent<Camera>();
        //Debug.Log("Point: ");
        if (((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0)))
        {
            Plane objPlane = new Plane(ARCamera.transform.forward * -1, this.transform.position);
            Ray mRay = ARCamera.ScreenPointToRay(Input.mousePosition);

            float rayDistance;
            if (objPlane.Raycast(mRay, out rayDistance))
            {
                //mousePosition부터 시작
                thisTrail = (GameObject)Instantiate(trailPrefab, mRay.GetPoint(rayDistance), Quaternion.identity);
                
                startPos = mRay.GetPoint(rayDistance);
                //var pos = thisTrail.transform.position;
                //Debug.Log("Point: " + startPos);
            }
            else
            {
                thisTrail = (GameObject)Instantiate(trailPrefab, -mRay.GetPoint(rayDistance), Quaternion.identity);

                startPos = -mRay.GetPoint(rayDistance);
                Debug.Log("Else " + objPlane.Raycast(mRay, out rayDistance));
            }
        }
        else if(((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0)))
        {
            Plane objPlane = new Plane(ARCamera.transform.forward * -1, this.transform.position);
            Ray mRay = ARCamera.ScreenPointToRay(Input.mousePosition);
            float rayDistance;
            if (objPlane.Raycast(mRay, out rayDistance))
            {
                thisTrail.transform.position = mRay.GetPoint(rayDistance);
                //var pos = thisTrail.transform.position;
                //Debug.Log("Point: " + pos);
            }
            else
            {
                thisTrail.transform.position = -mRay.GetPoint(rayDistance);
            }
        }else if(((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) || Input.GetMouseButtonUp(0)))
        {
            if(Vector3.Distance(thisTrail.transform.position, startPos) < 0.1)
            {
                Destroy(thisTrail);
            }
        }
    }
}
