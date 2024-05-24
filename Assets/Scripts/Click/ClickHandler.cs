using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action OnClick;
    public event Action OnPointerUp;
    [SerializeField] private RectTransform _textTransform;

    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            OnClick.Invoke();
            
        });
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _textTransform.anchoredPosition = new Vector3(30f, -30f, 0);
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        _textTransform.anchoredPosition = new Vector3(0, 0, 0);
    }
}
