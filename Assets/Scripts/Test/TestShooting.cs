using UnityEngine;

public class TestShooting : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !UI_Utility.IsMouseOverUI())
        {
            Debug.Log("발사!!!");
        }
    }
}
