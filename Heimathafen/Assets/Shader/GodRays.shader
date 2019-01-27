Shader "Heimathafen/GodRays"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Intensity ("Godray Intensity", float) = 1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
			#define TAU 6.28318530718
			#define MAX_ITER 5


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float4 screenPos : TEXCOORD1;

            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
			float _Intensity;

			// perf increase for god ray, eliminates Y
float causticX(float x, float power, float gtime)
{
    float p = fmod(x*TAU, TAU)-250.0;
    float time = gtime * .5+23.0;

	float i = p;;
	float c = 1.0;
	float inten = .005;

	for (int n = 0; n < MAX_ITER/2; n++) 
	{
		float t = time * (1.0 - (3.5 / float(n+1)));
		i = p + cos(t - i) + sin(t + i);
		c += 1.0/length(p / (sin(i+t)/inten));
	}
	c /= float(MAX_ITER);
	c = 1.17-pow(c, power);
    
    return c;
}


float GodRays(fixed2 uv)
{
    float light = 0.0;

    light += pow(causticX((uv.x+0.08*uv.y)/1.7+0.5, 1.8, _Time*2),10.0)*0.05;
    light-=pow((1.0-uv.y)*0.3,2.0)*0.2;
    light += pow(causticX(sin(uv.x), 0.3,_Time*3),9.0)*0.4; 
    light += pow(causticX(cos(uv.x*2.3), 0.3,_Time*5),4.0)*0.1;  
        
    light-=pow((1.0-uv.y)*0.3,3.0);
    light=clamp(light,0.0,1.0);
    
    return light;
}

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
				col += GodRays(i.uv) * _Intensity * 2.0 /**lerp(float(0.01),1.0,i.vertex.y*i.vertex.y)*//**fixed4(0.7,1.0,1.0,0)*/;
                return col;
            }
            ENDCG
        }
    }
}
