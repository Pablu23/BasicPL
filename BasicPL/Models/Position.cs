using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicPL.Models
{
    public record struct Position
    {
        public int Index;
        public int LineNum;
        public int ColumnNum;
        public string FileName;
        public string FileText;

        public Position(int index, int lineNum, int columnNum, string fileName, string fileText)
        {
            Index = index;
            LineNum = lineNum;
            ColumnNum = columnNum;
            FileName = fileName;
            FileText = fileText;
        }

        public void Advance(char? currentChar = null)
        {
            Index++;
            ColumnNum++;

            if (currentChar != null && currentChar == '\n')
            {
                LineNum++;
                ColumnNum = 0;
            }
        }
    }
}
