using System;
using System.IO;
using System.Collections.Generic;

namespace com.vge.esri
{
    public class Reader
    {
        private string readFilePath = string.Empty;
        private List<object> objectList = new List<object>();
        private int startReadLocation;
        private int interval;
        private int repetition;
        FileStream fs;
        BinaryReader autoReader;
        byte[] bytes;
        object dataItem;
        public void ReadFile(string filePath)
        {
            this.readFilePath = filePath;
            fs = new FileStream(readFilePath, FileMode.Open, FileAccess.Read);
            autoReader = new BinaryReader(fs);
        }
        public List<object> dataItemRead()
        {
            startReadLocation = 0;
            repetition = 1;
            while (repetition > 0)
            {
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(4);
                Array.Reverse(bytes);
                dataItem = BitConverter.ToInt32(bytes, 0);
                objectList.Add(dataItem);
                startReadLocation += 0;
                repetition -= 1;
            }
            startReadLocation = 4;
            repetition = 5;
            while (repetition > 0)
            {
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(4);
                Array.Reverse(bytes);
                dataItem = BitConverter.ToInt32(bytes, 0);
                objectList.Add(dataItem);
                startReadLocation += 4;
                repetition -= 1;
            }
            startReadLocation = 24;
            repetition = 1;
            while (repetition > 0)
            {
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(4);
                Array.Reverse(bytes);
                dataItem = BitConverter.ToInt32(bytes, 0);
                objectList.Add(dataItem);
                startReadLocation += 0;
                repetition -= 1;
            }
            startReadLocation = 28;
            repetition = 1;
            while (repetition > 0)
            {
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(4);
                dataItem = BitConverter.ToInt32(bytes, 0);
                objectList.Add(dataItem);
                startReadLocation += 0;
                repetition -= 1;
            }
            startReadLocation = 32;
            repetition = 1;
            while (repetition > 0)
            {
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(4);
                dataItem = BitConverter.ToInt32(bytes, 0);
                objectList.Add(dataItem);
                startReadLocation += 0;
                repetition -= 1;
            }
            startReadLocation = 36;
            repetition = 4;
            while (repetition > 0)
            {
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(8);
                dataItem = BitConverter.ToDouble(bytes, 0);
                objectList.Add(dataItem);
                startReadLocation += 8;
                repetition -= 1;
            }
            startReadLocation = 68;
            repetition = 1;
            while (repetition > 0)
            {
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(4);
                dataItem = BitConverter.ToInt32(bytes, 0);
                objectList.Add(dataItem);
                startReadLocation += 0;
                repetition -= 1;
            }
            startReadLocation = 72;
            repetition = 1;
            while (repetition > 0)
            {
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(4);
                dataItem = BitConverter.ToInt32(bytes, 0);
                objectList.Add(dataItem);
                startReadLocation += 0;
                repetition -= 1;
            }
            startReadLocation = 76;
            repetition = ((76 + 4 * NumParts) - (76)) / 4;
            while (repetition > 0)
            {
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(4);
                dataItem = BitConverter.ToInt32(bytes, 0);
                objectList.Add(dataItem);
                startReadLocation += 4;
                repetition -= 1;
            }
            startReadLocation = 76 + 4 * NumParts + 0;
            repetition = -1;
            while (startReadLocation < autoReader.BaseStream.Length)
            {
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(4);
                dataItem = BitConverter.ToInt32(bytes, 0);
                objectList.Add(dataItem);
                startReadLocation += 4;
                repetition -= 1;
            }
            return objectList;
        }
    }
}
