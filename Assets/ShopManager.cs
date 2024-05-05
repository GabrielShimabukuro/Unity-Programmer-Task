using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class ShopManager : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public GameObject itemPrefab;
    public Transform hatContainer;
    public List<Image> playerSegments = new List<Image>();


    private void OnEnable()
    {
        for (int i = 0; i < items.Count; i++)
        {
            GameObject instance_Item =  Instantiate(itemPrefab, hatContainer.position, Quaternion.identity);
            instance_Item.transform.localScale = Vector3.one;
            instance_Item.transform.SetParent(hatContainer);
            
            ItemButton itemButton = instance_Item.AddComponent<ItemButton>();
            itemButton.item = items[i];
            /*
            Firstly I decide to reference this script with an Instance but this could become
            a problem in the future because you can't have more than one shop on the same scene 
            and doing that with Tags would scale terribly so I decided it is better to reference the 
            ShopManager script when the button is being created so you can have multiple shops with
            diferent items with no problem.
            */
            itemButton.shopManager = this;
            
            //Here I get the child of the child of the instanced item, I could just write GetComponentInChildren
            //but with that I don't have much control if the object has more than one TextMeshProUGUI.
            instance_Item.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = items[i].Price.ToString();
            instance_Item.transform.GetChild(0).GetComponent<Image>().sprite = items[i].Sprite;
        }
    }

    public void CheckBodyPart(Item currentItem)
    {
        switch (currentItem.itemType)
        {
            case ItemType.Hair:
                playerSegments[0].enabled = true;
                playerSegments[0].sprite = currentItem.Sprite; 
                break;
            case ItemType.Hat:
                playerSegments[1].enabled = true;
                playerSegments[1].sprite = currentItem.Sprite;
                break;
            case ItemType.Shirt:
                playerSegments[2].enabled = true;
                playerSegments[2].sprite = currentItem.Sprite;
                break;
            case ItemType.Pants:
                playerSegments[3].enabled = true;
                playerSegments[3].sprite = currentItem.Sprite;
                break;
            case ItemType.Shoes:
                playerSegments[4].enabled = true;
                playerSegments[4].sprite = currentItem.Sprite;
                break;

        }
    }

}
//I made the ItemButton class on the script of the shop to be easier to change 
// some things
class ItemButton: MonoBehaviour
{
    public Item item;
    public ShopManager shopManager;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(BuyItem);
    }

    public void BuyItem()
    {
        if (GameManager.instance._playerCoin >= item.Price)
        {
            GameManager.instance.SpendCoins(item.Price);
            shopManager.CheckBodyPart(item);     
            Debug.Log("You purchased the item for:" + item.Price);
        }



       
    }
}