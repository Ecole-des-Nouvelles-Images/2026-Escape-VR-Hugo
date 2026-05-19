Shader "Custom/VROverlayShader"
{
    Properties
    {
        _MainColor ("Color", Color) = (0,0,0,1)
        _Alpha ("Fade Alpha", Range(0,1)) = 1
    }

    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent" 
            "Queue"="Overlay+100" // Se place tout à la fin de la file de rendu
        }

        Pass
        {
            // CONFIGURATION CRITIQUE POUR L'EFFET
            ZTest Always      // Ignore si un objet est devant : dessine TOUJOURS
            ZWrite Off        // N'écrit pas dans la profondeur
            Cull Off          // Dessine les deux côtés (intérieur/extérieur)
            Blend SrcAlpha OneMinusSrcAlpha // Permet la transparence

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            float4 _MainColor;
            float _Alpha;

            Varyings vert (Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                return output;
            }

            half4 frag (Varyings input) : SV_Target
            {
                half4 col = _MainColor;
                col.a *= _Alpha; // Applique ton curseur alpha
                return col;
            }
            ENDHLSL
        }
    }
}