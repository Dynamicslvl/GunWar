using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public TextMeshProUGUI coinText;

    private void Update()
    {
        coinText.text = "" + Utility.coin;
    }
}
