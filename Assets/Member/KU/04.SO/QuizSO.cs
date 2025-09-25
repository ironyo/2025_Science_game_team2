using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "QuizSO", menuName = "SO/QuizSO")]
public class QuizSO : ScriptableObject
{
    [Header("�����")]
    public string _quizTex;
    [Header("�����ȣ����")]
    public List<string> _quizTexList = new List<string>(3);
    [Header("����")]
    public int _collectNum = 1;
}
