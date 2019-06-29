Shader "Unlit/BG"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color("Color",color) = (1,1,1,1)
		_Sun("Sun",Vector) = (0,0,0,0)
		_Grad("Grad",Vector)=(0,0,0,0)
		_Pattern("Patten",Vector)=(10,0,0,0)
		_Fade("Fade",Float) = 1
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
		#include "Assets/ShaderBox.cginc"
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
			float _T;
			float4 _Sun;
			float4 _Grad;
			float4 _Pattern;
			float _Fade;
			fixed4 _Color;
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
				if (-i.uv.x < -_Fade)
					discard;
				fixed3 col2 = rgb2hsv(_Color);
				col2.x -= 0.1;
				col2.yz *= 0.7;
				col2 = hsv2rgb(col2);
				float2 uv = i.uv * 2 - 1;

				float2x2 rotMatPattern = float2x2(
					cos(_Pattern.x), -sin(_Pattern.x), 
					sin(_Pattern.x), cos(_Pattern.x));
				fixed3 pattern = tex2D(_MainTex, mul(rotMatPattern,uv*_Pattern.y)).rgb;
				//return fixed4(1,1,1,1)*smoothstep(_Pattern.w, _Pattern.w, pattern[floor(_Pattern.z)]);
				//return pattern.xyzz;

				float rot = _Grad.x + _T*0.1;
				float2x2 rotMat = float2x2(cos(rot), -sin(rot),sin(rot), cos(rot) );
				float2 uvRot = mul(rotMat, uv);

				float finalGrad = smoothstep(-_Grad.y, _Grad.y, (uvRot.y+_Grad.z+pattern[_Pattern.z])*0.5);
				//return lerp(_Color, col2.rgbb,finalGrad);



				uv += _Sun.xy;
				float r = length(uv);
				float a = atan2(uv.x,uv.y)/3.141529/2+0.5;
				float ray = abs(fmod((a * floor(_Sun.z)*2)+_T*0.5, 2)-1);
				ray = saturate(max(ray,1-smoothstep(_Sun.w, _Sun.w+.2,r)));
				float beam = smoothstep(0.6, 0.7, ray);
				fixed4 col = lerp(_Color, col2.rgbb, abs(finalGrad-beam));
                return col;
            }
            ENDCG
        }
    }
}
