using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayer : MonoBehaviour
{
    [SerializeField] private int id;
    private Image image;
    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (Utility.select == id)
        {
            image.color = new Color(1, 1, 1, 1);
        } else
        {
            image.color = new Color(50 / 255f, 50 / 255f, 50 / 255f, 1);
        }
    }

    public void OnSelect()
    {
        AudioManager.instance.Play("Tap");
        Utility.select = id;
    }
}
