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

	public GameObject ModelPrefab;

	public virtual void Initialize()
	{
	}

	public virtual void OnSelected()
	{
	}

	public virtual void OnDeselected()
	{
	}

	public virtual void OnLMBDown(Vector2 point)
	{
	}

	public virtual void OnLMB(Vector2 point)
	{
	}

	public virtual void OnRMBDown(Vector2 point)
	{
	}

	public virtual void OnRMB(Vector2 point)
	{
	}

	public virtual void OnMouseOverTile(IntVector2 point)
	{
	}

	public virtual void UpdateGUI()
	{
	}

	public virtual void Update()
	{
	}
}
