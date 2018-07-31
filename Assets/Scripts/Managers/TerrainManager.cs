using UnityEngine;
using System.Collections;

public class TerrainManager : MonoBehaviour
{
	private TrackManager _tm;
	public Transform Terrain;

	private Vector3[] _terrainVertices;

	void Awake()
	{
		_tm = GetComponent<TrackManager>();
	}

	/// <summary>
	/// Get space coordinates of the point on the terrain, where mouse points. 
	/// If the terrain is not present, return poosition on the zero plane
	/// </summary>
	/// <returns>Position in space</returns>
	public Vector3 GetMousePointOnTerrain()
	{
		//get the point on the Terrain where our mouse currenlty points
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
		RaycastHit info;
		Vector3 pos;
		if (Terrain.GetComponent<MeshCollider>().Raycast(ray, out info, float.MaxValue))
		{
			pos = info.point;
		}
		else
		{
			Plane plane = new Plane(Vector3.up, Vector3.zero);
			float outFloat;
			plane.Raycast(ray, out outFloat);
			pos = ray.GetPoint(outFloat);
		}

		return pos;
	}

	/// <summary>
	/// Deformates the given mesh accroding to the position on the map and the terrain
	/// </summary>
	/// <param name="mesh">Mesh which will be deformed</param>
	/// <param name="gridPosition">Position on the map</param>
	/// <param name="rotation">rotation byte 0 - not rotated , 1 - 90 etc</param>
	/// <param name="size">size of the mesh in tiles</param>
	/// <param name="isMirrored">should the mesh be mirrored</param>
	public void ApplyTerrainToMesh(Mesh mesh, IntVector2 gridPosition, int rotation, IntVector2 size, bool isMirrored)
	{
		Vector3[] vertices = mesh.vertices;

		ApplyTerrainToMesh(ref vertices, gridPosition, rotation, size, isMirrored);
		mesh.vertices = vertices;
		mesh.RecalculateBounds();
	}

	/// <summary>
	/// Deformates the given array of verticies accroding to the position on the map and the terrain
	/// </summary>
	/// <param name="vertices">vertices which will be deformed</param>
	/// <param name="gridPosition">Position on the map</param>
	/// <param name="rotation">rotation byte 0 - not rotated , 1 - 90 etc</param>
	/// <param name="size">size of the mesh in tiles</param>
	/// <param name="isMirrored">should the mesh be mirrored</param>
	public void ApplyTerrainToMesh(ref Vector3[] vertices, IntVector2 gridPosition, int rotation, IntVector2 size, bool isMirrored)
	{
		Quaternion rot = Quaternion.Euler(0, rotation*-90, 0);
		for (int i = 0; i < vertices.Length; i++)
		{
			Vector3 vertPosRotated = vertices[i];


			//do you want to ask why this works? Well, i dont know
			vertPosRotated.z *= -1;
			vertPosRotated = rot*vertPosRotated;

			if(isMirrored)
			{
				vertPosRotated.x *= -1;
			}

			float vertPosX;
			float vertPosY;
			if(rotation%2 == 1 && size.x + size.y == 3)
			{
				vertPosX = ((gridPosition.x) * 20 + (vertPosRotated.x+10*size.y))/5.0f;
				vertPosY = ((gridPosition.y) * 20 + (vertPosRotated.z+10*size.x))/5.0f;
			}
			else
			{
				vertPosX = ((gridPosition.x) * 20 + (vertPosRotated.x+10*size.x))/5.0f;
				vertPosY = ((gridPosition.y) * 20 + (vertPosRotated.z+10*size.y))/5.0f;
			}

			int posX = Mathf.FloorToInt(vertPosX);
			int posY = Mathf.FloorToInt(vertPosY);

			if(posX < 0 || posY < 0 || posX> _tm.CurrentTrack.Width*4 || posY> _tm.CurrentTrack.Height*4) continue;

			float height;

			float dx = (vertPosX - posX);
			float dy = (vertPosY - posY);

			if ((dx + dy) <= 1)
			{
				height = _tm.CurrentTrack.Heightmap[posY][posX];
				if(posX != _tm.CurrentTrack.Width*4)
					height += (_tm.CurrentTrack.Heightmap[posY][posX+1] - _tm.CurrentTrack.Heightmap[posY][posX]) * dx;
				if(posY != _tm.CurrentTrack.Height*4)
					height += (_tm.CurrentTrack.Heightmap[posY+1][posX] - _tm.CurrentTrack.Heightmap[posY][posX]) * dy;
			}
			else
			{
				height = _tm.CurrentTrack.Heightmap[posY+1][posX+1];
				height += (_tm.CurrentTrack.Heightmap[posY][posX+1] - _tm.CurrentTrack.Heightmap[posY+1][posX+1]) * (1.0f - dy);
				height += (_tm.CurrentTrack.Heightmap[posY+1][posX] - _tm.CurrentTrack.Heightmap[posY+1][posX+1]) * (1.0f - dx);
			}

			vertices[i].y += height * _tm.CurrentTrack.GroundBumpyness * 5;
		}
	}

