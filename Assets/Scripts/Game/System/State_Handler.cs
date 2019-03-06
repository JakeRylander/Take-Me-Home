using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using UnityEngine.Rendering.PostProcessing;

public class State_Handler : MonoBehaviour {
	
	//Reference to Map Data Script
	public Game_Data data;
	
	//Reference to Grid
	public Grid World_Grid;
	
	public GameObject video1;
	
	public GameObject OverlayUI;
	
	private bool finish;
	
	public ParticleSystem[] rain_effects;
	
	public GameObject Fireworks;
	
	public GameObject Howard;
	
	public UI ui_script;
	
	public AudioSource voice1;
	
	public AudioSource Music;
	public AudioSource Finale;
	
	public AudioSource Menu;
	
	public AudioSource Final_Voice;
	
	public AudioSource Rain;
	public AudioSource Crickets;
	
	public AudioSource Walk;
	public AudioSource Fireworks_SFX;
	public AudioSource Beep;
	
	private float Time_Passed;
	private bool Not_Done;
	public float Move_Duration;
	private bool Enviroment_Changing;
	
	public Light ambient_light;
	
	public Light unit_light;
	
	public GameObject Sun;
	
	//State Value
	public int State {set; get;}
	public int Previous_State {set; get;}
	
	//Camera Values
	private int Camera_Position;
	private bool Zoomed;
	
	//Selected Overlay Data
	public Tilemap Overlay;
	public Tile[] Overlay_Tile;
	
	//Unit Handler Stuff
	private GameObject Selected_Unit;
	private Unit Selected_Unit_Script;
	
	/**UI References**/ 
	//Lists for Moveable Spaces
	private List<Vector2Int> Walkable_Spaces = new List<Vector2Int>();
	
	//Previous Position for backpedalling.
	private Vector2Int Previous_Position;
	
	//Global Accesible Variable used by the various PathFinding and Range Calculator algorithms
	private Vector3Int place;
	private Vector2Int to_position;
	
	//Postproccess stuff
	public PostProcessVolume volume;
	ColorGrading     colorGradingLayer     = null;
	Grain grainLayer = null;
	Bloom bloomlayer = null;
	DepthOfField depthlayer = null;
	
	//Check if Unit can Walk on this Tile
	private bool CanWalk(Vector2Int position){
		foreach(var item in Selected_Unit_Script.Walkable){
			Map_Tile tile_to_check = data.Get_Tile(position);
			if (item == tile_to_check.Type){
				return true;
			}
		}
		return false;
	}
    IEnumerator Example(float time, int state)
    {
        yield return new WaitForSecondsRealtime(time);
		State = state;
    }
	
	//Calculate Path Possible and Displays the Overlay for walkable area
	//Function to call and Base case
	private void GetValidMoves(Vector2Int position, float? Movement){
		//add the tile unit curently on.
		place = new Vector3Int(position.x, position.y, (int)Overlay.transform.position.y);
		Overlay.SetTile(place,Overlay_Tile[0]);
		Walkable_Spaces.Add(position);
		
		GetValidMoves_Rec(position, Movement);
	}
	//Recursive Part
	private void GetValidMoves_Rec(Vector2Int position, float? Movement){
		
		if (!(Movement < 0)){			
			for (int i = 0; i < 4; i++){
				
				switch (i){ 
					case 0: //Checks Top Tile from Base
						to_position = position + Vector2Int.up;
						place = new Vector3Int(position.x, position.y+1, (int)Overlay.transform.position.y);
						break;
					case 1: //Checks Left Tile from Base
						to_position = position + Vector2Int.left;
						place = new Vector3Int(position.x-1,position.y, (int)Overlay.transform.position.y);
						break;
					case 2: //Checks Right Tile from Base
						to_position = position + Vector2Int.right;
						place = new Vector3Int(position.x+1,position.y, (int)Overlay.transform.position.y);
						break;
					case 3: //Checks Bottom Tile from Base
						to_position = position + Vector2Int.down;
						place = new Vector3Int(position.x,position.y-1, (int)Overlay.transform.position.y);
						break;
					default: //Never should end up here
						break;
				}
				//Checks if The Tile within the map. As too not show movement outside the game confines
				if (data.Within_Map(to_position)){
					var Movement_Cost = Selected_Unit_Script.Movement_Costs.Get_Cost(data.Get_Tile(to_position).Type); //Calcuate Movement Cost based on the Unit Movement Type
					if(CanWalk(to_position) & (Movement >= Movement_Cost)){ //Check if unit can legally walk into the tile and if it has enough movement left to do so
						
						if(!(Walkable_Spaces.Contains(to_position))){
							Walkable_Spaces.Add(to_position);
							Overlay.SetTile(place,Overlay_Tile[0]);
							GetValidMoves_Rec(to_position, Movement - Movement_Cost);
						}
					}
				}
			}
		}
	}
	
