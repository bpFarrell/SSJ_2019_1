Shader "Hidden/Panel"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Pan("Pan",Float) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
			float _Pan;
			fixed4 frag(v2f i) : SV_Target
			{
				i.uv.x = fmod(i.uv.x +_Pan,1);

				float2 texel = 1 / _ScreenParams.xy;
				float edge = min(i.uv.x * _ScreenParams.x, (1 - i.uv.x) * _ScreenParams.x);
				edge = min(edge, i.uv.y * _ScreenParams.y - 50);
				edge = min(edge, (1 - i.uv.y) * _ScreenParams.y - 50);
				if (edge < 20) {

					return fixed4(1, 1, 1, 1)* smoothstep(20,15,edge)* 0.8 + 0.1;
				}
                fixed4 col = tex2D(_MainTex, i.uv);

                return col;
            }
            ENDCG
        }
    }
}
