﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class P3DModel
{
    public enum P3DMaterial
    {
        Flat = 0,
        FlatMetal = 1,
        Gouraud = 2,
        GouraudMetal = 3,
        GouraudMetalEnv = 4,
        Shining = 5
    }

    public class RenderInfo
    {
        public string TextureFile;
    };

    public class TextureInfo
    {
        public short TextureStart;
        public short NumFlat;
        public short NumFlatMetal;
        public short NumGouraud;
        public short NumGouraudMetal;
        public short NumGouraudMetalEnv;
        public short NumShining;
    }

    public class P3DLight
    {
        public string Name;
        public Vector3 Pos;
        public float Range;
        public int Color;

        public byte ShowCorona;
        public byte ShowLensFlares;

        public byte LightUpEnvironment;
    };

    public class P3DTexPolygon
    {
        public string Texture;
        public P3DMaterial Material;

        public short P1;
        public short P2;
        public short P3;

        public float U1, V1;
        public float U2, V2;
        public float U3, V3;
    };

    public class P3DMesh
    {
        public string Name;
        public uint Flags;
        public short NumVertices;
        public short NumPolys;

        public TextureInfo[] TextureInfos;

        public Vector3[] Vertex;
        public P3DTexPolygon[] Poly;

        public Vector3 LocalPos;
        public float Length;
        public float Height;
        public float Depth;

        public P3DMesh(byte numTextures)
        {
            TextureInfos = new TextureInfo[numTextures];
        }
    };

    public short P3DNumMeshes;
    public P3DMesh[] P3DMeshes;

    public short P3DNumLights;
    public P3DLight[] P3DLights;

    public float P3DLength;
    public float P3DDepth;
    public float P3DHeight;

    public int P3DUserDataSize;
    public string P3DUserDataPtr;

    public byte P3DNumTextures;
    public RenderInfo[] P3DRenderInfo;

    public static Texture2D LoadTextureDXT(byte[] ddsBytes, TextureFormat textureFormat)
    {
        if (textureFormat != TextureFormat.DXT1 && textureFormat != TextureFormat.DXT5)
            Debug.LogError("Invalid TextureFormat. Only DXT1 and DXT5 formats are supported by this method.");

        byte ddsSizeCheck = ddsBytes[4];
        if (ddsSizeCheck != 124)
            Debug.LogError("Invalid DDS DXTn texture. Unable to read");  //this header byte should be 124 for DDS image files

        int height = ddsBytes[13] * 256 + ddsBytes[12];
        int width = ddsBytes[17] * 256 + ddsBytes[16];

        int DDS_HEADER_SIZE = 128;
        byte[] dxtBytes = new byte[ddsBytes.Length - DDS_HEADER_SIZE];
        Buffer.BlockCopy(ddsBytes, DDS_HEADER_SIZE, dxtBytes, 0, ddsBytes.Length - DDS_HEADER_SIZE);

        Texture2D texture = new Texture2D(width, height, textureFormat, true);
        texture.LoadRawTextureData(dxtBytes);
        texture.Apply();

        return texture;
    }

	public Material[] CreateMaterials()
	{
		List<Material> materials = new List<Material>();
		for (int i = 0; i < P3DNumTextures; i++)
		{
			materials.Add(CreateMaterial(i));
		}

		return materials.ToArray();
	}

	public Material CreateMaterial(int id)
	{
		Material mat;
		byte[] data = System.IO.File.ReadAllBytes(IO.GetCrashdayPath() + "/data/content/textures/" + P3DRenderInfo[id].TextureFile.Remove(P3DRenderInfo[id].TextureFile.Length - 4) + ".dds");
		Texture2D tex = LoadTextureDXT(data, TextureFormat.DXT5);
		tex.mipMapBias = -0.5f;
		tex.Apply(true);
		bool tr = P3DRenderInfo[id].TextureFile.Contains("transp");
		bool gls = P3DRenderInfo[id].TextureFile.Contains("gls");
		if ( tr || gls)
		{
			mat = new Material(Shader.Find("Standard"));

			Color c = gls ? new Color(0.1f, 0.1f, 0.1f, 0.4f) : Color.clear; 
			mat.SetColor("_Color", c);
			mat.SetFloat("_Mode", 2);
			
			mat.SetInt("_ZWrite", 0);
			mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
			mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

			mat.DisableKeyword("_ALPHATEST_ON");
			mat.EnableKeyword("_ALPHABLEND_ON");
			mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");

			mat.renderQueue = 3000;
		}
		else
		{
			mat = new Material(Shader.Find("Mobile/Bumped Diffuse"));

			mat.SetFloat("_Glossiness", 0);
		}

		mat.mainTexture = tex;
		
		
		return mat;
	}

    class Vert
    {
        public Vector3 Pos;
        public Vector2 Uv;
        public bool UvAssigned;

        public Vert(Vector3 pos)
        {
            Pos = pos;
            UvAssigned = false;
        }
    }

	public Mesh CreateMesh()
	{
		Mesh m = new Mesh();

		m.subMeshCount = P3DNumTextures;

	    List<string> textures = new List<string>();

		List<Vert> vertices = new List<Vert>();
		List<List<int>> tri = new List<List<int>>();

        //uvs and positions separated from verticies array - needed for splitting
        List<Vector2> uv = new List<Vector2>();
        List<Vector3> verts = new List<Vector3>();


        //for every texture we have a separate set of of triangles, so lets figure out how much textures we have for the model
		for (int n = 0; n < P3DNumTextures; n++)
		{
			textures.Add(P3DRenderInfo[n].TextureFile);
			tri.Add(new List<int>());
		}


		int meshSizeOffset = 0;

        //one model might contain more than one mesh
		for (int i = 0; i < P3DNumMeshes; i++)
		{
			//avoid loading LODs
			if(P3DMeshes[i].Name.Contains(".0") || P3DMeshes[i].Name.Contains(".1") || P3DMeshes[i].Name.Contains(".2") || P3DMeshes[i].Name.Contains(".3")
			   || P3DMeshes[i].Name.Contains(".4")) continue;

			//dont load destroyed parts of the mesh
			if (P3DMeshes[i].Name.Contains("dest_")) continue;

            //iterate through every vertex and add it's position. Dont forget local object position
			for (int v = 0; v < P3DMeshes[i].NumVertices; v++)
			{
				vertices.Add(new Vert(P3DMeshes[i].Vertex[v] + P3DMeshes[i].LocalPos));
			}

			for (int v = 0; v < P3DMeshes[i].NumPolys; v++)
			{
			    int index = textures.FindIndex(x => x.Contains(P3DMeshes[i].Poly[v].Texture));

			    Vector2 uv1 = new Vector2(P3DMeshes[i].Poly[v].U1, P3DMeshes[i].Poly[v].V1);
			    if (vertices[P3DMeshes[i].Poly[v].P1].UvAssigned && vertices[P3DMeshes[i].Poly[v].P1].Uv != uv1)
			    {
                    vertices.Add(new Vert(P3DMeshes[i].Vertex[P3DMeshes[i].Poly[v].P1] + P3DMeshes[i].LocalPos));
			        vertices[vertices.Count - 1].UvAssigned = true;
                    vertices[vertices.Count - 1].Uv = uv1;
			        tri[index].Add(vertices.Count - 1);
			    }
			    else
			    {
			        vertices[P3DMeshes[i].Poly[v].P1].UvAssigned = true;
			        vertices[P3DMeshes[i].Poly[v].P1].Uv = uv1;
			        tri[index].Add(meshSizeOffset + P3DMeshes[i].Poly[v].P1);
			    }

			    Vector2 uv2 = new Vector2(P3DMeshes[i].Poly[v].U2, P3DMeshes[i].Poly[v].V2);
			    if (vertices[P3DMeshes[i].Poly[v].P2].UvAssigned && vertices[P3DMeshes[i].Poly[v].P2].Uv != uv2)
			    {
			        vertices.Add(new Vert(P3DMeshes[i].Vertex[P3DMeshes[i].Poly[v].P2] + P3DMeshes[i].LocalPos));
			        vertices[vertices.Count - 1].UvAssigned = true;
			        vertices[vertices.Count - 1].Uv = uv2;
			        tri[index].Add(vertices.Count - 1);
			    }
			    else
			    {
			        vertices[P3DMeshes[i].Poly[v].P2].UvAssigned = true;
			        vertices[P3DMeshes[i].Poly[v].P2].Uv = uv2;
			        tri[index].Add(meshSizeOffset + P3DMeshes[i].Poly[v].P2);
			    }

			    Vector2 uv3 = new Vector2(P3DMeshes[i].Poly[v].U3, P3DMeshes[i].Poly[v].V3);
			    if (vertices[P3DMeshes[i].Poly[v].P3].UvAssigned && vertices[P3DMeshes[i].Poly[v].P3].Uv != uv3)
			    {
			        vertices.Add(new Vert(P3DMeshes[i].Vertex[P3DMeshes[i].Poly[v].P3] + P3DMeshes[i].LocalPos));
			        vertices[vertices.Count - 1].UvAssigned = true;
			        vertices[vertices.Count - 1].Uv = uv3;
			        tri[index].Add(vertices.Count - 1);
			    }
			    else
			    {
			        vertices[P3DMeshes[i].Poly[v].P3].UvAssigned = true;
			        vertices[P3DMeshes[i].Poly[v].P3].Uv = uv3;
			        tri[index].Add(meshSizeOffset + P3DMeshes[i].Poly[v].P3);
			    }
			}

		    meshSizeOffset += P3DMeshes[i].NumVertices;
		}

        foreach(Vert v in vertices)
	    {
            verts.Add(v.Pos);
            uv.Add(v.Uv);
	    }

		m.SetVertices(verts);
	    m.SetUVs(0, uv);
		for (int n = 0; n < P3DNumTextures; n++)
		{
			m.SetTriangles(tri[n], n);
		}

		m.RecalculateNormals();
		m.RecalculateBounds();

		return m;
	}
}
