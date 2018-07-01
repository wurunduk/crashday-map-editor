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

	private Vector2 _scrollPosition = Vector2.zero;

	// Use this for initialization
	void Start ()
	{
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

	    //pos.y = _tileManager.TileList[SelectedTileId].Model.P3DMeshes[0].Depth / 2;

		pos.x = Mathf.RoundToInt(pos.x / 20) * 20;
		pos.z = Mathf.RoundToInt(pos.z / 20) * 20;

		pos.x = Mathf.Clamp(pos.x, 0, (_trackManager.CurrentTrack.Width-1) * 20);
		pos.z = Mathf.Clamp(pos.z, (_trackManager.CurrentTrack.Height-1) * -20, 0);

		if (Input.GetMouseButtonDown(1))
		{
			_currenTile.Rotate();
			_currenTile.ApplyTerrain();
		}

		if (Mathf.RoundToInt(DummyShadowTile.transform.position.x) != Mathf.RoundToInt(pos.x) || 
		    Mathf.RoundToInt(DummyShadowTile.transform.position.z) != Mathf.RoundToInt(pos.z))
		{
			_currenTile.GridPosition = new Vector2(Mathf.Clamp(pos.x/20.0f, 0, _trackManager.CurrentTrack.Width-1), -1*Mathf.Clamp(pos.z/20.0f, 0, _trackManager.CurrentTrack.Height-1));
			_currenTile.ApplyTerrain();
		}

        DummyShadowTile.transform.position = pos;
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

				_currenTile.SetupTile(new TrackTileSavable(), _tileManager.TileList[i].Size, 
					new Vector2(Mathf.Clamp(DummyShadowTile.transform.position.x/20.0f, 0, _trackManager.CurrentTrack.Width-1), -1*Mathf.Clamp(DummyShadowTile.transform.position.z/20.0f, 0, _trackManager.CurrentTrack.Height-1)), _trackManager);
				_currenTile.ChangeVerticies(_tileManager.TileList[i].Model.CreateMeshes()[0].vertices);
				_currenTile.ApplyTerrain();
			}
		}
		GUI.EndScrollView ();
	}
}
