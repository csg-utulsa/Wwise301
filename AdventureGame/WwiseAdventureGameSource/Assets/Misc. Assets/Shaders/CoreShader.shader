// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.30 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.30;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:2865,x:33694,y:32150,varname:node_2865,prsc:2|diff-8406-OUT,spec-358-OUT,gloss-1813-OUT,emission-35-OUT,lwrap-4930-OUT,olwid-605-OUT,olcol-2025-OUT,voffset-9320-OUT;n:type:ShaderForge.SFN_Slider,id:358,x:33791,y:32033,ptovrint:False,ptlb:Metallic,ptin:_Metallic,varname:_Metallic,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Slider,id:1813,x:33791,y:31946,ptovrint:False,ptlb:Gloss,ptin:_Gloss,varname:_Gloss,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_FragmentPosition,id:730,x:31505,y:31850,varname:node_730,prsc:2;n:type:ShaderForge.SFN_Time,id:3551,x:31564,y:31620,varname:node_3551,prsc:2;n:type:ShaderForge.SFN_Multiply,id:6743,x:31751,y:31648,varname:node_6743,prsc:2|A-3551-TTR,B-203-OUT;n:type:ShaderForge.SFN_Slider,id:203,x:31407,y:31764,ptovrint:False,ptlb:WaveSpeed,ptin:_WaveSpeed,varname:_WaveSpeed,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:10,max:10;n:type:ShaderForge.SFN_Add,id:252,x:31904,y:31773,varname:node_252,prsc:2|A-6743-OUT,B-730-XYZ;n:type:ShaderForge.SFN_Sin,id:7321,x:32378,y:32046,varname:node_7321,prsc:2|IN-3274-OUT;n:type:ShaderForge.SFN_ComponentMask,id:1465,x:32751,y:31985,varname:node_1465,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-7008-OUT;n:type:ShaderForge.SFN_Vector3,id:9195,x:32887,y:32132,varname:node_9195,prsc:2,v1:0,v2:1,v3:0;n:type:ShaderForge.SFN_Multiply,id:9320,x:33033,y:32017,varname:node_9320,prsc:2|A-1465-OUT,B-9195-OUT;n:type:ShaderForge.SFN_Noise,id:6278,x:32246,y:32349,varname:node_6278,prsc:2|XY-8211-OUT;n:type:ShaderForge.SFN_Multiply,id:3274,x:32220,y:32110,varname:node_3274,prsc:2|A-252-OUT,B-8615-OUT;n:type:ShaderForge.SFN_Slider,id:4490,x:32246,y:32504,ptovrint:False,ptlb:WaveIntensity,ptin:_WaveIntensity,varname:_Intensity,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.08167338,max:1;n:type:ShaderForge.SFN_Multiply,id:8615,x:32436,y:32349,varname:node_8615,prsc:2|A-6278-OUT,B-4490-OUT;n:type:ShaderForge.SFN_Multiply,id:7008,x:32557,y:32032,varname:node_7008,prsc:2|A-7321-OUT,B-8615-OUT;n:type:ShaderForge.SFN_DDX,id:7474,x:31427,y:32280,cmnt:Flat-shading and normal recalculating,varname:node_7474,prsc:2|IN-730-XYZ;n:type:ShaderForge.SFN_DDY,id:8825,x:31427,y:32412,varname:node_8825,prsc:2|IN-730-XYZ;n:type:ShaderForge.SFN_Normalize,id:4245,x:31593,y:32280,varname:node_4245,prsc:2|IN-7474-OUT;n:type:ShaderForge.SFN_Normalize,id:1480,x:31593,y:32412,varname:node_1480,prsc:2|IN-8825-OUT;n:type:ShaderForge.SFN_Cross,id:8759,x:31778,y:32341,varname:node_8759,prsc:2|A-4245-OUT,B-1480-OUT;n:type:ShaderForge.SFN_ComponentMask,id:8211,x:31917,y:32022,varname:node_8211,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-730-XYZ;n:type:ShaderForge.SFN_Tex2d,id:7873,x:32923,y:31634,ptovrint:False,ptlb:ScrollingTexture,ptin:_ScrollingTexture,varname:node_7873,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:0b4c1564f776ac24f9df3708f138aeb6,ntxv:2,isnm:False|UVIN-7271-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:1204,x:32251,y:31672,varname:node_1204,prsc:2,uv:0;n:type:ShaderForge.SFN_Slider,id:6880,x:32141,y:31938,ptovrint:False,ptlb:ScrollSpeed,ptin:_ScrollSpeed,varname:node_6880,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:-0.3663967,max:1;n:type:ShaderForge.SFN_Time,id:3118,x:32114,y:31809,varname:node_3118,prsc:2;n:type:ShaderForge.SFN_Multiply,id:5514,x:32493,y:31852,varname:node_5514,prsc:2|A-3118-T,B-6880-OUT;n:type:ShaderForge.SFN_Panner,id:7271,x:32697,y:31738,varname:node_7271,prsc:2,spu:0,spv:1|UVIN-1204-UVOUT,DIST-5514-OUT;n:type:ShaderForge.SFN_Multiply,id:8406,x:33248,y:31677,varname:node_8406,prsc:2|A-7873-RGB,B-556-RGB;n:type:ShaderForge.SFN_NormalVector,id:8966,x:32909,y:32892,prsc:2,pt:False;n:type:ShaderForge.SFN_ViewVector,id:4945,x:33039,y:33016,varname:node_4945,prsc:2;n:type:ShaderForge.SFN_Dot,id:8448,x:33134,y:32892,varname:node_8448,prsc:2,dt:0|A-8966-OUT,B-4945-OUT;n:type:ShaderForge.SFN_OneMinus,id:4930,x:33299,y:32931,varname:node_4930,prsc:2|IN-8448-OUT;n:type:ShaderForge.SFN_Color,id:8542,x:33262,y:32748,ptovrint:False,ptlb:FresnelColor,ptin:_FresnelColor,varname:node_8542,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.8068966,c3:0,c4:1;n:type:ShaderForge.SFN_Multiply,id:35,x:33491,y:32834,varname:node_35,prsc:2|A-8542-RGB,B-4930-OUT,C-8244-OUT;n:type:ShaderForge.SFN_Color,id:556,x:32985,y:31839,ptovrint:False,ptlb:ColorTint,ptin:_ColorTint,varname:node_556,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5441177,c2:0.3220007,c3:0.08401819,c4:1;n:type:ShaderForge.SFN_Slider,id:8244,x:33409,y:33098,ptovrint:False,ptlb:FresnelStrength,ptin:_FresnelStrength,varname:node_8244,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5875902,max:1;n:type:ShaderForge.SFN_Slider,id:605,x:32910,y:32358,ptovrint:False,ptlb:OutlineWidth,ptin:_OutlineWidth,varname:node_605,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.03781155,max:1;n:type:ShaderForge.SFN_Color,id:5748,x:32938,y:32452,ptovrint:False,ptlb:OutlineColor,ptin:_OutlineColor,varname:node_5748,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.8482759,c3:0,c4:1;n:type:ShaderForge.SFN_Add,id:2025,x:33154,y:32477,varname:node_2025,prsc:2|A-8406-OUT,B-5748-RGB;proporder:358-1813-203-4490-7873-556-6880-8542-8244-605-5748;pass:END;sub:END;*/

