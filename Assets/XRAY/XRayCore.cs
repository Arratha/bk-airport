using UnityEngine;
using UnityEngine.Rendering;

namespace Airport
{
    public class XRayCore : MonoBehaviour
    {
        [SerializeField] private float size = 1.0F;
        [SerializeField] private float pixelPerUnit = 512;
        [SerializeField] private MeshRenderer[] target;
        [SerializeField] private Material output;
        [SerializeField] private CameraEvent cameraEvent = CameraEvent.AfterEverything;

        private Material _xrayMaterial;
        private RenderTexture _outputTexture;
        private Camera _xrayCamera;
        
        private void Awake()
        {
            var resolution = Mathf.RoundToInt(pixelPerUnit * size);
            _outputTexture = new RenderTexture(resolution, resolution, 16);
            _xrayMaterial = new Material(Shader.Find("Hidden/Xray"));
            
            output.SetTexture("_MainTex", _outputTexture);

            _xrayCamera = gameObject.GetComponent<Camera>();
            _xrayCamera.orthographic = true;
            _xrayCamera.orthographicSize = size * 0.5F;
            _xrayCamera.cullingMask = 1 << LayerMask.NameToLayer("XRayItems");
            _xrayCamera.backgroundColor = Color.black;
            _xrayCamera.clearFlags = CameraClearFlags.Color;
            _xrayCamera.targetTexture = _outputTexture;
        }

        private void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(size, size, 0));

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Vector3.zero, new Vector3(0, 0, size));
        }
    }
}
