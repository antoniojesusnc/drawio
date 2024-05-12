// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32680,y:32898,varname:node_3138,prsc:2|emission-8349-OUT,alpha-2736-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32322,y:32572,ptovrint:False,ptlb:ZColor,ptin:_ZColor,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.07843139,c2:0.7843137,c3:0.7843137,c4:1;n:type:ShaderForge.SFN_Color,id:5208,x:32075,y:33088,ptovrint:False,ptlb:YColor,ptin:_YColor,varname:node_5208,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:1,c3:0.08965516,c4:1;n:type:ShaderForge.SFN_Color,id:3182,x:32075,y:32917,ptovrint:False,ptlb:XColor,ptin:_XColor,varname:node_3182,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_NormalVector,id:3601,x:31484,y:32633,prsc:2,pt:False;n:type:ShaderForge.SFN_Abs,id:4646,x:31650,y:32633,varname:node_4646,prsc:2|IN-3601-OUT;n:type:ShaderForge.SFN_Lerp,id:3612,x:32322,y:32876,varname:node_3612,prsc:2|A-3182-RGB,B-5208-RGB,T-9553-OUT;n:type:ShaderForge.SFN_Vector2,id:887,x:31852,y:32945,varname:node_887,prsc:2,v1:0,v2:1;n:type:ShaderForge.SFN_Dot,id:9553,x:31852,y:32781,varname:node_9553,prsc:2,dt:0|A-4646-OUT,B-887-OUT;n:type:ShaderForge.SFN_Dot,id:4796,x:31852,y:32633,varname:node_4796,prsc:2,dt:0|A-4646-OUT,B-2974-OUT;n:type:ShaderForge.SFN_Vector3,id:2974,x:31852,y:32529,varname:node_2974,prsc:2,v1:0,v2:0,v3:1;n:type:ShaderForge.SFN_Lerp,id:8349,x:32322,y:32737,varname:node_8349,prsc:2|A-3612-OUT,B-7241-RGB,T-4796-OUT;n:type:ShaderForge.SFN_FragmentPosition,id:6760,x:31721,y:33199,varname:node_6760,prsc:2;n:type:ShaderForge.SFN_ObjectPosition,id:4378,x:31721,y:33337,varname:node_4378,prsc:2;n:type:ShaderForge.SFN_Add,id:4783,x:31958,y:33375,varname:node_4783,prsc:2|A-4378-Y,B-8031-OUT;n:type:ShaderForge.SFN_Subtract,id:8192,x:32158,y:33282,varname:node_8192,prsc:2|A-6760-Y,B-4783-OUT;n:type:ShaderForge.SFN_Multiply,id:2736,x:32368,y:33201,varname:node_2736,prsc:2|A-8192-OUT,B-5602-OUT;n:type:ShaderForge.SFN_ValueProperty,id:5602,x:32158,y:33471,ptovrint:False,ptlb:Factor,ptin:_Factor,varname:node_5602,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:8031,x:31721,y:33514,ptovrint:False,ptlb:YOffset,ptin:_YOffset,varname:node_8031,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;proporder:3182-5208-7241-5602-8031;pass:END;sub:END;*/

Shader "Shader Forge/FaceColorsAndPositionGradient" {
    Properties {
        _XColor ("XColor", Color) = (1,0,0,1)
        _YColor ("YColor", Color) = (0,1,0.08965516,1)
        _ZColor ("ZColor", Color) = (0.07843139,0.7843137,0.7843137,1)
        _Factor ("Factor", Float ) = 1
        _YOffset ("YOffset", Float ) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
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
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
            #pragma target 3.0
            uniform float4 _ZColor;
            uniform float4 _YColor;
            uniform float4 _XColor;
            uniform float _Factor;
            uniform float _YOffset;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float3 node_4646 = abs(i.normalDir);
                float3 emissive = lerp(lerp(_XColor.rgb,_YColor.rgb,dot(node_4646,float2(0,1))),_ZColor.rgb,dot(node_4646,float3(0,0,1)));
                float3 finalColor = emissive;
                return fixed4(finalColor,((i.posWorld.g-(objPos.g+_YOffset))*_Factor));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
