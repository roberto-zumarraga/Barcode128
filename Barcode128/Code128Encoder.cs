using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcode128
{
    public static class Code128Encoder
    {
        static int CurrentType = Common._StartCharB;

        //Ascii value - 32 = Code128 index
        public static readonly List<string> Code128 = new List<string>
        {
            "11011001100",
            "11001101100",
            "11001100110",
            "10010011000",
            "10010001100",
            "10001001100",
            "10011001000",
            "10011000100",
            "10001100100",
            "11001001000",
            "11001000100",
            "11000100100",
            "10110011100",
            "10011011100",
            "10011001110",
            "10111001100",
            "10011101100",
            "10011100110",
            "11001110010",
            "11001011100",
            "11001001110",
            "11011100100",
            "11001110100",
            "11101101110",
            "11101001100",
            "11100101100",
            "11100100110",
            "11101100100",
            "11100110100",
            "11100110010",
            "11011011000",
            "11011000110",
            "11000110110",
            "10100011000",
            "10001011000",
            "10001000110",
            "10110001000",
            "10001101000",
            "10001100010",
            "11010001000",
            "11000101000",
            "11000100010",
            "10110111000",
            "10110001110",
            "10001101110",
            "10111011000",
            "10111000110",
            "10001110110",
            "11101110110",
            "11010001110",
            "11000101110",
            "11011101000",
            "11011100010",
            "11011101110",
            "11101011000",
            "11101000110",
            "11100010110",
            "11101101000",
            "11101100010",
            "11100011010",
            "11101111010",
            "11001000010",
            "11110001010",
            "10100110000",
            "10100001100",
            "10010110000",
            "10010000110",
            "10000101100",
            "10000100110",
            "10110010000",
            "10110000100",
            "10011010000",
            "10011000010",
            "10000110100",
            "10000110010",
            "11000010010",
            "11001010000",
            "11110111010",
            "11000010100",
            "10001111010",
            "10100111100",
            "10010111100",
            "10010011110",
            "10111100100",
            "10011110100",
            "10011110010",
            "11110100100",
            "11110010100",
            "11110010010",
            "11011011110",
            "11011110110",
            "11110110110",
            "10101111000",
            "10100011110",
            "10001011110",
            "10111101000",
            "10111100010",
            "11110101000",
            "11110100010",
            "10111011110",
            "10111101110",
            "11101011110",
            "11110101110",
            "11010000100",
            "11010010000",
            "11010011100",
            "1100011101011",
        };

        public static string GetCodeForText( string Text )
        {
            string result = string.Empty;

            for( int i = 0; i < Text.Length; ++i )
            {
                result += GetPatternForText( Text, ref i );
            }

            int CheckSum = GetCheckSum( result );

            return result + Code128[CheckSum] + Code128[Common.StopChar];
        }

        public static int GetCheckSum( string EncodedText )
        {
            var TextSplit = Common.SplitInParts( EncodedText, 11 ).ToList();
            int Sum = Code128.IndexOf( TextSplit[0] );

            for( int i = 1; i < TextSplit.Count(); ++i )
            {
                Sum += Code128.IndexOf( TextSplit[i] ) * i;
            }

            return (Sum % 103);
        }

        private static string GetPatternForText( string Text, ref int i )
        {
            if( CharIsCodeType( Text[i] ) )
            {
                SetCurrentType( Text[i] );
                return GetCodePattern( Text[i] );
            }

            if( CurrentType != Common._CodeC && CurrentType != Common._StartCharC )
                return GetCodeForCharacter( Text[i] );
            
            return GetCodeForDigits( Text, ref i );
        }

        public static bool CharIsCodeType( char c )
        {
            return (int)c > 532;
        }

        public static void SetCurrentType( char Character )
        {
            int value = (int)Character;
            if( value > 500 )
                CurrentType = value - 532;
        }

        public static string GetCodePattern( char Code )
        {
            return Code128[(int)Code - 532];
        }

        public static string GetCodeForCharacter( char c )
        {
            int index = GetCharValue( c );
            if( ValueOutOfBounds( index ) )
                throw new Exception( "Barcode text is invalid" );

            return Code128[index];
        }

        public static int GetCharValue( char Character )
        {
            int value = (int)Character;

            if( value >= 32 ) return value - 32;
            else return value + 64;
        }

        public static bool ValueOutOfBounds( int Value )
        {
            return Value < 0 || Value > Common.StopChar;
        }

        public static string GetCodeForDigits( string Text, ref int i )
        {
            if( !Common.CharIsNumber( Text[i] ) )
                throw new Exception( "Invalid character" );

            if( i + 1 >= Text.Length || !Common.CharIsNumber( Text[i + 1] ) )
                return Code128[int.Parse( Text[i].ToString() )];

            int Num = int.Parse( Text.Substring( i, 2 ) );
            ++i;
            return Code128[Num];
        }
    }
}
