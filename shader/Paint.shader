
shader "Sbin/Cook-Torrance"

{

 properties{

  _Color ("Main Color", Color) = (1,1,1,1)

  _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}

  _BumpMap ("Normalmap", 2D) = "bump" {}

  _Fo("F0",range(0,1))=0.3

  _Dm("Dm",range(0,1))=0.3

  _ISpecular("ISpecular",range(0,0.3))=0.03

 }

 

 subshader

 {

  tags{"rendertype"="opaque"}

  CGPROGRAM

  #pragma surface surf Cook_Torrance

  #pragma target 3.0

  sampler2D _MainTex;

  sampler2D _BumpMap;

  fixed4 _Color;

  float _Fo;

  float _Dm;

  float _ISpecular;

  

  half4 LightingCook_Torrance(SurfaceOutput s, fixed3 lightDir, fixed3 viewDir, fixed atten)

  {  

   //calculate diffuse color

   float kd= saturate(dot( normalize(s.Normal),normalize(lightDir)));

   float4 diffuseColor;

   diffuseColor.rgb= kd  * s.Albedo * _LightColor0*  (atten *2);

   diffuseColor.a=1;

   

   //calculate specular color use Cook-Torrance lighting Model

   float3 V= normalize(viewDir);

   float3 L= normalize(lightDir);

   float3 N= normalize(s.Normal);

   float3 H= V+L;

   float Ks= saturate(dot(H,N));

    float F= _Fo+(1-_Fo)*(pow(1-dot(V,H),5));  

    float P=pow(Ks,2);

    float powDm=_Dm*_Dm;    

    float NdotH=Ks;

    float VdotH=saturate(dot(V,H));    

    float D= ( 1.0f/( powDm* pow(NdotH,4)) ) * exp( (P-1)/ ( powDm * P) );    

    float G1=( NdotH * saturate(dot(N,L)) *2 ) /  VdotH;

    float G2 = ( NdotH * saturate(dot(N,V)) *2 ) /  VdotH;

    float G= min(min(1,G1),G2);

        float4 cook_Torrance_Color= _LightColor0* ( F*D*G / (dot(N,V)* dot(N,L)) ) * _ISpecular * saturate(dot(N,L))* atten;

    half4 c;

   c= diffuseColor + cook_Torrance_Color ;

   c.a= 1;

   return c; 

    }

    struct Input {

   float2 uv_MainTex;

   float2 uv_BumpMap;

  };

  void surf(Input IN,inout SurfaceOutput o)

  {

   fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);

   o.Albedo = _Color.rgb * tex.rgb ;

   o.Gloss = tex.a;

   o.Alpha = tex.a * _Color.a;

   o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

  }

  

  ENDCG

 }

 Fallback "Diffuse"

}
