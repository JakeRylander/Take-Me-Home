using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Game_Loader : MonoBehaviour{ //Arrays which hold the Tile sets from which we build the map. In the future we can have different arrays for the "snow" and other terrain templates :D
	
	//External Game Data Component References
	public State_Handler state;
	public Game_Data data;
	
	//References to the Tilemaps in which to place the ctual Visual Tiles
	public Tilemap Ground;
	public Tilemap Terrain;
	public GameObject Forest_Parent;
	
	public Tilemap South;
	public Tilemap North;
	public Tilemap East;
	public Tilemap West;
	
	//References to the Actual Visual Tiles to place
	public Tile Ground_Tile;
	public GameObject Forest_Prefab;
	public RuleTile Road_Tile;
	public RuleTile River_Tile;
	public Tile Marsh_Tile;
	public GameObject Grass_Prefab;
	
	public Tile Iso_Tile;
	public AnimatedTile HQ_Prefab;
	
	public Tile Bridge_Ver;
	public Tile Bridge_Hor;
	
	public GameObject Food_Prefab;
	
	private void Generate_Iso(Vector2Int size){
		
		West.transform.position = new Vector3(size.x, 0, 0);
		North.transform.position = new Vector3(0,0,size.y);
		
		for (int x = 0 ; x < size.x; x++){
			Vector3Int localPlace = (new Vector3Int(x, 0, (int)Ground.transform.position.y));
			South.SetTile(localPlace,Iso_Tile);
		}
		for (int y = 0 ; y < size.y; y++){
			Vector3Int localPlace = (new Vector3Int(0, y, (int)Ground.transform.position.y));
			East.SetTile(localPlace,Iso_Tile);
		}
		for (int y = 0 ; y < size.y; y++){
			Vector3Int localPlace = (new Vector3Int(0, y, (int)Ground.transform.position.y));
			West.SetTile(localPlace,Iso_Tile);
		}
		for (int x = 0 ; x < size.x; x++){
			Vector3Int localPlace = (new Vector3Int(x, 0, (int)Ground.transform.position.y));
			North.SetTile(localPlace,Iso_Tile);
		}
	}

	void Generate_Map(Map_Tile[,] map, Vector2Int size){
		
		int x_size = size.x;
		int y_size = size.y;
		
		Map_Tile tile_to_place;
		
		Generate_Iso(size);
	
		//Generate Map
		for (int y = 0; y < y_size; y++){
            for (int x = 0; x < x_size; x++){
				
                Vector3Int localPlace = (new Vector3Int(x, y, (int)Ground.transform.position.y));

				tile_to_place = map[y,x]; //Don't ask why it's inverted, 2D Array lists are a meme and I'm a baddy.
				
				if (tile_to_place.Type == TileType.Ground){
					
					//TO DO: Random Ground Tile with prefab decal on top
					Ground.SetTile(localPlace,Ground_Tile);
					
					Vector3 position = new Vector3(x, 0, y);
					
					Vector3 rotation = new Vector3(0f, 0f, 0f);
					var new_grass = Instantiate(Grass_Prefab, position, Quaternion.Euler(rotation));
					new_grass.transform.parent = Forest_Parent.transform;
				}

				else if (tile_to_place.Type == TileType.Forest){
					
					Ground.SetTile(localPlace,Ground_Tile);
					Vector3 position = new Vector3(x, 0, y);
					
					Vector3 rotation = new Vector3(0f, 0f, 0f);
					var new_forest = Instantiate(Forest_Prefab, position, Quaternion.Euler(rotation));
					new_forest.transform.parent = Forest_Parent.transform;
					
				}
				
				else if (tile_to_place.Type == TileType.Road){
					
					Ground.SetTile(localPlace,Ground_Tile);
					Terrain.SetTile(localPlace,Road_Tile);
					
				}
				
				else if (tile_to_place.Type == TileType.River){
					Ground.SetTile(localPlace,River_Tile);
					
				}
				else if (tile_to_place.Type == TileType.Rough){
					Ground.SetTile(localPlace,Marsh_Tile);
				}
				else if (tile_to_place.Type == TileType.Bridge1){
					Terrain.SetTile(localPlace,Bridge_Hor);
					Ground.SetTile(localPlace,River_Tile);
				}
				else if (tile_to_place.Type == TileType.Bridge2){
					Terrain.SetTile(localPlace,Bridge_Ver);
					Ground.SetTile(localPlace,River_Tile);
				}
				else if (tile_to_place.Type == TileType.Food){
					Ground.SetTile(localPlace,Ground_Tile);
					Vector3 position = new Vector3(x, 0, y);
					Vector3 rotation = new Vector3(0f, 0f, 0f);
					var new_food = Instantiate(Food_Prefab, position, Quaternion.Euler(rotation));
					new_food.transform.parent = Forest_Parent.transform;
					
					data.Food.Add(new_food);
					data.Food_Pos.Add(new Vector2Int(x,y));
				}
				else if (tile_to_place.Type == TileType.Building){
					Ground.SetTile(localPlace,Ground_Tile);
				}
            }
        }
	}
	
	//void Set_Sun_Parameters(int x, int y){
	
		//Sun.transform.position = new Vector3(-5f,x,-5f);
		//Sun_Data.range = x * 3;
		//Sun_Data.intensity = Sun_Data.range/10;
	
	//}
	
	void Awake(){
		
		data.stam = 20;
		
		//Set Active Map
		data.Set_Active_Map(Map_List.test_map_1);
		
		//Call Function to Place Tiles into Map
		Generate_Map(data.Map, data.Map_Size);
		
	}
	
	// Use this for initialization
	void Start () {
		
		Debug.Log("Map Generated and stuff");
		
		
		//Set Light and Camera Initial Position
		//Set_Sun_Parameters(x_size,y_size);
	}
}
