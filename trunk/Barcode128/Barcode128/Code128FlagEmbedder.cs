using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcode128
{
    public static class Code128FlagEmbedder
    {
        public static string EmbedCodeFlags( string text )
        {
            var ControlCodes = GetControlCodeMarkers( text );
            foreach( var c in ControlCodes.Reverse() )
                text = text.Insert( c.Key, ((char)(c.Value)).ToString() );

            if( text.StartsWith( Common.EmbedCodeB.ToString() ) ) text = Common.StartEmbedB + text.TrimStart( Common.EmbedCodeB.ToString().ToCharArray() );
            if( text.StartsWith( Common.EmbedCodeC.ToString() ) ) text = Common.StartEmbedC + text.TrimStart( Common.EmbedCodeC.ToString().ToCharArray() );

            return text;
        }

        private static Dictionary<int, int> GetControlCodeMarkers( string text )
        {
            int numcount = 0;
            int CurrentCode = 0;
            Dictionary<int, int> result = new Dictionary<int, int>();

            if( TextIsAllLettersOrDigits( text ) ) return TextIsPure( text );

            string FirstFour = text.Substring( 0, 4 );
            if( FirstFour.All( char.IsDigit ) )
            {
                result.Add( 0, Common.StartEmbedC );
                CurrentCode = Common.EmbedCodeC;
            }

            for( int i = 0; i < text.Length; ++i )
            {
                if( char.IsDigit( text[i] ) ) ++numcount;
                else if( char.IsLetter( text[i] ) )
                {
                    if( numcount >= 6 )
                    {
                        if( CurrentCode != Common.EmbedCodeC )
                        {
                            CurrentCode = Common.EmbedCodeC;
                            result.Add( (i + 1) - (1 + numcount - (numcount % 2)), CurrentCode );
                        }
                    }

                    if( CurrentCode != Common.EmbedCodeB )
                    {
                        CurrentCode = Common.EmbedCodeB;
                        result.Add( (i + 1) - 1, CurrentCode );
                    }
                    numcount = 0;
                }
            }

            if( numcount >= 4 && CurrentCode != Common.EmbedCodeC )
                result.Add( text.Length - (numcount - (numcount % 2)), Common.EmbedCodeC );

            return result;
        }

        private static bool TextIsAllLettersOrDigits( string text )
        {
            return text.All( char.IsDigit ) || text.All( char.IsLetter );
        }

        private static Dictionary<int, int> TextIsPure( string text )
        {
            Dictionary<int, int> result = new Dictionary<int, int>();

            if( text.All( char.IsDigit ) )
            {
                if( text.Length % 2 == 0 )
                    result.Add( 0, Common.StartEmbedC );
                else
                {
                    result.Add( 0, Common.StartEmbedC );
                    result.Add( text.Length - 1, Common.EmbedCodeB );
                }
            }

            if( text.All( char.IsLetter ) ) result.Add( 0, Common.StartEmbedB );
            return result;
        }
    }
}
