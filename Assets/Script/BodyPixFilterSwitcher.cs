using UnityEngine;
using UnityEngine.InputSystem;

namespace NNCam2 {

public sealed class BodyPixFilterSwitcher : MonoBehaviour
{
    [SerializeField] MonoBehaviour[] _filters;
    [SerializeField] Key[] _keys;

    void Update()
    {
        var dev = Keyboard.current;

        var choice = -1;

        for (var i = 0; i < _keys.Length; i++)
        {
            if (dev[_keys[i]].wasPressedThisFrame)
            {
                choice = i;
                break;
            }
        }

        if (choice == -1) return;

        for (var i = 0; i < _filters.Length; i++)
            _filters[i].enabled = choice == i;
    }
}

} // namespace NNCam2
