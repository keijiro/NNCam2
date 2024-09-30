using UnityEngine;
using UnityEngine.InputSystem;

namespace NNCam2 {

public sealed class BodyPixOverlaySwitcher : MonoBehaviour
{
    [SerializeField] Key _bgOpacityKey;
    [SerializeField] Key _fgOpacityKey;
    [SerializeField] Key _shuffleKey;
    [SerializeField] Key _randomWiperKey;
    [SerializeField] Key _bgWiperKey;
    [SerializeField] Key _fgWiperKey;
    [SerializeField] Key _wiperKickKey;

    void Update()
    {
        var dev = Keyboard.current;
        var ctrl = GetComponent<BodyPixOverlayController>();

        if (dev[_bgOpacityKey].wasPressedThisFrame)
            ctrl.BackgroundOpacity = (ctrl.BackgroundOpacity == 0) ? 1 : 0;

        if (dev[_fgOpacityKey].wasPressedThisFrame)
            ctrl.ForegroundOpacity = (ctrl.ForegroundOpacity == 0) ? 1 : 0;

        if (dev[_shuffleKey].wasPressedThisFrame)
            ctrl.ShufflePalette();

        if (dev[_randomWiperKey].wasPressedThisFrame)
            ctrl.RandomizeWiperDirection ^= true;

        if (dev[_bgWiperKey].wasPressedThisFrame)
            ctrl.EnableBackgroundWiper ^= true;

        if (dev[_fgWiperKey].wasPressedThisFrame)
            ctrl.EnableForegroundWiper ^= true;

        if (dev[_wiperKickKey].wasPressedThisFrame)
            ctrl.KickWiper();
    }
}

} // namespace NNCam2
