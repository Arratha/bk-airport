Shader "Hidden/XRayColor"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"}
        LOD 100
        ZWrite Off Cull Off
        Pass
        {
            Name "ColorBlitPass"

            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            #pragma vertex Vert
            #pragma fragment frag

            TEXTURE2D_X(_CameraOpaqueTexture);
            SAMPLER(sampler_CameraOpaqueTexture);

            float _Cut;
            float _Min;
            float _Max;
            sampler2D _Gradient;

            float remap(float alpha)
            {
                const float alpha_value = saturate(max(_Cut, smoothstep(_Min, _Max, alpha)));
                return round(alpha_value * 31) / 32;
            }
            
            half4 frag (Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                const float4 color = SAMPLE_TEXTURE2D_X(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, input.texcoord);
                const float2 alpha = float2(remap(color.g), 0);
                const float4 xray_color = tex2D(_Gradient, alpha);
                const float4 output = lerp(color, xray_color, step(_Cut, color.a));
                return output;
            }
            ENDHLSL
        }
    }
}