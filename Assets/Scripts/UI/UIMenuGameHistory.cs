using UnityEngine;

public class UIMenuGameHistory : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        UIMenuController menuController = gameObject.GetComponentInParent<UIMenuController>();
        if (DebugUtilities.Verify(menuController != null, "MenuController script not found"))
        {
            menuController.ShowMenu(MenuType.MAIN);
        }
    }
}