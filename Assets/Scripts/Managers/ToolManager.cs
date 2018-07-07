using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour 
{
	public int SelectedTileId;
	public Transform DummyShadowTile;

	private Tile _currentTile;
	private TileManager _tileManager;
	private TrackManager _trackManager;

	private IntVector2 _gridPosition;

	private Vector2 _scrollPosition = Vector2.zero;

	// Use this for initialization
	void Start ()
	{
		_gridPosition = new IntVector2(0,0);
		_trackManager = GetComponent<TrackManager>();
		_tileManager = GetComponent<TileManager>();
		_currentTile = DummyShadowTile.GetComponent<Tile>();
		_currentTile.SetupTile(new TrackTileSavable(), new IntVector2(1,1), new IntVector2(0,0), _trackManager);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!_tileManager.Loaded || !_trackManager.LoadedTrack || _currentTile._trackTileSavable == null) return;

		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
	    Plane plane = new Plane(Vector3.up, Vector3.zero);
	    float outFloat;
	    plane.Raycast(ray, out outFloat);
	    Vector3 pos = ray.GetPoint(outFloat);

		IntVector2 newGridPosition = new IntVector2(Mathf.Clamp(Mathf.RoundToInt(pos.x / 20), 0, _trackManager.CurrentTrack.Width-1), 
			Mathf.Clamp(-1*Mathf.RoundToInt(pos.z / 20), 0, _trackManager.CurrentTrack.Height-1));


		if (Input.GetMouseButtonDown(0))
		{
			_trackManager.CurrentTrack.TrackTiles[_gridPosition.x, _gridPosition.y] = _currentTile._trackTileSavable;
		}

		if (Input.GetMouseButtonDown(1))
		{
			_currentTile.Rotate();
			_currentTile.ApplyTerrain();
		}

		if (_gridPosition.x != newGridPosition.x || _gridPosition.y != newGridPosition.y)
		{
			//placeholder to turn on/off tile rendering under current tile
			if(_trackManager.Tiles[_gridPosition.x, _gridPosition.y] != null)
				_trackManager.Tiles[_gridPosition.x, _gridPosition.y].GetComponent<Renderer>().enabled = true;

			if(_trackManager.Tiles[newGridPosition.x, newGridPosition.y] != null)
				_trackManager.Tiles[newGridPosition.x, newGridPosition.y].GetComponent<Renderer>().enabled = false;

			_gridPosition = newGridPosition;
			_currentTile.GridPosition = _gridPosition;
			_currentTile.ApplyTerrain();
		}

		DummyShadowTile.transform.position = _currentTile.GetTransformPosition();
	}

	void OnGUI()
	{
		if (!_tileManager || !_tileManager.Loaded) return;

		_scrollPosition = GUI.BeginScrollView (new Rect (Screen.width - 120, 10, 110, Screen.height - 10), _scrollPosition, new Rect (Screen.width - 110, 20, 100, (_tileManager.TileList.Count+1)*20));
		for(int i = 0; i < _tileManager.TileList.Count; i++)
		{
			if(GUI.Button(new Rect(Screen.width - 110, 20*(i+1), 100, 18), _tileManager.TileList[i].Name))
			{
				if (i == SelectedTileId) continue;
				
				DummyShadowTile.GetComponent<MeshFilter> ().mesh = _tileManager.TileList[i].Model.CreateMeshes()[0];
				DummyShadowTile.GetComponent<Renderer>().materials = _tileManager.TileList[i].Materials;

				DummyShadowTile.position = new Vector3(DummyShadowTile.position.x, _tileManager.TileList[i].Model.P3DMeshes[0].Height/2, DummyShadowTile.position.z);

				_currentTile.SetupTile(new TrackTileSavable(), _tileManager.TileList[i].Size, _gridPosition, _trackManager);
				_currentTile.ChangeVerticies(_tileManager.TileList[i].Model.CreateMeshes()[0].vertices);
				_currentTile.ApplyTerrain();
			}
		}
		GUI.EndScrollView ();
	}
}
