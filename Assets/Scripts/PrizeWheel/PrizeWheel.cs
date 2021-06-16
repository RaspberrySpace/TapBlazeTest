using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeWheel : MonoBehaviour
{
    public PrizePool prizePool;
    public PrizePivot pivotTemplate;
    public Transform spinTransform;
    [Header("Intro")]
    public AnimationCurve introPullbackCurve;
    public float introPullbackTime = 1.0f;
    public float pullbackAmount = 15.0f;
    [Header("Spin")]
    public AnimationCurve spinCurve;
    public float spinTime = 3.0f;
    public float spinSpeed = -600.0f;
    [Header("Outro")]
    public AnimationCurve overshootCurve;
    public float overshootTime = 0.5f;
    public float overshootAmount = 5.0f;

    public delegate void PrizeEvent(Prize prize);
    public static PrizeEvent onSpinEnd;

    private PrizePool currentPool;
    private List<PrizePivot> pivots = new List<PrizePivot>();

    public void Spin()
    {
        if (currentPool != null)
        {
            StartCoroutine(RunSpin());
        }
    }

    public void Reset()
    {
        spinTransform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            Spin();
        }
    }

    private void OnEnable()
    {
        SetupWheel();
    }

    private void SetupWheel()
    {
        pivotTemplate.gameObject.SetActive(false);
        if (currentPool != prizePool)
        {
            currentPool = prizePool;
            Prize[] prizes = currentPool.GetPrizes();
            while(pivots.Count < prizes.Length)
            {
                var obj = Instantiate(pivotTemplate.gameObject, pivotTemplate.transform.parent);
                pivots.Add(obj.GetComponent<PrizePivot>());
            }
            for(int i = 0; i < pivots.Count; i++)
            {
                if(i < prizes.Length)
                {
                    pivots[i].SpawnPrize(prizes[i]);
                    pivots[i].SetRotation(GetPrizeAngle(i));
                    pivots[i].gameObject.SetActive(true);
                }
                else
                {
                    pivots[i].gameObject.SetActive(false);
                }
            }
        }
    }

    private float GetPrizeAngle(int index)
    {
        float angleBetween = -360.0f / Mathf.Max(1, currentPool.Count);
        float offset = angleBetween / 2.0f;
        return offset + angleBetween * index;
    }

    private IEnumerator RunSpin()
    {
        PrizePoolDraw prizeDraw = currentPool.Draw();
        Debug.Log($"Prize Draw: {prizeDraw.prize.id}_{prizeDraw.prize.count}, {prizeDraw.index}");
        float prizeRotation = -GetPrizeAngle(prizeDraw.index);
        float timer = 0.0f;
        float rotation = 0.0f;
        //Intro
        while(timer < introPullbackTime)
        {
            rotation = pullbackAmount * introPullbackCurve.Evaluate(timer / introPullbackTime);
            SetRotation(rotation);
            yield return null;
            timer += Time.deltaTime;
        }
        //Spin
        float startRotation = NormalizeAngle(rotation);
        float endRotation = startRotation + (spinTime * spinSpeed * 0.5f);
        endRotation += NormalizeAngle(prizeRotation - overshootAmount) - NormalizeAngle(endRotation);
        timer = 0.0f;
        while (timer < spinTime)
        {
            rotation =  Mathf.Lerp(startRotation, endRotation, spinCurve.Evaluate(timer / spinTime));
            SetRotation(rotation);
            yield return null;
            timer += Time.deltaTime;
        }
        //Outro
        timer = 0.0f;
        startRotation = NormalizeAngle(endRotation);
        endRotation = NormalizeAngle(prizeRotation);
        while (timer < overshootTime)
        {
            rotation = Mathf.Lerp(startRotation, endRotation, overshootCurve.Evaluate(timer / overshootTime));
            SetRotation(rotation);
            yield return null;
            timer += Time.deltaTime;
        }
        SetRotation(prizeRotation);
        onSpinEnd?.Invoke(prizeDraw.prize);
    }

    private void SetRotation(float rotation)
    {
        spinTransform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, rotation));
    }

    private float NormalizeAngle(float angle)
    {
        while(angle < -180.0f)
        {
            angle += 360.0f;
        }
        while(angle >= 180.0f)
        {
            angle -= 360.0f;
        }
        return angle;
    }
}
