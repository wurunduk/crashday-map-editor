using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolManager : MonoBehaviour 
{
	public Transform SomePrefab;

	private TileManager _tileManager;
	private TrackManager _trackManager;
	private TerrainManager _terrainManager;

	private List<ToolGeneral> _tools;
	private ToolGeneral _currentTool;

	void Awake()
	{
		_trackManager = GetComponent<TrackManager>();
		_tileManager = GetComponent<TileManager>();
		_terrainManager = GetComponent<TerrainManager>();

		_tools = new List<ToolGeneral>();
	}

	void Start ()
	{
		InitializeTool(new Tool_TilePlace());
		InitializeTool(new Tool_TileEdit());

		_currentTool = _tools[0];
		_currentTool.OnSelected();
	}

	private void InitializeTool(ToolGeneral tool)
	{
		_tools.Add(tool);
		tool.TrackManager = _trackManager;
		tool.TileManager = _tileManager;
		tool.TerrainManager = _terrainManager;
		tool.SomePrefab = SomePrefab;
	}

	void OnGUI()
	{
		int i = 0;
		foreach (var tool in _tools)
		{
			if (GUI.Button(new Rect(10, 20 * (i++ + 1) + 55, 100, 18), tool.ToolName))
			{
				_currentTool.OnDeselected();
				_currentTool = tool;
				_currentTool.OnSelected();
			}
		}

		_currentTool.UpdateGUI();
	}
	
	void Update ()
	{
		if (!_tileManager.Loaded || _trackManager.CurrentTrackState == TrackManager.TrackState.TrackEmpty) return;

		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
		RaycastHit info;
		_terrainManager.Terrain.GetComponent<MeshCollider>().Raycast(ray, out info, float.MaxValue);
		Vector3 pos = info.point;

		IntVector2 newGridPosition = new IntVector2(Mathf.Clamp(Mathf.RoundToInt(pos.x / 20), 0, _trackManager.CurrentTrack.Width-1), 
			Mathf.Clamp(-1*Mathf.RoundToInt(pos.z / 20), 0, _trackManager.CurrentTrack.Height-1));

		_currentTool.OnMouseOverTile(newGridPosition);

		if (GUIUtility.hotControl != 0) return;

		if(Input.GetMouseButtonDown(0))
			_currentTool.OnLMBDown(pos);

		if(Input.GetMouseButtonDown(1))
			_currentTool.OnRMBDown(pos);
	}
}
