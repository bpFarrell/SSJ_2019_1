Shader "Hidden/OffRegister"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Offset1("Offset1",Vector) = (0,0,0,0)
		_Offset2("Offset2",Vector) = (0,0,0,0)
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
			float4 _Offset1;
			float4 _Offset2;
			fixed3 cmyk2rgb(fixed4 cmyk) {
				cmyk.xyz = (cmyk.xyz * (1 - cmyk.w) + cmyk.w);
				return  1 - cmyk.xyz;
				
			}
			fixed4 rgb2cmyk(fixed3 rgb) {

				fixed4 cmyk;
				cmyk.xyz = 1 - rgb.xyz;
				cmyk.w = 1;
				cmyk.w = min(cmyk.x, min(cmyk.y, min(cmyk.z, cmyk.w)));
				cmyk.xyz = cmyk.w == 1 ? float3(0, 0, 0) : (cmyk.xyz - cmyk.w) / (1 - cmyk.w);
				return cmyk;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				//return fixed4(1,0,0,1);
                fixed4 col1 = tex2D(_MainTex, i.uv);
				fixed4 cmyk1 = rgb2cmyk(col1.rgb);

				fixed4 col2 = tex2D(_MainTex, i.uv + _Offset1.xy * _Offset2.z);
				fixed4 cmyk2 = rgb2cmyk(col2.rgb);
				fixed4 col3 = tex2D(_MainTex, i.uv + _Offset1.zw * _Offset2.z);
				fixed4 cmyk3 = rgb2cmyk(col3.rgb);
				fixed4 col4 = tex2D(_MainTex, i.uv + _Offset2.xy* _Offset2.z);
				fixed4 cmyk4 = rgb2cmyk(col4.rgb);


				fixed4 rgb = cmyk2rgb(
					fixed4(
					cmyk2.x,
					cmyk3.y,
					cmyk4.z,
					cmyk1.w)).xyzz;
                return rgb;
            }
            ENDCG
        }
    }
}
