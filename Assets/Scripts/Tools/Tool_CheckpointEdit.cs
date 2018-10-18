using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_CheckpointEdit : ToolGeneral
{

	public override void Initialize()
	{
		ToolName = "Edit Checkpoints";
	}

	public override void OnSelected() {}

	public override void OnDeselected() {}

	public override void OnRMBDown(Vector3 point) {}

	public override void OnLMBDown(Vector3 point) {}

	public override void OnLMB(Vector3 point) { }

	public override void OnRMB(Vector3 point) { }

	public override void OnMouseOverTile(IntVector2 point) {}

	public override void Update() {}

	public override void UpdateGUI(Rect guiRect) {}
}
