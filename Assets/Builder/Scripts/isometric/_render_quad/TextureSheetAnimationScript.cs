using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RenderQuadScript))]
public class TextureSheetAnimationScript : MonoBehaviour
{

	/* object refs */
	public RenderQuadScript RenderQuad;

	/* private vars */
	private int _tilesX;
	private int _tilesY;
	private int _framesCount;
	private float _fps;

	private float _currentFrame = -1;

	private float _playStartTime;

	void Awake()
	{
		this._playStartTime = Time.time;
	}

	void Update()
	{
		if (RenderQuad == null)
		{
			return;
		}

		if (_framesCount > 1)
		{
			int frame = (int)((Time.time - _playStartTime) * _fps);
			frame = frame % _framesCount;
			this.SetFrame(frame);
		}
		//		else {
		//			this.SetFrame (0);
		//		}
	}

	/// <summary>
	/// Sets the texture sheet data.
	/// </summary>
	/// <param name="tilesX">Tiles x.</param>
	/// <param name="tilesY">Tiles y.</param>
	/// <param name="frames">Frames.</param>
	/// <param name="fps">Fps.</param>
	public void SetTextureSheetData(int tilesX, int tilesY, int frames, float fps)
	{
		this._tilesX = tilesX;
		this._tilesY = tilesY;
		this._framesCount = frames;
		this._fps = fps;
		this.SetFrame(0);
	}

	/// <summary>
	/// Sets the frame.
	/// </summary>
	/// <param name="frame">Frame.</param>
	public void SetFrame(int frame)
	{
		if (frame == _currentFrame)
		{
			//no change
			return;
		}

		//		if (frame > this._framesCount - 1 || frame < 0) {
		//			return;
		//		}

		float xUnitSize = 1.0f / this._tilesX;
		float yUnitSize = 1.0f / this._tilesY;

		int xIndex = frame % this._tilesX;
		int yIndex = frame / this._tilesX;
		yIndex = this._tilesY - yIndex - 1;


		Vector2[] uv = new Vector2[] {
			new Vector2(xIndex * xUnitSize, yIndex * yUnitSize),
            new Vector2(xIndex * xUnitSize, yIndex * yUnitSize) + new Vector2(xUnitSize, 0),
            new Vector2(xIndex * xUnitSize, yIndex * yUnitSize) + new Vector2(0, yUnitSize),
            new Vector2(xIndex * xUnitSize, yIndex * yUnitSize) + new Vector2(xUnitSize, yUnitSize),
		};

		this.RenderQuad.MeshFilter.mesh.uv = uv;

		this._currentFrame = frame;
	}
}
