using UnityEngine;

public class TestToggleUI : MonoBehaviour
{
    [SerializeField] private UI_Screen foodInventory;
    [SerializeField] private UI_Screen processedFoodHotbar;
    [SerializeField] private UI_Screen naturalFoodHotbar;

    private void Update()
    {
        ToggleInventory();
    }

    private void ToggleInventory()
    {
        void Toggle(UI_Screen screen)
        {
            if (screen.gameObject.activeSelf)
                screen.Close();
            else
                screen.Open();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            Toggle(foodInventory);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            Toggle(processedFoodHotbar);
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            Toggle(naturalFoodHotbar);
        }
    }
}
