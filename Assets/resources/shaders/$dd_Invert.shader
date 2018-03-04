// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ddShaders/dd_Invert" {
Properties 
<<<<<<< HEAD
{
_Color ("Tint Color", Color) = (1,1,1,1)
}

SubShader 
{
Tags { "Queue"="Transparent" }

Pass
{
ZWrite On
ColorMask 0
}
Blend OneMinusDstColor OneMinusSrcAlpha //invert blending, so long as FG color is 1,1,1,1
BlendOp Add

Pass
{ 

=======
	{
		_Color ("Tint Color", Color) = (1,1,1,1)
	}
	
	SubShader 
	{
		Tags { "Queue"="Transparent" }

		Pass
		{
		   ZWrite On
		   ColorMask 0
		}
        Blend OneMinusDstColor OneMinusSrcAlpha //invert blending, so long as FG color is 1,1,1,1
        BlendOp Add
        
        Pass
		{ 
		
>>>>>>> aed9a9175e408d39684ccd986bf34949781769fb
CGPROGRAM
#pragma vertex vert
#pragma fragment frag 
uniform float4 _Color;

struct vertexInput
{
<<<<<<< HEAD
float4 vertex: POSITION;
float4 color : COLOR;	
=======
	float4 vertex: POSITION;
    float4 color : COLOR;	
>>>>>>> aed9a9175e408d39684ccd986bf34949781769fb
};

struct fragmentInput
{
<<<<<<< HEAD
float4 pos : SV_POSITION;
float4 color : COLOR0; 
=======
	float4 pos : SV_POSITION;
	float4 color : COLOR0; 
>>>>>>> aed9a9175e408d39684ccd986bf34949781769fb
};

fragmentInput vert( vertexInput i )
{
<<<<<<< HEAD
fragmentInput o;
o.pos = UnityObjectToClipPos(i.vertex);
o.color = _Color;
return o;
=======
	fragmentInput o;
	o.pos = UnityObjectToClipPos(i.vertex);
	o.color = _Color;
	return o;
>>>>>>> aed9a9175e408d39684ccd986bf34949781769fb
}

half4 frag( fragmentInput i ) : COLOR
{
<<<<<<< HEAD
return i.color;
=======
	return i.color;
>>>>>>> aed9a9175e408d39684ccd986bf34949781769fb
}

ENDCG
}
}
}

