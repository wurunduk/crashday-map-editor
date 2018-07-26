using System;
using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;

public class TrackManager : MonoBehaviour
{
    public GameObject TilePrefab;
    public Transform MapParentTransform;
    public List<List<Transform>> Tiles;
	public TrackSavable CurrentTrack;

	public enum TrackState
	{
		TrackEmpty,
		TrackStart,
		TracckLoaded
	};

	public TrackState CurrentTrackState;

    public static int TileSize = 20;

	private TileManager _tm;

	void Awake()
	{
		_tm = GetComponent<TileManager>();
	}

	public void SetTileByAtlasId(ushort atlasId, IntVector2 position)
	{
		if (CurrentTrackState == TrackState.TrackEmpty || atlasId >= CurrentTrack.FieldFilesNumber) return;

		CurrentTrack.TrackTiles[position.y][position.x] = new TrackTileSavable(atlasId,0,0,0);

		UpdateTileAt(position.x, position.y);
	}

	public void SetTile(Tile tile)
	{
		if (CurrentTrackState == TrackState.TrackEmpty) return;

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
			Tiles[y][x].position = new Vector3(0, _tm.TileList[index].Model.P3DMeshes[0].Height / 2, 0);

			Tiles[y][x].name = x + ":" + y + " " + _tm.TileList[index].Name;

			//set the model and textures for the tile
			Tiles[y][x].GetComponent<MeshFilter>().mesh = _tm.TileList[index].Model.CreateMeshes()[0];
			Tiles[y][x].GetComponent<Renderer>().materials = _tm.TileList[index].Materials;

			//Tile tile = Tiles[y][x].AddComponent<Tile>();
			Tiles[y][x].GetComponent<Tile>().SetupTile(CurrentTrack.TrackTiles [y][x], _tm.TileList[index].Size, new IntVector2(x, y), this, _tm.TileList[index].Name);
			Tiles[y][x].GetComponent<Tile>().ForceVerticiesUpdate();
			Tiles[y][x].GetComponent<Tile>().ApplyTerrain();
		}
	}

	public TrackSavable GenerateStartTrack()
	{
		TrackSavable track = new TrackSavable();
		track.Author = "Author";
		track.Comment = "A track made in 3d editor";
		track.Style = 0;
		track.Ambience = "day.amb";

		track.FieldFilesNumber = 2;
		track.FieldFiles = new List<string>(2);
		track.FieldFiles.Add("field.cfl");
		track.FieldFiles.Add("chkpoint.cfl");

		track.Height = 5;
		track.Width = 5;

		track.TrackTiles = new List<List<TrackTileSavable>>(5);

		for (int y = 0; y < track.Height; y++)
		{
			track.TrackTiles.Add(new List<TrackTileSavable>(5));
			for (int x = 0; x < track.Width; x++)
			{
				TrackTileSavable tile = new TrackTileSavable(0,0,0,0);
				if(x == 2 && y == 2)
					tile = new TrackTileSavable(1,0,0,0);
				track.TrackTiles[y].Add(tile);
			}
		}

		track.DynamicObjectFilesNumber = 0;
		track.DynamicObjectFiles = new List<string>();

		track.DynamicObjectsNumber = 0;
		track.DynamicObjects = new List<DynamicObjectSavable>();

		track.CheckpointsNumber = 1;
		track.Checkpoints = new List<ushort>();
		track.Checkpoints.Add(12);

		track.Permission = 0;
		track.GroundBumpyness = 1.0f;
		track.Scenery = 0;

		track.Heightmap = new List<List<float>>(21);

		for (int y = 0; y < track.Height*4 + 1; y++)
		{
			track.Heightmap.Add(new List<float>(21));
			for (int x = 0; x < track.Width*4 + 1; x++)
			{
				track.Heightmap[y].Add(0.0f);
			}
		}

		return track;
	}

	public void LoadTrack()
	{
		LoadTrack(GenerateStartTrack());
		CurrentTrackState = TrackState.TrackStart;
	}

    public void LoadTrack(TrackSavable track)
    {
	    CurrentTrackState = TrackState.TrackEmpty;
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

	    CurrentTrackState = TrackState.TracckLoaded;
    }
}
