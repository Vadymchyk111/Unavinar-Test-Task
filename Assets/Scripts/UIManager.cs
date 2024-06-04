using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Main Settings")]
    [SerializeField] private Button _playButton;
    [SerializeField] private List<GameObject> _objectsToActivate;
    [SerializeField] private float _fadeTime;
    [SerializeField] private CanvasGroup _canvasGroup;

    [Header("Hand Settings")]
    [SerializeField] private Transform _handTransform;
    [SerializeField] private Transform _handEndPointTransform;
    [SerializeField] private float _moveTime;

    [Header("Text Settings")] 
    [SerializeField] private TextMeshProUGUI _swipeText;
    [SerializeField] private TextMeshProUGUI _tapText;
    [SerializeField] private float _scaleTime;

    private void Start()
    {
        _handTransform.DOLocalMove(_handEndPointTransform.localPosition, _moveTime).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        _swipeText.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), _scaleTime).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        _tapText.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), _scaleTime).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnEnable()
    {
        _playButton.onClick.AddListener(SetPlayPanelOff);
    }

    private void OnDisable()
    {
        _playButton.onClick.RemoveListener(SetPlayPanelOff);
    }

    private void SetPlayPanelOff()
    {
        _canvasGroup.DOFade(0, _fadeTime).onComplete += () =>
        {
            foreach (GameObject objects in _objectsToActivate)
            {
                objects.SetActive(true);
            }
        };
    }
}
