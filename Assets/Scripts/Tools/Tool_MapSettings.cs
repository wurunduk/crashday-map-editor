using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_MapSettings : ToolGeneral
{
	public override void Initialize()
	{
		ToolName = "Map settings";
	}

	public override void UpdateGUI(Rect guiRect)
	{
		Rect position = new Rect(guiRect);
		position.height = 25;
		TrackManager.CurrentTrack.Author =
			CustomGuiControls.DrawNamedTextField(position, "Author", TrackManager.CurrentTrack.Author);

		position.y += 30;
		TrackManager.CurrentTrack.Comment = 
			CustomGuiControls.DrawNamedTextField(position, "Comment",  TrackManager.CurrentTrack.Comment);

		position.y += 30;
		TrackManager.CurrentTrack.Ambience = 
			CustomGuiControls.DrawNamedTextField(position, "Ambience",  TrackManager.CurrentTrack.Ambience);

		position.y += 30;
		GUI.Label(position, "Heightmap bumpyness");
		position.x += 145;
		if (CustomGuiControls.DrawFloatSlider(position, ref TrackManager.CurrentTrack.GroundBumpyness))
		{
			TrackManager.UpdateTerrain();
		}
		position.x -= 145;
	}
}
