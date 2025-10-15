using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro; // TextMeshProUGUI를 사용하기 위해 꼭 필요합니다.
using UnityEngine;

public class BulbController : MonoBehaviour
{
    private enum BulbState
    {
        Separated,
        Combined
    }

    [Header(" 전구 설정")]
    [SerializeField] private GameObject bulbPrefab;
    [SerializeField] private int bulbCount = 3;
    [SerializeField] private float bulbGap = 1.5f;

    [Header(" 움직임 설정")]
    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private Ease moveEase = Ease.OutQuad;

    [Header(" 전력 설정")]
    [SerializeField] private int baseElecPower = 120; // 전체 전력의 기준값

    [Header(" UI 설정")]
    [SerializeField] private TextMeshProUGUI totalPowerText; // 전체 전력을 표시할 UI 텍스트

    private List<GameObject> bulbs = new List<GameObject>();
    private List<TextMeshPro> powerTexts = new List<TextMeshPro>();
    private List<Vector3> originalPositions = new List<Vector3>();
    private bool isMoving = false;
    private BulbState currentState = BulbState.Separated;

    // 각 전구의 개별 전력을 저장하는 리스트
    private List<int> bulbPowers = new List<int>();

    // 합쳐진 전구 인덱스를 추적하기 위한 필드 추가
    private int combinedTargetIndex = -1;

    private void Start()
    {
        InitializeBulbs();
        UpdateTotalPowerText(); // 게임 시작 시 전체 전력 텍스트 업데이트
    }

