Shader "Custom/OceanHorizon" 
	{
        Properties
        {
            _MainTex ("Texture", 2D) = "white" {}
            _FogColor ("fog color", Color) = (1,1,1,1)
            _FogBlend ("fog blend", Range (0.01,2.0)) = .1
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
                    float3 worldPos : TEXCOORD1;
                };
    
                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _Color;
                float4 _FogColor;
                
                float _FogBlend;
                
                v2f vert (appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    UNITY_TRANSFER_FOG(o,o.vertex);
                    o.worldPos = mul (unity_ObjectToWorld, v.vertex);
                    return o;
                }
                
                
                fixed4 frag (v2f i) : SV_Target
                {
                    float3 cdir = i.worldPos;
                    float3 cpos = _WorldSpaceCameraPos;	
                    cdir -= cpos;
                     
                    float3 oceanUpDir = float3(0.0, 1.0, 0.0);
                    float dist = -dot(cpos, oceanUpDir) / dot(cdir, oceanUpDir);
                    float3 pos = cpos + dist * cdir;
         
                    float3 pix;
                    if(dist > 0.0 && dist < 100.0) {
                     
                     // add you Ocean here use pos.xz instead of uv
                        float2 posn = float2(pos.x, pos.z);
                        posn.x += _MainTex_ST.z;
                        posn.y += _MainTex_ST.w;
                        posn.x *= _MainTex_ST.x/10.0;
                        posn.y *= _MainTex_ST.y/10.0;
                        float3 wat = tex2D(_MainTex, posn);
                        
                        pix = lerp(wat, _FogColor, min(dist * _FogBlend, 1.0));
                    } else {
                        // Sky
                        discard;
                        //pix =  lerp(float3(1,1,1), _Color, min(cdir.y * 4.0, 1.0));
                    }
                
                    return float4(pix ,1 );
                }
                ENDCG
            }
        }
    }
