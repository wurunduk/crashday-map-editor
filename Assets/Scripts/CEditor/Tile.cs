using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private TrackTileSavable _trackTileSavable;
    private string _size;

    public Vector2 GridPosition;

    public void SetupTile(TrackTileSavable trackTileSavable, string size, Vector2 gridPosition)
    {
		_trackTileSavable = trackTileSavable;
	    _size = size;
	    GridPosition = gridPosition;

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

        if (_size == "11")
        {
            transform.localPosition = new Vector3(GridPosition.y * TrackManager.TileSize, transform.localPosition.y, GridPosition.x * TrackManager.TileSize);
        }
        else if (_size == "22")
        {
            transform.localPosition = new Vector3(GridPosition.y * TrackManager.TileSize + TrackManager.TileSize / 2.0f, transform.localPosition.y, GridPosition.x * TrackManager.TileSize + TrackManager.TileSize / 2.0f);
        }
        else if (_size == "21")
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

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