Shader "Custom Shaders/Core Shader" {
    Properties {
        _Metallic ("Metallic", Range(0, 1)) = 0
        _Gloss ("Gloss", Range(0, 1)) = 0
        _WaveSpeed ("WaveSpeed", Range(0, 10)) = 10
        _WaveIntensity ("WaveIntensity", Range(0, 1)) = 0.08167338
        _ScrollingTexture ("ScrollingTexture", 2D) = "black" {}
        _ColorTint ("ColorTint", Color) = (0.5441177,0.3220007,0.08401819,1)
        _ScrollSpeed ("ScrollSpeed", Range(-1, 1)) = -0.3663967
        _FresnelColor ("FresnelColor", Color) = (1,0.8068966,0,1)
        _FresnelStrength ("FresnelStrength", Range(0, 1)) = 0.5875902
        _OutlineWidth ("OutlineWidth", Range(0, 1)) = 0.03781155
        _OutlineColor ("OutlineColor", Color) = (1,0.8482759,0,1)
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "Outline"
            Tags {
            }
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
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
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float _WaveSpeed;
            uniform float _WaveIntensity;
            uniform sampler2D _ScrollingTexture; uniform float4 _ScrollingTexture_ST;
            uniform float _ScrollSpeed;
            uniform float4 _ColorTint;
            uniform float _OutlineWidth;
            uniform float4 _OutlineColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
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
                UNITY_FOG_COORDS(4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                float4 node_3551 = _Time + _TimeEditor;
                float2 node_8211 = mul(unity_ObjectToWorld, v.vertex).rgb.rg;
                float2 node_6278_skew = node_8211 + 0.2127+node_8211.x*0.3713*node_8211.y;
                float2 node_6278_rnd = 4.789*sin(489.123*(node_6278_skew));
                float node_6278 = frac(node_6278_rnd.x*node_6278_rnd.y*(1+node_6278_skew.x));
                float node_8615 = (node_6278*_WaveIntensity);
                v.vertex.xyz += ((sin((((node_3551.a*_WaveSpeed)+mul(unity_ObjectToWorld, v.vertex).rgb)*node_8615))*node_8615).r*float3(0,1,0));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(float4(v.vertex.xyz + v.normal*_OutlineWidth,1) );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float4 node_3118 = _Time + _TimeEditor;
                float2 node_7271 = (i.uv0+(node_3118.g*_ScrollSpeed)*float2(0,1));
                float4 _ScrollingTexture_var = tex2D(_ScrollingTexture,TRANSFORM_TEX(node_7271, _ScrollingTexture));
                float3 node_8406 = (_ScrollingTexture_var.rgb*_ColorTint.rgb);
                return fixed4((node_8406+_OutlineColor.rgb),0);
            }
            ENDCG
        }
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
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float _Metallic;
            uniform float _Gloss;
            uniform float _WaveSpeed;
            uniform float _WaveIntensity;
            uniform sampler2D _ScrollingTexture; uniform float4 _ScrollingTexture_ST;
            uniform float _ScrollSpeed;
            uniform float4 _FresnelColor;
            uniform float4 _ColorTint;
            uniform float _FresnelStrength;
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
                UNITY_FOG_COORDS(7)
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD8;
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
                float4 node_3551 = _Time + _TimeEditor;
                float2 node_8211 = mul(unity_ObjectToWorld, v.vertex).rgb.rg;
                float2 node_6278_skew = node_8211 + 0.2127+node_8211.x*0.3713*node_8211.y;
                float2 node_6278_rnd = 4.789*sin(489.123*(node_6278_skew));
                float node_6278 = frac(node_6278_rnd.x*node_6278_rnd.y*(1+node_6278_skew.x));
                float node_8615 = (node_6278*_WaveIntensity);
                v.vertex.xyz += ((sin((((node_3551.a*_WaveSpeed)+mul(unity_ObjectToWorld, v.vertex).rgb)*node_8615))*node_8615).r*float3(0,1,0));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
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
                float specPow = exp2( gloss * 10.0+1.0);
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
                d.boxMax[0] = unity_SpecCube0_BoxMax;
                d.boxMin[0] = unity_SpecCube0_BoxMin;
                d.probePosition[0] = unity_SpecCube0_ProbePosition;
                d.probeHDR[0] = unity_SpecCube0_HDR;
                d.boxMax[1] = unity_SpecCube1_BoxMax;
                d.boxMin[1] = unity_SpecCube1_BoxMin;
                d.probePosition[1] = unity_SpecCube1_ProbePosition;
                d.probeHDR[1] = unity_SpecCube1_HDR;
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - gloss;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float LdotH = max(0.0,dot(lightDirection, halfDirection));
                float3 specularColor = _Metallic;
                float specularMonochrome;
                float4 node_3118 = _Time + _TimeEditor;
                float2 node_7271 = (i.uv0+(node_3118.g*_ScrollSpeed)*float2(0,1));
                float4 _ScrollingTexture_var = tex2D(_ScrollingTexture,TRANSFORM_TEX(node_7271, _ScrollingTexture));
                float3 node_8406 = (_ScrollingTexture_var.rgb*_ColorTint.rgb);
                float3 diffuseColor = node_8406; // Need this for specular when using metallic
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, specularColor, specularColor, specularMonochrome );
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = max(0.0,dot( normalDirection, viewDirection ));
                float NdotH = max(0.0,dot( normalDirection, halfDirection ));
                float VdotH = max(0.0,dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, 1.0-gloss );
                float normTerm = max(0.0, GGXTerm(NdotH, 1.0-gloss));
                float specularPBL = (NdotL*visTerm*normTerm) * (UNITY_PI / 4);
                if (IsGammaSpace())
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                specularPBL = max(0, specularPBL * NdotL);
                float3 directSpecular = (floor(attenuation) * _LightColor0.xyz)*specularPBL*FresnelTerm(specularColor, LdotH);
                half grazingTerm = saturate( gloss + specularMonochrome );
                float3 indirectSpecular = (gi.indirect.specular);
                indirectSpecular *= FresnelLerp (specularColor, grazingTerm, NdotV);
                float3 specular = (directSpecular + indirectSpecular);
