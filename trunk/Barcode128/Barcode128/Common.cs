using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcode128
{
    public static class Common
    {
        public const int ShiftAB = 98;
        public const int CodeC = 99;
        public const int CodeB = 100;
        public const int CodeA = 101;

        public const int StartCharA = 103;
        public const int StartCharB = 104;
        public const int StartCharC = 105;
        public const int StopChar = 106;

        public const char StartEmbedA = (char)(StartCharA + 532);
        public const char StartEmbedB = (char)(StartCharB + 532);
        public const char StartEmbedC = (char)(StartCharC + 532);
        public const char EmbedABShift = (char)(ShiftAB + 532);
        public const char EmbedCodeC = (char)(CodeC + 532);
        public const char EmbedCodeB = (char)(CodeB + 532);
        public const char EmbedCodeA = (char)(CodeA + 532);

        public static bool CharIsNumber( char c )
        {
            int unused;
            return int.TryParse( c.ToString(), out unused );
        }

        public static IEnumerable<String> SplitInParts( this String s, Int32 partLength )
        {
            if( s == null )
                throw new ArgumentNullException( "s" );
            if( partLength <= 0 )
                throw new ArgumentException( "Part length has to be positive.", "partLength" );

            for( var i = 0; i < s.Length; i += partLength )
                yield return s.Substring( i, Math.Min( partLength, s.Length - i ) );
        }
    }
}
