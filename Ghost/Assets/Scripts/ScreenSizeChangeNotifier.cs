using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ScreenSizeChangeNotifier : MonoBehaviour
{
    [SerializeField] private UnityEvent notifyScreenSizeChange;

    protected void OnRectTransformDimensionsChange()
    {
        if (notifyScreenSizeChange != null)
        {
            notifyScreenSizeChange.Invoke();
        }
    }
}
