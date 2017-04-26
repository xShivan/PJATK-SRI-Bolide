using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sri.Bolid.Logger
{
    public class TextLogger
    {
        private readonly string logFile;

        public TextLogger(string logFile)
        {
            this.logFile = logFile;
        }

        public void AppendLine(string text)
        {
            if (!File.Exists(this.logFile))
                File.Create(this.logFile);

            string existingContent = File.ReadAllText(this.logFile);
            existingContent += "\n" + text;
            File.WriteAllText(this.logFile, existingContent);
        }
    }
}
