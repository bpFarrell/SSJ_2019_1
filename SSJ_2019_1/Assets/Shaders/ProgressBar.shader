Shader "Unlit/ProgressBar"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Meta("x:percent y:t z:widthRatio w:rewindT", Vector) = (0,0,0,0)
		_BGColor("BGColor",Color) = (0,0,0,0)
		_JasonColor("JasonColor",Color) = (0,0,0,0)
		_Color1("Color1",Color) = (1,1,1,0)
		_Color2("Color2",Color) = (1,1,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _Meta;
			fixed4 _BGColor;
			fixed4 _Color1;
			fixed4 _Color2;
			fixed4 _JasonColor;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

			fixed4 frag(v2f i) : SV_Target
			{
				float2 uv = i.uv * 2 - 1;
				//uv.x += abs(uv.y);
				float scaled = uv.x * _Meta.z * 0.5;
				float sub = scaled + abs(uv.y);
				sub = fmod(sub +500-_Meta.x, 1); 

				float prog = smoothstep(_Meta.y - 0.01, _Meta.y + 0.01, i.uv.x);
				float bar = smoothstep(0.003, 0.0045,abs(i.uv.x - _Meta.y));
				float curProg = smoothstep(_Meta.w - 0.0025, _Meta.w + 0.0025, i.uv.x);
				//return fixed4(1, 1, 1, 1)* bar;
				fixed4 clr = lerp(_Color1, _Color2, sub);
				float4 Desat = clr / 2;
				clr = lerp(clr, Desat, curProg);
				clr = lerp(clr, _BGColor, prog);
				clr = lerp(clr, _JasonColor+1-(bar+0.5), 1-bar);
				return clr;
            }
            ENDCG
        }
    }
}