	//Selected Unit Handler
	public void Set_Selected(GameObject Unit){
		
		if (Unit == null){
			Selected_Unit = Unit;
			Overlay.ClearAllTiles();
			Walkable_Spaces.Clear();
		}
		else{
			
			Selected_Unit = Unit;
			Selected_Unit_Script = Unit.GetComponent<Unit> ();

			//Take Unit Movement and create the moveable area and the list with the coordinated it can move into.
			GetValidMoves(Selected_Unit_Script.Position, Selected_Unit_Script.Movement_Range);
	
			State = 2;
		}
	}
	
	//Zoom Setters and Getters
	public void Set_Zoomed(bool boolean){
		Zoomed = boolean;
	}
	
	public bool Get_Zoomed(){
		return Zoomed;
	}
	
	private void Check_Menu(){
		if (Input.GetKeyDown(KeyCode.Escape) & State == 0){
			State = 6;
		}
		else if (Input.GetKeyDown(KeyCode.Escape)){
			State = 0;
		}
	}
	
	private void Check_Unit_Menu(){
		if (Input.GetMouseButtonDown(1) & State == 8){
			State = 3;
			Selected_Unit_Script.Move_To(Previous_Position);
			print("unit deselected");
			print(Selected_Unit_Script);
		}
	}
	
	private void Check_Change_Camera_Position(int state){
		if(Input.GetKeyDown("a")){
			Change_Camera_Position("right");
			Previous_State = state;
		}
		else if (Input.GetKeyDown("d")){
			Change_Camera_Position("left");
			Previous_State = state;
		}
		else if (Input.GetKeyDown("s") & Zoomed){
			Change_Camera_Zoom("unzoom");
			Previous_State = state;
		}
		else if (Input.GetKeyDown("w") & !Zoomed){
			Change_Camera_Zoom("zoom");
			Previous_State = state;
		}
	}
	
	private void Change_Camera_Zoom(string zoom_value){
		
		if (zoom_value == "unzoom"){
			Zoomed = false;
		}
		else{
			Zoomed = true;
		}
		State = 1;
		BroadcastMessage("Update_Zoom", Zoomed);
	}
	
	private void Change_Camera_Position(string side){
		
		if (side == "left"){
			if (Camera_Position == 0){
				Camera_Position = 7;
			}
			else{
				Camera_Position = Camera_Position - 1;
			}
		}
		else if(side == "right"){
			if (Camera_Position == 7){
				Camera_Position = 0;
			}
			else{
				Camera_Position = Camera_Position + 1;
			}
		}
		State = 1;
		BroadcastMessage("Position_Set", Camera_Position);
		//Do the same for units, buildings, etc so they know to update
	}
	
	
	private void Change_Camera_Position(int side){
		
		Camera_Position = side;
		State = 1;
		BroadcastMessage("Position_Set", Camera_Position);
		//Do the same for units, buildings, etc so they know to update
	}
	
