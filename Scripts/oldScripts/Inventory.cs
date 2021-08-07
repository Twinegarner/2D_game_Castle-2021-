using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int score;//holds the players score
    public int money;//holds the players money

    private UImoneyManager moneyUI;


    // Start is called before the first frame update
    void Start()
    {
        moneyUI = FindObjectOfType<UImoneyManager>();
    }

    public void addMoney(int number)//add to the money counter
    {
        money += number;
        moneyUI.updateMoneyText(money);
    }

    public int getMoney()//return the number of moneys
    {
        return money;
    }

    public void resetMoney(int number)//reset the money counter
    {
        money = number;
        moneyUI.updateMoneyText(money);
    }
}
