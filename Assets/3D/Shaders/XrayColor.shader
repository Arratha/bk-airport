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

            const int firstOctave = 3;
            const int octaves = 8;
            const float persistence = 0.6;

            float _Noise;
            float _Cut;
            float _Min;
            float _Max;
            sampler2D _Gradient;

          
            uint hash(uint x, uint seed) {
                const uint m = 0x5bd1e995U;
                uint hash = seed;
                // process input
                uint k = x;
                k *= m;
                k ^= k >> 24;
                k *= m;
                hash *= m;
                hash ^= k;
                // some final mixing
                hash ^= hash >> 13;
                hash *= m;
                hash ^= hash >> 15;
                return hash;
            }

            uint hash(float2 x, uint seed)
            {
                const uint m = 0x5bd1e995U;
                uint hash = seed;
                // process first vector element
                uint k = x.x; 
                k *= m;
                k ^= k >> 24;
                k *= m;
                hash *= m;
                hash ^= k;
                // process second vector element
                k = x.y; 
                k *= m;
                k ^= k >> 24;
                k *= m;
                hash *= m;
                hash ^= k;
                // some final mixing
                hash ^= hash >> 13;
                hash *= m;
                hash ^= hash >> 15;
                return hash;
            }

            float2 gradientDirection(uint hash)
            {
                switch (int(hash) & 3) {
                    case 0: return float2(1.0, 1.0);
                    case 1: return float2(-1.0, 1.0);
                    case 2: return float2(1.0, -1.0);
                    case 3: return float2(-1.0, -1.0);
                }
            }

            float interpolate(float value1, float value2, float value3, float value4, float2 t)
            {
                return lerp(lerp(value1, value2, t.x), lerp(value3, value4, t.x), t.y);
            }

            float2 fade(float2 t) {
                // 6t^5 - 15t^4 + 10t^3
	            return t * t * t * (t * (t * 6.0 - 15.0) + 10.0);
            }

            float perlinNoise(float2 position, uint seed) {
                float2 floorPosition = floor(position);
                float2 fractPosition = position - floorPosition;
                float2 cellCoordinates = float2(floorPosition);
                float value1 = dot(gradientDirection(hash(cellCoordinates, seed)), fractPosition);
                float value2 = dot(gradientDirection(hash((cellCoordinates + float2(1, 0)), seed)), fractPosition - float2(1.0, 0.0));
                float value3 = dot(gradientDirection(hash((cellCoordinates + float2(0, 1)), seed)), fractPosition - float2(0.0, 1.0));
                float value4 = dot(gradientDirection(hash((cellCoordinates + float2(1, 1)), seed)), fractPosition - float2(1.0, 1.0));
                return interpolate(value1, value2, value3, value4, fade(fractPosition));
            }

            float perlinNoise(float2 position, int frequency, int octaveCount, float persistence, float lacunarity, uint seed) {
                float value = 0.0;
                float amplitude = 1.0;
                float currentFrequency = float(frequency);
                uint currentSeed = seed;
                for (int i = 0; i < octaveCount; i++) {
                    currentSeed = hash(currentSeed, 0x0U); // create a new seed for each octave
                    value += perlinNoise(position * currentFrequency, currentSeed) * amplitude;
                    amplitude *= persistence;
                    currentFrequency *= lacunarity;
                }
                return value;
            }
            
            float alpha_process(float alpha, float2 uv)
            {
                const float noise = _Noise * saturate(perlinNoise((uv * float2(2, 1)) * 100, 25, 6, 0.75, 8.0, _Time.x * 0x578437adU));
                const float alpha_value = saturate(max(_Cut, smoothstep(_Min, _Max, alpha)));
                return (round(alpha_value * 32) / 32) + noise;
            }
            
            half4 frag (Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                const float4 color = SAMPLE_TEXTURE2D_X(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, input.texcoord);
                const float sub_alpha = (1 - 0.1 * color.r) + color.b;
                const float2 alpha = float2(alpha_process(sub_alpha, input.texcoord), 0);
                const float4 xray_color = tex2D(_Gradient, alpha);
                const float4 output = lerp(1, xray_color, step(0.01, color.a));

                return output;
            }
            ENDHLSL
        }
    }
}