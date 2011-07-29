using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleCompression
{
    internal class FileResourceCollection : List<FileResource>
    {
        public override bool Equals(object obj)
        {
            var otherObj = obj as FileResourceCollection;
            if (otherObj == null)
            {
                return false;
            }
            var isEqual = this.Count == otherObj.Count && this.All(y => otherObj.Contains(y));
            return isEqual;
        }

        public override int GetHashCode()
        {
            var combinedString = this.Aggregate(string.Empty, (x, y) => x + y.FilePath);
            return Math.Abs(combinedString.GetHashCode());
        }
    }
}
