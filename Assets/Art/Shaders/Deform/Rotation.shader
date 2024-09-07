// Upgrade NOTE: commented out 'float3 _WorldSpaceCameraPos', a built-in variable


//MIT License

//Copyright(c) 2019 Shahriar Shahrabi
//Copyright(c) 2019 realities.io inc

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

Shader "Unlit/Rotation"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
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
				fixed4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			// float3 _WorldSpaceCameraPos;

			float4x4 r;

			v2f vert (appdata v)
			{
				v2f o;
				float4 mw = mul(unity_ObjectToWorld, v.vertex);

				float3 dZV = mw.xyz - _WorldSpaceCameraPos;
				float3 viewDir = UNITY_MATRIX_IT_MV[2].xyz;

		/*		float dz = dot(dZV, viewDir)*60.;*/
				float dis = distance(_WorldSpaceCameraPos, mw.xyz);
		
		

				
				float4 mv = mul(UNITY_MATRIX_V, mw);


				float dz = sin(_Time.x*20 + mw.z*0.2)*0.6 /**(1.-v.color)*/;
				//dz *= 4.0; dz = floor(dz);
				//dz /= 4.;
				r = float4x4(cos( dz), sin( dz), 0., 0.,
					-sin( dz), cos( dz), 0., 0.,
					0., 0., 1.0, 0.,
					0., 0., 0., 1.0);

				mv = mul(r, mv);
				float4 m = mul(UNITY_MATRIX_P, mv);

		

				//m.w += -abs(sin(_Time*40.0 + ( 1./ mw.y +1./mw.x)*0.3))*min(1.0,lerp(0., 1.0, smoothstep(0.,20.,dis)));
				//m.w += -abs(dz)*3.;
				o.vertex = m;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);




				
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
