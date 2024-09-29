using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace NNCam2 {

public sealed class BodyPixInjectionController : MonoBehaviour
{
    #region Public properties

    [field:SerializeField] public float BackgroundOpacity { get; set; } = 1;
    [field:SerializeField] public float ForegroundOpacity { get; set; } = 1;

    #endregion

    #region MonoBehaviour implementation

    Material _material;

    void Start()
    {
        // Get a reference to the FullScreenCustomPass object.
        var volume = GetComponent<CustomPassVolume>();
        var pass = (FullScreenCustomPass)volume.customPasses[0];

        // Clone and replace the original material.
        _material = new Material(pass.fullscreenPassMaterial);
        _material.name += " (Clone)";
        pass.fullscreenPassMaterial = _material;
    }

    void LateUpdate()
    {
        // Parameter update
        var oparams = new Vector2(BackgroundOpacity, ForegroundOpacity);
        _material.SetVector("_Opacity", oparams);
    }

    #endregion
}

} // namespace NNCam2
