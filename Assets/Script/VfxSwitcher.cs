using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.InputSystem;

namespace NNCam2 {

public sealed class VfxSwitcher : MonoBehaviour
{
    [SerializeField] VisualEffect[] _vfx;

    void Update()
    {
        var dev = Keyboard.current;

        // 1-3: Toogles
        for (var i = 0; i < 3; i++)
            _vfx[i].enabled ^= dev[Key.Digit1 + i].wasPressedThisFrame;

        // 4-9: Radio button-like selector
        // 0: Reset
        var choice = -1;

        for (var i = 3; i <= _vfx.Length; i++)
        {
            if (dev[Key.Digit1 + i].wasPressedThisFrame)
            {
                choice = i;
                break;
            }
        }

        if (choice == -1) return;

        for (var i = 3; i < _vfx.Length; i++)
            _vfx[i].SetFloat("Throttle", choice == i ? 1 : 0);
    }
}

} // namespace NNCam2
