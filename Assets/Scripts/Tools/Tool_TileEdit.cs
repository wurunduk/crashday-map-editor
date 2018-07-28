using UnityEngine;
using System.Collections;

/// <summary>
/// This tool is not used and can be used as a copy for other tools
/// </summary>
public class Tool_TileEdit : ToolGeneral
{

	public override void Initialize()
	{
		ToolName = "Edit Tiles";
	}

	public override void OnSelected() {}

	public override void OnDeselected() {}

	public override void OnRMBDown(Vector2 point) {}

	public override void OnLMBDown(Vector2 point) {}

	public override void OnLMB(Vector2 point) { }

	public override void OnRMB(Vector2 point) { }

	public override void OnMouseOverTile(IntVector2 point) {}

	public override void Update() {}

	public override void UpdateGUI(Rect guiRect) {}
}