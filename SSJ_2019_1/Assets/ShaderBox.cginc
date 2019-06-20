
float3 rgb2hsv(float3 c)
{
	float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
	float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
	float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

	float d = q.x - min(q.w, q.y);
	float e = 1.0e-10;
	return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}
float3 hsv2rgb(float3 c)
{
	float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
	float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
	return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}
float hash(float3 p)
{
	p = frac(p*0.3183099 + .1);
	p *= 17.0;
	return frac(p.x*p.y*p.z*(p.x + p.y + p.z));
}
float noise(in float3 x)
{
	float3 p = floor(x);
	float3 f = frac(x);
	f = f*f*(3.0 - 2.0*f);

	return lerp(lerp(lerp(hash(p + float3(0, 0, 0)),
		hash(p + float3(1, 0, 0)), f.x),
		lerp(hash(p + float3(0, 1, 0)),
			hash(p + float3(1, 1, 0)), f.x), f.y),
		lerp(lerp(hash(p + float3(0, 0, 1)),
			hash(p + float3(1, 0, 1)), f.x),
			lerp(hash(p + float3(0, 1, 1)),
				hash(p + float3(1, 1, 1)), f.x), f.y), f.z);
}
float4 noise2(float3 x)
{
	// grid
	float3 p = floor(x);
	float3 w = frac(x);
	// quintic interpolant
	float3 u = w*w*w*(w*(w*6.0 - 15.0) + 10.0);
	float3 du = 30.0*w*w*(w*(w - 2.0) + 1.0);

	// gradients
	float3 ga = hash(p + float3(0.0, 0.0, 0.0));
	float3 gb = hash(p + float3(1.0, 0.0, 0.0));
	float3 gc = hash(p + float3(0.0, 1.0, 0.0));
	float3 gd = hash(p + float3(1.0, 1.0, 0.0));
	float3 ge = hash(p + float3(0.0, 0.0, 1.0));
	float3 gf = hash(p + float3(1.0, 0.0, 1.0));
	float3 gg = hash(p + float3(0.0, 1.0, 1.0));
	float3 gh = hash(p + float3(1.0, 1.0, 1.0));

	// projections
	float va = dot(ga, w - float3(0.0, 0.0, 0.0));
	float vb = dot(gb, w - float3(1.0, 0.0, 0.0));
	float vc = dot(gc, w - float3(0.0, 1.0, 0.0));
	float vd = dot(gd, w - float3(1.0, 1.0, 0.0));
	float ve = dot(ge, w - float3(0.0, 0.0, 1.0));
	float vf = dot(gf, w - float3(1.0, 0.0, 1.0));
	float vg = dot(gg, w - float3(0.0, 1.0, 1.0));
	float vh = dot(gh, w - float3(1.0, 1.0, 1.0));

	// interpolations
	return float4(va + u.x*(vb - va) + u.y*(vc - va) + u.z*(ve - va) + u.x*u.y*(va - vb - vc + vd) + u.y*u.z*(va - vc - ve + vg) + u.z*u.x*(va - vb - ve + vf) + (-va + vb + vc - vd + ve - vf - vg + vh)*u.x*u.y*u.z,    // value
		ga + u.x*(gb - ga) + u.y*(gc - ga) + u.z*(ge - ga) + u.x*u.y*(ga - gb - gc + gd) + u.y*u.z*(ga - gc - ge + gg) + u.z*u.x*(ga - gb - ge + gf) + (-ga + gb + gc - gd + ge - gf - gg + gh)*u.x*u.y*u.z +   // derivatives
		du * (float3(vb, vc, ve) - va + u.yzx*float3(va - vb - vc + vd, va - vc - ve + vg, va - vb - ve + vf) + u.zxy*float3(va - vb - ve + vf, va - vb - vc + vd, va - vc - ve + vg) + u.yzx*u.zxy*(-va + vb + vc - vd + ve - vf - vg + vh)));
}
fixed4 texSpace(sampler2D text, float3 pos, float3 noraml) {

	fixed4 colX = tex2D(text, pos.yz);
	fixed4 colY = tex2D(text, pos.xz);
	fixed4 colZ = tex2D(text, pos.xy);
	float dotX = abs(dot(noraml, fixed3(1, 0, 0)));
	float dotY = abs(dot(noraml, fixed3(0, 1, 0)));
	float dotZ = abs(dot(noraml, fixed3(0, 0, 1)));
	float dSum = dotX + dotY + dotZ;
	dotX /= dSum;
	dotY /= dSum;
	dotZ /= dSum;
	return colX*dotX + colY*dotY + colZ*dotZ;
}
/*
const float effect_DepthSensitivity = 0.04;
const float effect_NormalSensitivity = .94;
float effect_GetGBufferDifference(fixed4 a, fixed4 b) {
	float deltaDepth = abs(DecodeFloatRG(a.zw) - DecodeFloatRG(b.zw)) > effect_DepthSensitivity;
	float2 diffNormal = abs(a.xy - b.xy)* effect_NormalSensitivity;
	float deltaNormal = (diffNormal.x + diffNormal.y)*effect_NormalSensitivity>0.1;
	return deltaDepth + deltaNormal;
}
float effect_GetEdge(sampler2D gBuffer, float2 uv) {
	float2 texel = 1 / _ScreenParams.xy;
	fixed4 g0 = tex2D(gBuffer, uv + texel*float2(0, 0));
	fixed4 g1 = tex2D(gBuffer, uv + texel*float2(0, 1));
	fixed4 g2 = tex2D(gBuffer, uv + texel*float2(0, -1));
	fixed4 g3 = tex2D(gBuffer, uv + texel*float2(-1, 0));
	fixed4 g4 = tex2D(gBuffer, uv + texel*float2(1, 0));
	float spread = effect_GetGBufferDifference(g0, g1);
	spread += effect_GetGBufferDifference(g0, g2);
	spread += effect_GetGBufferDifference(g0, g3);
	spread += effect_GetGBufferDifference(g0, g4);
	return spread;
}*/