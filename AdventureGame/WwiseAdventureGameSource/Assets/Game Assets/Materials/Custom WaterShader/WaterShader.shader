//NOTE: This Shader was created with Shader Forge. On some rainy day, this shader is optimized to run faster, and provide less 'shaderforge-y' shader code!

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:False,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:False,rfrpn:Refraction,coma:15,ufog:True,aust:False,igpj:True,qofs:0,qpre:3,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.09690036,fgcg:0.1050727,fgcb:0.1097426,fgca:1,fgde:0.0001,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:True,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:2865,x:33694,y:32150,varname:node_2865,prsc:2|diff-7865-OUT,emission-2552-OUT,voffset-9320-OUT;n:type:ShaderForge.SFN_Multiply,id:6343,x:32787,y:31302,varname:node_6343,prsc:2|A-4822-OUT,B-6884-OUT;n:type:ShaderForge.SFN_DepthBlend,id:8355,x:32467,y:31302,varname:node_8355,prsc:2|DIST-2426-OUT;n:type:ShaderForge.SFN_OneMinus,id:4822,x:32626,y:31302,varname:node_4822,prsc:2|IN-8355-OUT;n:type:ShaderForge.SFN_Slider,id:2426,x:32148,y:31302,ptovrint:False,ptlb:FoamWidth,ptin:_FoamWidth,varname:_FoamWidth,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.3801183,max:1;n:type:ShaderForge.SFN_Slider,id:6884,x:32148,y:31443,ptovrint:False,ptlb:FoamIntensity,ptin:_FoamIntensity,varname:_FoamIntensity,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_ChannelBlend,id:5260,x:33196,y:31292,varname:node_5260,prsc:2,chbt:1|M-4294-A,R-1130-OUT,BTM-4294-RGB;n:type:ShaderForge.SFN_Color,id:4199,x:32879,y:31642,ptovrint:False,ptlb:Water Color,ptin:_WaterColor,varname:_WaterColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.3235294,c2:0.7015572,c3:1,c4:1;n:type:ShaderForge.SFN_FragmentPosition,id:730,x:31505,y:31850,varname:node_730,prsc:2;n:type:ShaderForge.SFN_Time,id:3551,x:31564,y:31620,varname:node_3551,prsc:2;n:type:ShaderForge.SFN_Multiply,id:6743,x:31751,y:31648,varname:node_6743,prsc:2|A-3551-TTR,B-203-OUT;n:type:ShaderForge.SFN_Slider,id:203,x:31407,y:31764,ptovrint:False,ptlb:WaveSpeed,ptin:_WaveSpeed,varname:_WaveSpeed,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:6.581196,max:10;n:type:ShaderForge.SFN_Add,id:252,x:31904,y:31773,varname:node_252,prsc:2|A-6743-OUT,B-730-XYZ;n:type:ShaderForge.SFN_Sin,id:7321,x:32312,y:32162,varname:node_7321,prsc:2|IN-3274-OUT;n:type:ShaderForge.SFN_ComponentMask,id:1465,x:32735,y:32258,varname:node_1465,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-7008-OUT;n:type:ShaderForge.SFN_Vector3,id:9195,x:32735,y:32404,varname:node_9195,prsc:2,v1:0,v2:1,v3:0;n:type:ShaderForge.SFN_Multiply,id:9320,x:32980,y:32429,cmnt:VERTEX OFFSET,varname:node_9320,prsc:2|A-1465-OUT,B-9195-OUT;n:type:ShaderForge.SFN_Noise,id:6278,x:32112,y:32330,varname:node_6278,prsc:2|XY-8211-OUT;n:type:ShaderForge.SFN_Multiply,id:3274,x:32155,y:32162,varname:node_3274,prsc:2|A-252-OUT,B-8615-OUT;n:type:ShaderForge.SFN_Slider,id:4490,x:32112,y:32485,ptovrint:False,ptlb:WaveIntensity,ptin:_WaveIntensity,varname:_Intensity,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.2136752,max:1;n:type:ShaderForge.SFN_Multiply,id:8615,x:32302,y:32330,varname:node_8615,prsc:2|A-6278-OUT,B-4490-OUT;n:type:ShaderForge.SFN_Multiply,id:7008,x:32552,y:32258,varname:node_7008,prsc:2|A-7321-OUT,B-8615-OUT;n:type:ShaderForge.SFN_Tex2d,id:4294,x:32961,y:31138,ptovrint:False,ptlb:Foam Tex,ptin:_FoamTex,varname:_FoamTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c318cd66b25df4d44b3747e9c8677f65,ntxv:0,isnm:False;n:type:ShaderForge.SFN_ComponentMask,id:8211,x:31825,y:32243,varname:node_8211,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-730-XYZ;n:type:ShaderForge.SFN_Tex2d,id:7873,x:32719,y:31781,ptovrint:False,ptlb:ScrollingTexture,ptin:_ScrollingTexture,varname:node_7873,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:22c4c7a14707b48758da4efe1766837c,ntxv:2,isnm:False|UVIN-7271-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:1204,x:32312,y:31781,varname:node_1204,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Slider,id:6880,x:32155,y:31956,ptovrint:False,ptlb:ScrollSpeed,ptin:_ScrollSpeed,varname:node_6880,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:0.06102207,max:1;n:type:ShaderForge.SFN_Multiply,id:5514,x:32484,y:31941,varname:node_5514,prsc:2|A-3551-T,B-6880-OUT;n:type:ShaderForge.SFN_Panner,id:7271,x:32544,y:31781,varname:node_7271,prsc:2,spu:0,spv:1|UVIN-1204-UVOUT,DIST-5514-OUT;n:type:ShaderForge.SFN_Lerp,id:6076,x:33166,y:31754,varname:node_6076,prsc:2|A-4199-RGB,B-7873-RGB,T-8406-OUT;n:type:ShaderForge.SFN_Slider,id:4099,x:32671,y:31981,ptovrint:False,ptlb:waterTexOpacity,ptin:_waterTexOpacity,varname:node_4099,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.227659,max:1;n:type:ShaderForge.SFN_Multiply,id:8406,x:32969,y:31863,varname:node_8406,prsc:2|A-7873-A,B-4099-OUT;n:type:ShaderForge.SFN_AmbientLight,id:1660,x:33454,y:31862,varname:node_1660,prsc:2;n:type:ShaderForge.SFN_Multiply,id:8107,x:33454,y:31727,varname:node_8107,prsc:2|A-6076-OUT,B-1660-RGB;n:type:ShaderForge.SFN_Multiply,id:2552,x:33327,y:32344,varname:node_2552,prsc:2|A-6076-OUT,B-3686-OUT;n:type:ShaderForge.SFN_OneMinus,id:1130,x:32961,y:31302,varname:node_1130,prsc:2|IN-6343-OUT;n:type:ShaderForge.SFN_OneMinus,id:9909,x:33358,y:31292,varname:node_9909,prsc:2|IN-5260-OUT;n:type:ShaderForge.SFN_Add,id:7865,x:33673,y:31627,cmnt:FINAL COLOR,varname:node_7865,prsc:2|A-9909-OUT,B-8107-OUT;n:type:ShaderForge.SFN_Slider,id:3686,x:33117,y:32250,ptovrint:False,ptlb:EmissionStrength,ptin:_EmissionStrength,varname:node_3686,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;proporder:6884-2426-4199-203-4490-4294-7873-6880-4099-3686;pass:END;sub:END;*/

