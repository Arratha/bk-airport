Shader "Hidden/Xray"
{
    Properties
    {
        _Brightness ("Brightness", Range(0, 1)) = 0.5
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
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            float _Brightness;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return fixed4(_Brightness, _Brightness, _Brightness, 0);
            }
            ENDCG
        }
    }
}