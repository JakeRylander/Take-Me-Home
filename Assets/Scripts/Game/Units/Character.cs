using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Unit {
	
	// Use this for initialization
	void Start () {
		
		//Starting Unit Information
		Movement_Range = 1;
		Movement_Costs = new Movement_Costs(1,1,1,1,null,null,null,null,1,1,null); //Move to a Set and Get built into Movement_Type since they go hand in hand.
	
		Current_Stam = Max_Stam;
		Walkable = new List<TileType> {TileType.Ground, TileType.Forest,TileType.Food, TileType.Road, TileType.Rough,TileType.Bridge2, TileType.Bridge1, TileType.Building};
	
		//Position Set based on where it was placed.
		Position = new Vector2Int((int)(transform.position.x - 0.5),(int)(transform.position.z - 0.5));
		
		//Stores Color Values
		Set_Color_Values();
		
		//Sets the Color to the Material
		Set_Color(color_renderer, Base_Color);
		
		//Animation Stuff
		animation_name = "Mech_";
		animation_zoom = "Default";
	
		//PLay Idle Animation
		Play_Animation("Idle_");
	}
}