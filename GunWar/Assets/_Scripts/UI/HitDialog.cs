using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitDialog : MonoBehaviour
{
    [SerializeField] private Sprite[] images = new Sprite[3];
    private Image image;
    private Animator animator;

    private void Start()
    {
        image = GetComponent<Image>();
        animator = GetComponent<Animator>();
    }

    public void ShowHit(int id)
    {
        /*
         *  id is index of images
         *  1 = hit
         *  2 = headshot
         *  3 = miss
         */
        image.sprite = images[id];
        image.SetNativeSize();
        animator.SetTrigger("Active");
    }
}
