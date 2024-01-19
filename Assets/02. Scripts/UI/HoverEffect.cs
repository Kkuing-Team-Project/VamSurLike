using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3 hoverScale = new Vector3(1.1f, 1.1f, 1.1f);
    public float hoverHeightIncrease;
    public float x;
    public float y;
    private Vector3 originalScale;
    private Vector2 originalPosition;
    private RectTransform rectTransform;
    private HorizontalLayoutGroup layoutGroup;
    public bool select;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        layoutGroup = GetComponentInParent<HorizontalLayoutGroup>(); // 또는 해당하는 레이아웃 컴포넌트를 찾습니다.
        originalScale = transform.localScale;
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (layoutGroup != null)
        {
            layoutGroup.enabled = false;
        }
        if(select)
        {
            SoundManager.Instance.SelectSound();
        }
        transform.localScale = hoverScale;
        rectTransform.anchoredPosition = new Vector2(x, originalPosition.y + hoverHeightIncrease);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(select)
        {
            SoundManager.Instance.StopBackgroundMusic();
        }
        transform.localScale = originalScale;
        rectTransform.anchoredPosition = new Vector2(x, y);
        if (layoutGroup != null)
        {
            layoutGroup.enabled = true;
        }
    }
}
