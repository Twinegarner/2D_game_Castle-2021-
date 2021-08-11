using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int healthPoints;
    public int healthPointsCap;
    public int moneyPoints;
    public int playerAttackDamage;

    

    public int getPlayerAttackDamage()
    {
        return playerAttackDamage;
    }
    public void setPlayerAttackDamage(int attack)
    {
        playerAttackDamage = attack;
    }

    //get and return the number of health point the player currently has
    public int getHealthPoint()
    {
        return healthPoints;
    }
    //change the players current health point
    public void setHealthpoints(int health)
    {
        healthPoints = health;
    }
    //add or subtract healthpoints
    public void addHealthPoints(int health)
    {
        Debug.Log(health);
        healthPoints += health;
        Debug.Log(healthPoints);
        //check if below zero if so then change to zero
        if(healthPoints < 0)
        {
            healthPoints = 0;
        }
        else if(healthPoints > healthPointsCap)
        {
            healthPoints = healthPointsCap;
        }
        Debug.Log(healthPoints);
    }
    //get and return the amount of money the player has
    public int getMoneyPoints()
    {
        return moneyPoints;
    }
    //chnage the amount of money the player has
    public void setMoneyPoints(int money)
    {
        moneyPoints = money;

    }
    //add to money
    public void addMoneyPoints(int money)
    {
        moneyPoints += money;
        if(moneyPoints < 0)
        {
            moneyPoints = 0;
        }
    }
    
}
