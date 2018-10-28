using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TerrainBrush
{
	public float _currentHeight;

	//using floats so we can pass the array into shader
	//techincally this are still ints - indexes of the vertices selected
	public List<float> _selectedPoints;

	public TerrainBrush()
	{
		_selectedPoints = new List<float>();
	}

	public void ClearSelectedPoints()
	{
		_selectedPoints.Clear();
		_currentHeight = 0.0f;
	}

	public void AddPointToSelection(float pointId, float height)
	{
		_currentHeight *= _selectedPoints.Count;
		_selectedPoints.Add(pointId);
		_currentHeight += height;
		_currentHeight /= _selectedPoints.Count;
	}

	public void RemovePointFromSelection(int id, float height)
	{
		_currentHeight *= _selectedPoints.Count;
		_currentHeight -= height;
		_selectedPoints.RemoveAt(id);
		_currentHeight /= _selectedPoints.Count;
	}
}

public class Tool_TerrainEdit : ToolGeneral
{
	private List<TT_General> _tools;
	private TT_General _currentTool;

	public override void Initialize()
	{
		ToolName = "Edit Terrain";
		_tools = new List<TT_General>();

		InitializeTool(new TT_Set());
		_currentTool = _tools[0];
		_currentTool.OnSelected();
	}

	private void InitializeTool(TT_General tool)
	{
		_tools.Add(tool);
		tool.TrackManager = TrackManager;
		tool.TileManager = TileManager;
		tool.TerrainManager = TerrainManager;
		tool.SomePrefab = SomePrefab;
		tool.ModelPrefab = ModelPrefab;
		tool.Initialize();
	}

	public override void OnMapSizeChange()
	{
		_currentTool.OnMapSizeChange();
	}

	public override void OnSelected()
	{
		_currentTool.OnSelected();
		TerrainManager.Terrain.GetComponent<MeshRenderer>().enabled = true;
	}

	public override void OnDeselected()
	{
		_currentTool.OnDeselected();
		TerrainManager.Terrain.GetComponent<MeshRenderer>().enabled = false;
	}

	public override void Update()
	{
		_currentTool.Update();
	}

	public override void OnLMBUp(Vector3 point)
	{
		TerrainManager.UpdateCollider();
		_currentTool.OnLMBUp(point);
	}

	public override void OnLMBDown(Vector3 point)
	{
		_currentTool.OnLMBDown(point);
	}

	public override void OnLMB(Vector3 point)
	{
		_currentTool.OnLMB(point);
	}

	public override void OnRMB(Vector3 pos)
	{
		_currentTool.OnRMB(pos);
	}

	public override void UpdateGUI(Rect guiRect)
	{
		int i = 0;
		//draw button for every tool
		foreach (var tool in _tools)
		{
			if (GUI.Button(new Rect(5 + (i % 3) * 20, 22 * (i++ / 3) + 120, 30, 20), tool.ToolName))
			{
				_currentTool.OnDeselected();
				_currentTool = tool;
				_currentTool.OnSelected();
			}
		}
		_currentTool.UpdateGUI(new Rect(guiRect.x, guiRect.y+60, guiRect.width, guiRect.height - 60));
	}
}