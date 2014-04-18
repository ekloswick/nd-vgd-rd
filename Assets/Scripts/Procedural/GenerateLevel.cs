using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateLevel : MonoBehaviour
{
	public int levelSize = 50;
	public int roomSize = 5;
	public float tileSpawnChance = 0.75f;
	public float roomSpawnChance = 0.008f;
	public int tileStyle = 0; // 0 for dev, 1 for default dungeon, other values not currently defined
	
	//[HideInInspector]
	private int[] startingLocation;
	
	private int[,] levelMatrix;
	private List<int[]> rooms = new List<int[]>();
	private GameObject basicTile,
						pathTile,
						darknessTile,
						enemyTile,
						treasureTile,
						trapTile;
	private float enemyChance = 0.02f,
					treasureChance = 0.003f,
					trapChance = 0.025f;
	
	// Use this for initialization
	void Start ()
	{
		// use to get reliable dungeons
		//Random.seed = 140394581;
		//Random.seed = 608781738;
		
		Debug.Log (Random.seed);
		
		levelMatrix = new int[levelSize,levelSize];
		
		// load in prefabs
		loadPrefabs();
		
		// create the dungeon
		generateDungeon();
		
		// populate dungeon with actual stuff
		populateDungeon();
		
		// scan dungeon to make small tweaks
		refineDungeon();
		
		// draw the dungeon
		showDungeon();
		
		// move player to starting area
		GameObject.FindWithTag("Player").transform.position = new Vector3(startingLocation[0], 0, startingLocation[1]);
		
		Debug.Log("Done!");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown(MyInput.B_name) || Input.GetKeyDown (KeyCode.E))
		{
			Application.LoadLevel("firstPlayableCore");
		}
	}
	
	// loads in the appropriate prefabs
	void loadPrefabs()
	{
		if (tileStyle == 0)
		{
			// dev tiles
			basicTile = (GameObject)Resources.Load ("Prefabs/Dev Tiles/Basic");
			pathTile = (GameObject)Resources.Load ("Prefabs/Dev Tiles/Corridor");
			darknessTile = (GameObject)Resources.Load ("Prefabs/Dev Tiles/Darkness");
			enemyTile = (GameObject)Resources.Load ("Prefabs/Dev Tiles/Enemy");
			treasureTile = (GameObject)Resources.Load ("Prefabs/Dev Tiles/Treasure");
			trapTile = (GameObject)Resources.Load ("Prefabs/Dev Tiles/Trap");
		}
		/*else if (tileStyle == 1)
		{
			// dungeon tiles
			noWallTile = (GameObject)Resources.Load ("Prefabs/Dungeon Tiles/NoWall");
			oneWallTile = (GameObject)Resources.Load ("Prefabs/Dungeon Tiles/OneWall");
			twoWallTile = (GameObject)Resources.Load ("Prefabs/Dungeon Tiles/TwoWall");
			threeWallTile = (GameObject)Resources.Load ("Prefabs/Dungeon Tiles/ThreeWall");
			corridorTile = (GameObject)Resources.Load ("Prefabs/Dungeon Tiles/Corridor");
			
			upLeftTile = (GameObject)Resources.Load ("Prefabs/Dungeon Tiles/upLeft");
			upTile = (GameObject)Resources.Load ("Prefabs/Dungeon Tiles/up");
			upRightTile = (GameObject)Resources.Load ("Prefabs/Dungeon Tiles/upRight");
			leftTile = (GameObject)Resources.Load ("Prefabs/Dungeon Tiles/left");
			middleTile = (GameObject)Resources.Load ("Prefabs/Dungeon Tiles/middle");
			rightTile = (GameObject)Resources.Load ("Prefabs/Dungeon Tiles/right");
			downLeftTile = (GameObject)Resources.Load ("Prefabs/Dungeon Tiles/downLeft");
			downTile = (GameObject)Resources.Load ("Prefabs/Dungeon Tiles/down");
			downRightTile = (GameObject)Resources.Load ("Prefabs/Dungeon Tiles/downRight");
			vertCorridorTile = (GameObject)Resources.Load ("Prefabs/Dungeon Tiles/vertCorridor");
			horizCorridorTile = (GameObject)Resources.Load ("Prefabs/Dungeon Tiles/horizCorridor");
			
		}*/
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
		// randomly choose a corner or middle to place the starting room
		switch (Random.Range(1,6))
		{
			// top left
			case 1:
				startingLocation = new int[2] {levelSize/4, 3*levelSize/4};
				break;
			// top right
			case 2:
				startingLocation = new int[2] {3*levelSize/4, 3*levelSize/4};
				break;
			// bottom left
			case 3:
				startingLocation = new int[2] {levelSize/4, levelSize/4};
				break;
			// bottom right
			case 4:
				startingLocation = new int[2] {3*levelSize/4, levelSize/4};
				break;
			// middle
			default:
				startingLocation = new int[2] {levelSize/2, levelSize/2};
				break;
		}
		
		// add the starting room to rooms list
		rooms.Add(startingLocation);
		
		// variable for storing random values determined room spawn chances
		float roomSpawn;
		
		// go through all tiles and randomly set room seeds
		for (int x = 0; x < levelMatrix.GetLength(0); x++)
		{
			for (int z = 0; z < levelMatrix.GetLength(1); z++)
			{
				// edge cases, always darkness
				if (x <= 1 || z <= 1 || x >= levelMatrix.GetLength(0) - 2 || z >= levelMatrix.GetLength(1) - 2)
				{
					levelMatrix[x,z] = 0;
					continue;
				}
				
				roomSpawn = Random.value;
				
				// seed rooms
				if (roomSpawn < roomSpawnChance)
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
			if (rooms.IndexOf(item) == 0)
				setupStartingRoom(item[0], item[1]);
			else
				expandRoom(item[0], item[1], Random.Range (4, 4+roomSize));
		}
		
		// create corridors
		generateCorridors();
		
		return;
	}
	
	// creates the starting room that the player starts in
	void setupStartingRoom(int x, int z)
	{
		for (int i = -3; i < 4; i++)
		{
			for (int j = -3; j < 4; j++)
			{
				if (Mathf.Abs(i+j) <= 3)
					levelMatrix[x+i,z+j] = 11;
			}
		}
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
				if (levelMatrix[x+i,z+j] == 0 && (x+i != 0) && (x+i != levelMatrix.GetLength(0) - 2) && (z+j != 0) && (z+j != levelMatrix.GetLength(1) - 2))
				{
					if (Random.value < tileSpawnChance)
					{
						//Debug.Log ("Expanding tile " + (x+i) + ", " + (z+j) + ", Lifetime = " + lifetime);
						expandRoom((x+i), (z+j), lifetime);
					}
				}
			}
		}
		
		return;
	}
	
	// creates a relative neighbourhood graph of room nodes and connects them with corridors
	void generateCorridors()
	{
		int roomOne = 0, roomTwo = 0;
		float maxDistance = 0.0f;
		
		// horrible O(n^3) approach for finding relative neighbourhood graph
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
				
				// go through all other rooms not currently being compared
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
					spawnCorridor(rooms[i][0], rooms[i][1], rooms[j][0], rooms[j][1]);
			}
		}
		
		// create overarching corridor to hopefully make some cheap loops
		spawnCorridor(rooms[roomOne][0], rooms[roomOne][1], rooms[roomTwo][0], rooms[roomTwo][1]);
		
		return;
	}
	
	// returns distance between points a and b
	float distance(int[] a, int[] b)
	{
		return Mathf.Sqrt( Mathf.Pow(a[0]-b[0],2) + Mathf.Pow(a[1]-b[1],2) );
	}
	
	// creates L-corridor of varying size from point a to point b
	void spawnCorridor(int ax, int az, int bx, int bz)
	{
		int[] a = new int[2] {ax, az};
		int[] b = new int[2] {bx, bz};
		
		int corridorWidth = 1;
		int corridorColor = 5; // 5 for white, 11 for green
		
		// decide how wide these corridors will be
		float tempCorridor = Random.value;
		
		if (tempCorridor > 0.85f)
			corridorWidth = 3;
		else
			corridorWidth = 2;
		
		int xSlope = b[0] - a[0];
		int zSlope = b[1] - a[1];
		
		// first handle side corresponding to the X-axis
		if (xSlope > 0)
		{
			while (xSlope > 0)
			{
				levelMatrix[a[0],a[1]] = corridorColor;
				
				switch (corridorWidth)
				{
					case 2:
						if (a[1] != 1 || a[1] != levelSize - 2)
							levelMatrix[a[0],a[1]-1] = corridorColor;
						break;
					case 3:
						if (a[1] != 1 || a[1] != levelSize - 2)
						{
							levelMatrix[a[0],a[1]-1] = corridorColor;
							levelMatrix[a[0],a[1]+1] = corridorColor;
						}
						break;
				}
				
				xSlope--;
				a[0]++;
			}
		}
		else
		{
			while (xSlope < 0)
			{
				levelMatrix[a[0],a[1]] = corridorColor;
				
				switch (corridorWidth)
				{
					case 2:
						if (a[1] != 1 || a[1] != levelSize - 2)
							levelMatrix[a[0],a[1]-1] = corridorColor;
						break;
					case 3:
						if (a[1] != 1 || a[1] != levelSize - 2)
						{
							levelMatrix[a[0],a[1]-1] = corridorColor;
							levelMatrix[a[0],a[1]+1] = corridorColor;
						}
						break;
				}
				
				xSlope++;
				a[0]--;
			}
		}
		
		// then handle side corresponding to the Z-axis
		if (zSlope > 0)
		{
			while (zSlope > 0)
			{
				levelMatrix[a[0],a[1]] = corridorColor;
				
				switch (corridorWidth)
				{
					case 2:
						if (a[0] != 1 || a[0] != levelSize - 2)
							levelMatrix[a[0]-1,a[1]] = corridorColor;
						break;
					case 3:
						if (a[0] != 1 || a[0] != levelSize - 2)
						{
							levelMatrix[a[0]-1,a[1]] = corridorColor;
							levelMatrix[a[0]+1,a[1]] = corridorColor;
						}
						break;
				}
				
				zSlope--;
				a[1]++;
			}
		}
		else
		{
			while (zSlope < 0)
			{
				levelMatrix[a[0],a[1]] = corridorColor;
				
				switch (corridorWidth)
				{
					case 2:
						if (a[0] != 1 || a[0] != levelSize - 2)
							levelMatrix[a[0]-1,a[1]] = corridorColor;
						break;
					case 3:
						if (a[0] != 1 || a[0] != levelSize - 2)
						{
							levelMatrix[a[0]-1,a[1]] = corridorColor;
							levelMatrix[a[0]+1,a[1]] = corridorColor;
						}
						break;
				}
				
				zSlope++;
				a[1]--;
			}
		}
		
		return;
	}
	
	// passes over the levelMatrix to put in enemies, treasures, and traps
	void populateDungeon()
	{
		// only go over tiles that can have floor tiles
		for (int x = 2; x < levelMatrix.GetLength(0) - 2; x++)
		{
			for (int z = 2; z < levelMatrix.GetLength(1) - 2; z++)
			{
				if (levelMatrix[x,z] == 5 || levelMatrix[x,z] == 11)
				{					
					// chance to spawn special tiles
					float specialChance = Random.value;
					
					// 13 = enemy, 14 = treasure, 15 = trap
					if (specialChance > 1.0f - enemyChance)
						levelMatrix[x,z] = 13;
					else if (specialChance > 1.0f - enemyChance - treasureChance)
						levelMatrix[x,z] = 14;
					else if (specialChance > 1.0f - enemyChance - treasureChance - trapChance)
						levelMatrix[x,z] = 15;
				}
			}
		}
		
		return;
	}
	
	// passes over the levelMatrix to check each tile for possible tweaks
	void refineDungeon()
	{
		// go over all tiles
		for (int x = 0; x < levelMatrix.GetLength(0); x++)
		{
			for (int z = 0; z < levelMatrix.GetLength(1); z++)
			{
				// if outside tiles, hide them
				if ((x == 0 || x == levelMatrix.GetLength(0) - 1 || z == 0 || z == levelMatrix.GetLength(1) - 1))
					continue;
				// tweaks regarding surrounding tiles
				else //if (levelMatrix[x,z] == 5 || levelMatrix[x,z] == 11)
				{
					int wallCount = 0;
					
					// check surrounding tiles
					for (int i = -1; i < 2; i++)
					{
						for (int j = -1; j < 2; j++)
						{					
							if (i == 0 && j == 0)
								continue;
							else if (levelMatrix[x+i,z+j] == 0 || levelMatrix[x+i,z+j] == -1)
								wallCount++;
						}
					}
					
					// don't draw unnecessary tiles
					if (wallCount == 8)
						levelMatrix[x,z] = -1;
					// fill in unreachable floor tiles
					else if (wallCount >= 5)
						levelMatrix[x,z] = 0;
				}
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
				// code for if we want to not draw certain tiles
				if (levelMatrix[x,z] == -1)
					continue;
				
				if (tileStyle == 0)
				{
					GameObject obj = getDevTile(levelMatrix[x,z]);
					obj.transform.position = new Vector3(x, 0.0f, z);
				}
				/*else if (tileStyle == 1)
				{
					GameObject obj = getDungeonTile(levelMatrix[x,z], x, z);
					obj.transform.position = new Vector3(x, 0.0f, z);
				}*/
				
			}
		}
		
		return;
	}
	
	// returns the appropriate dev tile
	GameObject getDevTile(int tileNum)
	{
		switch (tileNum)
		{
			case 5:
				return (GameObject)Instantiate(basicTile);
			case 11:
				return (GameObject)Instantiate(pathTile);
			case 13:
				return (GameObject)Instantiate(enemyTile);
			case 14:
				return (GameObject)Instantiate(treasureTile);
			case 15:
				return (GameObject)Instantiate(trapTile);
			case 0:
				return (GameObject)Instantiate(darknessTile);
			default:
				return GameObject.CreatePrimitive(PrimitiveType.Sphere);
		}
	}
	
	// returns the appropriate dungeon tile
	/*GameObject getDungeonTile(int tileNum, int x, int z)
	{
		int wallCount = 0;
		
		// check surrounding tiles
		for (int i = -1; i < 2; i++)
		{
			for (int j = -1; j < 2; j++)
			{					
				if ((i + j) % 2 == 0)
					continue;
				else if (levelMatrix[x+i,z+j] == 0 || levelMatrix[x+i,z+j] == -1)
					wallCount++;
			}
		}
	
		// check for corner wall or corridor wall
		if (wallCount == 2)
		{
			
		}
		
		switch (tileNum)
		{
			case 1:
				break;
			case 2:
			case 3:
			case 4:
			case 5:
				return (GameObject)Instantiate(noWallTile);
			case 6:
			case 7:
			case 8:
			case 9:
			case 11:
				return (GameObject)Instantiate(pathTile);
			case 22:
				break;
			case 13:
				return (GameObject)Instantiate(enemyTile);
			case 14:
				return (GameObject)Instantiate(treasureTile);
			case 15:
				return (GameObject)Instantiate(trapTile);
			case 0:
				return (GameObject)Instantiate(darknessTile);
			default:
				return GameObject.CreatePrimitive(PrimitiveType.Sphere);
		}
	}*/
}
