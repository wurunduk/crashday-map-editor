using UnityEngine;
using System.Collections;

public class Tool_TilePlace : ToolGeneral
{
	private Vector3 _scrollPosition;
	private IntVector2 _gridPosition;
	private Tile _currentTile;

	public int SelectedTileId;

	public override void OnSelected()
	{
		ToolName = "Place Tiles";
		_gridPosition = new IntVector2(0, 0);

		_currentTile = SomePrefab.GetComponent<Tile>();
		_currentTile.SetupTile(new TrackTileSavable(), new IntVector2(1,1), new IntVector2(0,0), TrackManager);
	}

	public override void OnDeselected()
	{

	}

	public override void OnRMBDown(Vector2 point)
	{
		_currentTile.Rotate();
		_currentTile.ApplyTerrain();
	}

	public override void OnLMBDown(Vector2 point)
	{
		TrackManager.CurrentTrack.TrackTiles[_gridPosition.x, _gridPosition.y] = _currentTile._trackTileSavable;
	}

	public override void OnMouseOverTile(IntVector2 point)
	{
		if (_gridPosition.x != point.x || _gridPosition.y != point.y)
		{
			//placeholder to turn on/off tile rendering under current tile
			if(TrackManager.Tiles[_gridPosition.x, _gridPosition.y] != null)
				TrackManager.Tiles[_gridPosition.x, _gridPosition.y].GetComponent<Renderer>().enabled = true;

			if(TrackManager.Tiles[point.x, point.y] != null)
				TrackManager.Tiles[point.x, point.y].GetComponent<Renderer>().enabled = false;

			_gridPosition = point;
			_currentTile.GridPosition = _gridPosition;
			_currentTile.ApplyTerrain();
		}

		SomePrefab.transform.position = _currentTile.GetTransformPosition();
	}

	public override void Update()
	{

	}

	public override void UpdateGUI()
	{
		_scrollPosition = GUI.BeginScrollView(new Rect(Screen.width - 120, 10, 110, Screen.height - 10), _scrollPosition, new Rect(Screen.width - 110, 20, 100, (TileManager.TileList.Count + 1) * 20));
		for (int i = 0; i < TileManager.TileList.Count; i++)
		{
			if (GUI.Button(new Rect(Screen.width - 110, 20 * (i + 1), 100, 18), TileManager.TileList[i].Name))
			{
				if (i == SelectedTileId) continue;

				SomePrefab.GetComponent<MeshFilter>().mesh = TileManager.TileList[i].Model.CreateMeshes()[0];
				SomePrefab.GetComponent<Renderer>().materials = TileManager.TileList[i].Materials;

				SomePrefab.position = new Vector3(SomePrefab.position.x, TileManager.TileList[i].Model.P3DMeshes[0].Height / 2, SomePrefab.position.z);

				_currentTile.SetupTile(new TrackTileSavable(), TileManager.TileList[i].Size, _gridPosition, TrackManager);
				_currentTile.ChangeVerticies(TileManager.TileList[i].Model.CreateMeshes()[0].vertices);
				_currentTile.ApplyTerrain();
			}
		}
		GUI.EndScrollView();
	}
}
