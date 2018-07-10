using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour 
{
	public Transform SomePrefab;

	private TileManager _tileManager;
	private TrackManager _trackManager;

	private List<ToolGeneral> _tools;
	private ToolGeneral _currentTool;

	// Use this for initialization
	void Start ()
	{
		_trackManager = GetComponent<TrackManager>();
		_tileManager = GetComponent<TileManager>();

		_tools = new List<ToolGeneral>();
		InitializeTool(new Tool_TilePlace());

		_currentTool = _tools[0];
		_currentTool.OnSelected();
	}

	private void InitializeTool(ToolGeneral tool)
	{
		_tools.Add(tool);
		tool.TrackManager = _trackManager;
		tool.TileManager = _tileManager;
		tool.SomePrefab = SomePrefab;
	}

	void OnGUI()
	{
		int i = 0;
		foreach (var tool in _tools)
		{
			if (GUI.Button(new Rect(15, 20 * (i + 1) + 50, 100, 18), tool.ToolName))
			{
				_currentTool.OnDeselected();
				_currentTool = tool;
				_currentTool.OnSelected();
			}
		}

		_currentTool.UpdateGUI();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!_tileManager.Loaded || !_trackManager.LoadedTrack) return;

		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
	    Plane plane = new Plane(Vector3.up, Vector3.zero);
	    float outFloat;
	    plane.Raycast(ray, out outFloat);
	    Vector3 pos = ray.GetPoint(outFloat);

		IntVector2 newGridPosition = new IntVector2(Mathf.Clamp(Mathf.RoundToInt(pos.x / 20), 0, _trackManager.CurrentTrack.Width-1), 
			Mathf.Clamp(-1*Mathf.RoundToInt(pos.z / 20), 0, _trackManager.CurrentTrack.Height-1));

		_currentTool.OnMouseOverTile(newGridPosition);

		if(Input.GetMouseButtonDown(0))
			_currentTool.OnLMBDown(pos);

		if(Input.GetMouseButtonDown(1))
			_currentTool.OnRMBDown(pos);
	}
}
