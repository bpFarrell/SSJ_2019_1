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
			float GetPass(float2 offset,float2 texel, int index) {
				fixed4 col = tex2D(_MainTex, offset);
				fixed4 cmyk = rgb2cmyk(col.rgb);
				float2 subUV = fmod(offset, texel * _Offset2.w) / (texel * _Offset2.w);
				float v = 1 - distance(fixed2(0.5, 0.5), subUV) - cmyk[index];
				return smoothstep(0.2, 0.5, v);
			}
			fixed4 frag(v2f i) : SV_Target
			{
				float2 texel = 1 / _ScreenParams.xy;
				float2 subUV = fmod(i.uv, texel* _Offset2.w)/ (texel * _Offset2.w);
                fixed4 col1 = tex2D(_MainTex, i.uv);
				fixed4 cmyk1 = rgb2cmyk(col1.rgb);
				float4 dot = distance(fixed2(0.5, 0.5), subUV)-cmyk1.w;
				dot = smoothstep( 0.2, 0.5, dot);


				//return fixed4(subUV.xyyy);
				fixed4 col2 = tex2D(_MainTex, i.uv + texel * _Offset1.xy * _Offset2.z);
				fixed4 cmyk2 = rgb2cmyk(col2.rgb);
				subUV = fmod(i.uv + texel * _Offset1.xy * _Offset2.z, texel * _Offset2.w) / (texel * _Offset2.w);
				dot.x = 1-distance(fixed2(0.5, 0.5), subUV) - cmyk2.x;
				dot = smoothstep(0.2, 0.5, dot.x);
				//dot = GetPass(i.uv + texel * _Offset1.xy * _Offset2.z, texel, 0);
				//return dot;

				
				fixed4 col3 = tex2D(_MainTex, i.uv + texel * _Offset1.zw * _Offset2.z);
				fixed4 cmyk3 = rgb2cmyk(col3.rgb);
				subUV = fmod(i.uv + texel * _Offset1.zw * _Offset2.z, texel * _Offset2.w) / (texel * _Offset2.w);
				dot.y = 1-distance(fixed2(0.5, 0.5), subUV) - cmyk3.y;
				dot.y = smoothstep(0.2, 0.5, dot.y);

				fixed4 col4 = tex2D(_MainTex, i.uv + texel * _Offset2.xy* _Offset2.z);
				fixed4 cmyk4 = rgb2cmyk(col4.rgb);
				subUV = fmod(i.uv + texel * _Offset2.xy * _Offset2.z, texel * _Offset2.w) / (texel * _Offset2.w);
				dot.z = 1-distance(fixed2(0.5, 0.5), subUV) - cmyk4.z;
				dot.z = smoothstep(0.2, 0.5, dot.z);


				fixed4 rgb = cmyk2rgb(dot).xyzz;
                return dot;
            }
            ENDCG
        }
    }
}
