Shader "Custom/SliderShader"
{
    Properties
    {
        _Color1 ("Color1", Color) = (1,0,0,1)
        _Color2 ("Color2", Color) = (1,1,0,1)
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="UI" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _Color1;
            float4 _Color2;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex =UnityObjectToClipPos(v.vertex);
                o.uv = v.vertex.xy;
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv * 2.0 - 1.0;
                fixed4 color = lerp(_Color2, _Color1, i.uv.y);
                fixed4 texColor = tex2D(_MainTex, i.uv);
                 color *= texColor.y;
                return color;
            }
            ENDCG
        }
    }
}
