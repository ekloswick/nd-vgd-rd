using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateLevel : MonoBehaviour
{
	public int levelSize = 50; // must be even
	public int roomSize = 1;
	
	private int[,] levelMatrix;
	private List<int[]> rooms = new List<int[]>();
	private GameObject blankTile, darknessTile;
	
	private float tileSpawnChance = 0.7f;
	private float roomSpawnChance = 0.004f;
	
	// Use this for initialization
	void Start ()
	{
		// use to get reliable dungeons
		//Random.seed = 140394581;
		Debug.Log (Random.seed);
		
		levelMatrix = new int[levelSize,levelSize];
		
		// load in prefabs
		loadPrefabs();
		
		// create the dungeon
		generateDungeon();
		
		// draw the dungeon
		showDungeon();
		
		Debug.Log("Done!");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown("B_Win"))
		{
			Application.LoadLevel("firstPlayableCore");
		}
	}
	
	// loads in the appropriate prefabs
	void loadPrefabs()
	{
		blankTile = (GameObject)Resources.Load ("Prefabs/Floor Tile - Blank");
		darknessTile = (GameObject)Resources.Load ("Prefabs/Floor Tile - Darkness");
	}
	
	// fills dungeon matrix with empty "0" values (correspond to "darkness" tiles)
	void clearDungeon()
	{
		for (int x = 0; x < levelMatrix.GetLength(0); x++)
		{
			for (int z = 0; z < levelMatrix.GetLength(1); z++)
			{
				levelMatrix[x,z] = 0;
			}
		}
		
		return;
	}
	
	// seeds, grows, and connects rooms to form the dungeon
	void generateDungeon()
	{
		// add default room, always in every level
		rooms.Add(new int[2] {levelSize/2,levelSize/2});
		
		float randomNum;
		
		// go through all tiles and randomly set room seeds
		for (int x = 0; x < levelMatrix.GetLength(0); x++)
		{
			for (int z = 0; z < levelMatrix.GetLength(1); z++)
			{
				// edge case, always darkness
				if (x == 0 || z == 0 || x == levelMatrix.GetLength(0) - 1 || z == levelMatrix.GetLength(1) - 1)
				{
					levelMatrix[x,z] = 0;
					continue;
				}
				
				randomNum = Random.value;
				
				// seed rooms
				if (randomNum < roomSpawnChance)
					rooms.Add(new int[2] {x,z});
				//else if (randomNum > 0.6)
				else
					levelMatrix[x,z] = 0;
			}
		}
		
		// create each room
		foreach (int[] item in rooms)
		{
			//Debug.Log ("Expanding tile " + item[0] + ", " + item[1]);
			
			//levelMatrix[item[0], item[1]] = 5;
			expandRoom(item[0], item[1], Random.Range (4, 4+roomSize));
		}
		
		// calculate corridors
		generateCorridors();
		
		return;
	}
	
	// recursive function to fill out rooms
	void expandRoom(int x, int z, int lifetime)
	{
		lifetime--;
		
		if (lifetime <= 0)
			return;
	
		// call approrpriate recursion calls on surrounding cells
		levelMatrix[x,z] = 5;
	
		// start with top left cell, go right across then down a row, typewriter style
		for (int i = -1; i < 2; i++)
		{
			for (int j = -1; j < 2; j++)
			{					
				if (levelMatrix[x+i,z+j] == 0 && (x+i != 0) && (x+i != levelMatrix.GetLength(0) - 1) && (z+j != 0) && (z+j != levelMatrix.GetLength(1) - 1))
				{
					if (Random.value < tileSpawnChance)// || (i == 0 || j == 0))
					{
						//Debug.Log ("Expanding tile " + (x+i) + ", " + (z+j) + ", Lifetime = " + lifetime);
						expandRoom((x+i), (z+j), lifetime);
					}
				}
			}
		}
		
		return;
	}
	
	void generateCorridors()
	{
		int roomOne = 0, roomTwo = 0;
		float maxDistance = 0.0f;
	
		// horrible O(n^3) approach to finding relative neighbourhood graph
		for (int i = 0; i < rooms.Count; i++)
		{
			for (int j = i + 1; j < rooms.Count; j++)
			{
				bool isEdge = true;
				float testDistance = distance(rooms[i], rooms[j]);
				
				// store overall maximum distance to create long corridor for loop(s)
				if (testDistance > maxDistance)
				{
					maxDistance = testDistance;
					roomOne = i;
					roomTwo = j;
				}
				
				for (int k = 0; k < rooms.Count; k++)
				{
					if (k == i || k == j)
						continue;
					
					float tempOne = distance(rooms[i], rooms[k]);
					float tempTwo = distance(rooms[j], rooms[k]);
					
					if (testDistance > ((tempOne > tempTwo) ? tempOne : tempTwo))
					{
						// max distance to point k is closer to i and j than distance from i to j, this isn't an edge
						isEdge = false;
						break;
					}
				}
				
				if (isEdge)
				{
					spawnCorridor(rooms[i], rooms[j]);
				}
				
			}
		}
		
		// create overarching corridor to hopefully make some loops
		spawnCorridor(rooms[roomOne], rooms[roomTwo]);
		
		return;
	}
	
	float distance(int[] a, int[] b)
	{
		return Mathf.Sqrt( Mathf.Pow(a[0]-b[0],2) + Mathf.Pow(a[1]-b[1],2) );
	}
	
	// creates L-corridor
	// TODO make corridors wider
	void spawnCorridor(int[] a, int[] b)
	{
		int xSlope = b[0] - a[0];
		int zSlope = b[1] - a[1];
		
		//Debug.Log ("X-Slope: " + xSlope + ", Z-Slope: " + zSlope);
	
		if (xSlope > 0)
		{
			while (xSlope > 0)
			{
				levelMatrix[a[0],a[1]] = 5;
				
				xSlope--;
				a[0]++;
			}
		}
		else
		{
			while (xSlope < 0)
			{
				levelMatrix[a[0],a[1]] = 5;
				
				xSlope++;
				a[0]--;
			}
		}
		
		if (zSlope > 0)
		{
			while (zSlope > 0)
			{
				levelMatrix[a[0],a[1]] = 5;
				
				zSlope--;
				a[1]++;
			}
		}
		else
		{
			while (zSlope < 0)
			{
				levelMatrix[a[0],a[1]] = 5;
				
				zSlope++;
				a[1]--;
			}
		}
		
		return;
	}
	
	// draws the contents of the dungeon in the scene
	void showDungeon()
	{
		for (int x = 0; x < levelMatrix.GetLength(0); x++)
		{
			for (int z = 0; z < levelMatrix.GetLength(1); z++)
			{
				// code for if we want to not draw "darkness" tiles; if we want to do custom directioned-walled tiles instead of having "darkness" be the walls
				/*if (levelMatrix[x,z] == 0)
					continue;
					*/
					
				GameObject obj = getTile(levelMatrix[x,z]);
				
				obj.transform.position = new Vector3(x, 0.0f, z);
			}
		}
		
		return;
	}
	
	// returns the appropriate prefab GameObject
	GameObject getTile(int tileNum)
	{
		switch (tileNum)
		{
			case 1:
				return GameObject.CreatePrimitive(PrimitiveType.Sphere);
			case 2:
				return GameObject.CreatePrimitive(PrimitiveType.Sphere);
			case 3:
				return GameObject.CreatePrimitive(PrimitiveType.Sphere);
			case 4:
				return GameObject.CreatePrimitive(PrimitiveType.Sphere);
			case 5:
				return (GameObject)Instantiate(blankTile);
			case 6:
				return GameObject.CreatePrimitive(PrimitiveType.Sphere);
			case 7:
				return GameObject.CreatePrimitive(PrimitiveType.Sphere);
			case 8:
				return GameObject.CreatePrimitive(PrimitiveType.Sphere);
			case 9:
				return GameObject.CreatePrimitive(PrimitiveType.Sphere);
			case 10:
				return GameObject.CreatePrimitive(PrimitiveType.Sphere);
			case 11:
				return GameObject.CreatePrimitive(PrimitiveType.Sphere);
			case 22:
				return GameObject.CreatePrimitive(PrimitiveType.Sphere);
			default:
				return (GameObject)Instantiate(darknessTile);
		}
	}
}
