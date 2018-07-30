using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tool_TerrainEdit : ToolGeneral
{
	private GameObject _heightPointObject;
	private Transform[,] _heightPoints;
	private Transform _heightPointsParent;

	private List<IntVector2> _currentSelectedPoints;
	private List<IntVector2> _currentSelectedTiles;

	private Material _matBlue;
	private Material _matYellow;

	public float _oldHeight;
	public float _startHeight;

	public override void Initialize()
	{
		ToolName = "Edit Terrain";
		_heightPointsParent = new GameObject("Height points").transform;
		_currentSelectedPoints = new List<IntVector2>();
		_currentSelectedTiles = new List<IntVector2>();
		_matBlue = Resources.Load<Material>("BlueGlow");
		_matYellow = Resources.Load<Material>("YellowGlow");
	}

	public override void OnSelected()
	{
		_heightPoints = new Transform[TrackManager.CurrentTrack.Width*4+1,TrackManager.CurrentTrack.Height*4+1];
		_currentSelectedPoints.Clear();
		_currentSelectedTiles.Clear();
		TerrainManager.Terrain.GetComponent<MeshRenderer>().enabled = true;
	}

	public override void OnDeselected()
	{
		foreach (var obj in _heightPoints)
		{
			if(obj != null) Object.Destroy(obj.gameObject);
		}
		TerrainManager.Terrain.GetComponent<MeshRenderer>().enabled = false;
	}

	public override void Update()
	{
		if (Input.GetButtonDown("Select all"))
		{
			if (_currentSelectedPoints.Count > 0)
			{
				foreach (IntVector2 sp in _currentSelectedPoints)
				{
					Object.Destroy(_heightPoints[sp.x, sp.y].gameObject);
				}
				_currentSelectedPoints.Clear();
				_currentSelectedTiles.Clear();
			}
			else
			{
				for (int y = 0; y < TrackManager.CurrentTrack.Height * 4 + 1; y++)
				{
					for (int x = 0; x < TrackManager.CurrentTrack.Width * 4 + 1; x++)
					{
						//todo:
						//add all tiles to _selectedtileslist
						CreateSelectedCube(new IntVector2(x,y));
					}
				}

				for (int y = 0; y < TrackManager.CurrentTrack.Height; y++)
					for(int x = 0; x < TrackManager.CurrentTrack.Width; x++)
						_currentSelectedTiles.Add(new IntVector2(x,y));
			}
		}
	}

	public override void OnLMBDown(Vector3 point)
	{
		_startHeight = Input.mousePosition.y*5/Screen.height;
		_oldHeight = _startHeight;
	}

	public override void OnLMB(Vector3 point)
	{
		float delta = Input.mousePosition.y*5/Screen.height - _oldHeight;

		foreach (var hp in _currentSelectedPoints)
		{
			TrackManager.CurrentTrack.Heightmap[hp.y][hp.x] += delta;

			TerrainManager.UpdateTerrain(hp);

			_heightPoints[hp.x, hp.y].position = new Vector3(_heightPoints[hp.x, hp.y].position.x, 
				TrackManager.CurrentTrack.Heightmap[hp.y][hp.x]*TrackManager.CurrentTrack.GroundBumpyness*5, 
				_heightPoints[hp.x, hp.y].position.z);
		}

		foreach (var st in _currentSelectedTiles)
		{
			TrackManager.UpdateTileAt(st.x, st.y);
		}

		_oldHeight = Input.mousePosition.y*5/Screen.height;
	}

	public override void OnRMB(Vector3 pos)
	{
		pos.x += TrackManager.TileSize / 2;
		pos.z -= TrackManager.TileSize / 2;

		IntVector2 gridPosition = new IntVector2(Mathf.Clamp(Mathf.RoundToInt(pos.x / 5), 0, TrackManager.CurrentTrack.Width*4), 
			Mathf.Clamp(-1*Mathf.RoundToInt(pos.z / 5), 0, TrackManager.CurrentTrack.Height*4));

		if (Input.GetButton("Control"))
		{
			int res = _currentSelectedPoints.FindIndex(x => x.x == gridPosition.x && x.y == gridPosition.y);
			if (res >= 0)
			{
				_currentSelectedPoints.RemoveAt(res);
				Object.Destroy(_heightPoints[gridPosition.x, gridPosition.y].gameObject);
			}
		}
		else
		{
			if (!_currentSelectedPoints.Exists(x => x.x == gridPosition.x && x.y == gridPosition.y))
			{
				CreateSelectedCube(gridPosition);

				int ax = 0;
				int ay = 0;

				IntVector2 tilePos = new IntVector2(gridPosition.x, gridPosition.y);

				if (gridPosition.x % 4 == 0 && gridPosition.x > 0)
				{
					tilePos.x -= 1;
					if(gridPosition.x != TrackManager.CurrentTrack.Width*4)
						ax += 1;
				}


				if (gridPosition.y % 4 == 0 && gridPosition.y > 0)
				{
					tilePos.y -= 1;
					if(gridPosition.y != TrackManager.CurrentTrack.Height*4)
						ay += 1;
				}

				for(int y = 0; y <= ay; y++)
					for(int x = 0; x <= ax; x++)
						_currentSelectedTiles.Add(new IntVector2(tilePos.x/4 + x, tilePos.y/4 + y));
			}
		}
	}

	private void CreateSelectedCube(IntVector2 gridPosition)
	{
		_currentSelectedPoints.Add(gridPosition);

		_heightPoints[gridPosition.x, gridPosition.y] = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
		_heightPoints[gridPosition.x, gridPosition.y].localScale = new Vector3(2,2,2);
		_heightPoints[gridPosition.x, gridPosition.y].rotation = Quaternion.Euler(0, 45, 0);
		_heightPoints[gridPosition.x, gridPosition.y].position = new Vector3(gridPosition.x*5 - TrackManager.TileSize/2, 
			TrackManager.CurrentTrack.Heightmap[gridPosition.y][gridPosition.x]*TrackManager.CurrentTrack.GroundBumpyness*5, 
			gridPosition.y*-5 + TrackManager.TileSize/2);

		_heightPoints[gridPosition.x, gridPosition.y].SetParent(_heightPointsParent);
		_heightPoints[gridPosition.x, gridPosition.y].name = gridPosition.x + " : " + gridPosition.y;

		_heightPoints[gridPosition.x, gridPosition.y].GetComponent<MeshRenderer>().material = _matYellow;
	}
}