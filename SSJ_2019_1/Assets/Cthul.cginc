float _T;
inline float4 CthulMain(float4 vert) {
	return vert;
}
inline float4 CthulTent(float4 vert, float2 uv) {
	vert.z += sin(vert.y*1+_Time.x*100) * (1-uv.y)*0.5;

	return vert;
}