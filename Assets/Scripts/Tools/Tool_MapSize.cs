using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_MapSize : ToolGeneral
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

	private void ResetSizeChangers()
	{
		_addRight = _oldAddLeft = 0;
		_addLeft  = _oldAddLeft = 0;
		_addDown = _oldAddDown = 0;
		_addUp = _oldAddUp = 0;

		for(int i = 0; i < 9; i++)
			UpdateMapSizer(i);
	}

	public override void OnSelected()
	{
		ResetSizeChangers();

		for (int i = 0; i < 9; i++)
		{
			_mapSizers[i].GetComponent<MeshRenderer>().enabled = true;
			UpdateMapSizer(i);
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

	public override void OnMapSizeChange()
	{
		ResetSizeChangers();
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
		
	}

	public override void UpdateGUI(Rect guiRect)
	{
		GUI.Label(new Rect(guiRect.x, guiRect.y, 60, 30), "Right");
		if(CustomGuiControls.DrawIntSlider(new Rect(guiRect.x + 65, guiRect.y, 60, 30), ref _addRight))
		{
			int newWidth = TrackManager.CurrentTrack.Width + _addLeft + _addRight;
			int newHeight = TrackManager.CurrentTrack.Height + _addUp + _addDown;
			if ( newWidth <= 2 || newHeight*newWidth > TrackManager.MaxMapSizeLimit)
				_addRight = _oldAddRight;

			_oldAddRight = _addRight;
			UpdateMapSizer(2);
			UpdateMapSizer(5);
			UpdateMapSizer(8);
		}

		GUI.Label(new Rect(guiRect.x, guiRect.y + 40, 60, 30), "Left");
		if (CustomGuiControls.DrawIntSlider(new Rect(guiRect.x + 65, guiRect.y + 40, 60, 30), ref _addLeft))
		{
			int newWidth = TrackManager.CurrentTrack.Width + _addLeft + _addRight;
			int newHeight = TrackManager.CurrentTrack.Height + _addUp + _addDown;
			if ( newWidth <= 2 || newHeight*newWidth > TrackManager.MaxMapSizeLimit)
				_addLeft = _oldAddLeft;

			_oldAddLeft = _addLeft;
			UpdateMapSizer(0);
			UpdateMapSizer(3);
			UpdateMapSizer(6);
		}

		GUI.Label(new Rect(guiRect.x, guiRect.y + 80, 60, 30), "Up");
		if (CustomGuiControls.DrawIntSlider(new Rect(guiRect.x + 65, guiRect.y + 80, 60, 30), ref _addUp))
		{
			int newWidth = TrackManager.CurrentTrack.Width + _addLeft + _addRight;
			int newHeight = TrackManager.CurrentTrack.Height + _addUp + _addDown;
			if ( newHeight <= 2 || newHeight*newWidth > TrackManager.MaxMapSizeLimit)
				_addUp = _oldAddUp;

			_oldAddUp = _addUp;
			UpdateMapSizer(0);
			UpdateMapSizer(1);
			UpdateMapSizer(2);
		}

		GUI.Label(new Rect(guiRect.x, guiRect.y + 120, 60, 30), "Down");
		if(CustomGuiControls.DrawIntSlider(new Rect(guiRect.x + 65, guiRect.y + 120, 60, 30), ref _addDown))
		{
			int newWidth = TrackManager.CurrentTrack.Width + _addLeft + _addRight;
			int newHeight = TrackManager.CurrentTrack.Height + _addUp + _addDown;
			if ( newHeight <= 2 || newHeight*newWidth > TrackManager.MaxMapSizeLimit)
				_addDown = _oldAddDown;

			_oldAddDown = _addDown;
			UpdateMapSizer(6);
			UpdateMapSizer(7);
			UpdateMapSizer(8);
		}

		GUI.Label(new Rect(guiRect.x, guiRect.y + 160, guiRect.width, 100),
			"Current map size: " + TrackManager.CurrentTrack.Width + "x" + TrackManager.CurrentTrack.Height + 
			" = " + (TrackManager.CurrentTrack.Height*TrackManager.CurrentTrack.Width)+ "\n\n" + 
			"New map size: " + (TrackManager.CurrentTrack.Width + _addLeft + _addRight) + "x" + (TrackManager.CurrentTrack.Height + _addUp + _addDown) +
			" = " + (TrackManager.CurrentTrack.Width + _addLeft + _addRight)*(TrackManager.CurrentTrack.Height + _addDown + _addUp)+"/"+TrackManager.MaxMapSizeLimit);

		if (GUI.Button(new Rect(guiRect.x, guiRect.y + 220, guiRect.width, 45), "APPLY"))
		{
			TrackManager.UpdateTrackSize(_addLeft, _addRight, _addUp, _addDown);
			ResetSizeChangers();
		}
	}
}