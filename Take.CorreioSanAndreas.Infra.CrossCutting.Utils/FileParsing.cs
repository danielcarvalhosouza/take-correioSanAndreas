using System;
using System.IO;
using System.Text;

namespace Take.CorreioSanAndreas.Infra.CrossCutting.Utils
{
    public class FileParsing
    {
        private string[] _lines;
        private int _lineNumber = 0;


        public FileParsing(byte[] bytes)
        {
            try
            {
                var text = Encoding.ASCII.GetString(bytes);
                _lines = text.Split(Environment.NewLine);
            }
            catch
            {
                throw new Exception("Arquivo inválido");
            }
        }

        public string ReadNextLine()
        {
            if(_lineNumber >= _lines.Length)
            {
                _lineNumber = 0;
            }
            var line = _lines[_lineNumber];
            _lineNumber++;
            return line;
        }
    }
}
