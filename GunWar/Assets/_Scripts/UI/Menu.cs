
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public List<RectTransform> rect;
    public RectTransform pause, score;

    public void OnStartGame()
    {
        Sequence sq = DOTween.Sequence();
        sq.Append(rect[0].DOScale(Vector3.zero, 0.3f));
        for (int i = 1; i < rect.Count; i++)
        {
            sq.Join(rect[i].DOScale(Vector3.zero, 0.3f));
        }
        sq.OnComplete(() =>
        {
            Utility.inGame = true;
            gameObject.SetActive(false);
            pause.gameObject.SetActive(true);
            score.gameObject.SetActive(true);
        });
    }

    public void OnEnable()
    {
        Sequence sq = DOTween.Sequence();
        sq.Append(rect[0].DOScale(new Vector3(0.6f, 0.6f, 1), 0.5f).SetEase(Ease.OutBounce));
        for (int i = 1; i < rect.Count; i++)
        {
            sq.Join(rect[i].DOScale(Vector3.one, 0.3f));
        }

    }
}
