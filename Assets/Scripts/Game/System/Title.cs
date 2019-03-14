using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour {
	
	public void pressed (bool x){
		print("start");
		SceneManager.LoadScene("Game");
	}
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("enter")){
			print("start");}
		
	}
}
