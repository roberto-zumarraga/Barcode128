# Barcode128
A simple Code-128 barcode image generator

This is a basic Code-128 barcode image generator which will take a string of characters ascii character and convert it to a barcode image. I wrote this quickly to generate smaller barcodes because code-39 barcodes take way too much space!

# Installation

This is meant to be referenced within your c# project, I won't bother creating a nuget package of this since it's probably not that useful for most people, and the project itself is small so it should be easy to reference directly in your source if you really need to

# Usage

You can convert text strings to a barcode image using the GetBarcode method like so:

    //ctor is defined as follows:
    //Barcode128( int Height, int Weight, bool ShowFooting = true )

    //Height is how much vertical space it takes, you can get away with a pretty small size
    //Weight is how thick each line is, just sticking with a small number works best here
    //Also has a showfooter property, but this isn't currently working

    string MyText = "abc12345";
    Barcode128 gen = new Barcode128( 64, 1 )
    var img = gen.GetBarcode( MyText )

This barcode generator makes no effort to switch between the different code formats, (i.e: A, B, or C), and so consequently requires you manually inserting the format flags into your text before pass it into the generator. The code flags are defined in Common.cs and can be used like so

    //Code Embedding const strings for usage, defined in Barcode128.Common
    //StartEmbed* goes at the beginning of the text, all others go in between the text to represent a format switch
    public const char StartEmbedA;
    public const char StartEmbedB;
    public const char StartEmbedC;
    public const char EmbedABShift;
    public const char EmbedCodeC;
    public const char EmbedCodeB;
    public const char EmbedCodeA;

    string EmbedCodeFlags( string text )
    {
        return Common.StartEmbedA + text.substring(0,3) + Common.EmbedCodeC + text.substring(3);
    }

    void GetMyBarcodeImage( string text )
    {
        string EncodedText = EmbedCodeFlags( text );
        Barcode128 gen = new Barcode128( 64, 1 )
        var img = gen.GetBarcode( MyText )

        //do something with the image (print, show, etc)...
    }