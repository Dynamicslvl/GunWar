using UnityEngine.UI;
using UnityEngine;

public class MusicButton : MonoBehaviour
{
    public Sprite imageOn, imageOff;

    void Update()
    {
        if (Utility.onMusic == 1)
        {
            GetComponent<Image>().sprite = imageOff;
        }
        else
        {
            GetComponent<Image>().sprite = imageOn;
        }
    }

    public void OnClick()
    {
        Utility.onMusic = 1 - Utility.onMusic;
        AudioManager.instance.Play("Tap");
        if (Utility.onMusic == 0)
        {
            AudioManager.instance.Play("Background");
        } else
        {
            AudioManager.instance.Stop("Background");
        }
    }
}
