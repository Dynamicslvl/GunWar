using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour
{
    public Shop shop;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI selectText;
    public Image coinImg;

    public void OnClick()
    {
        AudioManager.instance.Play("Tap");
        if (HasBought())
        {
            // Select and close the shop
            GameManager.SaveGame();
            shop.gameObject.SetActive(false);
        } else
        {
            if (Utility.coin >= Utility.cost[Utility.select])
            {
                AudioManager.instance.Play("Buy");
                Buying();
            }
        }
    }

    private void Update()
    {
        if (HasBought())
        {
            selectText.gameObject.SetActive(true);
            costText.gameObject.SetActive(false);
            coinImg.gameObject.SetActive(false);
        } else
        {
            selectText.gameObject.SetActive(false);
            costText.gameObject.SetActive(true);
            coinImg.gameObject.SetActive(true);
            costText.text = "" + Utility.cost[Utility.select];
        }
    }

    private bool HasBought()
    {
        return GetBit(Utility.select, Utility.bought) == 1;
    }

    private void Buying()
    {
        Utility.coin -= Utility.cost[Utility.select];
        Utility.bought = OnBit(Utility.select, Utility.bought);
        GameManager.SaveGame();
    }

    private int GetBit(int i, int mask)
    {
        return (mask >> i) & 1;
    }

    private int OnBit(int i, int mask)
    {
        return (mask | (1 << i));
    }

}
