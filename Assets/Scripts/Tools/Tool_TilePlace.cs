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
		_currentTile.SetupTile(new TrackTileSavable(), new IntVector2(1,1), new IntVector2(0,0), TerrainManager, "field.cfl");

		SelectTile(1);
	}

	public override void OnDeselected()
	{
		SomePrefab.GetComponent<MeshRenderer>().enabled = false;

		ShowTiles(_gridPosition, _currentTile.Size, true);
	}

	public override void OnRMBDown(Vector3 point)
	{
		ShowTiles(_gridPosition, _currentTile.Size, true);
		_currentTile.Rotate();
		_currentTile.ApplyTerrain();
		ShowTiles(_gridPosition, _currentTile.Size, false);
	}

	public override void OnLMBDown(Vector3 point)
	{
		TrackManager.SetTile(_currentTile);
	}

	public override void OnMouseOverTile(IntVector2 point)
	{
		if (_gridPosition.x != point.x || _gridPosition.y != point.y)
		{
			ShowTiles(_gridPosition, _currentTile.Size, true);
			ShowTiles(point, _currentTile.Size, false);

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

	public override void UpdateGUI(Rect guiRect)
	{
		GUI.Label(new Rect(guiRect.x, guiRect.y, guiRect.width, 20), "Current tile: " + _currentTile.FieldName);

		_scrollPosition = GUI.BeginScrollView(new Rect(guiRect.x, guiRect.y + 25, guiRect.width, Screen.height - 165), _scrollPosition, 
			new Rect(guiRect.x, guiRect.y + 25, guiRect.width-10, (TileManager.TileList.Count/3 + 1) * 24), false, false);
		for (int i = 0; i < TileManager.TileList.Count; i++)
		{
			if (GUI.Button(new Rect(guiRect.x + (i%3)*105, guiRect.y + 25 + 24 * (i/3), guiRect.width/3 - 10, 22), TileManager.TileList[i].Name.Remove(TileManager.TileList[i].Name.Length-4)))
			{
				SelectTile(i);
			}
		}
		GUI.EndScrollView();
	}

	private void ShowTiles(IntVector2 pos, IntVector2 size, bool show)
	{
		for (int y = 0; y < size.y; y++)
		{
			for (int x = 0; x < size.x; x++)
			{
				if (_currentTile._trackTileSavable.Rotation % 2 == 1)
				{
					if (pos.x + y < TrackManager.CurrentTrack.Width && pos.y + x < TrackManager.CurrentTrack.Height)
					{
						TrackManager.Tiles[pos.y + x][pos.x + y].GetComponent<Renderer>().enabled = show;
					}
				}
				else
				{
					if (pos.x + x < TrackManager.CurrentTrack.Width && pos.y + y < TrackManager.CurrentTrack.Height)
					{
						TrackManager.Tiles[pos.y + y][pos.x + x].GetComponent<Renderer>().enabled = show;
					}
				}
			}
		}
	}

	private void SelectTile(int i)
	{
		if (i == SelectedTileId) return;

		ShowTiles(_gridPosition, _currentTile.Size, true);

		SelectedTileId = i;

		TileManager.LoadModelForTileId(i);

		Mesh m = TileManager.TileList[i].Model.CreateMesh();
		SomePrefab.GetComponent<MeshFilter>().mesh = m;
		SomePrefab.GetComponent<Renderer>().materials = TileManager.TileList[i].Materials.ToArray();

		SomePrefab.position = new Vector3(SomePrefab.position.x, TileManager.TileList[i].Model.P3DMeshes[0].Height / 2, SomePrefab.position.z);

		_currentTile.SetupTile(new TrackTileSavable(), TileManager.TileList[i].Size, _gridPosition, TerrainManager, TileManager.TileList[i].Name);
		_currentTile.SetOriginalVertices(m.vertices);
		_currentTile.ApplyTerrain();

		ShowTiles(_gridPosition, _currentTile.Size, false);
	}
}
