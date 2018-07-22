using System;
using UnityEngine;
using System.Collections.Generic;

public class TrackManager : MonoBehaviour
{
    public GameObject TilePrefab;
    public Transform MapParentTransform;
    public List<List<Transform>> Tiles;
	public TrackSavable CurrentTrack;

	public bool LoadedTrack;

    public static int TileSize = 20;

	private TileManager _tm;

	void Start()
	{
		_tm = GetComponent<TileManager>();
	}

	public void SetTile(Tile tile)
	{
		int index = CurrentTrack.FieldFiles.FindIndex(entry=>entry == tile.FieldName);
		if(index == -1)
		{
			CurrentTrack.FieldFiles.Add(tile.FieldName);
			index = CurrentTrack.FieldFilesNumber;
			CurrentTrack.FieldFilesNumber += 1;
		}

		tile._trackTileSavable.FieldId = Convert.ToUInt16(index);
		CurrentTrack.TrackTiles[tile.GridPosition.y][tile.GridPosition.x] = tile._trackTileSavable;

		UpdateTileAt(tile.GridPosition.x, tile.GridPosition.y);
	}

	public void UpdateTileAt(int x, int y)
	{
		if (CurrentTrack.TrackTiles[y][x].FieldId < CurrentTrack.FieldFilesNumber)
		{
			int index = _tm.TileList.FindIndex(entry=>entry.Name == CurrentTrack.FieldFiles[CurrentTrack.TrackTiles [y][x].FieldId]);
					
			//load our model in to the memory
			_tm.LoadModelForTileId(index);

			//The tile will be moved by the SetTile function later. The best moment to calcualte height is now.
			Tiles[y][x].transform.position = new Vector3(0, _tm.TileList[index].Model.P3DMeshes[0].Height / 2, 0);

			Tiles[y][x].name += _tm.TileList[index].Name;

			//set the model and textures for the tile
			Tiles[y][x].GetComponent<MeshFilter>().mesh = _tm.TileList[index].Model.CreateMeshes()[0];
			Tiles[y][x].GetComponent<Renderer>().materials = _tm.TileList[index].Materials;

			//Tile tile = Tiles[y][x].AddComponent<Tile>();
			Tiles[y][x].GetComponent<Tile>().SetupTile(CurrentTrack.TrackTiles [y][x], _tm.TileList[index].Size, new IntVector2(x, y), this, _tm.TileList[index].Name);
			Tiles[y][x].GetComponent<Tile>().ForceVerticiesUpdate();
			Tiles[y][x].GetComponent<Tile>().ApplyTerrain();
		}
	}

    public void LoadTrack(TrackSavable track)
    {
	    LoadedTrack = false;
	    CurrentTrack = track;

		//clear tiles left from the old loaded track
        for (int i = 0; i < MapParentTransform.childCount; i++)
        {
            Destroy(MapParentTransform.GetChild(i).gameObject);
        }

		GetComponent<TerrainManager>().GenerateTerrain();

		Tiles = new List<List<Transform>>(track.Height);
			
        for (int y = 0; y < track.Height; y++)
        {
			Tiles.Add(new List<Transform>(track.Width));
            for (int x = 0; x < track.Width; x++)
            {
	            GameObject newTile = (GameObject) Instantiate(TilePrefab, Vector3.zero, Quaternion.identity);

	            newTile.name = x + ":" + y + " ";
	            newTile.transform.SetParent(MapParentTransform);
	            newTile.AddComponent<Tile>();

	            Tiles[y].Add(newTile.transform);

				UpdateTileAt(x, y);
            }
        }

		FindObjectOfType<Camera>().gameObject.transform.localPosition = new Vector3(CurrentTrack.Width*10, 100, CurrentTrack.Height*-10);
		FindObjectOfType<Camera>().transform.LookAt(new Vector3(CurrentTrack.Width*20, 0, CurrentTrack.Height*-20));
	    LoadedTrack = true;
    }
}
