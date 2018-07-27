using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolManager : MonoBehaviour 
{
	public Transform SomePrefab;
	public GameObject ModelPrefab;

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
		InitializeTool(new Tool_ChangeMapSize());
		InitializeTool(new Tool_TilePlace());
		InitializeTool(new Tool_TileRemove());
		InitializeTool(new Tool_TerrainEdit());

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
		tool.ModelPrefab = ModelPrefab;
		tool.Initialize();
	}

	void OnGUI()
	{
		GUI.Box(new Rect(0, 0, 340, Screen.height), "");

		int i = 0;
		//draw button for every tool
		foreach (var tool in _tools)
		{
			if (GUI.Button(new Rect(5, 22 * (i++) + 45, 100, 20), tool.ToolName))
			{
				_currentTool.OnDeselected();
				_currentTool = tool;
				_currentTool.OnSelected();
			}
		}

		//draw gui of the current tool
		_currentTool.UpdateGUI();
	}
	
	void Update ()
	{
		if (!_tileManager.Loaded || _trackManager.CurrentTrackState == TrackManager.TrackState.TrackEmpty) return;

		//get the point on the Terrain where our mouse currenlty points
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
		RaycastHit info;
		Vector3 pos;
		if (_terrainManager.Terrain.GetComponent<MeshCollider>().Raycast(ray, out info, float.MaxValue))
		{
			pos = info.point;
		}
		else
		{
			Plane plane = new Plane(Vector3.up, Vector3.zero);
			float outFloat;
			plane.Raycast(ray, out outFloat);
			pos = ray.GetPoint(outFloat);
		}
		

		//calcualte tile position from position on the terrain
		IntVector2 newGridPosition = new IntVector2(Mathf.Clamp(Mathf.RoundToInt(pos.x / 20), 0, _trackManager.CurrentTrack.Width-1), 
			Mathf.Clamp(-1*Mathf.RoundToInt(pos.z / 20), 0, _trackManager.CurrentTrack.Height-1));

		//send updated information to the tool
		_currentTool.OnMouseOverTile(newGridPosition);

		_currentTool.Update();

		//only pass click events to tools if the click was in the world and not on UI
		if (GUIUtility.hotControl != 0) return;

		if(Input.GetMouseButtonDown(0))
			_currentTool.OnLMBDown(pos);

		if(Input.GetMouseButtonDown(1))
			_currentTool.OnRMBDown(pos);
	}
}
