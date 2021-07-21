using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace NNCam2 {

sealed class RecolorController : MonoBehaviour
{
    void Start()
    {
        var pass = (FullScreenCustomPass)GetComponent<CustomPassVolume>().customPasses[0];

        var m = new Material(pass.fullscreenPassMaterial);
        m.name += " (Cloned)";

        m.SetFloat("_TestValue", 0.8f);

        pass.fullscreenPassMaterial = m;
    }
}

} // namespace NNCam2
