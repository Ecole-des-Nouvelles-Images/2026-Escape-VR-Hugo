using UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonOnSelected : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private MainMenu _mainMenu;

    private void Awake()
    {
        _mainMenu = GetComponentInParent<MainMenu>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_mainMenu == null) return;
        _mainMenu.OnButtonSelected(transform.position);
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_mainMenu == null) return;
        _mainMenu.OnButtonDeselected();
    }
}
