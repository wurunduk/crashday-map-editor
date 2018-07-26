using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_ChangeMapSize : ToolGeneral
{
	public int SelectedTileId;

	private int _addRight, _addLeft, _addDown, _addUp;

	public override void Initialize()
	{
		ToolName = "Map size";
	}

	public override void OnSelected()
	{
		_addRight = 0;
		_addLeft = 0;
		_addDown = 0;
		_addUp = 0;
	}

	public override void OnDeselected()
	{

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