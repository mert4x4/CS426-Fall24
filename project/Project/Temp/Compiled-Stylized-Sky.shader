// Compiled shader for Windows, Mac, Linux

//////////////////////////////////////////////////////////////////////////
// 
// NOTE: This is *not* a valid shader file, the contents are provided just
// for information and for debugging purposes only.
// 
//////////////////////////////////////////////////////////////////////////
// Skipping shader variants that would not be included into build of current scene.

Shader "Stylized/Sky" {
Properties {
[Header(Sun Disc)]  _SunDiscColor ("Color", Color) = (1.000000,1.000000,1.000000,1.000000)
 _SunDiscMultiplier ("Multiplier", Float) = 25.000000
 _SunDiscExponent ("Exponent", Float) = 125000.000000
[Header(Sun Halo)]  _SunHaloColor ("Color", Color) = (0.897059,0.776056,0.666198,1.000000)
 _SunHaloExponent ("Exponent", Float) = 125.000000
 _SunHaloContribution ("Contribution", Range(0.000000,1.000000)) = 0.750000
[Header(Horizon Line)]  _HorizonLineColor ("Color", Color) = (0.904412,0.887259,0.791360,1.000000)
 _HorizonLineExponent ("Exponent", Float) = 4.000000
 _HorizonLineContribution ("Contribution", Range(0.000000,1.000000)) = 0.250000
[Header(Sky Gradient)]  _SkyGradientTop ("Top", Color) = (0.172549,0.568627,0.694118,1.000000)
 _SkyGradientBottom ("Bottom", Color) = (0.764706,0.815686,0.850981,1.000000)
 _SkyGradientExponent ("Exponent", Float) = 2.500000
}
SubShader { 
 LOD 100
 Tags { "QUEUE"="Background" "RenderType"="Background" }
 Pass {
  Tags { "QUEUE"="Background" "RenderType"="Background" }
  //////////////////////////////////
  //                              //
  //      Compiled programs       //
  //                              //
  //////////////////////////////////
//////////////////////////////////////////////////////
Keywords: <none>
-- Hardware tier variant: Tier 1
-- Vertex shader for "metal":
Uses vertex data channel "Vertex"
Uses vertex data channel "TexCoord0"

Constant Buffer "VGlobals" (128 bytes) on slot 0 {
  Matrix4x4 unity_ObjectToWorld at 0
  Matrix4x4 unity_MatrixVP at 64
}

Shader Disassembly:
#include <metal_stdlib>
#include <metal_texture>
using namespace metal;
struct VGlobals_Type
{
    float4 hlslcc_mtx4x4unity_ObjectToWorld[4];
    float4 hlslcc_mtx4x4unity_MatrixVP[4];
};

struct Mtl_VertexIn
{
    float4 POSITION0 [[ attribute(0) ]] ;
    float2 TEXCOORD0 [[ attribute(1) ]] ;
};

struct Mtl_VertexOut
{
    float4 mtl_Position [[ position, invariant ]];
    float2 TEXCOORD0 [[ user(TEXCOORD0) ]];
    float3 TEXCOORD1 [[ user(TEXCOORD1) ]];
};

vertex Mtl_VertexOut xlatMtlMain(
    constant VGlobals_Type& VGlobals [[ buffer(0) ]],
    Mtl_VertexIn input [[ stage_in ]])
{
    Mtl_VertexOut output;
    float4 u_xlat0;
    float4 u_xlat1;
    u_xlat0 = input.POSITION0.yyyy * VGlobals.hlslcc_mtx4x4unity_ObjectToWorld[1];
    u_xlat0 = fma(VGlobals.hlslcc_mtx4x4unity_ObjectToWorld[0], input.POSITION0.xxxx, u_xlat0);
    u_xlat0 = fma(VGlobals.hlslcc_mtx4x4unity_ObjectToWorld[2], input.POSITION0.zzzz, u_xlat0);
    u_xlat1 = u_xlat0 + VGlobals.hlslcc_mtx4x4unity_ObjectToWorld[3];
    output.TEXCOORD1.xyz = fma(VGlobals.hlslcc_mtx4x4unity_ObjectToWorld[3].xyz, input.POSITION0.www, u_xlat0.xyz);
    u_xlat0 = u_xlat1.yyyy * VGlobals.hlslcc_mtx4x4unity_MatrixVP[1];
    u_xlat0 = fma(VGlobals.hlslcc_mtx4x4unity_MatrixVP[0], u_xlat1.xxxx, u_xlat0);
    u_xlat0 = fma(VGlobals.hlslcc_mtx4x4unity_MatrixVP[2], u_xlat1.zzzz, u_xlat0);
    output.mtl_Position = fma(VGlobals.hlslcc_mtx4x4unity_MatrixVP[3], u_xlat1.wwww, u_xlat0);
    output.TEXCOORD0.xy = input.TEXCOORD0.xy;
    return output;
}


-- Hardware tier variant: Tier 1
-- Fragment shader for "metal":
Constant Buffer "FGlobals" (148 bytes) on slot 0 {
  Vector4 _WorldSpaceLightPos0 at 0
  Vector3 _SunDiscColor at 16
  Float _SunDiscExponent at 32
  Float _SunDiscMultiplier at 36
  Vector3 _SunHaloColor at 48
  Float _SunHaloExponent at 64
  Float _SunHaloContribution at 68
  Vector3 _HorizonLineColor at 80
  Float _HorizonLineExponent at 96
  Float _HorizonLineContribution at 100
  Vector3 _SkyGradientTop at 112
  Vector3 _SkyGradientBottom at 128
  Float _SkyGradientExponent at 144
}

Shader Disassembly:
#include <metal_stdlib>
#include <metal_texture>
using namespace metal;
#ifndef XLT_REMAP_O
	#define XLT_REMAP_O {0, 1, 2, 3, 4, 5, 6, 7}
#endif
constexpr constant uint xlt_remap_o[] = XLT_REMAP_O;
struct FGlobals_Type
{
    float4 _WorldSpaceLightPos0;
    float3 _SunDiscColor;
    float _SunDiscExponent;
    float _SunDiscMultiplier;
    float3 _SunHaloColor;
    float _SunHaloExponent;
    float _SunHaloContribution;
    float3 _HorizonLineColor;
    float _HorizonLineExponent;
    float _HorizonLineContribution;
    float3 _SkyGradientTop;
    float3 _SkyGradientBottom;
    float _SkyGradientExponent;
};

struct Mtl_FragmentIn
{
    float3 TEXCOORD1 [[ user(TEXCOORD1) ]] ;
};

struct Mtl_FragmentOut
{
    float4 SV_Target0 [[ color(xlt_remap_o[0]) ]];
};

fragment Mtl_FragmentOut xlatMtlMain(
    constant FGlobals_Type& FGlobals [[ buffer(0) ]],
    Mtl_FragmentIn input [[ stage_in ]])
{
    Mtl_FragmentOut output;
    float3 u_xlat0;
    float3 u_xlat1;
    float3 u_xlat2;
    float3 u_xlat3;
    float u_xlat6;
    float u_xlat9;
    u_xlat0.x = dot(input.TEXCOORD1.xyz, input.TEXCOORD1.xyz);
    u_xlat0.x = rsqrt(u_xlat0.x);
    u_xlat0.xyz = u_xlat0.xxx * input.TEXCOORD1.xyz;
    u_xlat9 = min(abs(u_xlat0.y), 1.0);
    u_xlat9 = u_xlat9 * FGlobals._SunHaloExponent;
    u_xlat0.x = dot(u_xlat0.xyz, FGlobals._WorldSpaceLightPos0.xyz);
    u_xlat0.x = clamp(u_xlat0.x, 0.0f, 1.0f);
    u_xlat0.x = log2(u_xlat0.x);
    u_xlat6 = u_xlat0.x * u_xlat9;
    u_xlat0.x = u_xlat0.x * FGlobals._SunDiscExponent;
    u_xlat0.x = exp2(u_xlat0.x);
    u_xlat0.x = u_xlat0.x * FGlobals._SunDiscMultiplier;
    u_xlat0.x = clamp(u_xlat0.x, 0.0f, 1.0f);
    u_xlat6 = exp2(u_xlat6);
    u_xlat9 = u_xlat0.y;
    u_xlat9 = clamp(u_xlat9, 0.0f, 1.0f);
    u_xlat3.x = -abs(u_xlat0.y) + 1.0;
    u_xlat3.x = log2(u_xlat3.x);
    u_xlat3.x = u_xlat3.x * FGlobals._HorizonLineExponent;
    u_xlat3.x = exp2(u_xlat3.x);
    u_xlat3.x = min(u_xlat3.x, 1.0);
    u_xlat1.xyz = u_xlat3.xxx * FGlobals._HorizonLineColor.xyzx.xyz;
    u_xlat1.xyz = u_xlat1.xyz * float3(FGlobals._HorizonLineContribution);
    u_xlat3.x = (-u_xlat9) + 1.0;
    u_xlat3.x = log2(u_xlat3.x);
    u_xlat9 = u_xlat3.x * 50.0;
    u_xlat3.x = u_xlat3.x * FGlobals._SkyGradientExponent;
    u_xlat3.x = exp2(u_xlat3.x);
    u_xlat9 = exp2(u_xlat9);
    u_xlat9 = (-u_xlat9) + 1.0;
    u_xlat6 = u_xlat9 * u_xlat6;
    u_xlat6 = min(u_xlat6, 1.0);
    u_xlat2.xyz = FGlobals._SunHaloColor.xxyz.yzw * float3(FGlobals._SunHaloContribution);
    u_xlat1.xyz = fma(u_xlat2.xyz, float3(u_xlat6), u_xlat1.xyz);
    u_xlat2.xyz = (-FGlobals._SkyGradientTop.xxyz.yzw) + FGlobals._SkyGradientBottom.xyzx.xyz;
    u_xlat3.xyz = fma(u_xlat3.xxx, u_xlat2.xyz, FGlobals._SkyGradientTop.xxyz.yzw);
    u_xlat3.xyz = u_xlat3.xyz + u_xlat1.xyz;
    u_xlat3.xyz = clamp(u_xlat3.xyz, 0.0f, 1.0f);
    u_xlat1.xyz = (-u_xlat3.xyz) + FGlobals._SunDiscColor.xyzx.xyz;
    output.SV_Target0.xyz = fma(u_xlat0.xxx, u_xlat1.xyz, u_xlat3.xyz);
    output.SV_Target0.w = 1.0;
    return output;
}


 }
}
}