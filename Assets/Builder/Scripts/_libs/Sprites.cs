/* **************************************************************************
 * IMAGES.CS
 * **************************************************************************
 * Written by: Coppra Games
 * Created: June 2017
 * *************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprites {
	
	public static Dictionary<int, SpriteCollection.SpriteData> sprites;
	public static Dictionary<Texture2D, Material>	textureMaterialMap;

	public static void LoadSprites(){
		sprites = new Dictionary<int, SpriteCollection.SpriteData> ();

		SpriteCollection spriteCollection = Resources.Load("SpriteCollection", typeof(SpriteCollection)) as SpriteCollection;
		if (spriteCollection != null) {
			for (int index = 0; index < spriteCollection.list.Count; index++) {
				SpriteCollection.SpriteData spriteData = spriteCollection.list [index];
				sprites.Add (spriteData.id, spriteData);
			}
		} else {
			Debug.LogError ("SpriteCollection is missing! please go to 'Windows/Item Editor'");
		}
	}

	public static SpriteCollection.SpriteData GetSprite(int spriteId){
		SpriteCollection.SpriteData sprite = null;
		sprites.TryGetValue (spriteId, out sprite);
		return sprite;
	}
		
	public static Material GetTextureMaterial(Texture2D texture, Common.RenderingLayer layer, int order){
		if (textureMaterialMap == null) {
			textureMaterialMap = new Dictionary<Texture2D, Material> ();
		}

		Material material = null;
		if (!textureMaterialMap.TryGetValue (texture, out material)) {
			material = MonoBehaviour.Instantiate (SceneManager.instance.RenderQuadMaterial) as Material;
			material.mainTexture = texture;
			textureMaterialMap.Add (texture, material);
		}

		material.renderQueue = 3000 + 100 * (int)layer + order;
		return material;
	}
}
