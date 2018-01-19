using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcode128
{
    public class Barcode128
    {
        int _Height = 64;
        int _Weight = 1;
        bool _ShowFooter = true;

        public Barcode128( int Height, int Weight, bool ShowFooter = true )
        {
            if( Height < 0 ) throw new Exception( "Invalid barcode height" );
            if( Weight < 0 || Weight > 4 ) throw new Exception( "Invalid barcode weight" );

            _Height = Height;
            _Weight = Weight;
            _ShowFooter = ShowFooter;
        }

        public Image GetBarcode( string Text )
        {
            string Code = Code128Encoder.GetCodeForText( Text );
            return DrawBarCode( Code );
        }

        private Image DrawBarCode( string Code )
        {
            Bitmap Surface = new Bitmap( (Code.Length * _Weight ) + 64, _Height + 16 );
            Graphics g = Graphics.FromImage( Surface );
            int x = 0;

            foreach( char c in Code )
            {
                Brush Bar = c == '1' ? Brushes.Black : Brushes.White;
                g.FillRectangle( Bar, x, 0, _Weight, _Height );
                x += _Weight;
            }

            return Surface;
        }

        private Font GetDefaultFont()
        {
            return new Font( FontFamily.GenericSansSerif, 8, FontStyle.Regular );
        }
    }
}
