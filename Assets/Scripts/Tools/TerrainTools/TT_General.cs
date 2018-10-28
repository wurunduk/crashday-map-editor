using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TT_General : ToolGeneral
{
	public TerrainBrush brush;
	public List<IntVector2> _selectedTiles;

	//selected points for selection highlite shader
	public ComputeBuffer _buf;

	public override void Initialize()
	{
		brush = new TerrainBrush();
		_selectedTiles = new List<IntVector2>();
		base.Initialize();
	}

	public override void OnSelected()
	{
		base.OnSelected();
		brush.ClearSelectedPoints();
		_selectedTiles.Clear();
		UpdateTerrainShader();
	}

	public override void OnDeselected()
	{
		base.OnDeselected();
		_buf.Dispose();
	}

	public virtual void UpdateTerrainShader()
	{
		if (_buf != null)
			_buf.Dispose();

		//prevent creation of the zero sized compute buffer
		if (brush._selectedPoints.Count == 0)
		{
			_buf = new ComputeBuffer(1, sizeof(float), ComputeBufferType.Default);
			float[] ar = { -1.0f };
			_buf.SetData(ar);
		}
		else
		{
			_buf = new ComputeBuffer(brush._selectedPoints.Count, sizeof(float), ComputeBufferType.Default);
			_buf.SetData(brush._selectedPoints);
		}

		Shader.SetGlobalBuffer("_Points", _buf);
		Shader.SetGlobalInt("_Points_Length", _buf.count);
	}
}
