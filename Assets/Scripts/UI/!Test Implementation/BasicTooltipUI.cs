using UnityEngine;
using UnityEngine.UI;

public class BasicTooltipUI : MonoBehaviour
{
    #region Var: Inspector
    [Header("Cursor Offset")]
    [SerializeField] protected Vector2 cursorOffset;

    [Header("Item Data")]
    [SerializeField] protected Text itemName;
    [SerializeField] protected Text itemDesc;
    [SerializeField] protected Text itemCount;
    #endregion

    #region Var: Components
    protected RectTransform rectTransfom;
    protected CanvasGroup canvasGroup;
    #endregion

    #region Var: Refs
    protected Canvas parentCanvas;
    #endregion

    #region Method: Unity
    protected virtual void Awake()
    {
        // Get Component
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransfom = transform as RectTransform;

        // Get Ref
        parentCanvas = transform.root.GetComponentInChildren<Canvas>();
    }
    protected virtual void Update()
    {
        MoveToMouse();
    }
    #endregion

    #region Method: Tooltip
    protected virtual void MoveToMouse()
    {
        if (canvasGroup.alpha == 0)
            return;

        // Move To Mouse Pos
        UI_Utility.MoveTo(parentCanvas, rectTransfom, (Vector2)Input.mousePosition + cursorOffset);
    }
    public virtual void Show()
    {
        canvasGroup.alpha = 1;
    }
    public virtual void Hide()
    {
        canvasGroup.alpha = 0;
    }
    public virtual void UpdateTooltip(Item item)
    {
        itemName.text = item.Name;
        itemDesc.text = item.Desc;
        itemCount.text = $"개수: {item.Count}";
        itemCount.text += item.StackLimit == 0 ? "" : $" / {item.StackLimit}";
    }
    #endregion
}
