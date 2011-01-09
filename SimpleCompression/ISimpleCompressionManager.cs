using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleCompression
{
    public interface ISimpleCompressionManager : ICombine, ICompress
    {
        string CombineAndCompressFiles(params string[] files);
    }
}
