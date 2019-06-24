Shader "Unlit/Projectile"
{
    Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Mask("Texture", 2D) = "white" {}
		_Col1("col1",Color) = (0,0,0,0)
		_Col2("col2",Color) = (0,0,0,0)
		_Blend("blend",Float) = 0
		_Kill("kill",Float) = 0
    }
    SubShader
    {
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
        LOD 100
		//Blend One One // Additive
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
			sampler2D _Mask;
            float4 _MainTex_ST;
			fixed4 _Col1;
			fixed4 _Col2;
			float _Blend;
			float _Kill;
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
                fixed4 col = tex2D(_MainTex, i.worldPos.xy*0.15+uv*0.1)-0.5;
				col+= (tex2D(_MainTex, i.worldPos.xy*0.5+uv*0.1) - 0.5)*0.5;
				float r = smoothstep(0.3,.6,1 - length(uv + fixed2(0, -0.3)));

				//r += max(smoothstep(0.3, .6, 1 - length(uv+fixed2(0,0.3))),0)*0.3;
				//r = saturate(r);
				//return (1, 1, 1, 1)* r;
				fixed4 m = tex2D(_Mask, i.uv);
				float t = lerp(m.b+m.r,-col,_Blend);

				if (t < .85+_Kill)
					discard;
				float black = smoothstep(1.1+_Kill, 1.2+_Kill, t);
				t -= 0.7;

				//return (1, 1, 1, 1)* t;
				col = lerp(_Col1,_Col2,t);

				return (col + pow(m.b,8))*black+r*0.2;
            }
            ENDCG
        }
    }
}
