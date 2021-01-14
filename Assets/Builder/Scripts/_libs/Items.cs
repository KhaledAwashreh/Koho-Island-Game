/* **************************************************************************
 * ITEMS
 * **************************************************************************
 * Written by: Coppra Games
 * Created: June 2017
 * *************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour {

	public static Dictionary<int, ItemsCollection.ItemData> items;


	public static void LoadItems(){
		items = new Dictionary<int, ItemsCollection.ItemData> ();

		ItemsCollection itemsCollection = Resources.Load("ItemsCollection", typeof(ItemsCollection)) as ItemsCollection;
		if (itemsCollection != null) {
			for (int index = 0; index < itemsCollection.list.Count; index++) {
				ItemsCollection.ItemData itemData = itemsCollection.list [index];
				items.Add (itemData.id, itemData);
			}
		} else {
			Debug.LogError ("ItemsCollection is missing! please go to 'Windows/Item Editor'");
		}
	}
		
	public static ItemsCollection.ItemData GetItem(int itemId){
		ItemsCollection.ItemData item = null;
		items.TryGetValue (itemId, out item);
		return item;
	}

}
