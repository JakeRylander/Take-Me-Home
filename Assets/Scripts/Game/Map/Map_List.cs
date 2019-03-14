using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_List {

	//Temperate Climate
	public static Map_Tile a = new Map_Tile("Grassland", "Test", TileType.Ground, 0);
	public static Map_Tile b = new Map_Tile("Forest", "Test", TileType.Forest, 0);
	public static Map_Tile c = new Map_Tile("Marsh", "Test", TileType.Rough,0);
	public static Map_Tile d = new Map_Tile("Road", "Test", TileType.Road,0);	
	public static Map_Tile e_1 = new Map_Tile("Bridge_Hor", "Test", TileType.Bridge1,0);
	public static Map_Tile e_2 = new Map_Tile("Bridge_Ver", "Test", TileType.Bridge2,0);
	public static Map_Tile f = new Map_Tile("River", "Test", TileType.River, 0);
	public static Map_Tile g = new Map_Tile("Food", "Test", TileType.Food, 0);
	public static Map_Tile h = new Map_Tile("House", "We win here", TileType.Building, 0);

	public static Map_Tile[,] test_map_1 = new Map_Tile[20,20]{
		
		{a,a,a,a,a,a,a,a,a,a,a,a,a,a,d,a,a,a,a,a},
		{a,b,b,b,a,a,a,a,a,a,a,a,a,a,d,a,a,a,a,a},
		{a,a,a,b,a,a,a,a,a,a,a,a,a,a,d,a,a,a,a,a},
		{a,a,b,b,a,a,a,a,a,g,a,a,a,a,d,d,d,d,d,d},
		{a,a,a,b,b,a,a,a,a,a,b,a,a,a,a,a,d,a,a,a},
		{a,a,a,a,b,a,a,a,a,a,b,b,b,a,a,a,d,a,b,a},
		{a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,d,a,b,a},
		{a,g,a,a,a,a,a,c,c,a,a,a,a,a,a,a,d,a,a,a},
		{a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,d,a,a,a},
		{a,a,a,a,a,a,a,b,a,a,a,a,a,a,a,a,d,d,d,d},
		{a,a,a,b,b,a,a,c,a,a,a,g,a,a,a,a,a,a,a,a},
		{a,a,a,a,b,a,a,a,c,c,a,a,a,f,f,f,e_2,f,f,f},
		{a,a,a,a,b,b,a,a,a,a,a,a,f,f,a,a,a,a,a,a},
		{a,a,a,a,a,a,a,a,a,a,a,a,e_1,a,a,a,a,a,a,a},
		{a,a,a,a,a,a,a,a,a,a,a,a,f,a,a,a,a,b,a,a},
		{a,a,b,b,b,a,a,a,a,a,a,a,f,a,a,a,a,a,b,a},
		{a,a,b,b,b,a,a,a,g,a,f,f,f,d,d,d,d,d,d,d},
		{a,a,a,a,b,b,a,b,a,a,f,a,a,d,a,d,a,h,a,a},
		{a,a,a,a,a,b,a,a,a,a,f,a,a,d,a,d,a,a,a,a},
		{a,a,a,a,a,a,a,a,a,a,f,a,a,d,b,d,a,a,a,a}
	
	};
}
