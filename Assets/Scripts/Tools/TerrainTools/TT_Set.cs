using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TT_Set : TT_General
{
	public float _oldHeight;
	public float _startHeight;

	public override void Initialize()
	{
		ToolName = "Set";
		base.Initialize();
	}

	public override void OnMapSizeChange()
	{
		OnSelected();
	}

	public override void OnSelected()
	{
		base.OnSelected();
	}

	public override void OnDeselected()
	{
		
	}

	public override void Update()
	{
		if (Input.GetButtonDown("Select all"))
		{
			if (brush._selectedPoints.Count > 0)
			{
				brush.ClearSelectedPoints();
				brush._selectedPoints.Clear();
			}
			else
			{
				for (int y = 0; y < TrackManager.CurrentTrack.Height * 4 + 1; y++)
				{
					for (int x = 0; x < TrackManager.CurrentTrack.Width * 4 + 1; x++)
					{
						brush.AddPointToSelection(GetPoint(new IntVector2(x, y)), TrackManager.CurrentTrack.Heightmap[y][x]);
					}
				}

				for (int y = 0; y < TrackManager.CurrentTrack.Height; y++)
					for (int x = 0; x < TrackManager.CurrentTrack.Width; x++)
						_selectedTiles.Add(new IntVector2(x, y));
			}

			UpdateTerrainShader();
		}
	}

	public override void OnLMBUp(Vector3 point)
	{
		TerrainManager.UpdateCollider();
	}

	public override void OnLMBDown(Vector3 point)
	{
		_startHeight = Input.mousePosition.y * 5 / Screen.height;
		_oldHeight = _startHeight;
	}

	public override void OnLMB(Vector3 point)
	{
		//calcualte how high we lifted the mouse
		float delta = Input.mousePosition.y * 5 / Screen.height - _oldHeight;

		//add height to the median point
		brush._currentHeight += delta;

		RaiseSelectedPoints(delta);

		_oldHeight = Input.mousePosition.y * 5 / Screen.height;
	}

	public override void OnRMB(Vector3 pos)
	{
		pos.x += TrackManager.TileSize / 2;
		pos.z -= TrackManager.TileSize / 2;

		IntVector2 gridPosition = new IntVector2(Mathf.Clamp(Mathf.RoundToInt(pos.x / 5), 0, TrackManager.CurrentTrack.Width * 4),
			Mathf.Clamp(-1 * Mathf.RoundToInt(pos.z / 5), 0, TrackManager.CurrentTrack.Height * 4));

		float p = GetPoint(gridPosition);

		//if we are in remove selection mode
		if (Input.GetButton("Deselect"))
		{
			//check if we are pressing on selected point and remove it if so
			int res = brush._selectedPoints.FindIndex(x => Mathf.Abs(x - p) < 0.01);
			if (res >= 0)
			{
				brush.RemovePointFromSelection(res, TrackManager.CurrentTrack.Heightmap[gridPosition.y][gridPosition.x]);
			}
		}
		else
		{
			//if current point is not selected
			//(the check is needed to avoid selecting one point multiple times)
			if (!brush._selectedPoints.Exists(x => Mathf.Abs(x - p) < 0.01))
			{
				brush.AddPointToSelection(GetPoint(gridPosition), TrackManager.CurrentTrack.Heightmap[gridPosition.y][gridPosition.x]);

				int ax = 0;
				int ay = 0;

				//also select the tile on the current height point and add it
				IntVector2 tilePos = new IntVector2(gridPosition.x, gridPosition.y);

				if (gridPosition.x % 4 == 0 && gridPosition.x > 0)
				{
					tilePos.x -= 1;
					if (gridPosition.x != TrackManager.CurrentTrack.Width * 4)
						ax += 1;
				}

				if (gridPosition.y % 4 == 0 && gridPosition.y > 0)
				{
					tilePos.y -= 1;
					if (gridPosition.y != TrackManager.CurrentTrack.Height * 4)
						ay += 1;
				}

				for (int y = 0; y <= ay; y++)
					for (int x = 0; x <= ax; x++)
						_selectedTiles.Add(new IntVector2(tilePos.x / 4 + x, tilePos.y / 4 + y));
			}
		}

		UpdateTerrainShader();
	}

	public override void UpdateGUI(Rect guiRect)
	{
		float height = brush._currentHeight;

		if (CustomGuiControls.DrawFloatSlider(new Rect(guiRect.x + 65, guiRect.y + 80, 60, 30), ref height, 0.5f))
		{
			RaiseSelectedPoints(height - brush._currentHeight);
			brush._currentHeight = height;
		}
	}

	private void RaiseSelectedPoints(float amount)
	{
		//for every selected point, change Track's heightmap, 
		//update terrain in the current point and move the heightpoint object
		foreach (var hp in brush._selectedPoints)
		{
			IntVector2 p = GetPoint(hp);
			TrackManager.CurrentTrack.Heightmap[p.y][p.x] += amount;
			TerrainManager.UpdateTerrain(p);
		}

		//update terrain's mesh to show the changes
		TerrainManager.PushTerrainChanges();

		//update wireframe shader
		UpdateTerrainShader();

		//also update every tile which is affected
		foreach (var st in _selectedTiles)
		{
			TrackManager.UpdateTerrainAt(st);
		}
	}

	private IntVector2 GetPoint(float p)
	{
		return new IntVector2(((int)p) % (TrackManager.CurrentTrack.Width * 4 + 1), ((int)p) / (TrackManager.CurrentTrack.Width * 4 + 1));
	}

	private float GetPoint(IntVector2 p)
	{
		return p.x + p.y * (TrackManager.CurrentTrack.Width * 4 + 1);
	}
}