    private void Update()
    {
        if (isMoving) return;

        switch (currentState)
        {
            case BulbState.Separated:
                for (int i = 0; i < bulbCount; i++)
                {
                    if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                    {
                        currentState = BulbState.Combined;
                        StartCoroutine(MoveToTarget(i));
                        break;
                    }
                }
                // 예시: 분리된 상태에서 0번 전구의 전력을 10 증가 (테스트용)
                if (Input.GetKeyDown(KeyCode.F1))
                {
                    SetBulbPower(0, bulbPowers[0] + 10);
                }
                break;

            case BulbState.Combined:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    currentState = BulbState.Separated;
                    StartCoroutine(ReturnToOriginalPositions());
                }
                // 예시: 합쳐진 상태에서 전체 전력을 20 증가 (테스트용)
                if (Input.GetKeyDown(KeyCode.F2))
                {
                    AddTotalPower(20);
                }
                // 예시: 합쳐진 상태에서 전체 전력을 5 감소 (테스트용)
                if (Input.GetKeyDown(KeyCode.F3))
                {
                    SubtractTotalPower(5);
                }
                break;
        }
    }

    private void UpdateTotalPowerText()
    {
        if (totalPowerText != null)
        {
            totalPowerText.text = $"전체전력: {baseElecPower}W";
        }
    }

    // ----------------------------------------------------------------------------------
    // 전력 관리 메서드 (이전과 동일)
    // ----------------------------------------------------------------------------------

    public void SetBulbPower(int index, int newPower)
    {
        if (index < 0 || index >= bulbPowers.Count) return;

        baseElecPower -= bulbPowers[index];
        bulbPowers[index] = newPower;
        baseElecPower += newPower;

        UpdateTotalPowerText();

        if (currentState == BulbState.Separated)
        {
            powerTexts[index].text = bulbPowers[index].ToString();
        }
        else if (currentState == BulbState.Combined)
        {
            UpdatePowerForCombine(combinedTargetIndex);
        }
    }

    public void AddTotalPower(int power)
    {
        baseElecPower += power;
        UpdateTotalPowerText();

        if (currentState == BulbState.Combined && combinedTargetIndex != -1)
        {
            UpdatePowerForCombine(combinedTargetIndex);
        }
    }

    public void SubtractTotalPower(int power)
    {
        baseElecPower -= power;
        if (baseElecPower < 0) baseElecPower = 0;

        UpdateTotalPowerText();

        if (currentState == BulbState.Combined && combinedTargetIndex != -1)
        {
            UpdatePowerForCombine(combinedTargetIndex);
        }
    }

    // ----------------------------------------------------------------------------------
    // 초기화 및 움직임 메서드 (주요 변경점은 UpdatePowerForSeparate에 있음)
    // ----------------------------------------------------------------------------------

    private void SetCurrentTargetIndex(int index) => combinedTargetIndex = index;

    private void InitializeBulbs()
    {
        int dividedPower = baseElecPower / bulbCount;
        for (int i = 0; i < bulbCount; i++)
        {
            GameObject bulb = Instantiate(bulbPrefab, transform);
            bulb.name = $"Bulb_{i}";
            bulbs.Add(bulb);
            TextMeshPro txt = bulb.transform.Find("Text").GetComponent<TextMeshPro>();
            powerTexts.Add(txt);

            // 초기 bulbPowers 설정
            bulbPowers.Add(dividedPower);

            int section = Mathf.CeilToInt(i / 2f);
            int direction = (i == 0) ? 0 : (i % 2 == 0 ? 1 : -1);
            float posY = direction * section * bulbGap;
            Vector3 initialPos = new Vector3(0, posY, 0);
            bulb.transform.localPosition = initialPos;
            originalPositions.Add(initialPos);
        }
        UpdatePowerForSeparate();
    }

    private IEnumerator MoveToTarget(int targetIndex)
    {
        isMoving = true;
        Vector3 targetPosition = originalPositions[targetIndex];

        SetCurrentTargetIndex(targetIndex);

        UpdatePowerForCombine(targetIndex);

        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < bulbs.Count; i++)
        {
            if (i == targetIndex) continue;
            sequence.Join(bulbs[i].transform.DOLocalMove(targetPosition, moveSpeed).SetEase(moveEase));
        }
        yield return sequence.WaitForCompletion();
        isMoving = false;
    }

    private IEnumerator ReturnToOriginalPositions()
    {
        isMoving = true;
        SetCurrentTargetIndex(-1);

        // 분리될 때 각 전구의 전력을 전체 전력의 평균으로 맞춥니다.
        UpdatePowerForSeparate();

        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < bulbs.Count; i++)
        {
            sequence.Join(bulbs[i].transform.DOLocalMove(originalPositions[i], moveSpeed).SetEase(moveEase));
        }
        yield return sequence.WaitForCompletion();
        isMoving = false;
    }

    // ----------------------------------------------------------------------------------
    // 핵심 수정 부분: 분리 시 전력 평균화
    // ----------------------------------------------------------------------------------

    /// <summary>
    /// 분리 상태일 때 각 전구의 전력을 현재 baseElecPower를 기준으로 평균화하고 텍스트를 업데이트합니다.
    /// </summary>
    private void UpdatePowerForSeparate()
    {
        // 1. 현재 전체 전력(baseElecPower)을 전구 수로 나누어 균등하게 분배할 값을 계산합니다.
        int dividedPower = baseElecPower / bulbCount;

        // 2. 모든 전구의 개별 전력(bulbPowers)을 이 값으로 재설정합니다 (평균화).
        for (int i = 0; i < powerTexts.Count; i++)
        {
            bulbPowers[i] = dividedPower;

            // 3. 텍스트를 업데이트하고 활성화합니다.
            powerTexts[i].text = bulbPowers[i].ToString();
            powerTexts[i].gameObject.SetActive(true);
        }

        // 참고: 정수 나눗셈으로 인해 baseElecPower의 총합과 bulbPowers의 총합이 1~2W 정도 차이날 수 있습니다.
        // 예를 들어 121W / 3 = 40W. baseElecPower는 121W이지만, 각 전구의 합은 120W가 됩니다.
    }

    // ----------------------------------------------------------------------------------

    private void UpdatePowerForCombine(int targetIndex)
    {
        for (int i = 0; i < powerTexts.Count; i++)
        {
            if (i == targetIndex)
            {
                powerTexts[i].text = baseElecPower.ToString();
                powerTexts[i].gameObject.SetActive(true);
            }
            else
            {
                powerTexts[i].gameObject.SetActive(false);
            }
        }
    }
}