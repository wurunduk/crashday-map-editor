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
		TrackLoaded
	};

	public TrackState CurrentTrackState;

    public static int TileSize = 20;

	public static int MaxMapSizeLimit = 2000;

	private TileManager _tm;
	private TerrainManager _terrainManager;

	void Awake()
	{
		_tm = GetComponent<TileManager>();
		_terrainManager = GetComponent<TerrainManager>();
	}


	public void UpdateTrackSize(int addLeft, int addRight, int addUp, int addDown)
	{
		//TODO:
		//Move DynamicObjects
		TrackSavable newTrack = new TrackSavable(CurrentTrack);

		newTrack.Width += (ushort) (addLeft + addRight);
		newTrack.Height += (ushort) (addUp + addDown);

		List<ushort> newCPs = new List<ushort>();

		for (int i = 0; i < CurrentTrack.CheckpointsNumber; i++)
		{
			int newPosX = CurrentTrack.Checkpoints[i] % CurrentTrack.Width;
			int newPosY = CurrentTrack.Checkpoints[i] / CurrentTrack.Height;

			newPosX += addLeft;
			newPosY += addUp;
			if(newPosX >= 0 && newPosX < newTrack.Width && newPosY >= 0 && newPosY < newTrack.Height)
				newCPs.Add((ushort)(newPosY * newTrack.Width + newPosX));
		}

		newTrack.CheckpointsNumber = (ushort)newCPs.Count;
		newTrack.Checkpoints = newCPs;


		newTrack.TrackTiles = new List<List<TrackTileSavable>>(newTrack.Height);

		for (int y = 0; y < newTrack.Height; y++)
		{
			newTrack.TrackTiles.Add(new List<TrackTileSavable>(newTrack.Width));
			for (int x = 0; x < newTrack.Width; x++)
			{
				TrackTileSavable tile;
				if (x - addLeft >= 0 && y - addUp >= 0 && x - addLeft < CurrentTrack.Width && y - addUp < CurrentTrack.Height)
				{
					tile = new TrackTileSavable(CurrentTrack.TrackTiles[y-addUp][x-addLeft]);
				}
				else
				{
					tile = new TrackTileSavable(0, 0, 0, 0);
				}

				newTrack.TrackTiles[y].Add(tile);
			}
		}

		newTrack.Heightmap = new List<List<float>>(newTrack.Height*4+1);

		for (int y = 0; y < newTrack.Height*4+1; y++)
		{
			newTrack.Heightmap.Add(new List<float>(newTrack.Width*4+1));
			for (int x = 0; x < newTrack.Width*4+1; x++)
			{
				if (x + addLeft*4 >= 0 && y + addUp*4 >= 0 && x + addLeft*4 < CurrentTrack.Width*4+1 && y + addUp*4 < CurrentTrack.Height*4+1)
				{
					newTrack.Heightmap[y].Add(CurrentTrack.Heightmap[y + addUp*4][x + addLeft*4]);
				}
				else
				{
					newTrack.Heightmap[y].Add(0.0f);
				}
			}
		}

		LoadTrack(newTrack);

		GetComponent<ToolManager>().OnMapSizeChange();
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
			Mesh m = _tm.TileList[index].Model.CreateMesh();
			Tiles[y][x].GetComponent<MeshFilter>().mesh = m;
			Tiles[y][x].GetComponent<Renderer>().materials = _tm.TileList[index].Materials.ToArray();

			Tiles[y][x].GetComponent<Tile>().SetupTile(CurrentTrack.TrackTiles [y][x], _tm.TileList[index].Size, new IntVector2(x, y), _terrainManager, _tm.TileList[index].Name);
			Tiles[y][x].GetComponent<Tile>().SetOriginalVertices(m.vertices);
			Tiles[y][x].GetComponent<Tile>().ApplyTerrain();
		}
	}

	public void UpdateTerrain()
	{
		for(int y = 0; y < CurrentTrack.Height; y++)
			for(int x = 0; x < CurrentTrack.Height; x++)
				UpdateTerrainAt(new IntVector2(x, y));
	}

	public void UpdateTerrainAt(IntVector2 pos)
	{
		Tiles[pos.y][pos.x].GetComponent<Tile>().ApplyTerrain();
	}

	public void LoadTrack()
	{
		LoadTrack(new TrackSavable());
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

	    GetComponent<ToolManager>().OnMapSizeChange();

	    CurrentTrackState = TrackState.TrackLoaded;
    }
}
