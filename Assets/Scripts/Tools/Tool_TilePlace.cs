using UnityEngine;
using System.Collections;

public class Tool_TilePlace : ToolGeneral
{
	private Vector3 _scrollPosition;

	private IntVector2 _gridPosition;
	private Tile _currentTile;

	public int SelectedTileId;

	public override void Initialize()
	{
		ToolName = "Place Tiles";
		_gridPosition = new IntVector2(0, 0);
	}

	public override void OnSelected()
	{
		SomePrefab.GetComponent<MeshRenderer>().enabled = true;
		_currentTile = SomePrefab.GetComponent<Tile>();
		_currentTile.SetupTile(new TrackTileSavable(), new IntVector2(1,1), new IntVector2(0,0), TrackManager, "field.cfl");

		SelectTile(1);
	}

	public override void OnDeselected()
	{
		SomePrefab.GetComponent<MeshRenderer>().enabled = false;

		if(TrackManager.Tiles[_gridPosition.y][_gridPosition.x] != null)
			TrackManager.Tiles[_gridPosition.y][_gridPosition.x].GetComponent<Renderer>().enabled = true;
	}

	public override void OnRMBDown(Vector2 point)
	{
		_currentTile.Rotate();
		_currentTile.ApplyTerrain();
	}

	public override void OnLMBDown(Vector2 point)
	{
		TrackManager.SetTile(_currentTile);
	}

	public override void OnMouseOverTile(IntVector2 point)
	{
		if (_gridPosition.x != point.x || _gridPosition.y != point.y)
		{
			//placeholder to turn on/off tile rendering under current tile
			if(TrackManager.Tiles[_gridPosition.y][_gridPosition.x] != null)
				TrackManager.Tiles[_gridPosition.y][_gridPosition.x].GetComponent<Renderer>().enabled = true;

			if(TrackManager.Tiles[point.y][point.x] != null)
				TrackManager.Tiles[point.y][point.x].GetComponent<Renderer>().enabled = false;

			_gridPosition = point;
			_currentTile.GridPosition = _gridPosition;
			_currentTile.ApplyTerrain();
		}

		SomePrefab.transform.position = _currentTile.GetTransformPosition();
	}

	public override void Update()
	{
		if (Input.GetButtonDown("Control"))
		{
			_currentTile.Flip();
			_currentTile.ApplyTerrain();
		}
	}

	public override void UpdateGUI()
	{
		GUI.Label(new Rect(5, 140, 300, 20), "Current tile: " + _currentTile.FieldName);

		_scrollPosition = GUI.BeginScrollView(new Rect(5, 160, 330, Screen.height - 165), _scrollPosition, new Rect(5, 160, 310, (TileManager.TileList.Count/3 + 1) * 24), false, false);
		for (int i = 0; i < TileManager.TileList.Count; i++)
		{
			if (GUI.Button(new Rect(5 + (i%3)*105, 160 + 24 * (i/3), 100, 22), TileManager.TileList[i].Name.Remove(TileManager.TileList[i].Name.Length-4)))
			{
				SelectTile(i);
			}
		}
		GUI.EndScrollView();
	}

	private void SelectTile(int i)
	{
		if (i == SelectedTileId) return;

		SelectedTileId = i;

		TileManager.LoadModelForTileId(i);

		SomePrefab.GetComponent<MeshFilter>().mesh = TileManager.TileList[i].Model.CreateMeshes()[0];
		SomePrefab.GetComponent<Renderer>().materials = TileManager.TileList[i].Materials.ToArray();

		SomePrefab.position = new Vector3(SomePrefab.position.x, TileManager.TileList[i].Model.P3DMeshes[0].Height / 2, SomePrefab.position.z);

		_currentTile.SetupTile(new TrackTileSavable(), TileManager.TileList[i].Size, _gridPosition, TrackManager, TileManager.TileList[i].Name);
		_currentTile.ForceVerticiesUpdate();
		_currentTile.ApplyTerrain();
	}
}
