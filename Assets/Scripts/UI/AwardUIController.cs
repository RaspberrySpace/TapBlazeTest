using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AwardUIController : MonoBehaviour
{
    public Button spinButton;
    public Button confirButton;
    public Animator wheelAnimator;
    public Animator prizeAnimator;
    public PrizePivot awardPrizePivot;
    public PrizeWheel prizeWheel;

    public float SpinEndWait = 1.0f;

    private void Awake()
    {
        spinButton.onClick.AddListener(OnSpinPress);
        confirButton.onClick.AddListener(OnConfirmPress);
        PrizeWheel.onSpinEnd += OnSpinEnd;
    }

    private void OnDestroy()
    {
        PrizeWheel.onSpinEnd -= OnSpinEnd;
    }

    public void OnSpinPress()
    {
        spinButton.gameObject.SetActive(false);
        prizeWheel.Spin();
    }

    public void OnConfirmPress()
    {
        confirButton.gameObject.SetActive(false);
        StartCoroutine(RunTransitionBackToWheel());
    }

    private void OnSpinEnd(Prize prize)
    {
        StartCoroutine(RunPrizeAward(prize));
    }

    private IEnumerator RunPrizeAward(Prize prize)
    {
        yield return new WaitForSeconds(SpinEndWait);
        wheelAnimator.SetTrigger("Exit");
        yield return new WaitForSeconds(wheelAnimator.GetCurrentAnimatorStateInfo(0).length);
        prizeWheel.Reset();
        wheelAnimator.gameObject.SetActive(false);
        awardPrizePivot.SpawnPrize(prize);
        prizeAnimator.gameObject.SetActive(true);
        confirButton.gameObject.SetActive(true);
    }

    private IEnumerator RunTransitionBackToWheel()
    {
        prizeAnimator.SetTrigger("Exit");
        yield return new WaitForSeconds(prizeAnimator.GetCurrentAnimatorStateInfo(0).length);
        prizeAnimator.gameObject.SetActive(false);
        wheelAnimator.gameObject.SetActive(true);
        spinButton.gameObject.SetActive(true);
    }
}
