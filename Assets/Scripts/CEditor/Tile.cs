using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TrackTileSavable _trackTileSavable;
    private IntVector2 _size;
	private TrackManager _tm;
	private Vector3[] _originalVerticies;

    public IntVector2 GridPosition;

    public void SetupTile(TrackTileSavable trackTileSavable, IntVector2 size, IntVector2 gridPosition, TrackManager tm)
    {
		_trackTileSavable = trackTileSavable;
	    _size = size;
	    GridPosition = gridPosition;
	    _tm = tm;

		SetRotation(trackTileSavable.Rotation);
    }

	public void Rotate()
	{
		byte newRot = _trackTileSavable.Rotation;
		newRot += 1;
		if (newRot > 3) newRot = 0;

		SetRotation(newRot);
	}

    public void SetRotation(byte rotation)
    {
        _trackTileSavable.Rotation = rotation;
        transform.localRotation = Quaternion.Euler(0, rotation * 90, 0);

		if(_trackTileSavable.IsMirrored != 0)
		{
			if(rotation%2 == 1)
				transform.localScale = new Vector3(1, 1, -1);
			else
				transform.localScale = new Vector3(-1, 1, 1);
		}

	    if (_size.x + _size.y == 3 && rotation%2 == 1)
	    {
			transform.localPosition = new Vector3((GridPosition.x + (_size.y/2)/2.0f)*TrackManager.TileSize, transform.localPosition.y, -1*(GridPosition.y + (_size.x/2)/2.0f)*TrackManager.TileSize);
	    }
	    else
	    {
		    transform.localPosition = new Vector3((GridPosition.x + (_size.x/2)/2.0f)*TrackManager.TileSize, transform.localPosition.y, -1*(GridPosition.y + (_size.y/2)/2.0f)*TrackManager.TileSize);
	    }
    }

	public void ChangeVerticies(Vector3[] newVerticies)
	{
		_originalVerticies = newVerticies;
	}

	public void ApplyTerrain()
	{
		Vector3[] vertices = GetComponent<MeshFilter>().mesh.vertices;

		if (_originalVerticies == null)
			_originalVerticies = vertices;

		Quaternion rot = Quaternion.Euler(0, _trackTileSavable.Rotation*-90, 0);
		for (int i = 0; i < vertices.Length; i++)
		{
			Vector3 vertPosRotated = vertices[i];


			//do you want to ask why this works? Well, i dont know
			vertPosRotated.z *= -1;
			vertPosRotated = rot*vertPosRotated;

			if(_trackTileSavable.IsMirrored != 0)
			{
				vertPosRotated.x *= -1;
			}

			//GridPosition.x*4 + (vertPosition+10*size)/(5*size) = 
			float vertPosX;
			float vertPosY;
			if(_trackTileSavable.Rotation%2 == 1 && _size.x + _size.y == 3)
			{
				vertPosX = ((GridPosition.x) * 20 + (vertPosRotated.x+10*_size.y))/5.0f;
				vertPosY = ((GridPosition.y) * 20 + (vertPosRotated.z+10*_size.x))/5.0f;
			}
			else
			{
				vertPosX = ((GridPosition.x) * 20 + (vertPosRotated.x+10*_size.x))/5.0f;
				vertPosY = ((GridPosition.y) * 20 + (vertPosRotated.z+10*_size.y))/5.0f;
			}

			int posX = Mathf.FloorToInt(vertPosX);
			int posY = Mathf.FloorToInt(vertPosY);

			if(posX < 0 || posY < 0 || posX> _tm.CurrentTrack.Width*4-1 || posY> _tm.CurrentTrack.Height*4-1) continue;

			float height;

			float dx = (vertPosX - posX);
			float dy = (vertPosY - posY);

			if ((dx + dy) < 1)
			{
				height = _tm.CurrentTrack.Heightmap[posX, posY];
				height += (_tm.CurrentTrack.Heightmap[posX+1, posY] - _tm.CurrentTrack.Heightmap[posX, posY]) * dx;
				height += (_tm.CurrentTrack.Heightmap[posX, posY+1] - _tm.CurrentTrack.Heightmap[posX, posY]) * dy;
			}
			else
			{
				height = _tm.CurrentTrack.Heightmap[posX+1, posY+1];
				height += (_tm.CurrentTrack.Heightmap[posX+1, posY] - _tm.CurrentTrack.Heightmap[posX+1, posY+1]) * (1.0f - dy);
				height += (_tm.CurrentTrack.Heightmap[posX, posY+1] - _tm.CurrentTrack.Heightmap[posX+1, posY+1]) * (1.0f - dx);
			}

			vertices[i].y = _originalVerticies[i].y + height*_tm.CurrentTrack.Height/20;
		}
			
		GetComponent<MeshFilter>().mesh.vertices = vertices;
		GetComponent<MeshFilter>().mesh.RecalculateBounds();
	}
}
