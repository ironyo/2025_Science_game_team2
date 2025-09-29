using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class QuizManager : MonoBehaviour
{
    [SerializeField] List<QuizSO> quizSOs = new List<QuizSO>();
    [SerializeField] List<TextMeshProUGUI> quizTexs = new List<TextMeshProUGUI>();
    private QuizSO realSo;
    public void QuizStart()
    {
        Time.timeScale = 0.0f;
        realSo = quizSOs[Random.Range(0, quizSOs.Count)];
        quizTexs[0].text = realSo._quizTex;
        for (int i = 1; i < quizTexs.Count; i++)
        {
            quizTexs[i].text = realSo._quizTexList[i-1];
        }
    }
    public void WhatIsCollect(int answer)
    {
        if(answer == realSo._collectNum)
        {

        }
    }
}
