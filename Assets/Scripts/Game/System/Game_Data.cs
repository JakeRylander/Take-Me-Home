using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Game_Data : MonoBehaviour {

	//Refernce to State Handler
	public State_Handler state;
	
	//Refernce to Displayed Day
	public Text Stamina;
	
	//Use to Reference Units
	private Unit unit_script;
	
	/**Map Data**/
	//Size
	public Vector2Int Map_Size {get; set;}
	//Tile Data
	public Map_Tile[,] Map {get; set;}
	
	public List<GameObject> Food = new List<GameObject>();
	public List<Vector2Int> Food_Pos = new List<Vector2Int>();
	
	//Current Turn
	//Actual Variable
	private int day_number;
	//Special Handler to also update Displayed Day.
	public int stam {get{return day_number;} set{day_number = value; Stamina.text = "Stamina " + value.ToString(); if (value <= 0){state.State = 8;}else{state.State = 4;}}}
	
	//Max Players;
	public int Max_Players {get; set;}

	//Tile Get
	public Map_Tile Get_Tile(Vector2Int position){
		return Map[position.y,position.x];
	}
	
	public void Set_Active_Map(Map_Tile[,] map){
		
		Map = map;
		Map_Size = new Vector2Int (map.GetLength(0), map.GetLength(1));
		
	}
	
	public bool Within_Map(Vector2Int position){
		return ((position.x >= 0) & (position.x < Map_Size.x) & (position.y >= 0) & (position.y < Map_Size.y));
	}
	
	public int IsFoodAt(Vector2Int pos){
		int i = 0;
		foreach (var food in Food_Pos){
			
			if(Food_Pos[i] == pos){
				return i;
			}
			i++;
		}
		return -1;
	}
	
	
	public GameObject GetFoodAt (int i){
		return Food[i];
	}
}
