Shader "UI/RadioWave_Canvas"
{
    Properties
    {
        [Header(Wave)]
        _Frequency ("Frequency", Range(0.5, 20)) = 3
        _Amplitude ("Amplitude", Range(0, 1)) = 0.5
        _Speed ("Scroll Speed", Range(0, 10)) = 2

        [Header(Noise)]
        _NoiseAmount ("Noise Amount", Range(0, 1)) = 0.3
        _NoiseSpeed ("Noise Speed", Range(0, 20)) = 10
        _NoiseScale ("Noise Scale", Range(1, 50)) = 20

        [Header(Line)]
        _Thickness ("Thickness", Range(0.001, 0.05)) = 0.01
        _Glow ("Glow", Range(0, 0.1)) = 0.02

        [Header(Colors)]
        _WaveColor ("Wave Color", Color) = (0, 1, 0.5, 1)
        _BGColor ("Background Color", Color) = (0.05, 0.05, 0.1, 1)

        [Header(UI Rendering)]
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
            "RenderPipeline" = "UniversalPipeline"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Cull Off
        ColorMask [_ColorMask]

        Pass
        {
            Name "RadioWave"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
                float4 color       : COLOR;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv         : TEXCOORD0;
                float4 color       : COLOR;
                float4 worldPos    : TEXCOORD1;
            };

            // Properties
            half _Frequency;
            half _Amplitude;
            half _Speed;
            half _Thickness;
            half _Glow;
            half _NoiseAmount;
            half _NoiseSpeed;
            half _NoiseScale;
            half4 _WaveColor;
            half4 _BGColor;

            // UI clipping
            float4 _ClipRect;

            // Hash para ruido
            float hash(float2 p)
            {
                float3 p3 = frac(float3(p.xyx) * 0.1031);
                p3 += dot(p3, p3.yzx + 33.33);
                return frac((p3.x + p3.y) * p3.z);
            }

            // Value noise suave
            float valueNoise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);
                f = f * f * (3.0 - 2.0 * f);

                float a = hash(i);
                float b = hash(i + float2(1.0, 0.0));
                float c = hash(i + float2(0.0, 1.0));
                float d = hash(i + float2(1.0, 1.0));

                return lerp(lerp(a, b, f.x), lerp(c, d, f.x), f.y);
            }

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                o.color = v.color;
                o.worldPos = v.positionOS;
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                float2 uv = i.uv;

                // Centrar Y: de [0,1] a [-0.5, 0.5]
                float y = uv.y - 0.5;
                float x = uv.x;

                float t = _Time.y;

                // === ONDA SINUSOIDAL ===
                float wave = sin(x * _Frequency * 6.2832 + t * _Speed) * _Amplitude * 0.5;

                // === RUIDO / ESTATICA ===
                float noise = 0.0;
                if (_NoiseAmount > 0.001)
                {
                    float noiseSample = valueNoise(
                        float2(x * _NoiseScale + t * _NoiseSpeed, t * 0.5)
                    );
                    noise = (noiseSample - 0.5) * _NoiseAmount;
                }

                // Combinar onda + ruido
                float waveY = wave + noise;

                // === DIBUJAR LA LINEA ===
                float dist = abs(y - waveY);

                // Smoothstep: borde interior nitido, borde exterior difuso (glow)
                float waveLine = smoothstep(_Thickness + _Glow, _Thickness * 0.3, dist);

                // === COLOR FINAL ===
                half4 col;
                col.rgb = lerp(_BGColor.rgb, _WaveColor.rgb * i.color.rgb, waveLine);
                half bgAlpha = _BGColor.a;
                half waveAlpha = _WaveColor.a * i.color.a;
                col.a = lerp(bgAlpha, waveAlpha, (half)waveLine);

                // UI Clipping (para que funcione con Mask/RectMask2D)
                #ifdef UNITY_UI_CLIP_RECT
                    float2 inside = step(_ClipRect.xy, i.worldPos.xy) * step(i.worldPos.xy, _ClipRect.zw);
                    col.a *= inside.x * inside.y;
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                    clip(col.a - 0.001);
                #endif

                return col;
            }
            ENDHLSL
        }
    }
}
