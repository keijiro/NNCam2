using UnityEngine;
using UnityEngine.InputSystem;

namespace NNCam2 {

public sealed class BodyPixInjectionSwitcher : MonoBehaviour
{
    [SerializeField] Key _backgroundKey;
    [SerializeField] Key _foregroundKey;

    void Update()
    {
        var dev = Keyboard.current;
        var ctrl = GetComponent<BodyPixInjectionController>();

        if (dev[_backgroundKey].wasPressedThisFrame)
            ctrl.BackgroundOpacity = (ctrl.BackgroundOpacity == 0) ? 1 : 0;

        if (dev[_foregroundKey].wasPressedThisFrame)
            ctrl.ForegroundOpacity = (ctrl.ForegroundOpacity == 0) ? 1 : 0;
    }
}

} // namespace NNCam2
