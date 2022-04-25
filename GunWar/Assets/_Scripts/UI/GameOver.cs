using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] List<RectTransform> rect = new List<RectTransform>();

    private void Awake()
    {
        GameMaster.GameOver += Enable;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameMaster.GameOver -= Enable;
    }

    void Enable()
    {
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        AudioManager.instance.Stop("Background");
        GameManager.SaveGame();
        for (int i = 0; i < rect.Count; i++)
            rect[i].localScale = Vector3.zero;
        Sequence sq = DOTween.Sequence();
        sq.Append(rect[0].DOScale(Vector3.one, 0.3f)).SetEase(Ease.InQuad)
            .Join(rect[1].DOScale(Vector3.one, 0.3f)).SetEase(Ease.InQuad);
        if (!Utility.haveNewBest)
        {
            AudioManager.instance.Play("GameOver");
            sq.Join(rect[2].DOScale(Vector3.one, 0.3f)).SetEase(Ease.InQuad);
        } else
        {
            AudioManager.instance.Play("NewRecord");
        }
        sq.Append(rect[3].DOScale(Vector3.one, 0.3f)).SetEase(Ease.InQuad);
        for (int i = 4; i < rect.Count; i++)
        {
            if (i < 8 || Utility.haveNewBest)
                sq.Join(rect[i].DOScale(Vector3.one, 0.3f)).SetEase(Ease.InQuad);
        }
    }

    public void OnRestartGame()
    {
        AudioManager.instance.Play("Background");
        AudioManager.instance.Play("Tap");
        Sequence sq = DOTween.Sequence();
        sq.Append(rect[3].DOScale(Vector3.zero, 0.2f))
            .Append(rect[5].DOScale(Vector3.zero, 0.2f))
            .Join(rect[6].DOScale(Vector3.zero, 0.2f))
            .Append(rect[4].DOScale(Vector3.zero, 0.2f))
            .Join(rect[7].DOScale(Vector3.zero, 0.2f));
        for (int i = 0; i < 3; i++)
        {
            sq.Join(rect[i].DOScale(Vector3.zero, 0.2f));
        }
        sq.Join(rect[8].DOScale(Vector3.zero, 0.2f));
        sq.Join(rect[9].DOScale(Vector3.zero, 0.2f));
        sq.OnComplete(() =>
        {
            Utility.haveNewBest = false;
            GameMaster.Restart?.Invoke();
            gameObject.SetActive(false);
        });
    }

}
