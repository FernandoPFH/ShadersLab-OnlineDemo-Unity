#ifndef GAUSSIANBLUR_INCLUDED
#define GAUSSIANBLUR_INCLUDED
#define E 2.71828f

TEXTURE2D(_CameraOpaqueTexture);
SAMPLER(sampler_CameraOpaqueTexture);
float4 _CameraOpaqueTexture_TexelSize;

float Gaussian(float spread,int x)
{
    float sigmaSqu = spread * spread;
    return (1 / sqrt(TWO_PI * sigmaSqu)) * pow(E, -(x * x) / (2 * sigmaSqu));
}

void GaussianBlur_float(float2 UV, int GridSize , float Spread, out float3 Out) {
    Out = float3(0,0,0);
    float GridSum = 0;

    int upper = ((GridSize - 1) / 2);
    int lower = -upper;

    for (int x = lower; x <= upper; ++x)
    {
    	float gauss = Gaussian(Spread,x);
	GridSum += gauss;
	float2 uv = UV + float2(_CameraOpaqueTexture_TexelSize.x * x, 0);
	Out += gauss * SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, uv).rgb;
    }

    for (int y = lower; y <= upper; ++y)
    {
    	float gauss = Gaussian(Spread,y);
	GridSum += gauss;
	float2 uv = UV + float2(0, _CameraOpaqueTexture_TexelSize.y * y);
	Out += gauss * SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, uv).rgb;
    }

    Out /= GridSum;
}

#endif
