using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerToBlocks : MonoBehaviour {

    public bool m_PlayerLockOn;
    public Vector3 m_PlayerLockOnPosition;
    public float m_PlayerLockOnDelay;
    public float m_PlayerLockOnCoolDown;
    public Text PlayerDebugger;

    void Awake()
    {
        m_PlayerLockOnPosition = Vector3.zero;
    }

	// Use this for initialization
	void Start () 
    {
        Cursor.visible = false;
        m_PlayerLockOn = false;
        m_PlayerLockOnDelay = 1.0f;
        m_PlayerLockOnCoolDown = 20.0f;
        PlayerDebugger.text = "";
	}
	
	// Update is called once per frame
	void Update () {
        PlayerDebugger.text = "cooldown" + m_PlayerLockOnCoolDown.ToString();
	    if(m_PlayerLockOn == true)
        {
            m_PlayerLockOnCoolDown -= Time.deltaTime;
            if(m_PlayerLockOnCoolDown < 0.0f)
            {
                PlayerReset();
            }
        }
	}

    public void PlayerReset()
    {
        m_PlayerLockOn = false;
        m_PlayerLockOnCoolDown = 20.0f;
    }
}
