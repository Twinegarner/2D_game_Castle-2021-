using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int enemyHealthPoints;
    public int enemyAttackDamage;


    // Start is called before the first frame update
    void Start()
    {

    }

    public void setEnemyHealthPoints(int health)
    {
        enemyHealthPoints = health;
    }
    public void addEnemyHealthPoints(int health)
    {
        enemyHealthPoints += health;

        if(enemyHealthPoints <= 0)
        {
            enemyHealthPoints = 0;
        }
    }
    public int getEnemyHealthPoints()
    {
        return enemyHealthPoints;
    }


    public int getEnemyAttackDamage()
    {
        return enemyAttackDamage;
    }
    public void setEnemyAttackDamage(int attack)
    {
        enemyAttackDamage = attack;
    }



}
