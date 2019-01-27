using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if !UNITY_EDITOR_OSX
public class CameraController : MonoBehaviour
{
    public GameObject sub;
    GameManager gm;

    public float minFOV;
    public float maxFOV;

    [Range(1,10)]
    public float fovSensitivity =1.0f;

    [Range(1, 10)]
    public float camPOISensitivity = 1.0f;

    public float distToSub;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        float tempFOV = GetComponent<Camera>().fieldOfView;
        tempFOV -= ControllerManager.instance.statePlayer2.ThumbSticks.Right.Y*Time.deltaTime*fovSensitivity;
        tempFOV = Mathf.Clamp(tempFOV, minFOV, maxFOV);
        GetComponent<Camera>().fieldOfView = tempFOV;

        distToSub = Vector3.Distance(transform.position, sub.transform.position);
        transform.LookAt(sub.transform.position + new Vector3(sub.GetComponent<SubControl>().forwardSpeed + (ControllerManager.instance.statePlayer2.ThumbSticks.Right.X* camPOISensitivity), 0, 0));
        if (distToSub > 12.0f)
        {
            transform.parent = sub.transform;
        }
        else
        {
            transform.parent = null;
        }
    }

}
#endif