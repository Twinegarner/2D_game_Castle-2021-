using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImoneyManager : MonoBehaviour
{
    private Text textOBJ;
    // Start is called before the first frame update
    void Start()
    {
        textOBJ = FindObjectOfType<Text>();
    }

    public void updateMoneyText(int newAmount)
    {
        textOBJ.text = "Money: " + newAmount;
    }

}
