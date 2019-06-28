float _T;
inline float4 CthulMain(float4 vert) {
	return vert;
}
inline float4 CthulTent(float4 vert, float2 uv,float2 uv2) {
	vert.z += sin(vert.y * 1 + _T * (uv2.y * 0.5 + 0.5) + uv2.y * 10) * (1 - uv.y) * 0.5;

	vert.x += -cos(vert.y * 1 + _T * (uv2.y * 0.5 + 0.5) + uv2.y * 10) * (1 - uv.y) * 0.5;

	return vert;
}