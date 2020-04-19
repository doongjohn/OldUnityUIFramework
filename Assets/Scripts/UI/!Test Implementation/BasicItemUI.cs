using UnityEngine;
using UnityEngine.UI;

public class BasicItemUI : DraggableItemUI
{
    #region Var: Inspector
    [Header("Item")]
    [SerializeField] private Image iconImage;
    [SerializeField] private Text countText;

    [Header("Tooltip")]
    [SerializeField] private BasicTooltipUI tooltipPrefab;
    #endregion

    #region Var: Refs
    private BasicTooltipUI tooltipObject;
    #endregion

    #region Method: Unity
    private void Start()
    {
        if (tooltipObject == null)
            tooltipObject = Instantiate(tooltipPrefab.gameObject, transform, false).GetComponent<BasicTooltipUI>();

        tooltipObject.Hide();
    }
    #endregion

    #region Method Override: Item UI
    public override void OnUpdateUI(Item item)
    {
        base.OnUpdateUI(item);
        iconImage.sprite = item.Icon;
        countText.text = $"{item.Count}";
    }
    #endregion

    #region Method Override: Tooltip UI
    public override void ShowTooltip()
    {
        if (tooltipObject == null)
            return;

        tooltipObject.Show();
        UpdateTooltip();
    }
    public override void HideTooltip()
    {
        if (tooltipObject == null)
            return;

        tooltipObject.Hide();
    }
    public override void UpdateTooltip()
    {
        if (tooltipObject == null)
            return;

        tooltipObject.UpdateTooltip(ItemObj);
    }
    #endregion
}
