Shader "Roystan/Toon/Water"
{
	Properties
	{
		_DepthGradientShallow("Depth Gradient Shallow", Color) = (0.325, 0.807, 0.971, 0.725)
		_DepthGradientDeep("Depth Gradient Deep", Color) = (0.086, 0.407, 1, 0.749)
		_DepthMaxDistance("Depth Maximum Distance", Float) = 1

		_SurfaceNoise("Surface Noise", 2D) = "white" {}
		_SurfaceNoiseCutoff("Surface Noise Cutoff", Range(0,1)) = 0.77
		_FoamDistance("Foam Distance", Float) = 0.4
		_SurfaceNoiseScroll("Surface Noise Scroll Amount", Vector) = (0.03, 0.03, 0, 0)

		_SurfaceDistortion("Surface Distortion", 2D) = "white" {}

		_SurfaceDistortionAmount("Surface Distortion AMount", Range(0,1)) = 0.27

		_FoamColor("Foam Color", Color) = (1,1,1,1)

	}
		SubShader
		{
			Pass
			{
				Blend SrcAlpha OneMinusSrcAlpha

				CGPROGRAM
				#define SMOOTHSTEP_AA 0.01
				#pragma vertex vert
				#pragma fragment frag



				#include "UnityCG.cginc"

				float4 alphaBlend(float4 top, float4 bottom)
				{
					float3 color = (top.rgb * top.a) + (bottom.rgb * (1 - top.a));
					float alpha = top.a + bottom.a * (1 - top.a);

					return float4(color, alpha);
				}


				struct appdata
				{
					float4 vertex : POSITION;
					float4 uv : TEXCOORD0;
				};

				sampler2D _SurfaceDistortion;
				float4 _SurfaceDistortion_ST;

				float _SurfaceDistortionAmount;

				struct v2f
				{
					float4 vertex : SV_POSITION;
					float4 screenPosition : TEXCOORD2;
					float2 noiseUV: TEXCOORD0;
					float2 distortUV : TEXCOORD1;

				};

				sampler2D _SurfaceNoise;
				float4 _SurfaceNoise_ST;


				v2f vert(appdata v)
				{
					v2f o;

					o.vertex = UnityObjectToClipPos(v.vertex);
					o.screenPosition = ComputeScreenPos(o.vertex);
					o.noiseUV = TRANSFORM_TEX(v.uv, _SurfaceNoise);
					o.distortUV = TRANSFORM_TEX(v.uv, _SurfaceDistortion);

					return o;
				}

				float4 _DepthGradientShallow;
				float4 _DepthGradientDeep;
				float _DepthMaxDistance;
				float _SurfaceNoiseCutoff;
				float _FoamDistance;
				float2 _SurfaceNoiseScroll;
				float4 _FoamColor;


				sampler2D _CameraDepthTexture;

				float4 frag(v2f i) : SV_Target
				{
					float existingDepth01 = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPosition)).r;
					float existingDepthLinear = LinearEyeDepth(existingDepth01);
					float depthDifference = existingDepthLinear - i.screenPosition.w;

					float waterDepthDifference01 = saturate(depthDifference / _DepthMaxDistance);
					float4 waterColor = lerp(_DepthGradientShallow, _DepthGradientDeep, waterDepthDifference01);

					float foamDepthDifference01 = saturate(depthDifference / _FoamDistance);
					float surfaceNoiseCutoff = foamDepthDifference01 * _SurfaceNoiseCutoff;

					float2 distortSample = (tex2D(_SurfaceDistortion, i.distortUV).xy * 2 - 1) * _SurfaceDistortionAmount;

					float2 noiseUV = float2((i.noiseUV.x + _Time.y * _SurfaceNoiseScroll.x) + distortSample.x, i.noiseUV.y + _Time.y * _SurfaceNoiseScroll.y) + distortSample.y;
					float surfaceNoiseSample = tex2D(_SurfaceNoise, noiseUV).r;

					float surfaceNoise = smoothstep(surfaceNoiseCutoff - SMOOTHSTEP_AA, surfaceNoiseCutoff + SMOOTHSTEP_AA, surfaceNoiseSample);

					float4 surfaceNoiseColor = _FoamColor;
					surfaceNoiseColor.a *= surfaceNoise;

					return alphaBlend(surfaceNoiseColor, waterColor);

					//returns a still cartoony water
					return waterColor + surfaceNoiseColor;

					//returns a very noisey water
					return waterColor + surfaceNoiseSample;

					//returns a noiseless water
					return waterColor;

					//returns a cloudy texture
					return depthDifference;

					//returns white
					return float4(1, 1, 1, 0.5);
				}
				ENDCG
			}
		}
}