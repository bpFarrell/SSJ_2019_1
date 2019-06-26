float _T;
inline float4 CthulMain(float4 vert) {
	vert.z += sin(vert + _T*0.2);
	return vert;
}