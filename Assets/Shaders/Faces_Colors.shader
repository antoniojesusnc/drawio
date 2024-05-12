// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32680,y:32898,varname:node_3138,prsc:2|emission-8349-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32418,y:32832,ptovrint:False,ptlb:ZColor,ptin:_ZColor,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.07843139,c2:0.7843137,c3:0.7843137,c4:1;n:type:ShaderForge.SFN_Color,id:5208,x:32171,y:33400,ptovrint:False,ptlb:YColor,ptin:_YColor,varname:node_5208,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:1,c3:0.08965516,c4:1;n:type:ShaderForge.SFN_Color,id:3182,x:32171,y:33232,ptovrint:False,ptlb:XColor,ptin:_XColor,varname:node_3182,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_NormalVector,id:3601,x:31342,y:32891,prsc:2,pt:False;n:type:ShaderForge.SFN_Abs,id:4646,x:31746,y:32893,varname:node_4646,prsc:2|IN-4929-OUT;n:type:ShaderForge.SFN_Lerp,id:3612,x:32418,y:33136,varname:node_3612,prsc:2|A-3182-RGB,B-5208-RGB,T-9553-OUT;n:type:ShaderForge.SFN_Vector2,id:887,x:31948,y:33205,varname:node_887,prsc:2,v1:0,v2:1;n:type:ShaderForge.SFN_Dot,id:9553,x:31948,y:33041,varname:node_9553,prsc:2,dt:0|A-4646-OUT,B-887-OUT;n:type:ShaderForge.SFN_Dot,id:4796,x:31948,y:32893,varname:node_4796,prsc:2,dt:0|A-4646-OUT,B-2974-OUT;n:type:ShaderForge.SFN_Vector3,id:2974,x:31948,y:32789,varname:node_2974,prsc:2,v1:0,v2:0,v3:1;n:type:ShaderForge.SFN_Lerp,id:8349,x:32418,y:32997,varname:node_8349,prsc:2|A-3612-OUT,B-7241-RGB,T-4796-OUT;n:type:ShaderForge.SFN_ToggleProperty,id:5768,x:31342,y:33109,ptovrint:False,ptlb:LocalSpace,ptin:_LocalSpace,varname:node_5768,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False;n:type:ShaderForge.SFN_Lerp,id:4929,x:31552,y:32893,varname:node_4929,prsc:2|A-3601-OUT,B-8759-XYZ,T-5768-OUT;n:type:ShaderForge.SFN_Transform,id:8759,x:31552,y:33033,varname:node_8759,prsc:2,tffrom:0,tfto:1|IN-3601-OUT;proporder:3182-5208-7241-5768;pass:END;sub:END;*/

Shader "Shader Forge/Faces_Colors" {
    Properties {
        _XColor ("XColor", Color) = (1,0,0,1)
        _YColor ("YColor", Color) = (0,1,0.08965516,1)
        _ZColor ("ZColor", Color) = (0.07843139,0.7843137,0.7843137,1)
        [MaterialToggle] _LocalSpace ("LocalSpace", Float ) = 0
    }
    SubShader {
        Tags {
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
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
            #pragma target 3.0
            uniform float4 _ZColor;
            uniform float4 _YColor;
            uniform float4 _XColor;
            uniform fixed _LocalSpace;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float3 normalDir : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float3 node_4646 = abs(lerp(i.normalDir,mul( unity_WorldToObject, float4(i.normalDir,0) ).xyz.rgb,_LocalSpace));
                float3 emissive = lerp(lerp(_XColor.rgb,_YColor.rgb,dot(node_4646,float2(0,1))),_ZColor.rgb,dot(node_4646,float3(0,0,1)));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
