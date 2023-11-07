using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomClickHandler : MonoBehaviour
{
    public UnityEvent onLeft;
    public UnityEvent onRight;
    public UnityEvent onMiddle;
 
    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                onLeft.Invoke();
                break;
            case PointerEventData.InputButton.Right:
                onRight.Invoke();
                break;
            case PointerEventData.InputButton.Middle:
                onMiddle.Invoke();
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
