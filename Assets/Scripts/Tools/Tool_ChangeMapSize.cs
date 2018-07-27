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
		_mapSizers = new GameObject[8];
		for (int i = 0; i < 8; i++)
		{
			List<Type> test = new List<Type>();
			test.Add(typeof(MeshRenderer));
			test.Add(typeof(MeshFilter));
			_mapSizers[i] = new GameObject("Map size display " + i, test.ToArray());
			_mapSizers[i].transform.parent = _parent.transform;

			_mapSizers[i].GetComponent<MeshRenderer>().enabled = false;
		}
	}

	public override void OnSelected()
	{
		_addRight = _oldAddLeft = 0;
		_addLeft  = _oldAddLeft = 0;
		_addDown = _oldAddDown = 0;
		_addUp = _oldAddUp = 0;

		for (int i = 0; i < 8; i++)
		{
			_mapSizers[i].GetComponent<MeshRenderer>().enabled = true;
		}
	}

	public override void OnDeselected()
	{
		for (int i = 0; i < 8; i++)
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

	public override void Update()
	{
		if (_oldAddLeft != _addLeft)
		{
			_oldAddLeft = _addLeft;
			_mapSizers[3].transform.position = new Vector3(-20*_addLeft,5,-1*TrackManager.CurrentTrack.Height*10);
			_mapSizers[3].GetComponent<MeshFilter>().mesh = MeshGenerator.GenerateCubeMesh(1, new Vector3(Mathf.Abs(_addLeft)*20, 10, TrackManager.CurrentTrack.Height*20));
			_mapSizers[3].GetComponent<MeshRenderer>().materials = _addLeft > 0 ? new[] {_greenMaterial} : new[] {_redMaterial};
		}
	}

	public override void UpdateGUI()
	{
		GUI.Label(new Rect(5, 160, 60, 30), "Right");
		_addRight = DrawIntSlider(_addRight, new Rect(5 + 65, 160, 60, 30));

		GUI.Label(new Rect(5, 160 + 60, 60, 30), "Left");
		_addLeft = DrawIntSlider(_addLeft, new Rect(5 + 65, 160 + 60, 60, 30));

		GUI.Label(new Rect(5, 160 + 120, 60, 30), "Up");
		_addUp = DrawIntSlider(_addUp, new Rect(5 + 65, 160 + 120, 60, 30));

		GUI.Label(new Rect(5, 160 + 180, 60, 30), "Down");
		_addDown = DrawIntSlider(_addDown, new Rect(5 + 65, 160 + 180, 60, 30));

		if (GUI.Button(new Rect(5, 160 + 210, 330, 45), "APPLY"))
		{

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