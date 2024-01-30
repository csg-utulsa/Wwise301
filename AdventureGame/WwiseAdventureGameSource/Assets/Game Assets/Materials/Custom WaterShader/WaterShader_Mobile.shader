// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:True,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:2865,x:33694,y:32150,varname:node_2865,prsc:2|diff-6343-OUT,spec-358-OUT,gloss-1813-OUT,normal-8759-OUT,emission-3324-OUT,alpha-4199-A,refract-2712-OUT,voffset-9320-OUT;n:type:ShaderForge.SFN_Multiply,id:6343,x:32833,y:31538,varname:node_6343,prsc:2|A-4822-OUT,B-6884-OUT;n:type:ShaderForge.SFN_Slider,id:358,x:33791,y:32033,ptovrint:False,ptlb:Metallic,ptin:_Metallic,varname:_Metallic,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Slider,id:1813,x:33791,y:31946,ptovrint:False,ptlb:Gloss,ptin:_Gloss,varname:_Gloss,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.8,max:1;n:type:ShaderForge.SFN_DepthBlend,id:8355,x:32477,y:31473,varname:node_8355,prsc:2|DIST-2426-OUT;n:type:ShaderForge.SFN_OneMinus,id:4822,x:32636,y:31473,varname:node_4822,prsc:2|IN-8355-OUT;n:type:ShaderForge.SFN_Slider,id:2426,x:32158,y:31473,ptovrint:False,ptlb:FoamWidth,ptin:_FoamWidth,varname:_FoamWidth,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.3801183,max:1;n:type:ShaderForge.SFN_Slider,id:6884,x:32415,y:31638,ptovrint:False,ptlb:FoamIntensity,ptin:_FoamIntensity,varname:_FoamIntensity,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_ChannelBlend,id:3324,x:33401,y:31502,varname:node_3324,prsc:2,chbt:1|M-6343-OUT,R-4294-RGB,BTM-6076-OUT;n:type:ShaderForge.SFN_ChannelBlend,id:5260,x:33533,y:31300,varname:node_5260,prsc:2,chbt:1|M-6343-OUT,R-4294-A,BTM-4199-A;n:type:ShaderForge.SFN_TexCoord,id:3369,x:31927,y:32586,varname:node_3369,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Tex2d,id:8959,x:32463,y:32614,ptovrint:False,ptlb:DisplacementTex,ptin:_DisplacementTex,varname:_DisplacementTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:e08c295755c0885479ad19f518286ff2,ntxv:3,isnm:True|UVIN-235-UVOUT;n:type:ShaderForge.SFN_Multiply,id:8412,x:32798,y:32627,varname:node_8412,prsc:2|A-8959-RGB,B-4852-OUT;n:type:ShaderForge.SFN_Slider,id:4852,x:32384,y:32828,ptovrint:False,ptlb:RefractionIntensity,ptin:_RefractionIntensity,varname:_RefractionIntensity,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.341835,max:1;n:type:ShaderForge.SFN_ComponentMask,id:2712,x:33024,y:32621,varname:node_2712,prsc:2,cc1:0,cc2:0,cc3:-1,cc4:-1|IN-8412-OUT;n:type:ShaderForge.SFN_Color,id:4199,x:32969,y:31620,ptovrint:False,ptlb:Water Color,ptin:_WaterColor,varname:_WaterColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.3235294,c2:0.7015572,c3:1,c4:1;n:type:ShaderForge.SFN_FragmentPosition,id:730,x:31505,y:31850,varname:node_730,prsc:2;n:type:ShaderForge.SFN_Time,id:3551,x:31641,y:31500,varname:node_3551,prsc:2;n:type:ShaderForge.SFN_Multiply,id:6743,x:31751,y:31648,varname:node_6743,prsc:2|A-3551-TTR,B-203-OUT;n:type:ShaderForge.SFN_Slider,id:203,x:31407,y:31764,ptovrint:False,ptlb:WaveSpeed,ptin:_WaveSpeed,varname:_WaveSpeed,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:6.581196,max:10;n:type:ShaderForge.SFN_Add,id:252,x:31904,y:31773,varname:node_252,prsc:2|A-6743-OUT,B-730-XYZ;n:type:ShaderForge.SFN_Sin,id:7321,x:32378,y:32046,varname:node_7321,prsc:2|IN-3274-OUT;n:type:ShaderForge.SFN_ComponentMask,id:1465,x:32751,y:31985,varname:node_1465,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-7008-OUT;n:type:ShaderForge.SFN_Vector3,id:9195,x:32887,y:32134,varname:node_9195,prsc:2,v1:0,v2:1,v3:0;n:type:ShaderForge.SFN_Multiply,id:9320,x:33033,y:32017,varname:node_9320,prsc:2|A-1465-OUT,B-9195-OUT;n:type:ShaderForge.SFN_Noise,id:6278,x:32246,y:32349,varname:node_6278,prsc:2|XY-8211-OUT;n:type:ShaderForge.SFN_Multiply,id:3274,x:32220,y:32110,varname:node_3274,prsc:2|A-252-OUT,B-8615-OUT;n:type:ShaderForge.SFN_Slider,id:4490,x:32246,y:32504,ptovrint:False,ptlb:WaveIntensity,ptin:_WaveIntensity,varname:_Intensity,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.2136752,max:1;n:type:ShaderForge.SFN_Multiply,id:8615,x:32436,y:32349,varname:node_8615,prsc:2|A-6278-OUT,B-4490-OUT;n:type:ShaderForge.SFN_Multiply,id:7008,x:32557,y:32032,varname:node_7008,prsc:2|A-7321-OUT,B-8615-OUT;n:type:ShaderForge.SFN_Panner,id:235,x:32239,y:32730,varname:node_235,prsc:2,spu:0.1,spv:0.1|UVIN-3369-UVOUT,DIST-489-OUT;n:type:ShaderForge.SFN_Time,id:4602,x:31893,y:32834,varname:node_4602,prsc:2;n:type:ShaderForge.SFN_Slider,id:321,x:31839,y:33040,ptovrint:False,ptlb:RefractionSlideSpeed,ptin:_RefractionSlideSpeed,varname:_RefractionSlideSpeed,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.02564129,max:1;n:type:ShaderForge.SFN_Multiply,id:489,x:32137,y:32908,varname:node_489,prsc:2|A-4602-T,B-321-OUT;n:type:ShaderForge.SFN_Tex2d,id:4294,x:33134,y:31125,ptovrint:False,ptlb:Foam Tex,ptin:_FoamTex,varname:_FoamTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_DDX,id:7474,x:31427,y:32280,cmnt:Flat-shading and normal recalculating,varname:node_7474,prsc:2|IN-730-XYZ;n:type:ShaderForge.SFN_DDY,id:8825,x:31427,y:32412,varname:node_8825,prsc:2|IN-730-XYZ;n:type:ShaderForge.SFN_Normalize,id:4245,x:31593,y:32280,varname:node_4245,prsc:2|IN-7474-OUT;n:type:ShaderForge.SFN_Normalize,id:1480,x:31593,y:32412,varname:node_1480,prsc:2|IN-8825-OUT;n:type:ShaderForge.SFN_Cross,id:8759,x:31778,y:32341,varname:node_8759,prsc:2|A-4245-OUT,B-1480-OUT;n:type:ShaderForge.SFN_ComponentMask,id:8211,x:31917,y:32022,varname:node_8211,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-730-XYZ;n:type:ShaderForge.SFN_Tex2d,id:7873,x:32857,y:31750,ptovrint:False,ptlb:ScrollingTexture,ptin:_ScrollingTexture,varname:node_7873,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:2,isnm:False|UVIN-7271-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:1204,x:32251,y:31672,varname:node_1204,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Slider,id:6880,x:32156,y:31926,ptovrint:False,ptlb:ScrollSpeed,ptin:_ScrollSpeed,varname:node_6880,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:-0.3663967,max:1;n:type:ShaderForge.SFN_Multiply,id:5514,x:32493,y:31852,varname:node_5514,prsc:2|A-3551-T,B-6880-OUT;n:type:ShaderForge.SFN_Panner,id:7271,x:32697,y:31738,varname:node_7271,prsc:2,spu:0,spv:1|UVIN-1204-UVOUT,DIST-5514-OUT;n:type:ShaderForge.SFN_Lerp,id:6076,x:33166,y:31754,varname:node_6076,prsc:2|A-4199-RGB,B-7873-RGB,T-8406-OUT;n:type:ShaderForge.SFN_Slider,id:4099,x:32887,y:31939,ptovrint:False,ptlb:waterTexOpacity,ptin:_waterTexOpacity,varname:node_4099,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.227659,max:1;n:type:ShaderForge.SFN_Multiply,id:8406,x:33227,y:31892,varname:node_8406,prsc:2|A-7873-A,B-4099-OUT;proporder:358-1813-6884-2426-4199-8959-4852-203-4490-321-4294-7873-6880-4099;pass:END;sub:END;*/

