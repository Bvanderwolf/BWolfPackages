using System;
using UnityEngine;
using UnityEngine.UI;

public class PointBar : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float _border = 10f;

    [Header("Colors")]
    [SerializeField]
    private Color _borderColor = _defaultBorderColor;
    
    [SerializeField]
    private Color _backColor = _defaultBackColor;

    [SerializeField]
    private Color _delayedColor = _defaultDelayedColor;

    [SerializeField]
    private Color _frontColor = _defaultFrontColor;

    [Header("Images")]
    [SerializeField]
    private Image _backImage;

    [SerializeField]
    private Image _delayedImage;

    [SerializeField]
    private Image _frontImage;
    
    [Header("BarParent")]
    [SerializeField]
    private RectTransform _barParent;

    private Image _borderImage;

    private static readonly Color _defaultBorderColor = new Color32(53, 53, 53, 255);
    
    private static readonly Color _defaultBackColor = new Color32(255, 60, 60, 255);
    
    private static readonly Color _defaultDelayedColor = new Color32(255, 49, 49, 255);
    
    private static readonly Color _defaultFrontColor = new Color32(255, 27, 27, 255);

    private void Awake()
    {
        _borderImage = GetComponent<Image>();
    }

    private void OnValidate()
    {
        _borderImage = GetComponent<Image>();
        
        SetColors();
        SetBorder();
    }

    private void SetColors()
    {
        if (_borderImage != null)
            _borderImage.color = _borderColor;
        if (_backImage != null)
            _backImage.color = _backColor;
        if (_delayedImage != null)
            _delayedImage.color = _delayedColor;
        if (_frontImage != null)
            _frontImage.color = _frontColor;
    }

    private void SetBorder()
    {
        if (_barParent == null)
            return;

        _barParent.offsetMin = new Vector2(_border, _border);
        _barParent.offsetMax = new Vector2(-_border, -_border);
    }
}
