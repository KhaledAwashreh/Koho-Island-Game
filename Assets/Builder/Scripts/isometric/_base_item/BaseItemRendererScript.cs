using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItemRendererScript : MonoBehaviour
{
	/* object refs */
	public BaseItemScript BaseItem;
	public GameObject RenderQuadsContainer;

	/* private vars */
	private List<RenderQuadScript> _renderQuads;
	private Common.Direction _oldDirection = Common.Direction.BOTTOM;
	private Common.State _oldState = Common.State.IDLE;

	private RenderQuadScript _groundPatch;
	public void Init()
	{
		this.Clear();
		this.UpdateRenderQuads();

		if (this.BaseItem.itemData.configuration.isCharacter)
		{
			RandomizeRenderQuadsContainerPosition();
		}
	}

	public void RandomizeRenderQuadsContainerPosition()
	{
		//for characters randomize some distance to avoid their overlap
		Vector3 randomDeltaPosition = new Vector3(Random.Range(-0.3f, 0.3f), 0, Random.Range(-0.3f, 0.3f));
		this.RenderQuadsContainer.transform.localPosition += randomDeltaPosition;
	}

	public void Refresh()
	{
		if (BaseItem.direction != _oldDirection || BaseItem.state != _oldState)
		{
			this.Clear();
			this.UpdateRenderQuads();

			_oldDirection = BaseItem.direction;
			_oldState = BaseItem.state;
		}
	}

	public void Clear()
	{
		if (this._renderQuads != null)
		{
			foreach (RenderQuadScript renderQuad in this._renderQuads)
			{
				Destroy(renderQuad.gameObject);
			}
		}
		this._renderQuads = new List<RenderQuadScript>();
	}

	public void UpdateRenderQuads()
	{
		SpriteCollection.TextureData[] textureDataList = this._GetCurrentImageLayers();
		if (textureDataList != null)
		{
			for (int index = 0; index < textureDataList.Length; index++)
			{
				this.AddRenderQuad(textureDataList[index], index);
			}
		}

		//flip renderer for topleft, bottomleft, left
		if (BaseItem.itemData.configuration.isCharacter)
		{
			if (BaseItem.direction == Common.Direction.BOTTOM_LEFT || BaseItem.direction == Common.Direction.LEFT || BaseItem.direction == Common.Direction.TOP_LEFT)
			{
				this._FlipRenderers(true);
			}
			else
			{
				this._FlipRenderers(false);
			}
		}
	}

	private void _FlipRenderers(bool isTrue)
	{
		if (isTrue)
		{
			this.RenderQuadsContainer.transform.localScale = new Vector3(-1, 1, 1);
		}
		else
		{
			this.RenderQuadsContainer.transform.localScale = new Vector3(1, 1, 1);
		}
	}

	public void AddRenderQuad(SpriteCollection.TextureData textureData, int layer)
	{
		if (textureData == null)
		{
			return;
		}

		if (textureData.texture == null)
		{
			return;
		}

		RenderQuadScript renderQuadInstance = Utilities.CreateRenderQuad();
		renderQuadInstance.transform.SetParent(this.RenderQuadsContainer.transform);

		//POSITIONING AND SCALING
		Vector3 defaultImgSize = new Vector3(1.4142f, 1.4142f, 1.4142f) * 4 * textureData.scale / 100.0f / textureData.parent.gridSize;
		float heightFactor = ((float)textureData.texture.height / (float)textureData.texture.width) * ((float)textureData.numberOfColumns / textureData.numberOfRows);

		float offsetX = (1.414f / 256.0f) * textureData.offsetX * 4 / textureData.parent.gridSize;
		float offsetY = (1.414f / 256.0f) * textureData.offsetY * 4 / textureData.parent.gridSize;
		renderQuadInstance.transform.localPosition = new Vector3(offsetX, offsetY, 0);
		renderQuadInstance.transform.localRotation = Quaternion.Euler(Vector3.zero);
		renderQuadInstance.transform.localScale = new Vector3(defaultImgSize.x, defaultImgSize.x * heightFactor, 1);

		renderQuadInstance.SetData(textureData, layer);
		renderQuadInstance.GetComponent<TextureSheetAnimationScript>()
			.SetTextureSheetData(textureData.numberOfColumns, textureData.numberOfRows, textureData.framesCount, textureData.fps);

		this._renderQuads.Add(renderQuadInstance);

		if (layer == 0)
		{
			//ground patch layer
			this._groundPatch = renderQuadInstance;
		}
	}

	public void ShowGroundPatch(bool isTrue)
	{
		if (this._groundPatch == null)
		{
			return;
		}
		this._groundPatch.gameObject.SetActive(isTrue);
	}

	private SpriteCollection.TextureData[] _GetCurrentImageLayers()
	{
		List<SpriteCollection.TextureData> layers = new List<SpriteCollection.TextureData>();

		Common.State state = BaseItem.state;
		Common.Direction direction = BaseItem.direction;

		List<int> spriteIds = BaseItem.itemData.GetSprites(state);

		if (spriteIds == null || spriteIds.Count == 0)
		{
			return null;
		}

		for (int index = 0; index < spriteIds.Count; index++)
		{
			SpriteCollection.SpriteData sprite = Sprites.GetSprite(spriteIds[index]);
			layers.Add(sprite.GetTextureData(direction));
		}

		return layers.ToArray();
	}

	public List<RenderQuadScript> GetRenderQuads()
	{
		return this._renderQuads;
	}
}
