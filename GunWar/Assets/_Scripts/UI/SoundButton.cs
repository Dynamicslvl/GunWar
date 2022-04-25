using UnityEngine.UI;
using UnityEngine;

public class SoundButton : MonoBehaviour
{
    public Sprite imageOn, imageOff;

    void Update()
    {
        if (Utility.onSound == 1)
        {
            GetComponent<Image>().sprite = imageOff;
        } else
        {
            GetComponent<Image>().sprite = imageOn;
        }
    }

    public void OnClick()
    { 
        Utility.onSound = 1 - Utility.onSound;
        AudioManager.instance.Play("Tap");
    }
}