Shader "Custom/WaterShader Mobile" {
    Properties {
        _FoamIntensity ("FoamIntensity", Range(0, 1)) = 0
        _FoamWidth ("FoamWidth", Range(0, 1)) = 0.3801183
        _WaterColor ("Water Color", Color) = (0.3235294,0.7015572,1,1)
        _WaveSpeed ("WaveSpeed", Range(0, 10)) = 6.581196
        _WaveIntensity ("WaveIntensity", Range(0, 1)) = 0.2136752
        _FoamTex ("Foam Tex", 2D) = "white" {}
        _ScrollingTexture ("ScrollingTexture", 2D) = "black" {}
        _ScrollSpeed ("ScrollSpeed", Range(-1, 1)) = 0.06102207
        _waterTexOpacity ("waterTexOpacity", Range(0, 1)) = 0.227659
        _EmissionStrength ("EmissionStrength", Range(0, 1)) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
            #pragma target 2.0
            uniform float4 _LightColor0;
            uniform sampler2D _CameraDepthTexture;
            uniform float _FoamWidth;
            uniform float _FoamIntensity;
            uniform float4 _WaterColor;
            uniform float _WaveSpeed;
            uniform float _WaveIntensity;
            uniform sampler2D _FoamTex; uniform float4 _FoamTex_ST;
            uniform sampler2D _ScrollingTexture; uniform float4 _ScrollingTexture_ST;
            uniform float _ScrollSpeed;
            uniform float _waterTexOpacity;
            uniform float _EmissionStrength;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 projPos : TEXCOORD3;
                UNITY_FOG_COORDS(4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_3551 = _Time;
                float2 node_8211 = mul(unity_ObjectToWorld, v.vertex).rgb.rg;
                float2 node_6278_skew = node_8211 + 0.2127+node_8211.x*0.3713*node_8211.y;
                float2 node_6278_rnd = 4.789*sin(489.123*(node_6278_skew));
                float node_6278 = frac(node_6278_rnd.x*node_6278_rnd.y*(1+node_6278_skew.x));
                float node_8615 = (node_6278*_WaveIntensity);
                float3 node_9320 = ((sin((((node_3551.a*_WaveSpeed)+mul(unity_ObjectToWorld, v.vertex).rgb)*node_8615))*node_8615).r*float3(0,1,0)); // VERTEX OFFSET
                v.vertex.xyz += node_9320;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 _FoamTex_var = tex2D(_FoamTex,TRANSFORM_TEX(i.uv0, _FoamTex));
                float4 node_3551 = _Time;
                float2 node_7271 = (i.uv0+(node_3551.g*_ScrollSpeed)*float2(0,1));
                float4 _ScrollingTexture_var = tex2D(_ScrollingTexture,TRANSFORM_TEX(node_7271, _ScrollingTexture));
                float3 node_6076 = lerp(_WaterColor.rgb,_ScrollingTexture_var.rgb,(_ScrollingTexture_var.a*_waterTexOpacity));
                float3 node_7865 = ((1.0 - (lerp( _FoamTex_var.rgb.r, (1.0 - ((1.0 - saturate((sceneZ-partZ)/_FoamWidth))*_FoamIntensity)), _FoamTex_var.a.r )))+(node_6076*UNITY_LIGHTMODEL_AMBIENT.rgb)); // FINAL COLOR
                float3 diffuseColor = node_7865;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float3 emissive = (node_6076*_EmissionStrength);
/// Final Color:
                float3 finalColor = diffuse + emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
            #pragma target 2.0
            uniform float4 _LightColor0;
            uniform sampler2D _CameraDepthTexture;
            uniform float _FoamWidth;
            uniform float _FoamIntensity;
            uniform float4 _WaterColor;
            uniform float _WaveSpeed;
            uniform float _WaveIntensity;
            uniform sampler2D _FoamTex; uniform float4 _FoamTex_ST;
            uniform sampler2D _ScrollingTexture; uniform float4 _ScrollingTexture_ST;
            uniform float _ScrollSpeed;
            uniform float _waterTexOpacity;
            uniform float _EmissionStrength;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 projPos : TEXCOORD3;
                LIGHTING_COORDS(4,5)
                UNITY_FOG_COORDS(6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_3551 = _Time;
                float2 node_8211 = mul(unity_ObjectToWorld, v.vertex).rgb.rg;
                float2 node_6278_skew = node_8211 + 0.2127+node_8211.x*0.3713*node_8211.y;
                float2 node_6278_rnd = 4.789*sin(489.123*(node_6278_skew));
                float node_6278 = frac(node_6278_rnd.x*node_6278_rnd.y*(1+node_6278_skew.x));
                float node_8615 = (node_6278*_WaveIntensity);
                float3 node_9320 = ((sin((((node_3551.a*_WaveSpeed)+mul(unity_ObjectToWorld, v.vertex).rgb)*node_8615))*node_8615).r*float3(0,1,0)); // VERTEX OFFSET
                v.vertex.xyz += node_9320;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 _FoamTex_var = tex2D(_FoamTex,TRANSFORM_TEX(i.uv0, _FoamTex));
                float4 node_3551 = _Time;
                float2 node_7271 = (i.uv0+(node_3551.g*_ScrollSpeed)*float2(0,1));
                float4 _ScrollingTexture_var = tex2D(_ScrollingTexture,TRANSFORM_TEX(node_7271, _ScrollingTexture));
                float3 node_6076 = lerp(_WaterColor.rgb,_ScrollingTexture_var.rgb,(_ScrollingTexture_var.a*_waterTexOpacity));
                float3 node_7865 = ((1.0 - (lerp( _FoamTex_var.rgb.r, (1.0 - ((1.0 - saturate((sceneZ-partZ)/_FoamWidth))*_FoamIntensity)), _FoamTex_var.a.r )))+(node_6076*UNITY_LIGHTMODEL_AMBIENT.rgb)); // FINAL COLOR
                float3 diffuseColor = node_7865;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
            #pragma target 2.0
            uniform float _WaveSpeed;
            uniform float _WaveIntensity;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float4 posWorld : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                float4 node_3551 = _Time;
                float2 node_8211 = mul(unity_ObjectToWorld, v.vertex).rgb.rg;
                float2 node_6278_skew = node_8211 + 0.2127+node_8211.x*0.3713*node_8211.y;
                float2 node_6278_rnd = 4.789*sin(489.123*(node_6278_skew));
                float node_6278 = frac(node_6278_rnd.x*node_6278_rnd.y*(1+node_6278_skew.x));
                float node_8615 = (node_6278*_WaveIntensity);
                float3 node_9320 = ((sin((((node_3551.a*_WaveSpeed)+mul(unity_ObjectToWorld, v.vertex).rgb)*node_8615))*node_8615).r*float3(0,1,0)); // VERTEX OFFSET
                v.vertex.xyz += node_9320;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
