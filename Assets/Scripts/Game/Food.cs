using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {
	
	void OnCollisionEnter (Collision col)
     {
        Destroy(gameObject);
	 }


	void OnDestroy () {
     Destroy(transform.parent.gameObject);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
