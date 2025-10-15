using UnityEngine;

public class electricItem : item
{
    public QuizManager quizManager;
    public override void GetItem(BulbController bulbController)
    {
        quizManager.QuizStart();
    }
}
