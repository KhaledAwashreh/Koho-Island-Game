using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderQuadScript : MonoBehaviour
{

	/* object refs */
	public MeshFilter MeshFilter;
	public MeshRenderer MeshRenderer;

	/* private vars */

	public void SetData(SpriteCollection.TextureData textureData, int layer)
	{
		this.MeshRenderer.material = Sprites.GetTextureMaterial(textureData.texture, textureData.parent.renderingLayer, textureData.parent.renderingOrder);
	}

}
