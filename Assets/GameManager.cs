using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int _playerCoin;
    public List<Item> _playerInventory = new List<Item>();

    //This Awake I copied from one of my games 
    private void Awake()
    {

        if (instance == null)
        {
            Debug.Log("Same Scene");
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Debug.Log("New Scene");
            Destroy(gameObject);
        }


    }

    public void GetCoins(int coinAmount)
    {
        _playerCoin += coinAmount;
    }
    public void SpendCoins(int coinAmount) 
    {
        _playerCoin -= coinAmount;    
    }
}
