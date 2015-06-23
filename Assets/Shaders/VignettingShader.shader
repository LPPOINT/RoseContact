Shader "Custom/Vignette" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("MainColor", Color) = (1, 1, 1, 1)
        _VignettePower ("VignettePower", Range(0.0,6.0)) = 5.5
		_VignetteColor ("Vignette Color", Color) = (0, 0, 0, 1)
    }
    SubShader
    {
        Pass
        {
        
        CGPROGRAM
        #pragma vertex vert_img
        #pragma fragment frag
        #pragma fragmentoption ARB_precision_hint_fastest
        #include "UnityCG.cginc"

        uniform sampler2D _MainTex;
        uniform float _VignettePower;
		uniform float4 _VignetteColor;
		uniform float4 _Color;

        struct v2f
        {
            float2 texcoord    : TEXCOORD0;
        };
        
        float4 frag(v2f_img i) : COLOR
        {
        float4 renderTex =  tex2D(_MainTex, i.uv) * _Color;
        float2 dist = (i.uv - 0.5f) * 1.25f;
        dist.x = 1 - dot(dist, dist) * _VignettePower;
        renderTex *= dist.x ;
        return renderTex;
        
        }

        ENDCG
        }
    }
}