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
    public List<Transform> itemsContainer = new List<Transform>();
   
    [Header("Inventory Variables")]
    public Transform inventoryContainer;

    public bool inventoryIsActive = false;
    private List<Item> inventoryitems = new List<Item>();

    public List<Image> playerSegments = new List<Image>();
   
    public TextMeshProUGUI priceUI;
    public TextMeshProUGUI buyUI;

    public List<Item> selectedItems = new List<Item>();
    private List<GameObject> buttonsItems = new List<GameObject>(); 
    public int totalValue;

    public TextMeshProUGUI playerCoinUI;


    private void Start()
    {
      
        playerCoinUI.text = GameManager.instance._playerCoin.ToString();
        buyUI.text = "Buy";

        inventoryitems = GameManager.instance._playerInventory;
        //Load Game Manager Inventory
        for (int i = 0; i < inventoryitems.Count; i++)
        {
            GameObject instance_InventoryItem = Instantiate(itemPrefab, inventoryContainer.position, Quaternion.identity);
            instance_InventoryItem.transform.SetParent(inventoryContainer);
            instance_InventoryItem.transform.localScale = Vector3.one;
            

            ItemButton itemButton = instance_InventoryItem.AddComponent<ItemButton>();
            itemButton.item = inventoryitems[i];

            itemButton.shopManager = this;

            instance_InventoryItem.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = inventoryitems[i].Price.ToString();
            instance_InventoryItem.transform.GetChild(0).GetComponent<Image>().sprite = inventoryitems[i].Sprite;
        }

        for (int i = 0; i < items.Count; i++)
        {
            for (int a = 0; a < itemsContainer.Count; a++)
            {
                if(items[i].itemType == itemsContainer[a].GetComponent<ItemContainer>().containerType)
                {
                    GameObject instance_Item =  Instantiate(itemPrefab, itemsContainer[a].position, Quaternion.identity);
                    instance_Item.transform.SetParent(itemsContainer[a]);
                    Debug.Log(instance_Item.transform.localScale);
                    instance_Item.transform.localScale = Vector3.one;
                    Debug.Log(instance_Item.transform.localScale);
                    
            
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
        }


    }
    //I made funtions for the Inventory change to have more control
    //and to not have to check every frame if something changed
    public void InventoryOpen()
    {
        buyUI.text = "Sell";
        inventoryIsActive = true;
    }
    public void InventoryClose()
    {
        buyUI.text = "Buy";
        inventoryIsActive = false;
    }

    public void AddItem(GameObject addedButton ,Item addedItem)
    {
        buttonsItems.Add(addedButton);
        selectedItems.Add(addedItem);
        totalValue += addedItem.Price;
        priceUI.text = totalValue.ToString();
    }

    public void SellorBuy()
    {
        if(inventoryIsActive)
        {
            GameManager.instance.GetCoins(totalValue);
        }
        else
        {
            GameManager.instance.SpendCoins(totalValue);
           
        }

        playerCoinUI.text = GameManager.instance._playerCoin.ToString();
        UpdateItems(buttonsItems);
       
        
    }
    public void UpdateItems(List<GameObject> itemsButtons)
    {
        for (int i = 0; i < items.Count; i++) 
        {
            if(inventoryIsActive)
            {
                GameManager.instance._playerInventory.Add(selectedItems[i]);
            }
            
            Destroy(itemsButtons[i]); 
            
        }
        selectedItems.Clear();
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
    public bool Selected;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(BuyItem);
    }

    public void BuyItem()
    {
        if(shopManager.inventoryIsActive == true)
        {
            Selected = true;
            shopManager.AddItem(gameObject, item);
            Debug.Log("You sold the item for:" + item.sellAmount);
        }
        else
        {
            if(GameManager.instance._playerCoin >= item.Price && Selected == false)
            {
                Selected = true;
                shopManager.AddItem(gameObject ,item);
            
                shopManager.CheckBodyPart(item);     
                Debug.Log("You purchased the item for:" + item.Price);
            }
       
        }
        
    }
}