using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "QuizSO", menuName = "SO/QuizSO")]
public class QuizSO : ScriptableObject
{
    [Header("퀴즈내용")]
    public string _quizTex;
    [Header("퀴즈번호내용")]
    public List<string> _quizTexList = new List<string>(3);
    [Header("정답")]
    public int _collectNum = 1;
}
