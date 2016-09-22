using UnityEngine;
using System.Collections;

public class BlackColor : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.GetComponent<Renderer>().material.color = Color.white;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
