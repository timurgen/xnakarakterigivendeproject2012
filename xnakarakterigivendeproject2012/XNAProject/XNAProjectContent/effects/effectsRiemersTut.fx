//------------------------------------------------------
//--                                                  --
//--           www.riemers.net                    --
//--               Basic shaders                     --
//--        Use/modify as you like                --
//--                                                  --
//------------------------------------------------------

struct VertexToPixel
{
    float4 Position       : POSITION;    
    float2 TexCoords: TEXCOORD0;
	float3 Normal        : TEXCOORD1;
	float3 Position3D    : TEXCOORD2;
};

struct PixelToFrame
{
    float4 Color : COLOR0;
};

float DotProduct(float3 lightPos, float3 pos3D, float3 normal)
{
    float3 lightDir = normalize(pos3D - lightPos);
    return dot(-lightDir, normal);    
}


//------- Constants --------
float4x4 xView;
float4x4 xProjection;
float4x4 xWorld;
float xAmbient;
float3 xLightPos;
float xLightPower;
bool isEmissive;
float4 xEmissiveColor;
Texture xTexture;
//------- Samplers --------


sampler TextureSampler = sampler_state 
{ 
	texture = <xTexture>; 
	magfilter = LINEAR; 
	minfilter = LINEAR; 
	mipfilter=LINEAR; 
	AddressU = mirror; 
	AddressV = mirror;
};

//------- Technique: Textured --------

VertexToPixel TexturedVS( float4 inPos : POSITION, float3 inNormal: NORMAL, float2 inTexCoords: TEXCOORD0)
{    
    VertexToPixel Output = (VertexToPixel)0;

    float4x4 preViewProjection = mul (xView, xProjection);
    float4x4 preWorldViewProjection = mul (xWorld, preViewProjection);
    Output.Position = mul(inPos, preWorldViewProjection);    
    Output.TexCoords = inTexCoords;
    
    //float3 Normal = normalize(mul(normalize(inNormal), xWorld));
	Output.Normal = normalize(mul(inNormal, (float3x3)xWorld));   
	Output.Position3D = mul(inPos, xWorld);
    
    return Output;    
}

PixelToFrame TexturedPS(VertexToPixel PSIn)
{
    PixelToFrame Output = (PixelToFrame)0;        

	float diffuseLightingFactor = DotProduct(xLightPos, PSIn.Position3D, PSIn.Normal);
	diffuseLightingFactor = saturate(diffuseLightingFactor);
    diffuseLightingFactor *= xLightPower;

    PSIn.TexCoords.y--;
    float4 baseColor = tex2D(TextureSampler, PSIn.TexCoords);
    Output.Color = baseColor*(diffuseLightingFactor + xAmbient);
	if(isEmissive)
		Output.Color = baseColor*(diffuseLightingFactor*xEmissiveColor + xAmbient );
    return Output;
		
}

technique Textured
{
    pass Pass0
    {   
        VertexShader = compile vs_2_0 TexturedVS();
        PixelShader  = compile ps_2_0 TexturedPS();
    }
}
