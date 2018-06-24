using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour 
{

	private TileManager _tileManager;
	public int SelectedTileId;
	public Transform DummyShadowTile;


	// Use this for initialization
	void Start () 
	{
		_tileManager = GetComponent<TileManager>();	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
	    Plane plane = new Plane(Vector3.up, Vector3.zero);
	    float outFloat;
	    plane.Raycast(ray, out outFloat);
	    Vector3 pos = ray.GetPoint(outFloat);

	    pos.y = _tileManager.tileModels[SelectedTileId].P3DMeshes[0].Depth / 2;

        DummyShadowTile.transform.position = pos;
	}

	void OnGUI()
	{
		GUI.BeginScrollView (new Rect (Screen.width - 120, 10, 110, Screen.height - 10), new Vector2 (0, 0), new Rect (Screen.width - 110, 20, 100, Screen.height - 20));
		for(int i = 0; i < _tileManager.tileNames.Count; i++)
		{
			if(GUI.Button(new Rect(Screen.width - 110, 20*(i+1), 100, 18), _tileManager.tileNames[i]))
			{
				SelectedTileId = i;
				DummyShadowTile.GetComponent<MeshFilter> ().mesh = _tileManager.tileMeshes [i];

			}
		}
		GUI.EndScrollView ();
	}
}
