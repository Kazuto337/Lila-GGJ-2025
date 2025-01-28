Shader "Custom/BubbleShader"
{
    Properties
    {
        _MainColor ("Base Color", Color) = (1, 1, 1, 1)
        _Smoothness ("Smoothness", Range(0,1)) = 1
        _Transparency ("Transparency", Range(0,1)) = 0.5
        _FresnelColor ("Fresnel Color", Color) = (0.5, 0.7, 1, 1)
        _IridescenceStrength ("Iridescence Strength", Range(0,1)) = 0.5
        _ReflectionColor ("Reflection Color", Color) = (0.8, 0.8, 0.8, 1)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 300

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
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
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            float _Smoothness;
            float _Transparency;
            float4 _MainColor;
            float4 _FresnelColor;
            float4 _ReflectionColor;
            float _IridescenceStrength;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.viewDir = normalize(_WorldSpaceCameraPos - o.worldPos);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Fresnel Effect (Edge glow)
                float fresnel = pow(1.0 - saturate(dot(i.worldNormal, i.viewDir)), 2.0);
                float4 fresnelColor = fresnel * _FresnelColor;

                // Iridescence effect based on view angle
                float3 iridescence = sin(i.worldNormal * 10.0 + _Time.y) * _IridescenceStrength;
                float4 iridescenceColor = float4(iridescence, 1.0);

                // Reflection color
                float4 reflectionColor = _ReflectionColor * fresnel;

                // Final Color Calculation
                float4 baseColor = _MainColor;
                baseColor.a = _Transparency;
                float4 finalColor = lerp(baseColor, fresnelColor + iridescenceColor + reflectionColor, fresnel);

                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Transparent/Diffuse"
}
