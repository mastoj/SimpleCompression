using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleCompression
{
    public interface ICompress
    {
        string CompressJavascriptString(string javascript);

        string CompressCssString(string cssString);
    }
}
