using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ToolGeneral
{
    public string ToolName;
    public string ToolDescription;

	public TrackManager TrackManager;
	public TileManager TileManager;
	public TerrainManager TerrainManager;
	public Transform SomePrefab;

	public abstract void OnSelected();
	public abstract void OnDeselected();

	public abstract void OnLMBDown(Vector2 point);
	public abstract void OnRMBDown(Vector2 point);

	public abstract void OnMouseOverTile(IntVector2 point);
	public abstract void UpdateGUI();
	public abstract void Update();
}