Shader "Shader Forge/WaterShader" {
    Properties {
        _Metallic ("Metallic", Range(0, 1)) = 0
        _Gloss ("Gloss", Range(0, 1)) = 0.8
        _FoamIntensity ("FoamIntensity", Range(0, 1)) = 0
        _FoamWidth ("FoamWidth", Range(0, 1)) = 0.3801183
        _WaterColor ("Water Color", Color) = (0.3235294,0.7015572,1,1)
        _DisplacementTex ("DisplacementTex", 2D) = "bump" {}
        _RefractionIntensity ("RefractionIntensity", Range(0, 1)) = 0.341835
        _WaveSpeed ("WaveSpeed", Range(0, 10)) = 6.581196
        _WaveIntensity ("WaveIntensity", Range(0, 1)) = 0.2136752
        _RefractionSlideSpeed ("RefractionSlideSpeed", Range(0, 1)) = 0.02564129
        _FoamTex ("Foam Tex", 2D) = "white" {}
        _ScrollingTexture ("ScrollingTexture", 2D) = "black" {}
        _ScrollSpeed ("ScrollSpeed", Range(-1, 1)) = -0.3663967
        _waterTexOpacity ("waterTexOpacity", Range(0, 1)) = 0.227659
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles n3ds wiiu 
            #pragma target 2.0
            uniform sampler2D _GrabTexture;
            uniform sampler2D _CameraDepthTexture;
            uniform float _Metallic;
            uniform float _Gloss;
            uniform float _FoamWidth;
            uniform float _FoamIntensity;
            uniform sampler2D _DisplacementTex; uniform float4 _DisplacementTex_ST;
            uniform float _RefractionIntensity;
            uniform float4 _WaterColor;
            uniform float _WaveSpeed;
            uniform float _WaveIntensity;
            uniform float _RefractionSlideSpeed;
            uniform sampler2D _FoamTex; uniform float4 _FoamTex_ST;
            uniform sampler2D _ScrollingTexture; uniform float4 _ScrollingTexture_ST;
            uniform float _ScrollSpeed;
            uniform float _waterTexOpacity;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                float4 projPos : TEXCOORD7;
                UNITY_FOG_COORDS(8)
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD9;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                #ifdef LIGHTMAP_ON
                    o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                    o.ambientOrLightmapUV.zw = 0;
                #elif UNITY_SHOULD_SAMPLE_SH
                #endif
                #ifdef DYNAMICLIGHTMAP_ON
                    o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
                #endif
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float4 node_3551 = _Time;
                float2 node_8211 = mul(unity_ObjectToWorld, v.vertex).rgb.rg;
                float2 node_6278_skew = node_8211 + 0.2127+node_8211.x*0.3713*node_8211.y;
                float2 node_6278_rnd = 4.789*sin(489.123*(node_6278_skew));
                float node_6278 = frac(node_6278_rnd.x*node_6278_rnd.y*(1+node_6278_skew.x));
                float node_8615 = (node_6278*_WaveIntensity);
                v.vertex.xyz += ((sin((((node_3551.a*_WaveSpeed)+mul(unity_ObjectToWorld, v.vertex).rgb)*node_8615))*node_8615).r*float3(0,1,0));
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
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalLocal = cross(normalize(ddx(i.posWorld.rgb)),normalize(ddy(i.posWorld.rgb)));
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                float4 node_4602 = _Time;
                float2 node_235 = (i.uv0+(node_4602.g*_RefractionSlideSpeed)*float2(0.1,0.1));
                float3 _DisplacementTex_var = UnpackNormal(tex2D(_DisplacementTex,TRANSFORM_TEX(node_235, _DisplacementTex)));
                float2 sceneUVs = (i.projPos.xy / i.projPos.w) + (_DisplacementTex_var.rgb*_RefractionIntensity).rr;
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = _Gloss;
                float perceptualRoughness = 1.0 - _Gloss;
                float roughness = perceptualRoughness * perceptualRoughness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
/////// GI Data:
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
                #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
                    d.ambient = 0;
                    d.lightmapUV = i.ambientOrLightmapUV;
                #else
                    d.ambient = i.ambientOrLightmapUV;
                #endif
                #if UNITY_SPECCUBE_BLENDING || UNITY_SPECCUBE_BOX_PROJECTION
                    d.boxMin[0] = unity_SpecCube0_BoxMin;
                    d.boxMin[1] = unity_SpecCube1_BoxMin;
                #endif
                #if UNITY_SPECCUBE_BOX_PROJECTION
                    d.boxMax[0] = unity_SpecCube0_BoxMax;
                    d.boxMax[1] = unity_SpecCube1_BoxMax;
                    d.probePosition[0] = unity_SpecCube0_ProbePosition;
                    d.probePosition[1] = unity_SpecCube1_ProbePosition;
                #endif
                d.probeHDR[0] = unity_SpecCube0_HDR;
                d.probeHDR[1] = unity_SpecCube1_HDR;
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - gloss;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float3 specularColor = _Metallic;
                float specularMonochrome;
                float node_6343 = ((1.0 - saturate((sceneZ-partZ)/_FoamWidth))*_FoamIntensity);
                float3 diffuseColor = float3(node_6343,node_6343,node_6343); // Need this for specular when using metallic
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, specularColor, specularColor, specularMonochrome );
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = abs(dot( normalDirection, viewDirection ));
                float NdotH = saturate(dot( normalDirection, halfDirection ));
                float VdotH = saturate(dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, roughness );
                float normTerm = GGXTerm(NdotH, roughness);
                float specularPBL = (visTerm*normTerm) * UNITY_PI;
                #ifdef UNITY_COLORSPACE_GAMMA
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                #endif
                specularPBL = max(0, specularPBL * NdotL);
                #if defined(_SPECULARHIGHLIGHTS_OFF)
                    specularPBL = 0.0;
                #endif
                half surfaceReduction;
                #ifdef UNITY_COLORSPACE_GAMMA
                    surfaceReduction = 1.0-0.28*roughness*perceptualRoughness;
                #else
                    surfaceReduction = 1.0/(roughness*roughness + 1.0);
                #endif
                specularPBL *= any(specularColor) ? 1.0 : 0.0;
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                half grazingTerm = saturate( gloss + specularMonochrome );
                float3 indirectSpecular = (gi.indirect.specular);
                indirectSpecular *= FresnelLerp (specularColor, grazingTerm, NdotV);
                indirectSpecular *= surfaceReduction;
                float3 specular = (directSpecular + indirectSpecular);
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += gi.indirect.diffuse;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float4 node_3551 = _Time;
                float2 node_7271 = (i.uv0+(node_3551.g*_ScrollSpeed)*float2(0,1));
                float4 _ScrollingTexture_var = tex2D(_ScrollingTexture,TRANSFORM_TEX(node_7271, _ScrollingTexture));
                float4 _FoamTex_var = tex2D(_FoamTex,TRANSFORM_TEX(i.uv0, _FoamTex));
                float3 emissive = (lerp( lerp(_WaterColor.rgb,_ScrollingTexture_var.rgb,(_ScrollingTexture_var.a*_waterTexOpacity)), _FoamTex_var.rgb, node_6343.r ));
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
                fixed4 finalRGBA = fixed4(lerp(sceneColor.rgb, finalColor,_WaterColor.a),1);
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
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles n3ds wiiu 
            #pragma target 2.0
            uniform sampler2D _GrabTexture;
            uniform sampler2D _CameraDepthTexture;
            uniform float _Metallic;
            uniform float _Gloss;
            uniform float _FoamWidth;
            uniform float _FoamIntensity;
            uniform sampler2D _DisplacementTex; uniform float4 _DisplacementTex_ST;
            uniform float _RefractionIntensity;
            uniform float4 _WaterColor;
            uniform float _WaveSpeed;
            uniform float _WaveIntensity;
            uniform float _RefractionSlideSpeed;
            uniform sampler2D _FoamTex; uniform float4 _FoamTex_ST;
            uniform sampler2D _ScrollingTexture; uniform float4 _ScrollingTexture_ST;
            uniform float _ScrollSpeed;
            uniform float _waterTexOpacity;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                float4 projPos : TEXCOORD7;
                LIGHTING_COORDS(8,9)
                UNITY_FOG_COORDS(10)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float4 node_3551 = _Time;
                float2 node_8211 = mul(unity_ObjectToWorld, v.vertex).rgb.rg;
                float2 node_6278_skew = node_8211 + 0.2127+node_8211.x*0.3713*node_8211.y;
                float2 node_6278_rnd = 4.789*sin(489.123*(node_6278_skew));
                float node_6278 = frac(node_6278_rnd.x*node_6278_rnd.y*(1+node_6278_skew.x));
                float node_8615 = (node_6278*_WaveIntensity);
                v.vertex.xyz += ((sin((((node_3551.a*_WaveSpeed)+mul(unity_ObjectToWorld, v.vertex).rgb)*node_8615))*node_8615).r*float3(0,1,0));
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
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalLocal = cross(normalize(ddx(i.posWorld.rgb)),normalize(ddy(i.posWorld.rgb)));
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                float4 node_4602 = _Time;
                float2 node_235 = (i.uv0+(node_4602.g*_RefractionSlideSpeed)*float2(0.1,0.1));
                float3 _DisplacementTex_var = UnpackNormal(tex2D(_DisplacementTex,TRANSFORM_TEX(node_235, _DisplacementTex)));
                float2 sceneUVs = (i.projPos.xy / i.projPos.w) + (_DisplacementTex_var.rgb*_RefractionIntensity).rr;
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = _Gloss;
                float perceptualRoughness = 1.0 - _Gloss;
                float roughness = perceptualRoughness * perceptualRoughness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float3 specularColor = _Metallic;
                float specularMonochrome;
                float node_6343 = ((1.0 - saturate((sceneZ-partZ)/_FoamWidth))*_FoamIntensity);
                float3 diffuseColor = float3(node_6343,node_6343,node_6343); // Need this for specular when using metallic
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, specularColor, specularColor, specularMonochrome );
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = abs(dot( normalDirection, viewDirection ));
                float NdotH = saturate(dot( normalDirection, halfDirection ));
                float VdotH = saturate(dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, roughness );
                float normTerm = GGXTerm(NdotH, roughness);
                float specularPBL = (visTerm*normTerm) * UNITY_PI;
                #ifdef UNITY_COLORSPACE_GAMMA
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                #endif
                specularPBL = max(0, specularPBL * NdotL);
                #if defined(_SPECULARHIGHLIGHTS_OFF)
                    specularPBL = 0.0;
                #endif
                specularPBL *= any(specularColor) ? 1.0 : 0.0;
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * _WaterColor.a,0);
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
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles n3ds wiiu 
            #pragma target 2.0
            uniform float _WaveSpeed;
            uniform float _WaveIntensity;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                float4 node_3551 = _Time;
                float2 node_8211 = mul(unity_ObjectToWorld, v.vertex).rgb.rg;
                float2 node_6278_skew = node_8211 + 0.2127+node_8211.x*0.3713*node_8211.y;
                float2 node_6278_rnd = 4.789*sin(489.123*(node_6278_skew));
                float node_6278 = frac(node_6278_rnd.x*node_6278_rnd.y*(1+node_6278_skew.x));
                float node_8615 = (node_6278*_WaveIntensity);
                v.vertex.xyz += ((sin((((node_3551.a*_WaveSpeed)+mul(unity_ObjectToWorld, v.vertex).rgb)*node_8615))*node_8615).r*float3(0,1,0));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_META 1
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles n3ds wiiu 
            #pragma target 2.0
            uniform sampler2D _CameraDepthTexture;
            uniform float _Metallic;
            uniform float _Gloss;
            uniform float _FoamWidth;
            uniform float _FoamIntensity;
            uniform float4 _WaterColor;
            uniform float _WaveSpeed;
            uniform float _WaveIntensity;
            uniform sampler2D _FoamTex; uniform float4 _FoamTex_ST;
            uniform sampler2D _ScrollingTexture; uniform float4 _ScrollingTexture_ST;
            uniform float _ScrollSpeed;
            uniform float _waterTexOpacity;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float4 projPos : TEXCOORD4;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                float4 node_3551 = _Time;
                float2 node_8211 = mul(unity_ObjectToWorld, v.vertex).rgb.rg;
                float2 node_6278_skew = node_8211 + 0.2127+node_8211.x*0.3713*node_8211.y;
                float2 node_6278_rnd = 4.789*sin(489.123*(node_6278_skew));
                float node_6278 = frac(node_6278_rnd.x*node_6278_rnd.y*(1+node_6278_skew.x));
                float node_8615 = (node_6278*_WaveIntensity);
                v.vertex.xyz += ((sin((((node_3551.a*_WaveSpeed)+mul(unity_ObjectToWorld, v.vertex).rgb)*node_8615))*node_8615).r*float3(0,1,0));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                float node_6343 = ((1.0 - saturate((sceneZ-partZ)/_FoamWidth))*_FoamIntensity);
                float4 node_3551 = _Time;
                float2 node_7271 = (i.uv0+(node_3551.g*_ScrollSpeed)*float2(0,1));
                float4 _ScrollingTexture_var = tex2D(_ScrollingTexture,TRANSFORM_TEX(node_7271, _ScrollingTexture));
                float4 _FoamTex_var = tex2D(_FoamTex,TRANSFORM_TEX(i.uv0, _FoamTex));
                o.Emission = (lerp( lerp(_WaterColor.rgb,_ScrollingTexture_var.rgb,(_ScrollingTexture_var.a*_waterTexOpacity)), _FoamTex_var.rgb, node_6343.r ));
                
                float3 diffColor = float3(node_6343,node_6343,node_6343);
                float specularMonochrome;
                float3 specColor;
                diffColor = DiffuseAndSpecularFromMetallic( diffColor, _Metallic, specColor, specularMonochrome );
                float roughness = 1.0 - _Gloss;
                o.Albedo = diffColor + specColor * roughness * roughness * 0.5;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
