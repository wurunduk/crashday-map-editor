using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private TrackTileSavable trackTileSavable;
    public string size = "";

    public int PositionX, PositionY;

    Tile(TrackTileSavable trackTileSavable)
    {
        this.trackTileSavable = trackTileSavable;
    }

	public void SetTileSavable(TrackTileSavable trackTileSavable)
	{
		this.trackTileSavable = trackTileSavable;
	}

    public void SetRotation(byte rotation)
    {
        trackTileSavable.Rotation = rotation;
        transform.localRotation = Quaternion.Euler(0, (rotation - 1) * 90, 0);

        if (trackTileSavable.IsMirrored != 0)
        {
            if (rotation == 1 || rotation == 3)
            {
                transform.localScale = new Vector3(1, 1, -1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        if (size == "11")
        {
            transform.localPosition = new Vector3(PositionY * TrackManager.TileSize, transform.localPosition.y, PositionX * TrackManager.TileSize);
        }
        else if (size == "22")
        {
            transform.localPosition = new Vector3(PositionY * TrackManager.TileSize + TrackManager.TileSize / 2, transform.localPosition.y, PositionX * TrackManager.TileSize + TrackManager.TileSize / 2);
        }
        else if (size == "21")
        {
            if (rotation == 1 || rotation == 3)
            {
                transform.localPosition = new Vector3(PositionY * TrackManager.TileSize + TrackManager.TileSize / 2, transform.localPosition.y, PositionX * TrackManager.TileSize);
            }
            else
                transform.localPosition = new Vector3(PositionY * TrackManager.TileSize, transform.localPosition.y, PositionX * TrackManager.TileSize + TrackManager.TileSize - TrackManager.TileSize / 2);
        }
        else
        {
            if (rotation == 1 || rotation == 3)
            {
                transform.localPosition = new Vector3(PositionY * TrackManager.TileSize, transform.localPosition.y, PositionX * TrackManager.TileSize + TrackManager.TileSize / 2);
            }
            else
                transform.localPosition = new Vector3(PositionY * TrackManager.TileSize + TrackManager.TileSize, transform.localPosition.y, PositionX * TrackManager.TileSize);
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
