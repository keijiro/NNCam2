using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

namespace NNCam2 {

[AddComponentMenu("VFX/Property Binders/NNCam2/BodyPix Keypoint Binder")]
[VFXBinder("NNCam2/BodyPix Keypoint")]
public class BodyPixKeypointBinder : VFXBinderBase
{
    public string Property
      { get => (string)_property;
        set => _property = value; }

    [VFXPropertyBinding("GraphicsBuffer"), SerializeField]
    ExposedProperty _property = "KeypointBuffer";

    public BodyPixInput Target = null;

    public override bool IsValid(VisualEffect component)
      => Target != null && component.HasGraphicsBuffer(_property);

    public override void UpdateBinding(VisualEffect component)
      => component.SetGraphicsBuffer(_property, Target.KeypointBuffer);

    public override string ToString()
      => $"Keypoints : '{_property}' -> {Target?.name ?? "(null)"}";
}

} // namespace NNCam2
