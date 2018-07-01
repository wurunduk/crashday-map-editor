using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour 
{
	public int SelectedTileId;
	public Transform DummyShadowTile;

	private Tile _currenTile;
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
		_currenTile = DummyShadowTile.GetComponent<Tile>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!_tileManager.Loaded || !_trackManager.LoadedTrack || _currenTile._trackTileSavable == null) return;

		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
	    Plane plane = new Plane(Vector3.up, Vector3.zero);
	    float outFloat;
	    plane.Raycast(ray, out outFloat);
	    Vector3 pos = ray.GetPoint(outFloat);

		IntVector2 newGridPosition = new IntVector2(Mathf.Clamp(Mathf.RoundToInt(pos.x / 20), 0, _trackManager.CurrentTrack.Width-1), 
			Mathf.Clamp(-1*Mathf.RoundToInt(pos.z / 20), 0, _trackManager.CurrentTrack.Height-1));

		if (Input.GetMouseButtonDown(1))
		{
			_currenTile.Rotate();
			_currenTile.ApplyTerrain();
		}

		if (_gridPosition.x != newGridPosition.x || _gridPosition.y != newGridPosition.y)
		{
			_gridPosition = newGridPosition;
			_currenTile.GridPosition = _gridPosition;
			_currenTile.ApplyTerrain();
		}

        DummyShadowTile.transform.position = new Vector3(_gridPosition.x*20, DummyShadowTile.position.y ,_gridPosition.y*-20);
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

				_currenTile.SetupTile(new TrackTileSavable(), _tileManager.TileList[i].Size, _gridPosition, _trackManager);
				_currenTile.ChangeVerticies(_tileManager.TileList[i].Model.CreateMeshes()[0].vertices);
				_currenTile.ApplyTerrain();
			}
		}
		GUI.EndScrollView ();
	}
}
