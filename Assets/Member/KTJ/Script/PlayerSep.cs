using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class PlayerSep : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject BodyPref;
    [SerializeField] private float BodyGap;
    [SerializeField] private int BodyCount;

    private List<GameObject> bodyObjects = new List<GameObject>();
    private int currentBodyCounts;

    private void OnValidate()
    {
        if ((BodyCount - 1) % 2 != 0 )
        {
            Debug.Log("[SerializeField] BodyCount 값에는 1, 3, 5...(+ 2) 단위로 입력해라 시봉방것");
            BodyCount = 0;
        }
    }
    private void Awake()
    {
        currentBodyCounts = BodyCount;
    }

    private void Start()
    {
        InitializedBodys();
    }

    public void InitializedBodys()
    {
        for (int i = 0; i < BodyCount; i++)
        {
            GameObject _clonedBody = (Instantiate(BodyPref, Player.transform));
            _clonedBody.name = "Body" + i.ToString();
            int section = Mathf.CeilToInt(i / 2f);
            int upDown = 0;
            if (i != 0)
            {
                if (i % 2 == 0)
                {
                    upDown = 1;
                }
                else
                {
                    upDown = -1;
                }
            }
            _clonedBody.transform.localPosition = new Vector3(0, upDown * section * BodyGap, 0);
        }
    }

    public void SeparatePlayer()
    {

    }

    public void CombinePlayer()
    {

    }
}