	// Use this for initialization
	void Start () {
		
		State = 6;
		Camera_Position = 0;
		Zoomed = false;
		//State = 0 | Idle (AKA Moving through the Map, no Unit clicked, nothing, base state that goes to very other.)
		//State = 1 | Camera either changing orientation or changing zoom level.
		//State = 2 | Unit Clicked/Selected (Prevent Selection of any other unit, buildings, menu, etc.)
		//State = 3 | Unit Selected and Waiting for Target.
		//State = 4 | Unit Ended Turn on Space.
		//State = 5 | Target Selected (Get whats at the tile I.E. if Unit (Enemy or Friendly), Building, Etc.
		//State = 6 | Main Menu Open
		//State = 7 | Turn Over (Also checks if a full cycle done to refresh fuel, units, etc.)
		//State = 8 | Unit moved and opens menu to select action.
		//State = 9 | Selected to Attack something (Displays attack range and hide unit menu) And move to state 10
		//State = 10 | Waits to select attack target or backpedal to 8.
		volume.profile.TryGetSettings(out grainLayer);
		volume.profile.TryGetSettings(out colorGradingLayer);
		volume.profile.TryGetSettings(out bloomlayer);
		volume.profile.TryGetSettings(out depthlayer);
		Menu.Play();
		Rain.Play();
		Crickets.Play();
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (State == 0){
			Check_Change_Camera_Position(State);
			Check_Menu();
		}
		
		else if (State == 6){
			Check_Menu();
		}
		
		else if (State == 2){
			//Unit has been Selected Waiting to Select Target
			State = 3;
		}
		else if (State == 3){
			Check_Change_Camera_Position(State);
			if (Input.GetMouseButtonDown(0)){
				
				//Shenanigans to see what spot on the map we clicked with the mouse
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				Vector3 worldPoint = ray.GetPoint(-ray.origin.y / ray.direction.y);
				
				Vector2Int position = new Vector2Int((int)worldPoint.x, (int)worldPoint.z);
				//Check if where clicked is the same unit already selected (Special abilities I.E. Rig build temp airport)
				//Check if within movement Range.
				if (Walkable_Spaces.Contains(position)){
					
						
					//Move Unit to that position
					//var tile = data.Get_Tile(position.x, position.y);
					Previous_Position = Selected_Unit_Script.Position;
					Selected_Unit_Script.Move_To(position);
					
					if (data.Get_Tile(position).Type == TileType.Building){
						State = 5; //Game Ending Start
						Selected_Unit_Script.Play_Animation("Idle_");
						Set_Selected(null);
					}
					
					else if (data.Get_Tile(position).Type == TileType.Food){
						
						var index = data.IsFoodAt(position);
						
						var food = data.GetFoodAt(index);
						
						Destroy(food);
						
						//Update Current Stamina Left;
						data.stam = data.stam + 10;
						Beep.Play();
						
						
					}
					
					else{
						var consumed = Mathf.Abs(position.x - Previous_Position.x) + Mathf.Abs(position.y - Previous_Position.y);
						
						if (data.Get_Tile(position).Type == TileType.Forest){
							consumed = consumed + 3;
						}
						
						data.stam = data.stam - consumed;
						Walk.Play();
					}
				}
			}
		}
		
		else if (State == 5){
			Enviroment_Changing = true;
			Time_Passed = 0f;
			Not_Done = true;
			State = 12;
			Sun.SetActive(true);
			Finale.Play();
			Howard.SetActive(true);
		}
		
		else if (State == 12){
			if(Enviroment_Changing){
				
				Time_Passed += Time.deltaTime / Move_Duration;

				colorGradingLayer.saturation.value = Mathf.Lerp(-55.5f, 0.0f, Time_Passed);
				grainLayer.intensity.value = Mathf.Lerp(0.8f, 0.0f, Time_Passed);
				grainLayer.size.value = Mathf.Lerp(1.2f, 0.0f, Time_Passed);
				ambient_light.intensity = Mathf.Lerp(0.0f, 3.0f, Time_Passed);
				unit_light.intensity = Mathf.Lerp(2.5f, 0.0f, Time_Passed);
				
				var ho = new Vector3 (17f,20f,17f);
				var hn = new Vector3 (17f,4f,17f);
				
				Howard.transform.position = Vector3.Lerp(ho, hn, Time_Passed);
				
				var n = new Vector3 (50.0f,-100.0f,150.0f);
				var o = new Vector3 (50.0f,50.0f,150.0f); 
				
				Sun.transform.position = Vector3.Lerp(n, o, Time_Passed);
				
				Finale.volume = Mathf.Lerp(0.0f, 1.0f, Time_Passed);
				Music.volume = Mathf.Lerp(1.0f, 0.0f, Time_Passed);
				Rain.volume = Mathf.Lerp(1.0f, 0.0f, Time_Passed);
				Crickets.volume = Mathf.Lerp(1.0f, 0.0f, Time_Passed);
				
				var rain_value = Mathf.Lerp(100.0f, 0.0f, Time_Passed);
				
				foreach (var rain in rain_effects){
					var em = rain.emission;
					var rate = em.rate;
					rate.constantMin = rain_value;
					rate.constantMax = rain_value;
					em.rate = rate;
				}
				/*
				var nc = new Color(188.0f,192.0f,233.0f);
				var oc = new Color(54.0f,58.0f,66.0f);
				RenderSettings.ambientLight = Color.Lerp(oc, nc, Time_Passed);
				*/
				/*
				LeftBound = Mathf.Lerp(Current_LeftBound, New_LeftBound, Time_Passed);
				RightBound = Mathf.Lerp(Current_RightBound, New_RightBound, Time_Passed);
				BottomBound = Mathf.Lerp(Current_BottomBound, New_BottomBound, Time_Passed);
				TopBound = Mathf.Lerp(Current_TopBound, New_TopBound, Time_Passed);
				*/
				
				if (Time_Passed > 1.0f) {
					Enviroment_Changing = false;
					print("enviroment done");
					State = 13;
				}
			}
			
		}
		
		else if (State == 8){
			Set_Selected(null);
			ui_script.Fade(true);
			StartCoroutine(Example(2,9));
			voice1.Play();
		}
		
		else if (State == 9){
			StartCoroutine(Example(5,10));
		}
		
		else if (State == 11){
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		
		else if (State == 13){
			Final_Voice.Play();
			Fireworks_SFX.Play();

			Vector3 position = new Vector3(20.0f, 0, 20.0f);

			Vector3 rotation = new Vector3(-90.0f, 0f, 0.0f);
			Instantiate(Fireworks, position, Quaternion.Euler(rotation));
			
			 position = new Vector3(15.0f, 0, 20.0f);

			Instantiate(Fireworks, position, Quaternion.Euler(rotation));
			
			 position = new Vector3(20.0f, 0, 15.0f);

			Instantiate(Fireworks, position, Quaternion.Euler(rotation));
			
			State = 14;
			StartCoroutine(Example(30,15));
		}
		
		else if (State == 15){

			State = 16;
			ui_script.Fade(true);
			finish = true;
			Time_Passed = 0f;
			Not_Done = true;
			print("Play meme video");
		}
		else if (State == 16){
			
			if(finish){
				
				Time_Passed += Time.deltaTime / 5.0f;
				
				//Finale.volume = Mathf.Lerp(1.0f, 0.0f, Time_Passed);
				Fireworks_SFX.volume = Mathf.Lerp(1.0f, 0.0f, Time_Passed);

				
				if (Time_Passed > 1.0f) {
					finish = false;
					print("enviroment done");
					State = 17;
				}
			}
		}
		else if (State ==17){
			Sun.SetActive(false);
			StartCoroutine(Example(5,18));
		}
		
		else if (State == 18){
			video1.SetActive(true);
			Destroy(volume);
			StartCoroutine(Example(1.0f,20));
			State = 19;
			
		}
		else if (State == 20){
			OverlayUI.SetActive(false);
		}
	}
}
