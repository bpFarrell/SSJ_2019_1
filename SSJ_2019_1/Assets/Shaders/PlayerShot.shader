﻿Shader "Unlit/PlayerShot"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Col1("col1",Color) = (0,0,0,0)
		_Col2("col2",Color) = (0,0,0,0)
    }
    SubShader
    {
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
        LOD 100
		ZWrite Off
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
				float4 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			fixed4 _Col1;
			fixed4 _Col2;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

			fixed4 frag(v2f i) : SV_Target
			{
				float2 uv = i.uv * 2 - 1;
				float bar = 1 - abs(uv.y*2.1);
				bar *= smoothstep(0.1,0.2,1 - abs(uv.x-0.1));
				//fixed4 n = tex2D(_MainTex, (i.worldPos + fixed2(0, sin(i.worldPos.x))) * 0.25 + pow(uv, 2));
				fixed4 n = tex2D(_MainTex, (i.worldPos + fixed2(pow(i.uv.x, 8), 0))*0.2);
				n += tex2D(_MainTex, (i.worldPos + fixed2(1-i.uv.x, 0))*0.5);

				if (bar+n.g*0.4 < .2+smoothstep(0.1,-0.05,i.uv.x)+ smoothstep(0.9, 1, i.uv.x))
					discard;
				float black = smoothstep(0, .1, abs(bar-0.4));
                return lerp(_Col1,_Col2,bar+(n-0.5)*0.25)*black;
            }
            ENDCG
        }
    }
}