/////// Diffuse:
                NdotL = dot( normalDirection, lightDirection );
                float node_4930 = (1.0 - dot(i.normalDir,viewDirection));
                float3 w = float3(node_4930,node_4930,node_4930)*0.5; // Light wrapping
                float3 NdotLWrap = NdotL * ( 1.0 - w );
                float3 forwardLight = max(float3(0.0,0.0,0.0), NdotLWrap + w );
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotLWrap);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = (forwardLight + ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL)) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += gi.indirect.diffuse;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float3 emissive = (_FresnelColor.rgb*node_4930*_FresnelStrength);
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
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
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float _Metallic;
            uniform float _Gloss;
            uniform float _WaveSpeed;
            uniform float _WaveIntensity;
            uniform sampler2D _ScrollingTexture; uniform float4 _ScrollingTexture_ST;
            uniform float _ScrollSpeed;
            uniform float4 _FresnelColor;
            uniform float4 _ColorTint;
            uniform float _FresnelStrength;
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
                LIGHTING_COORDS(7,8)
                UNITY_FOG_COORDS(9)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float4 node_3551 = _Time + _TimeEditor;
                float2 node_8211 = mul(unity_ObjectToWorld, v.vertex).rgb.rg;
                float2 node_6278_skew = node_8211 + 0.2127+node_8211.x*0.3713*node_8211.y;
                float2 node_6278_rnd = 4.789*sin(489.123*(node_6278_skew));
                float node_6278 = frac(node_6278_rnd.x*node_6278_rnd.y*(1+node_6278_skew.x));
                float node_8615 = (node_6278*_WaveIntensity);
                v.vertex.xyz += ((sin((((node_3551.a*_WaveSpeed)+mul(unity_ObjectToWorld, v.vertex).rgb)*node_8615))*node_8615).r*float3(0,1,0));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
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
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float LdotH = max(0.0,dot(lightDirection, halfDirection));
                float3 specularColor = _Metallic;
                float specularMonochrome;
                float4 node_3118 = _Time + _TimeEditor;
                float2 node_7271 = (i.uv0+(node_3118.g*_ScrollSpeed)*float2(0,1));
                float4 _ScrollingTexture_var = tex2D(_ScrollingTexture,TRANSFORM_TEX(node_7271, _ScrollingTexture));
                float3 node_8406 = (_ScrollingTexture_var.rgb*_ColorTint.rgb);
                float3 diffuseColor = node_8406; // Need this for specular when using metallic
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, specularColor, specularColor, specularMonochrome );
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = max(0.0,dot( normalDirection, viewDirection ));
                float NdotH = max(0.0,dot( normalDirection, halfDirection ));
                float VdotH = max(0.0,dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, 1.0-gloss );
                float normTerm = max(0.0, GGXTerm(NdotH, 1.0-gloss));
                float specularPBL = (NdotL*visTerm*normTerm) * (UNITY_PI / 4);
                if (IsGammaSpace())
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                specularPBL = max(0, specularPBL * NdotL);
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = dot( normalDirection, lightDirection );
                float node_4930 = (1.0 - dot(i.normalDir,viewDirection));
                float3 w = float3(node_4930,node_4930,node_4930)*0.5; // Light wrapping
                float3 NdotLWrap = NdotL * ( 1.0 - w );
                float3 forwardLight = max(float3(0.0,0.0,0.0), NdotLWrap + w );
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotLWrap);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = (forwardLight + ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL)) * attenColor;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
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
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
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
                float4 node_3551 = _Time + _TimeEditor;
                float2 node_8211 = mul(unity_ObjectToWorld, v.vertex).rgb.rg;
                float2 node_6278_skew = node_8211 + 0.2127+node_8211.x*0.3713*node_8211.y;
                float2 node_6278_rnd = 4.789*sin(489.123*(node_6278_skew));
                float node_6278 = frac(node_6278_rnd.x*node_6278_rnd.y*(1+node_6278_skew.x));
                float node_8615 = (node_6278*_WaveIntensity);
                v.vertex.xyz += ((sin((((node_3551.a*_WaveSpeed)+mul(unity_ObjectToWorld, v.vertex).rgb)*node_8615))*node_8615).r*float3(0,1,0));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
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
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float _Metallic;
            uniform float _Gloss;
            uniform float _WaveSpeed;
            uniform float _WaveIntensity;
            uniform sampler2D _ScrollingTexture; uniform float4 _ScrollingTexture_ST;
            uniform float _ScrollSpeed;
            uniform float4 _FresnelColor;
            uniform float4 _ColorTint;
            uniform float _FresnelStrength;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
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
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_3551 = _Time + _TimeEditor;
                float2 node_8211 = mul(unity_ObjectToWorld, v.vertex).rgb.rg;
                float2 node_6278_skew = node_8211 + 0.2127+node_8211.x*0.3713*node_8211.y;
                float2 node_6278_rnd = 4.789*sin(489.123*(node_6278_skew));
                float node_6278 = frac(node_6278_rnd.x*node_6278_rnd.y*(1+node_6278_skew.x));
                float node_8615 = (node_6278*_WaveIntensity);
                v.vertex.xyz += ((sin((((node_3551.a*_WaveSpeed)+mul(unity_ObjectToWorld, v.vertex).rgb)*node_8615))*node_8615).r*float3(0,1,0));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                float node_4930 = (1.0 - dot(i.normalDir,viewDirection));
                o.Emission = (_FresnelColor.rgb*node_4930*_FresnelStrength);
                
                float4 node_3118 = _Time + _TimeEditor;
                float2 node_7271 = (i.uv0+(node_3118.g*_ScrollSpeed)*float2(0,1));
                float4 _ScrollingTexture_var = tex2D(_ScrollingTexture,TRANSFORM_TEX(node_7271, _ScrollingTexture));
                float3 node_8406 = (_ScrollingTexture_var.rgb*_ColorTint.rgb);
                float3 diffColor = node_8406;
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
