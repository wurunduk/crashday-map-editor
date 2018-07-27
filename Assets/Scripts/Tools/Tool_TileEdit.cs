using UnityEngine;
using System.Collections;

public class Tool_TileEdit : ToolGeneral
{
	private IntVector2 _gridPosition;
	private TrackTileSavable _currentTile;

	public int SelectedTileId;

	public override void Initialize()
	{
		ToolName = "Edit Tiles";
		_gridPosition = new IntVector2(0, 0);
	}

	public override void OnSelected()
	{
		_currentTile = TrackManager.CurrentTrack.TrackTiles[0][0];
	}

	public override void OnDeselected()
	{

	}

	public override void OnRMBDown(Vector2 point)
	{
		
	}

	public override void OnLMBDown(Vector2 point)
	{
		_currentTile = TrackManager.CurrentTrack.TrackTiles[_gridPosition.y][_gridPosition.x];
	}

	public override void OnMouseOverTile(IntVector2 point)
	{
		if (_gridPosition.x != point.x || _gridPosition.y != point.y)
		{
			_gridPosition = point;
		}
	}

	public override void Update()
	{
		
	}

	public override void UpdateGUI()
	{
		
	}
}