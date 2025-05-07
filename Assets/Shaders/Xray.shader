Shader "Hidden/Xray"
{
    Properties
    {
        _Brightness ("Brightness", Range(0, 1)) = 0.5
        _LightDir ("Light Position", Vector) = (0, 1, 0, 0)
        _IlluminationSmoothness ("Illumintaion Smoothness", Int) = 5
        _MinIlluminationValue ("Min Illumination Value", Float) = 0
        _MaxIlluminationValue ("Max Illumination Value", Float) = 1
    }
    SubShader
    {
        Tags { 
            "Queue"="Transparent" 
            "RenderType"="Transparent"
        }
        
        Pass
        {
            Stencil
            {
                Ref 2
            }
            ZWrite On
            ZTest Always
            Blend OneMinusDstColor One
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
            };

            float _Brightness;
            float4 _LightDir;
            int _IlluminationSmoothness;
            float _MinIlluminationValue;
            float _MaxIlluminationValue;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float diff = max(0, dot(normalize(i.worldNormal), _LightDir));
                
                float stepSize = 1.0 / _IlluminationSmoothness;
                float steppedDiff = floor(diff / stepSize) * stepSize;

                float illumination = lerp(_MinIlluminationValue, _MaxIlluminationValue, steppedDiff);
                
                return fixed4(illumination, _Brightness, _Brightness, 0);
            }
            ENDCG
        }
    }
}