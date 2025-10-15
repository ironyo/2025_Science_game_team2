using UnityEngine;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;

public class QuizManager : MonoBehaviour
{
    [SerializeField] List<QuizSO> quizSOs = new List<QuizSO>();
    [SerializeField] List<TextMeshProUGUI> quizTexs = new List<TextMeshProUGUI>();
    [SerializeField] GameObject quizGroup;
    [SerializeField] TextMeshProUGUI wrongTex;
    [SerializeField] TextMeshProUGUI collectTex;

    private QuizSO realSo;

    bool isRoad = false;
    public void QuizStart()
    {
        quizGroup.SetActive(true);
        Time.timeScale = 0.0f;
        QuizMake();
    }
    public void WhatIsCollect(int answer)
    {
        if(answer == realSo._collectNum)
        {
            MessageUp(collectTex, true);
        }
        else
        {
            QuizMake();
            MessageUp(wrongTex, false);
        }
    }

    private void MessageUp(TextMeshProUGUI tex , bool isCollect)
    {
        if (isRoad)
            return;

        isRoad = true;
        
        Sequence seq = DOTween.Sequence();

        tex.gameObject.SetActive(true);
        Time.timeScale = 1.0f;
        seq.Append(tex.DOFade(1, 0.2f));
        seq.AppendInterval(0.5f);
        seq.Append(tex.DOFade(0, 0.2f).OnComplete(() =>
        {
            if(isCollect)
                quizGroup.SetActive(false);
            tex.gameObject.SetActive(false);
            isRoad = false;
        }));
        seq.SetUpdate(true);

    }
    private void QuizMake()
    {
        realSo = quizSOs[Random.Range(0, quizSOs.Count)];
        quizTexs[0].text = realSo._quizTex;
        for (int i = 1; i < quizTexs.Count; i++)
        {
            quizTexs[i].text = realSo._quizTexList[i - 1];
        }
    }
}