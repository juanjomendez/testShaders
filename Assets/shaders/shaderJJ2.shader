Shader "Custom/shaderJJ2" {
    Properties{
      _MainTex("Texture", 2D) = "white" {}
      _JitterHalfResolution("Jitter", Float) = 10.0

    }
        SubShader{
          Tags { "RenderType" = "Opaque" }
          CGPROGRAM
          #pragma surface surf Lambert addshadow fullforwardshadows vertex:vert

          struct Input {
              float2 uv_MainTex;
          };

      float _JitterHalfResolution;
      sampler2D _MainTex;

      float rand(float3 co)
      {
          return frac(sin(_Time * dot(co.xyz, float3(12.9898, 78.233, 45.5432))) * 43758.5453) / 50;
      }

      void vert(inout appdata_full v) {
          float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
          float4 viewPos = mul(unity_MatrixV, worldPos);
          float4 clipPos = mul(unity_CameraProjection, viewPos);
          clipPos.xy /= clipPos.w;
          clipPos.xy = floor(clipPos.xy * _JitterHalfResolution + 0.5) / _JitterHalfResolution;
          clipPos.xy *= clipPos.w;
          viewPos = mul(unity_CameraInvProjection, clipPos);
          worldPos = mul(unity_MatrixInvV, clipPos);
          //v.vertex = mul(unity_WorldToObject, worldPos);

          float a = _Time % 1;
          float b = _Time % 1;
          float c = _Time % 1;
          v.vertex += rand(float3(a,b,c));
      }

      void surf(Input IN, inout SurfaceOutput o) 
      {
          //o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
          //o.Albedo = fixed4(0, 1, 0, 1);
      }
      ENDCG
      }
          FallBack "Diffuse"
}