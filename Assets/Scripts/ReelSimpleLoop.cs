using UnityEngine;
using DG.Tweening;

public class ReelSimpleLoop : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private RectTransform container;

    [SerializeField] private float maxSpeed = 1800f;
    [SerializeField] private float startY = 90f;
    [SerializeField] private float resetY = 850f;
    [SerializeField] private float gap = 190f;

    [SerializeField] private float minStopTime = 2f;

    #endregion
public System.Action OnReelStopped;
    #region Private Fields

    private float currentSpeed = 0f;

    private bool isSpinning = false;
    private bool isStopping = false;
    private RectTransform[] items;

    #endregion

    #region Unity Lifecycle
    private void Awake()
{
    int count = container.childCount;
    items = new RectTransform[count];

    for (int i = 0; i < count; i++)
    {
        items[i] = container.GetChild(i).GetComponent<RectTransform>();
    }
}

    private void Start()
    {
        container.anchoredPosition = new Vector2(0f, startY);
    }

    private void Update()
    {
        if (!isSpinning || isStopping) return;

        container.anchoredPosition += Vector2.up * currentSpeed * Time.deltaTime;

        if (container.anchoredPosition.y >= resetY)
        {
            container.anchoredPosition = new Vector2(0f, startY);
        }
    }
    public float GetNormalizedY()
{
    float loopHeight = resetY - startY;

    float normalizedY = startY + Mathf.Repeat(container.anchoredPosition.y - startY, loopHeight);

    return normalizedY;
}

    #endregion

    #region Callable Functions

    public void StartSpin()
    {
        isSpinning = true;
        isStopping = false;

        DOTween.To(() => currentSpeed, x => currentSpeed = x, maxSpeed, 0.5f)
            .SetEase(Ease.OutQuad);
    }
    public void StopSpin()
    {
        if (!isSpinning || isStopping) return;

        isStopping = true;

        float loopHeight = resetY - startY;

        int index = Random.Range(0, 7);
        float baseTarget = startY + (index * gap);

        float currentY = container.anchoredPosition.y;
        float normalizedY = startY + Mathf.Repeat(currentY - startY, loopHeight);

        float targetY = baseTarget;

        if (targetY <= normalizedY)
        {
            targetY += loopHeight;
        }

        float requiredDistance = (currentSpeed * minStopTime) / 2f;

        float distance = targetY - currentY;

        while (distance < requiredDistance)
        {
            targetY += loopHeight;
            distance = targetY - currentY;
        }

        float duration = (2f * distance) / currentSpeed;

        float virtualY = currentY;

        DOTween.To(() => virtualY, x =>
        {
            virtualY = x;

            float wrappedY = startY + Mathf.Repeat(virtualY - startY, loopHeight);
            container.anchoredPosition = new Vector2(0f, wrappedY);

        }, targetY, duration)
        .SetEase(Ease.OutCubic)
        .OnComplete(() =>
{
    float finalY = startY + Mathf.Repeat(targetY - startY, loopHeight);
    container.anchoredPosition = new Vector2(0f, finalY);

    currentSpeed = 0f;
    isSpinning = false;
    isStopping = false;

    float normalizedY = GetNormalizedY();
    OnReelStopped?.Invoke();

    Debug.Log($"{gameObject.name} stopped at Y: {normalizedY}");
});

        DOTween.To(() => currentSpeed, x => currentSpeed = x, 0f, duration)
            .SetEase(Ease.OutCubic);
    }

    #endregion
    #region Get Symbol
    public SlotSymbol GetCurrentSymbol()
{
    float normalizedY = GetNormalizedY();

    int index = Mathf.RoundToInt((normalizedY - startY) / gap);

    
    index = (index + 1) % items.Length;

    return items[index].GetComponent<ReelItem>().symbol;
}
#endregion
}