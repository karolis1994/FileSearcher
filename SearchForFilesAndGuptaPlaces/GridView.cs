using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchForFilesAndGuptaPlaces
{
    public class GridView
    {
        public String GuptaObjectName { get; set; }
        public String GuptaClassName { get; set; }
        public String SearchedText { get; set; }
        public String ResultText { get; set; }
        public String FileName { get; set; }
        public String FilePath { get; set; }

        public Boolean IsGuptaFile { get; set; }

        public Int32 RowNumber { get; set; }
        public Int32 Id { get; set; }
    }
}
