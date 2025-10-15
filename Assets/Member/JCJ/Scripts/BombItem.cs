using UnityEngine;

public class BombItem : item
{
    public override void GetItem(BulbController bulbController)
    {
        bulbController.SubtractTotalPower(30);
    }
}
