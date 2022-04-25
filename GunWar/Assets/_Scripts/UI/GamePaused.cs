using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class GamePaused : MonoBehaviour
{
    [SerializeField] List<RectTransform> rect = new List<RectTransform>();

    private void OnEnable()
    {
        AudioManager.instance.Play("Tap");
        GameManager.SaveGame();
        Time.timeScale = 0;
        rect[0].GetComponent<Image>().color = new Color(1, 1, 1, 0);
        for (int i = 1; i < rect.Count; i++) 
            rect[i].localScale = Vector3.zero;
        Sequence sq = DOTween.Sequence();
        sq.Append(rect[0].GetComponent<Image>().DOColor(new Color(1, 1, 1, 100f / 255), 0.3f))
            .Join(rect[1].DOScale(Vector3.one, 0.3f)).SetEase(Ease.InQuad)
            .Append(rect[2].DOScale(Vector3.one, 0.3f)).SetEase(Ease.InQuad);
        for (int i = 3; i < rect.Count; i++)
        {
            sq.Join(rect[i].DOScale(Vector3.one, 0.3f)).SetEase(Ease.InQuad);
        }
        sq.SetUpdate(true);
    }

    public void OnContinued()
    {
        AudioManager.instance.Play("Tap");
        Sequence sq = DOTween.Sequence();
        sq.Append(rect[2].DOScale(Vector3.zero, 0.2f));

        for (int i = 3; i < rect.Count; i++)
        {
            sq.Join(rect[i].DOScale(Vector3.zero, 0.3f));
        }

        sq.Append(rect[0].GetComponent<Image>().DOColor(new Color(1, 1, 1, 0), 0.3f))
            .Join(rect[1].DOScale(Vector3.zero, 0.3f));

        sq.SetUpdate(true).OnComplete(() =>
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
        });
    }

    public void OnRestartGame()
    {
        AudioManager.instance.Play("Background");
        AudioManager.instance.Play("Tap");
        Utility.haveNewBest = false;
        Time.timeScale = 1;
        GameMaster.Restart?.Invoke();
        gameObject.SetActive(false);
    }

    public void OnBackToMenu()
    {
        AudioManager.instance.Play("Background");
        AudioManager.instance.Play("Tap");
        Utility.score = 0;
        Utility.haveNewBest = false;
        Utility.inGame = false;
        GameMaster.EnterMenu?.Invoke();
        GameMaster.StopMove?.Invoke();
        Time.timeScale = 1;
        PoolingSystem.instance.ResetPool();
    }

}
