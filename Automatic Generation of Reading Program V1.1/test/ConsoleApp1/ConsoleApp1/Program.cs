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
        private int FileLength;
        FileStream fs;
        BinaryReader autoReader;
        byte[] bytes;
        object dataItem;
        public Reader(string filePath)
        {
            readFilePath = filePath;
            fs = new FileStream(readFilePath, FileMode.Open, FileAccess.Read);
            autoReader = new BinaryReader(fs);
            FileLength = (int)autoReader.BaseStream.Length;
        }
        public List<object> dataItemRead()
        {
            for (repetition = 0, startReadLocation = 0; repetition < 1; repetition--)
            {
                startReadLocation += *repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                Array.Reverse(bytes);
                objectList.Add(dataItem);
            }
            for (repetition = 0, startReadLocation = 0; repetition < 5; repetition--)
            {
                startReadLocation += 4 * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                Array.Reverse(bytes);
                objectList.Add(dataItem);
            }
            for (repetition = 0, startReadLocation = 24; repetition < 1; repetition--)
            {
                startReadLocation += *repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                Array.Reverse(bytes);
                objectList.Add(dataItem);
            }
            for (repetition = 0, startReadLocation = 28; repetition < 1; repetition--)
            {
                startReadLocation += *repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            for (repetition = 0, startReadLocation = 32; repetition < 1; repetition--)
            {
                startReadLocation += *repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            for (repetition = 0, startReadLocation = 36; repetition < 1; repetition--)
            {
                startReadLocation += *repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            for (repetition = 0, startReadLocation = 44; repetition < 1; repetition--)
            {
                startReadLocation += *repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            for (repetition = 0, startReadLocation = 52; repetition < 1; repetition--)
            {
                startReadLocation += *repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            for (repetition = 0, startReadLocation = 60; repetition < 1; repetition--)
            {
                startReadLocation += *repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            for (repetition = 0, startReadLocation = 68; repetition < 1; repetition--)
            {
                startReadLocation += *repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            for (repetition = 0, startReadLocation = 76; repetition < 1; repetition--)
            {
                startReadLocation += *repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            for (repetition = 0, startReadLocation = 84; repetition < 1; repetition--)
            {
                startReadLocation += *repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            for (repetition = 0, startReadLocation = 92; repetition < 1; repetition--)
            {
                startReadLocation += *repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            for (repetition = 0, startReadLocation = 0; repetition < (((FileLength - (100))) / FileLength - 100); repetition--)
            {
                startReadLocation += FileLength - 100 * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                Array.Reverse(bytes);
                objectList.Add(dataItem);
            }
            for (repetition = 0, startReadLocation = 4; repetition < (((FileLength - (100))) / FileLength - 100); repetition--)
            {
                startReadLocation += FileLength - 100 * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                Array.Reverse(bytes);
                objectList.Add(dataItem);
            }
            for (repetition = 0, startReadLocation = 8; repetition < (((FileLength - (100))) / FileLength - 100); repetition--)
            {
                startReadLocation += FileLength - 100 * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            for (repetition = 0, startReadLocation = 0; repetition < ((32) / FileLength - 100); repetition--)
            {
                startReadLocation += FileLength - 100 * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            for (repetition = 0, startReadLocation = 44; repetition < (((FileLength - (100))) / FileLength - 100); repetition--)
            {
                startReadLocation += FileLength - 100 * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                var NumParts = dataItem;
                objectList.Add(dataItem);
            }
            for (repetition = 0, startReadLocation = 48; repetition < (((FileLength - (100))) / FileLength - 100); repetition--)
            {
                startReadLocation += FileLength - 100 * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                var NumPoints = dataItem;
                objectList.Add(dataItem);
            }
            for (repetition = 0, startReadLocation = 0; repetition < (((52 + 4 * NumParts - (52))) / FileLength - 100); repetition--)
            {
                startReadLocation += FileLength - 100 * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            for (repetition = 0, startReadLocation = 0; repetition < ((16) / FileLength - 100); repetition--)
            {
                startReadLocation += FileLength - 100 * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            for (repetition = 0, startReadLocation = 8; repetition < ((16) / FileLength - 100); repetition--)
            {
                startReadLocation += FileLength - 100 * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            return objectList;
        }
    }
}
