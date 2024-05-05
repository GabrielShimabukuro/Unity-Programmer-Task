using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public ShopManager shopManager;
    public List<TabButton> tabButtons;

    [Header("Inventory")]
    public TabButton inventoryTab;
    public GameObject inventoryPage;

    [Header("Tab Sprites")]
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;

    public TabButton selectedTab;
    public List<GameObject> objectsToSwap = new List<GameObject>();
  

    private void Start()
    {
        ResetTabs();
        OnTabEnter(tabButtons[1]);
        OnTabSelected(tabButtons[1]);
    }

    public void Subscribe(TabButton button)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if(selectedTab == null || button != selectedTab)
        {
            
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(button.GetComponent<RectTransform>().anchoredPosition.x,2);
            button.background.sprite = tabHover;
        }
        
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButton button)
    {
        selectedTab = button;
        ResetTabs();
        button.background.sprite = tabActive;
       
        //IMPORTANT! because this system uses the Sibling Index the tabs and panels needs to be on the same order.
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if(i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }    

        if(selectedTab == inventoryTab)
        {
            shopManager.InventoryOpen();
        }
        else
        {
            shopManager.InventoryClose();
        }
        
        
    }

    public void ResetTabs()
    {
        foreach(TabButton button in tabButtons)
        {
            if(selectedTab != null && button == selectedTab)
            {
                continue;
                
            }
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(button.GetComponent<RectTransform>().anchoredPosition.x, -4.2f);
            button.background.sprite = tabIdle;
        }
    }
}
