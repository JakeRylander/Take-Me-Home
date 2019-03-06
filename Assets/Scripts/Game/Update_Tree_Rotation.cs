using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Update_Tree_Rotation : MonoBehaviour {

	private Quaternion New_Rotation;
	//Set Position Rotation based on Camera
	public void Position_Set(int position){
		
		switch (position)
		{
			case 0: 
				New_Rotation = Quaternion.Euler(0,45,0);
				break;
			case 1:
				New_Rotation = Quaternion.Euler(0,0,0);
				break;
			case 2:
				New_Rotation = Quaternion.Euler(0,-45,0);
				break;
			case 3:
				New_Rotation = Quaternion.Euler(0,-90,0);
				break;
			case 4:
				New_Rotation = Quaternion.Euler(0,-135,0);
				break;
			case 5:
				New_Rotation = Quaternion.Euler(0,-180,0);
				break;
			case 6:
				New_Rotation = Quaternion.Euler(0,-225,0);
				break;
			case 7:
				New_Rotation = Quaternion.Euler(0,-270,0);
				break;
			default:
				//You should never end up here, if you do, something went really wrong.
				break;
		}
		transform.rotation = New_Rotation;
	}
}
