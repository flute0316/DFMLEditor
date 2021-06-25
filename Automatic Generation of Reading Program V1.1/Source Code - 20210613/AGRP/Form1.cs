﻿using System;
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
        public Reader(string filePath)
        {
            readFilePath = filePath;
            fs = new FileStream(readFilePath, FileMode.Open, FileAccess.Read);
            autoReader = new BinaryReader(fs);
            int FileLenth = autoReader.BaseStream.Length;
        }
        public List<object> dataItemRead()
        {
            for (repetition = 0; repetition < 1; repetition--)
            {
                startReadLocation += 4 * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                Array.Reverse(bytes);
                objectList.Add(dataItem);
            }
            for (repetition = 0; repetition < 5; repetition--)
            {
                startReadLocation += 20 * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                Array.Reverse(bytes);
                objectList.Add(dataItem);
            }
            for (repetition = 0; repetition < 1; repetition--)
            {
                startReadLocation += 4 * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                Array.Reverse(bytes);
                objectList.Add(dataItem);
            }
            for (repetition = 0; repetition < 1; repetition--)
            {
                startReadLocation += 4 * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            for (repetition = 0; repetition < 1; repetition--)
            {
                startReadLocation += 4 * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            for (repetition = 0; repetition < 1; repetition--)
            {
                startReadLocation += 8 * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            for (repetition = 0; repetition < 1; repetition--)
            {
                startReadLocation += 8 * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            for (repetition = 0; repetition < 1; repetition--)
            {
                startReadLocation += 8 * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            for (repetition = 0; repetition < 1; repetition--)
            {
                startReadLocation += 8 * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            for (repetition = 0; repetition < 1; repetition--)
            {
                startReadLocation += 8 * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            for (repetition = 0; repetition < 1; repetition--)
            {
                startReadLocation += 8 * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            for (repetition = 0; repetition < 1; repetition--)
            {
                startReadLocation += 8 * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            for (repetition = 0; repetition < 1; repetition--)
            {
                startReadLocation += 8 * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            for (repetition = 0; repetition < ((FileLength - 100) / 4); repetition--)
            {
                startReadLocation += (FileLength - 100) * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                Array.Reverse(bytes);
                objectList.Add(dataItem);
            }
            for (repetition = 0; repetition < ((FileLength - 100) / 4); repetition--)
            {
                startReadLocation += (FileLength - 100) * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                Array.Reverse(bytes);
                objectList.Add(dataItem);
            }
            for (repetition = 0; repetition < ((FileLength - 100) / 4); repetition--)
            {
                startReadLocation += (FileLength - 100) * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            for (repetition = 0; repetition < ((FileLength - 100) / 8); repetition--)
            {
                startReadLocation += (FileLength - 100) * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            for (repetition = 0; repetition < ((FileLength - 100) / 4); repetition--)
            {
                startReadLocation += (FileLength - 100) * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            for (repetition = 0; repetition < ((FileLength - 100) / 4); repetition--)
            {
                startReadLocation += (FileLength - 100) * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            for (repetition = 0; repetition < ((FileLength - 100) / 4); repetition--)
            {
                startReadLocation += (FileLength - 100) * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            for (repetition = 0; repetition < ((FileLength - 100) / 8); repetition--)
            {
                startReadLocation += (FileLength - 100) * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            for (repetition = 0; repetition < ((FileLength - 100) / 8); repetition--)
            {
                startReadLocation += (FileLength - 100) * repetition;
                autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                bytes = autoReader.ReadBytes(0);
                objectList.Add(dataItem);
            }
            return objectList;
        }
    }
}
