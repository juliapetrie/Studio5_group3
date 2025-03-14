using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class LiveCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI current;
    [SerializeField] private TextMeshProUGUI toUpdate;
    [SerializeField] private Transform coinTextContainer;
    [SerializeField] private float duration;
    [SerializeField] private Ease animationCurve;

    private float containerInitPosition;
    private float moveAmount;

    private void Start()
    {
        Canvas.ForceUpdateCanvases();
        current.SetText("3");
        toUpdate.SetText("2");
        containerInitPosition = coinTextContainer.localPosition.y;
        moveAmount = current.rectTransform.rect.height;
    }

    public void UpdateLives(int maxLives)
    {
        toUpdate.SetText($"{maxLives}");

        coinTextContainer.DOLocalMoveY(containerInitPosition + moveAmount, duration);
        coinTextContainer.DOLocalMoveY(containerInitPosition + moveAmount, duration).SetEase(animationCurve);

        StartCoroutine(ResetCoinContainer(maxLives));
    }

    private IEnumerator ResetCoinContainer(int maxLives)
    {
        yield return new WaitForSeconds(duration);

        current.SetText($"{maxLives}");
        Vector3 localPosition = coinTextContainer.localPosition;
        coinTextContainer.localPosition = new Vector3(localPosition.x, containerInitPosition, localPosition.z);

    }

}

