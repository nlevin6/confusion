Shader "Unlit/Window"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Size ("Size", float) = 1
        _T ("Time", float) = 1
        _Distortion ("Distortion", range(-5, 5)) = 1
        _Blur("Blur", range(0, 1)) = 1
        _FogDensity ("Fog Density", Range(0, 1)) = 0.5 // Fog density
        _FogColor ("Fog Color", Color) = (0.8, 0.8, 0.8, 1) // Fog color
        _RainIntensity ("Rain Intensity", Range(0, 1)) = 0.5 // Added rain intensity
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha // Enable transparency blending
        ZWrite Off                      // Disable ZWrite for transparent objects

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Size, _T, _Distortion, _Blur;
            float _FogDensity;
            fixed4 _FogColor;
            float _RainIntensity; // Added rain intensity variable

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            float N21(float2 p) {
                p = frac(p * float2(123.34, 345.45));
                p += dot(p, p + 34.345);
                return frac(p.x * p.y);
            }

            float3 Layer(float2 UV, float t) {
                float2 aspect = float2(2, 1);
                float2 uv = UV * _Size * aspect; // Adjust size of grid
                uv.y += t * 0.25; // Move the grid downwards
                float2 gv = frac(uv) - 0.5;
                float2 id = floor(uv); // ID each box

                float n = N21(id);
                t += n * 6.2831;

                float w = UV.y * 10;
                float x = (n - 0.5) * 0.8; // How the water drop moves on the x-axis
                x += (0.4 - abs(x)) * sin(3 * w) * pow(sin(w), 6) * 0.45; // Drops wiggle on x-axis

                float y = -sin(t + sin(t + sin(t) * 0.5)) * 0.45; // How the water drop moves on the y-axis
                y -= (gv.x - x) * (gv.x - x); // Change the main drop's shape

                float2 dropPos = (gv - float2(x, y)) / aspect; // Pos of drop
                float drop = smoothstep(0.05, 0.03, length(dropPos)); // Size of drop

                float2 trailPos = (gv - float2(x, t * 0.25)) / aspect; // Pos of trail
                trailPos.y = (frac(trailPos.y * 8) - 0.5) / 8;
                float trail = smoothstep(0.03, 0.01, length(trailPos)); // Size of trail
                float fogTrail = smoothstep(-0.05, 0.05, dropPos.y); // No little drops under the main drop
                fogTrail *= smoothstep(0.5, y, gv.y); // Fade effect on the trail
                trail *= fogTrail;
                fogTrail *= smoothstep(0.05, 0.04, abs(dropPos.x)); // Fog trail for each water drop

                float2 offs = drop * dropPos + trail * trailPos;

                // Ensure return value
                return float3(offs, fogTrail); // Returning float3 as intended
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float t = fmod(_Time.y + _T, 7200);
                float4 col = tex2D(_MainTex, i.uv);

                // Calculate the number of raindrops based on _RainIntensity
                int numDrops = (int)(3 + _RainIntensity * 10); // Base drops (3) + up to 10 more with intensity

                float3 drops = float3(0, 0, 0);
                float multiplier = 1.0;

                // Add layers of rain drops based on the intensity
                for (int d = 0; d < numDrops; d++)
                {
                    float layerOffset = (d * 0.25 + 0.54) * multiplier;
                    drops += Layer(i.uv * (1.0 + 0.23 * d) + layerOffset, t);
                    multiplier *= 1.1;
                }

                float blur = _Blur * 7 * (1 - drops.z);
                col = tex2Dlod(_MainTex, float4(i.uv + drops.xy * _Distortion, 0, blur));
                
                // Adjust alpha based on your requirements (e.g., opacity control)
                col.a = drops.z; // Use drops.z or set a custom alpha value
                
                // Apply fog effect
                col.rgb = lerp(col.rgb, _FogColor.rgb, _FogDensity * (1.0 - drops.z));
                col.a = max(col.a, _FogDensity); // Ensure the alpha includes the fog density

                return col;
            }
            ENDCG
        }
    }
}
