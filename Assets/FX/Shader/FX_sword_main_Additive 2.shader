// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FX_sword_main2"
{
	Properties
	{
		_Wind_Cuttind_A_Retuch("Wind_Cuttind_A_Retuch", 2D) = "white" {}
		_noisetex("noisetex", 2D) = "white" {}
		_mask_test("mask_test", 2D) = "white" {}
		_TimeScale("TimeScale", Float) = 0
		_distortTex("distortTex", 2D) = "white" {}
		_distortamount("distortamount", Range( 0 , 0.1)) = 0.1
		[HideInInspector] _tex4coord2( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Overlay"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Off
		ZWrite Off
		ZTest LEqual
		Blend One One , One One
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
			float4 uv2_tex4coord2;
		};

		uniform sampler2D _Wind_Cuttind_A_Retuch;
		uniform float4 _Wind_Cuttind_A_Retuch_ST;
		uniform sampler2D _mask_test;
		uniform sampler2D _distortTex;
		uniform float4 _distortTex_ST;
		uniform float _distortamount;
		uniform sampler2D _noisetex;
		uniform float4 _noisetex_ST;
		uniform float _TimeScale;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_Wind_Cuttind_A_Retuch = i.uv_texcoord * _Wind_Cuttind_A_Retuch_ST.xy + _Wind_Cuttind_A_Retuch_ST.zw;
			float4 tex2DNode1 = tex2D( _Wind_Cuttind_A_Retuch, uv_Wind_Cuttind_A_Retuch );
			float2 uv0_distortTex = i.uv_texcoord * _distortTex_ST.xy + _distortTex_ST.zw;
			float2 panner27 = ( 1.0 * _Time.y * float2( -0.25,0 ) + uv0_distortTex);
			float2 uv0_noisetex = i.uv_texcoord * _noisetex_ST.xy + _noisetex_ST.zw;
			float2 panner9 = ( 1.0 * _Time.y * float2( 0.5,0 ) + uv0_noisetex);
			float temp_output_12_0 = ( tex2DNode1.r * tex2D( _noisetex, panner9 ).r );
			float mulTime22 = _Time.y * _TimeScale;
			float2 temp_cast_0 = (mulTime22).xx;
			float4 uv2_TexCoord20 = i.uv2_tex4coord2;
			uv2_TexCoord20.xy = i.uv2_tex4coord2.xy + temp_cast_0;
			float smoothstepResult8 = smoothstep( ( uv2_TexCoord20.z + 0 ) , 1.0 , tex2DNode1.r);
			o.Emission = ( ( i.vertexColor * ( ( tex2DNode1.r * tex2D( _mask_test, ( uv0_distortTex + ( _distortamount * (-1.0 + (tex2D( _distortTex, panner27 ).r - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) ) ) ).r ) + temp_output_12_0 ) ) * smoothstepResult8 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
22;780;1702;276;3240.295;911.5663;2.456113;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;26;-2760.323,162.2099;Inherit;False;0;28;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;27;-2505.225,170.8463;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.25,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;28;-2294.577,79.73463;Inherit;True;Property;_distortTex;distortTex;5;0;Create;True;0;0;False;0;False;-1;bcaf3ccd398c2fe4c9cbec88770f74b0;e21f8c151537dd04688b751a70956cf5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;29;-1971.936,41.50247;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-2070.216,-70.65808;Inherit;False;Property;_distortamount;distortamount;6;0;Create;True;0;0;False;0;False;0.1;0.06088163;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-1792.476,-97.04136;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;24;-1990.13,-361.2603;Inherit;False;0;28;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;11;-1671.184,282.9635;Inherit;False;0;10;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;9;-1429.587,300.1535;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.5,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;32;-1659.299,-327.2826;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-2003.721,644.2228;Inherit;False;Property;_TimeScale;TimeScale;4;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;22;-1833.721,624.2228;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-1155.215,-338.1146;Inherit;True;Property;_Wind_Cuttind_A_Retuch;Wind_Cuttind_A_Retuch;1;0;Create;True;0;0;False;0;False;-1;e85ddf1d8e459d44ba8bdc0730664d96;e85ddf1d8e459d44ba8bdc0730664d96;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;16;-1266.103,-88.67616;Inherit;True;Property;_mask_test;mask_test;3;0;Create;True;0;0;False;0;False;-1;c364a4d32cf10a4408b179cc2c279810;c364a4d32cf10a4408b179cc2c279810;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;10;-1251.629,193.3362;Inherit;True;Property;_noisetex;noisetex;2;0;Create;True;0;0;False;0;False;-1;bcaf3ccd398c2fe4c9cbec88770f74b0;bcaf3ccd398c2fe4c9cbec88770f74b0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;20;-1546.162,470.5673;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-862.8232,-95.939;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-807.0792,162.8444;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;19;-1527.688,680.272;Inherit;False;Constant;_custom1;custom1;4;0;Create;True;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.VertexColorNode;3;-589.5724,-427.911;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;21;-1219.372,594.3503;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;13;-543.2613,-142.9745;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;8;-410.1806,31.10971;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;-326.1632,-334.3507;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;6;-253.9589,270.5516;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;4;-483.359,313.7517;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-715.9743,413.7399;Inherit;False;Constant;_Float0;Float 0;2;0;Create;True;0;0;False;0;False;0.03;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-20.7465,-85.43945;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;227.3703,-8.962027;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;FX_sword_main2;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;2;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Overlay;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;4;1;False;-1;1;False;-1;4;1;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;27;0;26;0
WireConnection;28;1;27;0
WireConnection;29;0;28;1
WireConnection;31;0;30;0
WireConnection;31;1;29;0
WireConnection;9;0;11;0
WireConnection;32;0;24;0
WireConnection;32;1;31;0
WireConnection;22;0;23;0
WireConnection;16;1;32;0
WireConnection;10;1;9;0
WireConnection;20;1;22;0
WireConnection;17;0;1;1
WireConnection;17;1;16;1
WireConnection;12;0;1;1
WireConnection;12;1;10;1
WireConnection;21;0;20;3
WireConnection;21;1;19;1
WireConnection;13;0;17;0
WireConnection;13;1;12;0
WireConnection;8;0;1;1
WireConnection;8;1;21;0
WireConnection;2;0;3;0
WireConnection;2;1;13;0
WireConnection;6;0;4;0
WireConnection;4;0;12;0
WireConnection;4;1;5;0
WireConnection;18;0;2;0
WireConnection;18;1;8;0
WireConnection;0;2;18;0
ASEEND*/
//CHKSM=80DDFD450986F6780DFEF652FD16F404F706D874