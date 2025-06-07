Shader "Custom/SpriteDissolve"
{
    Properties
    {
        _MainTex("Sprite",     2D)    = "white" {}
        _NoiseTex("Noise Mask",2D)    = "white" {}
        _Cutoff("Dissolve Amount", Range(0,1)) = 0
        _EdgeColor("Edge Color", Color)        = (1,1,1,1)
        _EdgeWidth("Edge Width", Range(0,1))   = 0.05
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // 1) Defina a struct de entrada com o POSITION e o TEXCOORD0
            struct appdata_t {
                float4 vertex   : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            // 2) Seus uniforms
            sampler2D _MainTex;
            float4   _MainTex_ST;
            sampler2D _NoiseTex;
            float     _Cutoff;
            float4    _EdgeColor;
            float     _EdgeWidth;

            // 3) Dados que vão pro fragment
            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };

            // 4) Vertex: transforma vértice e passa UV
            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv  = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            // 5) Fragment: dissolve + contorno
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 sprite = tex2D(_MainTex, i.uv);
                float   noise  = tex2D(_NoiseTex, i.uv).r;

                // descarta pixels abaixo do cutoff
                if (noise < _Cutoff) discard;

                // calcula contorno
                float edge = smoothstep(_Cutoff, _Cutoff + _EdgeWidth, noise);
                sprite.rgb = lerp(_EdgeColor.rgb, sprite.rgb, edge);

                return sprite;
            }
            ENDCG
        }
    }
}
