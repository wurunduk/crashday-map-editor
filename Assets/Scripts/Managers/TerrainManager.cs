using UnityEngine;
using System.Collections;

public class TerrainManager : MonoBehaviour
{
	private TrackManager _tm;
	public Transform Terrain;

	void Awake()
	{
		_tm = GetComponent<TrackManager>();
	}

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

	public void ApplyTerrainToMesh(Mesh mesh, IntVector2 gridPosition, int rotation, IntVector2 size, bool isMirrored)
	{
		Vector3[] vertices = mesh.vertices;

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

			vertices[i].y += height*_tm.CurrentTrack.GroundBumpyness*5;
		}
			
		mesh.vertices = vertices;
		mesh.RecalculateBounds();
	}

	public void UpdateTerrain(IntVector2 pos)
	{
		Vector3[] verticies = Terrain.GetComponent<MeshFilter>().mesh.vertices;

		verticies[pos.x + (_tm.CurrentTrack.Width*4+1)*pos.y] = 
			new Vector3(-10 + pos.x*5, _tm.CurrentTrack.Heightmap[pos.y][pos.x]*_tm.CurrentTrack.GroundBumpyness*5, 10 + pos.y*-5);

		Terrain.GetComponent<MeshFilter>().mesh.vertices = verticies;
		Terrain.GetComponent<MeshFilter>().mesh.RecalculateBounds();
	}

	public void GenerateTerrain()
	{
		if (!_tm) return;

		int sizeX = _tm.CurrentTrack.Width * 4 + 1;
		int sizeY = _tm.CurrentTrack.Height * 4 + 1;

		Vector3[] verticies = new Vector3[sizeX*sizeY];
		Vector3[] normals = new Vector3[sizeX*sizeY];
		Vector2[] uvs = new Vector2[sizeX*sizeY];
		int[] tris = new int[(sizeX*sizeY-sizeX)*6];

		for (int y = 0; y < sizeY; y++)
		{
			for (int x = 0; x < sizeX; x++)
			{
				//-10 is half of the Tile size to the center the terrain. According to thethe tile is divided every 5 meters
				verticies[x+sizeX*y] = new Vector3(-10 + x*5, _tm.CurrentTrack.Heightmap[y][x]*_tm.CurrentTrack.GroundBumpyness*5, 10 + y*-5);
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
		m.vertices = verticies;
		m.triangles = tris;
		m.normals = normals;
		m.uv = uvs;
		m.RecalculateNormals();
		m.RecalculateBounds();
		Terrain.GetComponent<MeshFilter>().mesh = m;
		

		Terrain.GetComponent<MeshCollider>().sharedMesh = Terrain.GetComponent<MeshFilter>().sharedMesh;
	}
}
