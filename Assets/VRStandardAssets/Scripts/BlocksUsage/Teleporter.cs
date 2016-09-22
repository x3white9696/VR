using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using VRStandardAssets.Utils;

public class Teleporter : MonoBehaviour {

    [SerializeField]    private VRInteractiveItem m_InteractiveItem;       // Reference to the VRInteractiveItem to determine when to lock on.
    [SerializeField]    private GameObject m_Camera;                       // PlayerReference

    private float TeleportCoolDown;
    public bool HaveTeleportOnce;
    public GameObject NextTeleporterGO;
    public bool Activation;

    void Awake() {
        Activation = true;
    }


	// Use this for initialization
	void Start () {
        TeleportCoolDown = 5.0f;
        HaveTeleportOnce = false;
	}

	// Update is called once per frame
	void Update () {

        if(Activation == true)
        {
            if (m_Camera.transform.position.x > m_InteractiveItem.transform.position.x - m_InteractiveItem.transform.localScale.x * 1.0 && m_Camera.transform.position.x < m_InteractiveItem.transform.position.x + m_InteractiveItem.transform.localScale.x * 1.0 &&
            m_Camera.transform.position.z > m_InteractiveItem.transform.position.z - m_InteractiveItem.transform.localScale.z * 1.0 && m_Camera.transform.position.z < m_InteractiveItem.transform.position.z + m_InteractiveItem.transform.localScale.z * 1.0 &&
            m_Camera.transform.position.y > m_InteractiveItem.transform.position.y - m_InteractiveItem.transform.localScale.y * 1.0 && m_Camera.transform.position.y < m_InteractiveItem.transform.position.y + m_InteractiveItem.transform.localScale.y * 1.0 &&
            HaveTeleportOnce == false)
            {
                Teleporter NextTeleport = NextTeleporterGO.GetComponent<Teleporter>();
                m_Camera.transform.position = NextTeleporterGO.transform.position;
                //HaveTeleportOnce = true;
                m_Camera.GetComponent<PlayerToBlocks>().PlayerReset();
                NextTeleport.HaveTeleportOnce = true;
            }

            if (HaveTeleportOnce == true)
            {
                TeleportCoolDown -= Time.deltaTime;
                if (TeleportCoolDown < 0.0f)
                {
                    HaveTeleportOnce = false;
                    TeleportCoolDown = 5.0f;
                }
            }
        }
        
	}
}
