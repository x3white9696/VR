using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using VRStandardAssets.Utils;

public class AccelerometerTest : MonoBehaviour
{
    [SerializeField] private Transform m_Camera;
    [SerializeField] private VRInput m_VRInput;
    public Text Debuggingtext;
    public Text BackDebuggingtext;
    public bool LookingUp;
    Vector3 acceleration;

	// Use this for initialization
	void Start () {
        BackDebuggingtext.text = "";
        Debuggingtext.text = "";
        acceleration = Vector3.zero;
        LookingUp = false;
	}

    private void OnEnable()
    {
        m_VRInput.OnSwipe += CheckSwipe;
    }


    private void OnDisable()
    {
        m_VRInput.OnSwipe -= CheckSwipe;
    }

    private void CheckSwipe(VRInput.SwipeDirection SwipeDir)
    {
        /*
        switch(SwipeDir)
        {
            case VRInput.SwipeDirection.UP:
                {
                    transform.Translate(-transform.right);
                    break;
                }
            case VRInput.SwipeDirection.DOWN:
                {
                    transform.Translate(transform.right);
                    break;
                }
            case VRInput.SwipeDirection.LEFT:
                {
                    transform.Translate(-transform.forward);
                    break;
                }
            case VRInput.SwipeDirection.RIGHT:
                {
                    transform.Translate(transform.forward);
                    break;
                }
        }*/
    }

	// Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            transform.Translate(0.0f, 0.0f, 1.0f);
        if (Input.GetKeyDown(KeyCode.L))
            transform.Translate(0.0f, 0.0f, -1.0f);
        if (Input.GetKeyDown(KeyCode.DownArrow))
            transform.Translate(0.0f, -1.0f, 0.0f);
        if (Input.GetKeyDown(KeyCode.UpArrow))
            transform.Translate(0.0f, 1.0f, 0.0f);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            transform.Translate(-1.0f, 0.0f, 0.0f);
        if (Input.GetKeyDown(KeyCode.RightArrow))
            transform.Translate(1.0f, 0.0f, 0.0f);
        

       // acceleration += Input.acceleration;

        if (Input.accelerationEventCount > 0)
        {
            foreach (AccelerationEvent accEvent in Input.accelerationEvents)
            {
                acceleration += accEvent.acceleration * accEvent.deltaTime;
                //transform.Translate(0.0f, 0.0f, acceleration.z);
            }
            acceleration.Normalize();

            if (acceleration.z > 0.2 && acceleration.z < 0.6 && LookingUp == false)
            {
                //transform.Translate(transform.forward);
                LookingUp = true;
            }
            else if (acceleration.z < 0.2 && LookingUp == true)
            {
                LookingUp = false;
            }
            BackDebuggingtext.text = "acceleration: " + acceleration.x.ToString() + ", " + acceleration.y.ToString() + ", " + acceleration.z.ToString() +
                           "\n Camera: " + m_Camera.position.x.ToString() + ", " + m_Camera.position.y.ToString() + ", " + m_Camera.position.z.ToString();

            Debuggingtext.text = "acceleration: " + acceleration.x.ToString() + ", " + acceleration.y.ToString() + ", " + acceleration.z.ToString() +
                           "\n Camera: " + m_Camera.position.x.ToString() + ", " + m_Camera.position.y.ToString() + ", " + m_Camera.position.z.ToString();
            //transform.Translate(0.0f, 0.0f, acceleration.z);
            acceleration = Vector3.zero;
        }
        //m_Camera.Translate(acceleration.x, 0.0f, acceleration.z);

        //transform.Translate(acceleration.x, 0.0f, acceleration.z);
         
        //Debuggingtext.text = "acceleration: " + Input.acceleration.x.ToString() + ", " + Input.acceleration.y.ToString() + ", " + Input.acceleration.z.ToString() +
        //                    "\n Camera: " + m_Camera.position.x.ToString() + ", " + m_Camera.position.y.ToString() + ", " + m_Camera.position.z.ToString();

        //Vector3 acceleration = Vector3.zero;
        //if (Input.accelerationEventCount > 0)
        //{
        //    foreach (AccelerationEvent accEvent in Input.accelerationEvents)
        //    {
        //        acceleration += accEvent.acceleration;

        //        if (acceleration.sqrMagnitude > 1)
        //            acceleration.Normalize();

        //        //acceleration *= Time.deltaTime;

        //        m_Camera.Translate(acceleration.x, 0.0f, acceleration.z);
        //        Debuggingtext.text = "acceleration: " + acceleration.x.ToString() + ", " + acceleration.y.ToString() + ", " + acceleration.z.ToString() +
        //                            "\n Camera: " + m_Camera.position.x.ToString() + ", " + m_Camera.position.y.ToString() + ", " + m_Camera.position.z.ToString();
        //    }
        //}
    }
    /* if (Input.accelerationEventCount > 0) {
            Vector3 acceleration = Vector3.zero;
            foreach (AccelerationEvent accEvent in Input.accelerationEvents) {
                acceleration += accEvent.acceleration * accEvent.deltaTime;
                m_Camera.Translate(acceleration.x * Speed, 0.0f, -acceleration.z * Speed);
                Debuggingtext.text = "acceleration: " + acceleration.x.ToString() + ", " + acceleration.y.ToString() + ", " + acceleration.z.ToString() +
                                    "\n Camera: " + m_Camera.position.x.ToString() + ", " + m_Camera.position.y.ToString() + ", " + m_Camera.position.z.ToString();
            }
        }*/
    /*   if (Input.GetKeyDown(KeyCode.DownArrow))
           m_Camera.Translate(0.0f, -1.0f, 0.0f);
       if (Input.GetKeyDown(KeyCode.UpArrow))
           m_Camera.Translate(0.0f, 1.0f, 0.0f);
       if (Input.GetKeyDown(KeyCode.LeftArrow))
           m_Camera.Translate(-1.0f, 0.0f, 0.0f);
       if (Input.GetKeyDown(KeyCode.RightArrow))
           m_Camera.Translate(1.0f, 0.0f, 0.0f);
       */

    //if (Input.accelerationEventCount > 0) {
    //        int i = 0;
    //        Vector3 acceleration = Vector3.zero;
    //        while (i < Input.accelerationEventCount) {
    //            AccelerationEvent accEvent = Input.GetAccelerationEvent(i);
    //            acceleration += accEvent.acceleration * accEvent.deltaTime;
    //            ++i;
    //            transform.Translate(acceleration.x, 0, -acceleration.y);
    //        }
    //    }

    //    if (Input.accelerationEventCount > 0) {
    //        Vector3 acceleration = Vector3.zero;
    //        foreach (AccelerationEvent accEvent in Input.accelerationEvents) {
    //            acceleration += accEvent.acceleration * accEvent.deltaTime;
    //            transform.Translate(acceleration.x, 0, -acceleration.y);
    //        }
    //    }
}
