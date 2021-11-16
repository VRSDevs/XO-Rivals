Shader "Custom/ImageEffectAgujeroNegro"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        
        _PosPlayer("Posicion Jugador", Vector) = (200, 888, 0)
        _distVistaCompleta("distanciaVistaCompleta", float) = 200
        _distVistaMedia("distanciaVistaMedia", float) = 250
        _distorsionVistaMedia("distorsionVistaMedia", float) = 100000
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;

            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 worldPosition : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPosition = v.vertex;
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            Vector _PosPlayer;
            float _distVistaCompleta;
            float _distVistaMedia;
            float _distorsionVistaMedia;



            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

            float dist = distance(_PosPlayer, i.vertex);




    

            
            if (dist < _distVistaCompleta) {//RADIO DE VISTA COMPLETA

            }
            else if (dist < _distVistaMedia) {//RADIO DE VISTA DIFUSA
                col.rgb = col.rgb - dist*dist/ _distorsionVistaMedia;
            }
            else {//LO DEMAS NO SE VE
                col.rgb = 0;
            }

            ////VISION CONICA

            //     //ELEGIMOS RECTA QUE VER
            //float m = -1;
            //float n = _PosPlayer.y + (_PosPlayer.x * -m);


            //float Ar = m;
            //float Br = -1;
            //float Cr = n;




            //float distR = abs(Ar * i.vertex.x + Br * i.vertex.y + Cr) / sqrt(Ar * Ar + Br * Br);

            //if (distR < 10) {

            //    col.rgb = 1;

            //}
            //if(i.vertex.y == m* i.vertex.x + n)
            //    col.rgb = 0.4;




            return col;

           
               
            
            }
            ENDCG
        }
    }
}
