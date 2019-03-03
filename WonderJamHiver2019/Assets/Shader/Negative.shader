Shader "SpriteEffect/Negative"
{
    Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Team("Team", Int) = 0 
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="TransparentCutout"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        // No culling or depth
        Cull Off ZWrite Off

		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
			int _Team;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
				if(_Team == 1)
					col.r = (col.r + col.g + col.b)/2;

				if(_Team == 2)
					col.g = (col.r + col.g + col.b) / 2;

                return col;
            }
            ENDCG
        }
    }

    Fallback "Sprites/Default"
}
