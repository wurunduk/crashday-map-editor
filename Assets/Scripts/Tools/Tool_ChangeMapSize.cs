using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_ChangeMapSize : ToolGeneral
{
	public int SelectedTileId;

	private int _addRight, _addLeft, _addDown, _addUp;
	private int _oldAddRight, _oldAddLeft, _oldAddDown, _oldAddUp;

	private GameObject[] _mapSizers;

	private GameObject _parent;

	private Material _redMaterial;
	private Material _greenMaterial;

	public override void Initialize()
	{
		ToolName = "Map size";

		_redMaterial = Resources.Load<Material>("RedTransparent");
		_greenMaterial = Resources.Load<Material>("GreenTransparent");

		_parent = new GameObject("Map size parent");
		_mapSizers = new GameObject[9];
		for (int i = 0; i < 9; i++)
		{
			List<Type> test = new List<Type>();
			test.Add(typeof(MeshRenderer));
			test.Add(typeof(MeshFilter));
			_mapSizers[i] = new GameObject("Map size display " + i, test.ToArray());
			_mapSizers[i].transform.parent = _parent.transform;

			_mapSizers[i].GetComponent<MeshRenderer>().enabled = false;
		}
	}

	private void ResetSizeChanges()
	{
		_addRight = _oldAddLeft = 0;
		_addLeft  = _oldAddLeft = 0;
		_addDown = _oldAddDown = 0;
		_addUp = _oldAddUp = 0;
	}

	public override void OnSelected()
	{
		ResetSizeChanges();

		for (int i = 0; i < 9; i++)
		{
			_mapSizers[i].GetComponent<MeshRenderer>().enabled = true;
		}

		//turn off the middle sizer
		_mapSizers[4].GetComponent<MeshRenderer>().enabled = false;
	}

	public override void OnDeselected()
	{
		for (int i = 0; i < 9; i++)
		{
			_mapSizers[i].GetComponent<MeshRenderer>().enabled = false;
		}
	}

	public override void OnRMBDown(Vector2 point)
	{
		
	}

	public override void OnLMBDown(Vector2 point)
	{
		
	}

	public override void OnMouseOverTile(IntVector2 point)
	{

	}

	private int S(int a, int firstChoice, int secChoice, int thirdChoice)
	{
		if (a < 0) 
			return firstChoice;

		if (a > 0) 
			return secChoice;

		return thirdChoice;
	}

	private void UpdateMapSizer(int id)
	{
		int x = id % 3 - 1;
		int y = id / 3 - 1;
		_mapSizers[id].transform.position = new Vector3(x*(10*S(x, _addLeft, _addRight, 0) + 10) + (x+1)*(TrackManager.CurrentTrack.Width-1)*10, 
			5, 
			-1*(y*(10*S(y, _addUp, _addDown, 0) + 10) + (y+1)*(TrackManager.CurrentTrack.Height-1)*10));

		_mapSizers[id].GetComponent<MeshFilter>().mesh = MeshGenerator.GenerateCubeMesh(1, new Vector3(
			Mathf.Abs(S(x, _addLeft, _addRight, TrackManager.CurrentTrack.Width))*20, 
			10, 
			Mathf.Abs(S(y, _addUp, _addDown, TrackManager.CurrentTrack.Height))*20 ) );

		_mapSizers[id].GetComponent<MeshRenderer>().materials = S(x, _addLeft, _addRight, 0) >= 0 && S(y, _addUp, _addDown, 0) >= 0 ? new[] {_greenMaterial} : new[] {_redMaterial};
	}

	public override void Update()
	{
		if (_oldAddLeft != _addLeft)
		{
			_oldAddLeft = _addLeft;
			UpdateMapSizer(0);
			UpdateMapSizer(3);
			UpdateMapSizer(6);
		}

		if (_oldAddRight != _addRight)
		{
			_oldAddRight = _addRight;
			UpdateMapSizer(2);
			UpdateMapSizer(5);
			UpdateMapSizer(8);
		}

		if (_oldAddUp != _addUp)
		{
			_oldAddUp = _addUp;
			UpdateMapSizer(0);
			UpdateMapSizer(1);
			UpdateMapSizer(2);
		}

		if (_oldAddDown != _addDown)
		{
			_oldAddDown = _addDown;
			UpdateMapSizer(6);
			UpdateMapSizer(7);
			UpdateMapSizer(8);
		}
	}

	public override void UpdateGUI()
	{
		GUI.Label(new Rect(5, 160, 60, 30), "Right");
		_addRight = DrawIntSlider(_addRight, new Rect(5 + 65, 160, 60, 30));

		GUI.Label(new Rect(5, 160 + 40, 60, 30), "Left");
		_addLeft = DrawIntSlider(_addLeft, new Rect(5 + 65, 160 + 40, 60, 30));

		GUI.Label(new Rect(5, 160 + 80, 60, 30), "Up");
		_addUp = DrawIntSlider(_addUp, new Rect(5 + 65, 160 + 80, 60, 30));

		GUI.Label(new Rect(5, 160 + 120, 60, 30), "Down");
		_addDown = DrawIntSlider(_addDown, new Rect(5 + 65, 160 + 120, 60, 30));

		GUI.Label(new Rect(5, 160 + 160, 200, 330),
			"Current map size: " + TrackManager.CurrentTrack.Width + "x" + TrackManager.CurrentTrack.Height + "\n" + 
			"New map size: " + (TrackManager.CurrentTrack.Width + _addLeft + _addRight) + "x" + (TrackManager.CurrentTrack.Height + _addUp + _addDown));

		if (GUI.Button(new Rect(5, 160 + 410, 330, 45), "APPLY"))
		{
			TrackManager.UpdateTrackSize(_addLeft, _addRight, _addUp, _addDown);
			ResetSizeChanges();
			for(int i = 0; i < 9; i++)
				UpdateMapSizer(i);
		}
	}

	public static int DrawIntSlider(int number, Rect position)
	{

		string fieldText = GUI.TextField(new Rect(position.x + 30, position.y, 85, 25), number.ToString());

		int answer;

		//if we completely removed everything from the field, change it to 0
		if (string.IsNullOrEmpty(fieldText))
			fieldText = "0";

		if(!int.TryParse(fieldText, out answer))
			answer = number;

		if (GUI.Button(new Rect(position.x, position.y, 25, 25), "-"))
			answer -= 1;

		if (GUI.Button(new Rect(position.x + 120, position.y, 25, 25), "+"))
			answer += 1;

		return answer;
	}
}