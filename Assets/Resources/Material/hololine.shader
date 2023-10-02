Shader "Custom/hololine"
{
    Properties
    {
        _BumpMap("NormalMap", 2D) = "bump" {}
        _Color("Color", Color) = (1,1,1,1)
    }
        SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}

        CGPROGRAM

        #pragma surface surf Lambert noambient alpha:fade

        sampler2D _BumpMap;
        fixed4 _Color;
        
        struct Input
        {
            float2 uv_BumpMap;
            float3 viewDir;
            float3 worldPos;    //월드 포지션
        };


        void surf(Input IN, inout SurfaceOutput o)
        {
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            o.Emission = _Color;                                               //원하는 홀로그램 색상 넣기
            float rim = saturate(dot(o.Normal, IN.viewDir));
            rim = saturate(pow(1 - rim, 3)+pow(frac(IN.worldPos.g*3-_Time.y),5)*0.1);   //라인 움직이기
            o.Alpha = rim;
        }

        float4 Lightingnolight(SurfaceOutput s, float3 lightDir, float atten)
        {
            return float4(0, 0, 0, s.Alpha);
        }
        ENDCG
    }
    FallBack "Transparent/Diffuse"
}
