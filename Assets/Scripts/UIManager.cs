using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static event Action OnGameStarted;
    
    [Header("Main Settings")]
    [SerializeField] private Button _playButton;
    [SerializeField] private List<GameObject> _objectsToActivate;
    [SerializeField] private float _fadeTime;
    [SerializeField] private CanvasGroup _controllerCanvasGroup;
    [SerializeField] private CanvasGroup _winnerCanvasGroup;
    [SerializeField] private Button _nextButton;

    [Header("Hand Settings")]
    [SerializeField] private Transform _handTransform;
    [SerializeField] private Transform _handEndPointTransform;
    [SerializeField] private float _moveTime;

    [Header("Text Settings")] 
    [SerializeField] private TextMeshProUGUI _swipeText;
    [SerializeField] private TextMeshProUGUI _tapText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private float _scaleTime;

    private void Start()
    {
        _levelText.text = $"Level {SceneManager.GetActiveScene().buildIndex + 1}";
        _handTransform.DOLocalMove(_handEndPointTransform.localPosition, _moveTime).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        _swipeText.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), _scaleTime).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        _tapText.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), _scaleTime).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnEnable()
    {
        _playButton.onClick.AddListener(SetPlayPanelOff);
        WinZone.OnPlayerArrived += SetWinnerPanel;
        _nextButton.onClick.AddListener((() =>
        {
            if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
            {
                return;
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }));
    }

    private void OnDisable()
    {
        _playButton.onClick.RemoveListener(SetPlayPanelOff);
        WinZone.OnPlayerArrived -= SetWinnerPanel;
    }

    private void SetPlayPanelOff()
    {
        _controllerCanvasGroup.DOFade(0, _fadeTime).onComplete += () =>
        {
            foreach (GameObject objects in _objectsToActivate)
            {
                objects.SetActive(true);
            }
            OnGameStarted?.Invoke();
        };
    }

    private void SetWinnerPanel()
    {
        _winnerCanvasGroup.DOFade(1, _fadeTime);
    }
}
