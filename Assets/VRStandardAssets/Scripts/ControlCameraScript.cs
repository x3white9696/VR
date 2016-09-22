using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControlCameraScript : MonoBehaviour 
{
    [SerializeField]
    private GameObject m_Camera;



	void Start () 
    {
	}
	
	void Update () 
    {

        float x = 4 * Input.GetAxis("Mouse X"); 
        float y = 4 * Input.GetAxis("Mouse Y");

        //GameObject.Find("Canvas/Text X").GetComponent<Text>().text = "" + Input.GetAxis("Mouse X");
        //GameObject.Find("Canvas/Text Y").GetComponent<Text>().text = "" + Input.GetAxis("Mouse Y");

        m_Camera.transform.Rotate(y, x, 0);

        float z = m_Camera.transform.eulerAngles.z;

        m_Camera.transform.Rotate(0, 0, -z);
	}
}