	/// <summary>
	/// Updates the heightmap point of the terrain mesh in the given point accroding to CurrentTrack's heightmap
	/// !Call PushTerrainChanges() after changing needed vertices to actually update the mesh
	/// </summary>
	/// <param name="pos">grid position of the vertex to be updated</param>
	public void UpdateTerrain(IntVector2 pos)
	{
		_terrainVertices[pos.x + (_tm.CurrentTrack.Width*4+1)*pos.y] = 
			new Vector3(-10 + pos.x*5, _tm.CurrentTrack.Heightmap[pos.y][pos.x]*_tm.CurrentTrack.GroundBumpyness*5, 10 + pos.y*-5);
	}

	/// <summary>
	/// Applied local terrain vertices to the mesh
	/// </summary>
	public void PushTerrainChanges()
	{
		Terrain.GetComponent<MeshFilter>().mesh.vertices = _terrainVertices;
		Terrain.GetComponent<MeshFilter>().mesh.RecalculateBounds();
	}

	/// <summary>
	/// Update Terrain's collider
	/// </summary>
	public void UpdateCollider()
	{
		Terrain.GetComponent<MeshCollider>().sharedMesh = Terrain.GetComponent<MeshFilter>().sharedMesh;
	}

	/// <summary>
	/// Generates a mesh according to CurrentTrack's heightmap
	/// </summary>
	public void GenerateTerrain()
	{
		int sizeX = _tm.CurrentTrack.Width * 4 + 1;
		int sizeY = _tm.CurrentTrack.Height * 4 + 1;

		_terrainVertices = new Vector3[sizeX*sizeY];
		Vector3[] normals = new Vector3[sizeX*sizeY];
		Vector2[] uvs = new Vector2[sizeX*sizeY];
		int[] tris = new int[(sizeX*sizeY-sizeX)*6];

		for (int y = 0; y < sizeY; y++)
		{
			for (int x = 0; x < sizeX; x++)
			{
				//-10 is half of the Tile size to the center the terrain. According to thethe tile is divided every 5 meters
				_terrainVertices[x+sizeX*y] = new Vector3(-10 + x*5, _tm.CurrentTrack.Heightmap[y][x]*_tm.CurrentTrack.GroundBumpyness*5, 10 + y*-5);
				normals[x + sizeX * y] = Vector3.down;
				uvs[x+sizeX*y] = new Vector2(x*10%sizeX, y%sizeY);
			}
		}

		for (int i = 0; i < tris.Length / 6; i++) 
		{			
			if ((i + 1)%sizeX == 0)
			{
				continue;
			}

			tris[i * 6 + 2] = i + sizeX;
			tris[i * 6 + 1] = i + sizeX + 1;
			tris[i * 6 + 0] = i + 1;
			
			tris[i * 6 + 5] = i + sizeX;
			tris[i * 6 + 4] = i + 1;
			tris[i * 6 + 3] = i;
		}

		Mesh m = new Mesh();
		m.vertices = _terrainVertices;
		m.triangles = tris;
		m.normals = normals;
		m.uv = uvs;
		m.RecalculateNormals();
		m.RecalculateBounds();
		Terrain.GetComponent<MeshFilter>().mesh = m;
		

		Terrain.GetComponent<MeshCollider>().sharedMesh = Terrain.GetComponent<MeshFilter>().sharedMesh;
	}
}
