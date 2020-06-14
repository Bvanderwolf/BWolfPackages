using BWolf.Utilities.StatModification;
using UnityEngine;
using UnityEngine.UI;

public class ModifyableSystem : MonoBehaviour
{
    [Header("Stack System")]
    [SerializeField]
    protected StatSystem stackSystem = null;

    [SerializeField]
    protected StatModifierInfo stackModifier = new StatModifierInfo();

    [SerializeField]
    protected Image imgStackSystemFillable = null;

    [SerializeField]
    protected Text txtStackSystemDisplayText = null;

    [SerializeField]
    protected Text txtStackSystemEventDisplay = null;

    [Header("Non Stack System")]
    [SerializeField]
    protected StatSystem nonStackSystem = null;

    [SerializeField]
    protected StatModifierInfo nonStackModifier = new StatModifierInfo();

    [SerializeField]
    protected Image imgNonStackSystemFillable = null;

    [SerializeField]
    protected Text txtNonStackSystemDisplayText = null;

    [SerializeField]
    protected Text txtNonStackSystemEventDisplay = null;

    protected const float alphaCrossfadeTime = 3f;

    protected virtual void Awake()
    {
        stackSystem.SetCurrentToMax();
        stackSystem.AttachFillableBar(imgStackSystemFillable);
        stackSystem.AttachDisplayText(txtStackSystemDisplayText);

        stackSystem.OnDecreaseStart += () => ShowStackEventTextWithFade("Decrease Started");
        stackSystem.OnDecreaseStop += () => ShowStackEventTextWithFade("Decrease Stopped");
        stackSystem.OnReachedMax += () => ShowStackEventTextWithFade("Reached Max");
        stackSystem.OnReachedZero += () => ShowStackEventTextWithFade("Reached Zero");
        stackSystem.OnIncreaseStart += () => ShowStackEventTextWithFade("Increase Started");
        stackSystem.OnIncreaseStop += () => ShowStackEventTextWithFade("Increase Stopped");

        nonStackSystem.SetCurrentToMax();
        nonStackSystem.AttachFillableBar(imgNonStackSystemFillable);
        nonStackSystem.AttachDisplayText(txtNonStackSystemDisplayText);

        nonStackSystem.OnDecreaseStart += () => txtNonStackSystemEventDisplay.text = "Decrease Started";
        nonStackSystem.OnDecreaseStop += () => txtNonStackSystemEventDisplay.text = "Decrease Stopped";
        nonStackSystem.OnReachedMax += () => txtNonStackSystemEventDisplay.text = "Reached Max";
        nonStackSystem.OnReachedZero += () => txtNonStackSystemEventDisplay.text = "Reached Zero";
        nonStackSystem.OnIncreaseStart += () => txtNonStackSystemEventDisplay.text = "Increase Started";
        nonStackSystem.OnIncreaseStop += () => txtNonStackSystemEventDisplay.text = "Increase Stopped";
    }

    private void Update()
    {
        stackSystem.UpdateModifiers();
        stackSystem.UpdateVisuals();

        nonStackSystem.UpdateModifiers();
        nonStackSystem.UpdateVisuals();
    }

    private void ShowStackEventTextWithFade(string text)
    {
        txtStackSystemEventDisplay.canvasRenderer.SetAlpha(1f);
        txtStackSystemEventDisplay.text = text;
        txtStackSystemEventDisplay.CrossFadeAlpha(0, alphaCrossfadeTime, false);
    }

    private void ShowNonStackEventTextWithFade(string text)
    {
        txtNonStackSystemEventDisplay.canvasRenderer.SetAlpha(1f);
        txtNonStackSystemEventDisplay.text = text;
        txtNonStackSystemEventDisplay.CrossFadeAlpha(0, alphaCrossfadeTime, false);
    }

    public virtual void OnValueChanged(string changedValue)
    {
    }

    public virtual void OnTimeChanged(string changedTime)
    {
    }

    public virtual void OnAddStackModifierButtonClick()
    {
    }

    public virtual void OnAddNonStackModifierButtonClick()
    {
    }

    public void OnStackModifierIncreaseToggled(bool value) => stackModifier.Increase = value;

    public void OnStackModifierModifyCurrentToggled(bool value) => stackModifier.ModifiesCurrent = value;

    public void OnStackModifierModifyCurrentWithMaxToggled(bool value) => stackModifier.ModifiesCurrentWithMax = value;

    public void OnNonStackModifierIncreaseToggled(bool value) => nonStackModifier.Increase = value;

    public void OnNonStackModifierModifyCurrentToggled(bool value) => nonStackModifier.ModifiesCurrent = value;

    public void OnNonStackModifierModifyCurrentWithMaxToggled(bool value) => nonStackModifier.ModifiesCurrentWithMax = value;
}