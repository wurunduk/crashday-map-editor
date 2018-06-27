using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private TrackTileSavable _trackTileSavable;
    private Vector2 _size;
	private TrackManager _tm;

    public Vector2 GridPosition;

    public void SetupTile(TrackTileSavable trackTileSavable, Vector2 size, Vector2 gridPosition, TrackManager tm)
    {
		_trackTileSavable = trackTileSavable;
	    _size = size;
	    GridPosition = gridPosition;
	    _tm = tm;

		SetRotation(trackTileSavable.Rotation);
    }

    public void SetRotation(byte rotation)
    {
        _trackTileSavable.Rotation = rotation;
        transform.localRotation = Quaternion.Euler(0, (rotation - 1) * 90, 0);

        if (_trackTileSavable.IsMirrored != 0)
        {
			//ghetto way of flipping
            if (rotation == 1 || rotation == 3)
            {
                transform.localScale = new Vector3(1, 1, -1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        if (_size.x == 1 && _size.y == 1)
        {
            transform.localPosition = new Vector3(GridPosition.y * TrackManager.TileSize, transform.localPosition.y, GridPosition.x * TrackManager.TileSize);
        }
        else if (_size.x == 2 && _size.y == 2)
        {
            transform.localPosition = new Vector3(GridPosition.y * TrackManager.TileSize + TrackManager.TileSize / 2.0f, transform.localPosition.y, GridPosition.x * TrackManager.TileSize + TrackManager.TileSize / 2.0f);
        }
        else if (_size.x == 2 && _size.y == 1)
        {
            if (rotation == 1 || rotation == 3)
            {
                transform.localPosition = new Vector3(GridPosition.y * TrackManager.TileSize + TrackManager.TileSize / 2.0f, transform.localPosition.y, GridPosition.x * TrackManager.TileSize);
            }
            else
                transform.localPosition = new Vector3(GridPosition.y * TrackManager.TileSize, transform.localPosition.y, GridPosition.x * TrackManager.TileSize + TrackManager.TileSize - TrackManager.TileSize / 2.0f);
        }
        else
        {
            if (rotation == 1 || rotation == 3)
            {
                transform.localPosition = new Vector3(GridPosition.y * TrackManager.TileSize, transform.localPosition.y, GridPosition.x * TrackManager.TileSize + TrackManager.TileSize / 2.0f);
            }
            else
                transform.localPosition = new Vector3(GridPosition.y * TrackManager.TileSize + TrackManager.TileSize, transform.localPosition.y, GridPosition.x * TrackManager.TileSize);
        }

    }

    public void Flip()
    {
        
    }

	public void ApplyTerrain()
	{
		Vector3[] vertices = GetComponent<MeshFilter>().mesh.vertices;

		Quaternion rot = new Quaternion()
		{
			eulerAngles = new Vector3(0, (_trackTileSavable.Rotation-1) * 90, 0)
		};

		for (int i = 0; i < vertices.Length; i++)
		{
			Vector3 vertPosRotated = rot*vertices[i];

			if (_trackTileSavable.IsMirrored != 0)
			{
				if (_trackTileSavable.Rotation == 1 || _trackTileSavable.Rotation == 3)
				{
					vertPosRotated.z *= -1;
				}
				else
				{
					vertPosRotated.x *= -1;
				}
			}

			float vertPosX = GridPosition.x * 4 + vertPosRotated.x / (5 * _size.x) + 2;
			float vertPosY = GridPosition.y * 4 + vertPosRotated.z / (5 * _size.y) + 2;
			int posX = Mathf.FloorToInt(vertPosX);
			int posY = Mathf.FloorToInt(vertPosY);

			if(posX < 0 || posY < 0 || posX> _tm.CurrentTrack.Width*4-1 || posY> _tm.CurrentTrack.Height*4-1) continue;

			float height;

			float dx = vertPosX - posX;
			float dz = vertPosY - posY;

			if ((dx + dz) < 1)
			{
				height = _tm.CurrentTrack.Heightmap[posX, posY];
				height += (_tm.CurrentTrack.Heightmap[posX+1, posY] - _tm.CurrentTrack.Heightmap[posX, posY])* dx;
				height += (_tm.CurrentTrack.Heightmap[posX, posY+1] - _tm.CurrentTrack.Heightmap[posX, posY])* dz;
			}
			else
			{
				height = _tm.CurrentTrack.Heightmap[posX+1, posY+1];
				height += (_tm.CurrentTrack.Heightmap[posX+1, posY] - _tm.CurrentTrack.Heightmap[posX+1, posY+1]) * (1.0f - dz);
				height += (_tm.CurrentTrack.Heightmap[posX, posY+1] - _tm.CurrentTrack.Heightmap[posX+1, posY+1]) * (1.0f - dx);
			}

			vertices[i].y += height*_tm.CurrentTrack.Height/20;
		}
			
		GetComponent<MeshFilter>().mesh.vertices = vertices;
	}
}
