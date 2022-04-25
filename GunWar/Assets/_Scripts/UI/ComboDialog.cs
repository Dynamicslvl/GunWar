using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboDialog : MonoBehaviour
{
    [SerializeField] private Sprite[] images = new Sprite[2];
    private Image image;
    private Animator animator;

    private void Start()
    {
        image = GetComponent<Image>();
        animator = GetComponent<Animator>();
    }

    public void ShowCombo(int id)
    {
        /*
         *  id is index of images
         *  1 = CRAZY
         *  2 = ULTRA KILL
         */
        image.sprite = images[id];
        image.SetNativeSize();
        animator.SetTrigger("Active");
    }
}
