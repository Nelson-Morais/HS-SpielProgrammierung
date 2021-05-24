Shader "Custom/DissolveShader"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_NoiseTex("Noise Texture", 2D) = "white" {}
		_VanishingThreshold("Vanishing", Range(0,1)) = 0.5
		_LightBandColor("Light Band Color", Color) = (1,1,1,1)
		_LightBandThreshold("Light Band Threshold", Range(0,1)) = 0.5
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}
		SubShader
		{
			Tags { "RenderType" = "Transparent" "IgnoreProjector" = "True" "Queue" = "Transparent" }
			LOD 200

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard fullforwardshadows alpha:fade

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			sampler2D _MainTex;
			sampler2D _NoiseTex;

			struct Input
			{
				float2 uv_MainTex;
				float2 uv_NoiseTex;
			};

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;

			fixed4 _LightBandColor;
			half _LightBandThreshold;
			
			half _VanishingThreshold;

			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			UNITY_INSTANCING_BUFFER_START(Props)
				// put more per-instance properties here
			UNITY_INSTANCING_BUFFER_END(Props)

			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				// Metallic and smoothness come from slider variables
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				half noiseValue = tex2D(_NoiseTex, IN.uv_NoiseTex);
				if (noiseValue > _VanishingThreshold) {
					o.Alpha = c.a;
					if (noiseValue < _VanishingThreshold + _LightBandThreshold) {
						o.Emission = _LightBandColor + fixed4(1,1,1,1);
					}
				}
				else {
					o.Alpha = 0;
				}
			}
			ENDCG
		}
			FallBack "Diffuse"
}
