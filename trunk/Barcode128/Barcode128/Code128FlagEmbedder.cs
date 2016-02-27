using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcode128
{
    public static class Code128FlagEmbedder
    {
        public static string EmbedCodeFlags( string Text )
        {
            string result = GetStartCodeType( Text ).ToString();

            int ACount = 0;
            int BCount = 0;
            int CCount = 0;
            int APos = -1;
            int BPos = -1;
            int CPos = -1;

            for( int i = 0; i < Text.Length; ++i )
            {
                var CharType = GetCharValueType( Text[i] );

                if( CharType == "C" )
                {
                    ++CCount;
                    CPos = i;
                }
                else if( CharType == "B" )
                {
                    ++BCount;
                    BPos = i;
                }
                else
                {
                    ++ACount;
                    APos = i;
                }
            }

            return result;
        }

        private static char GetStartCodeType( string Text )
        {
            int ACount = 0;
            int BCount = 0;

            if( ShouldUseStartCodeC( Text ) ) return Common.StartEmbedC;

            foreach( char c in Text )
            {
                if( GetCharValueType( c ) == "B" ) ++BCount;
                else ++ACount;

                if( ACount >= 3 || BCount >= 3 ) break;
            }

            if( BCount > ACount ) return Common.StartEmbedB;
            else return Common.StartEmbedA;
        }

        public static bool ShouldUseStartCodeC( string Text )
        {
            int CCount = 0;

            for( int i = 0; i < Text.Length; ++i )
            {
                if( !Common.CharIsNumber( Text[i] ) ) break;
                ++CCount;
            }

            if( CCount == Text.Length || CCount >= 4 ) return true;
            return false;
        }

        public static bool ShouldSwitchToC( string Text )
        {
            int CCount = 0;

            for( int i = 0; i < Text.Length; ++i )
            {
                if( !Common.CharIsNumber( Text[i] ) ) CCount = 0;
                else ++CCount;

                //if( CCount >= 6 )
            }

            return false;
        }

        public static string GetCharValueType( char Character )
        {
            int value = (int)Character;
            if( value < 0 ) throw new Exception( "Invalid character type" );


            if( Common.CharIsNumber( Character ) ) return "C";
            if( value >= 32 ) return "B";
            else return "A";
        }
    }
}
