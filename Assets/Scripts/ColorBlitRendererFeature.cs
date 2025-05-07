using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorBlitRendererFeature : ScriptableRendererFeature
{
    [SerializeField][Range(0, 1)] private float cutEdge = 0.01F;
    [SerializeField][Range(-5, 5)] private float minEdge = 0.5F;
    [SerializeField][Range(-5, 5)] private float maxEdge = 0.9F;
    [SerializeField] private Texture2D gradient;
    
    private Material _material;
    private ColorBlitPass _renderPass;

    public override void AddRenderPasses(ScriptableRenderer renderer,
        ref RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
            renderer.EnqueuePass(_renderPass);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer,
        in RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            // Calling ConfigureInput with the ScriptableRenderPassInput.Color argument
            // ensures that the opaque texture is available to the Render Pass.
            _renderPass.ConfigureInput(ScriptableRenderPassInput.Color);
            _renderPass.SetTarget(renderer.cameraColorTargetHandle);
        }
    }

    public override void Create()
    {
        var shader = Shader.Find("Hidden/XRayColor");
        _material = CoreUtils.CreateEngineMaterial(shader);
        _material.SetFloat("_Cut", cutEdge);
        _material.SetFloat("_Min", minEdge);
        _material.SetFloat("_Max", maxEdge);
        _material.SetTexture("_Gradient", gradient);
        _renderPass = new ColorBlitPass(_material);
    }

    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(_material);
    }
}