using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using MessageBox = System.Windows.Forms.MessageBox;


namespace AGRP
{
    public partial class Form1 : Form
    {
        // A list type variable for store the linearized element
        List<XmlElement> linearSequence = new List<XmlElement>();
        // The file path of the read program file
        string programFilePath = string.Empty;
        // The file path of the DFML document file
        string DFMLFilePath = string.Empty;
        // The file path of the file to be read
        string readFilePath = string.Empty;
        List<DFML> DFMLList = new List<DFML>();
        // Initialize a empty string type variable for read code content
        string code = string.Empty;
        // Save Vars value
        Hashtable VarsDict = new Hashtable();
        XmlElement rootElement;
        // 目前支持的数据类型
        string[] basicDataType = { "string", "integer", "real", "boolean", "date", "time", "datetime", "path" };
        public XmlElement targetElement;
        // 显示功能相关的变量
        TableLayoutPanel DataHeaderTableLayoutPanel = null;
        TableLayoutPanel DataContentTableLayoutPanel = null;
        //记录鼠标按下的次数
        int m_MouseClicks = 0; 
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 生成读取代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateButton_Click(object sender, EventArgs e)
        {
            // 选择编程语言
            if (this.ProgrammingLanguageListBox.SelectedItem == null)
            {
                MessageBox.Show("Programming Language has not been selected.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // 获取选中的DFML文档
                DFML DFMLFile = (DFML)DFMLListBox.SelectedItem;
                if (DFMLFile != null)
                {
                    // initialize 
                    DFMLFilePath = DFMLFile.filePath;
                    // Load the DFML data in DFMLFilePath as a XML object
                    XmlDocument DFMLXmlObject = new XmlDocument();
                    DFMLXmlObject.Load(DFMLFilePath);
                    //// Acquire the root element of DFMLXmlObject
                    rootElement = DFMLXmlObject.DocumentElement;
                    linearSequence.Clear();
                    // 筛选出选中的节点
                    ConverDFMLToSequence(rootElement, linearSequence);
                    UpdateSequence();
                    initGenerateCodesProgressBar();
                    string programmingLanguage = ProgrammingLanguageListBox.SelectedItem.ToString();
                    // 根据选择的编程语言生成对应的代码
                    switch (this.ProgrammingLanguageListBox.SelectedItem.ToString())
                    {
                        case "C#":
                            GenerateCSharpReadProgram();
                            break;
                        case "Python":
                            GeneratePythonReadProgram();
                            break;
                        default:
                            MessageBox.Show(string.Format("{0} has not been expanded", programmingLanguage), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Dfml document has not been selected.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            GenerateCodesProgressBarPanel.SendToBack();
        }

        /// <summary>
        /// 生成Python语言的读取代码
        /// </summary>
        private void GeneratePythonReadProgram()
        {
            code = GeneratePythonCodeStructure(rootElement);
            //ConverDFMLToSequence(rootElement, linearSequence);
            //// 筛选出选中的节点
            //UpdateSequence();
            initGenerateCodesProgressBar();
            // Acquire the value of "mode" attribute of rootElement
            string readMode = rootElement.GetAttribute("mode");
            string readMothodCode = string.Empty;
            // According to the read mode, generate the corresponding read mothod code by DFML XML document respectively
            if (readMode == "byte")
            {
                readMothodCode = GeneratePythonByteReadMothodCode(linearSequence);
            }
            else if (readMode == "char")
            {
                readMothodCode = GeneratePythonCharacterReadMothodCode(linearSequence);
            }
            // Append readMothodCode to the data item read method in code
            code = string.Format(code, readMothodCode);
            CodeContentRichTextBox.Text = code;
        }

        /// <summary>
        ///  Generate the character file python read mothod code by linear sequence
        /// </summary>
        /// <param name="linearSequence">A list type variable for store the linearized element</param>
        /// <returns></returns>
        private string GeneratePythonCharacterReadMothodCode(List<XmlElement> linearSequence)
        {

            string readMothodCode = string.Empty;
            int retractLevel = 3;
            string elemName = string.Empty;
            string startReadLocation = string.Empty;
            string locationAtt = string.Empty;
            string startLocation = string.Empty;
            string endLocation = string.Empty;
            string readLength = "0";
            int interval = 0;
            int repetition = 0;
            foreach (XmlElement element in linearSequence)
            {
                // Acquire the "name" value of element
                elemName = element.Name;
                // Acquire the "startReadLocation" attribute value of element
                startReadLocation = element.GetAttribute("startReadLocation");
                // 开始读取的行数
                string startReadRow = startReadLocation.Split(' ')[0];
                // 开始读取的列数
                string startReadColumn = startReadLocation.Split(' ')[1];
                // Acquire the "length" attribute value of element
                readLength = element.GetAttribute("length").Split(' ')[1];
                // Acquire the "interval" attribute value of element
                interval = int.Parse(element.GetAttribute("interval"));
                // Acquire the "repetition" attribute value of element
                repetition = int.Parse(element.GetAttribute("repetition"));
                // random read
                if (element.HasAttribute("targetIndex"))
                {
                    int targetIndex = int.Parse(element.GetAttribute("targetIndex"));
                    startReadRow = (int.Parse(startReadRow) + interval * (targetIndex - 1)).ToString();
                    // 生成代码初始化startReadLocation变量
                    readMothodCode += new string(' ', retractLevel * 4) + string.Format("startReadLocation = {0}", startReadRow) + Environment.NewLine;
                    // 生成代码用于从startReadLocation位置开始读取readLength长度的数据
                    readMothodCode += new string(' ', retractLevel * 4) + "line = self.allLines[startReadLocation]" + Environment.NewLine;
                    if (int.Parse(readLength) >= 0)
                    {
                        readMothodCode += new string(' ', retractLevel * 4) + string.Format("dataString = line[{0}:{0}+{1}]", startReadColumn, readLength) + Environment.NewLine;
                    }
                    else if (int.Parse(readLength) == -1)
                    {
                        readMothodCode += new string(' ', retractLevel * 4) + string.Format("dataString = line[{0}:]", startReadColumn, readLength) + Environment.NewLine;
                    }
                    // 生成代码用于将读取的数据转化为elemName表示的数据类型
                    switch (elemName)
                    {
                        case "string":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = dataString" + Environment.NewLine;
                            break;
                        case "integer":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = int(dataString)" + Environment.NewLine;
                            break;
                        case "real":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = float(dataString)" + Environment.NewLine;
                            break;
                        case "boolean":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = bool(dataString)" + Environment.NewLine;
                            break;
                        case "date":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = datetime.datetime.strptime(dataString, '%Y-%m-%d').date()" + Environment.NewLine;
                            break;
                        case "time":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = datetime.datetime.strptime(dataString, '%Y-%m-%d').time()" + Environment.NewLine;
                            break;
                        case "datetime":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = datetime.datetime.strptime(dataString, '%Y-%m-%d'" + Environment.NewLine;
                            break;
                        case "path":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = dataString" + Environment.NewLine;
                            break;
                    }
                    // 生成代码用于将数据加入到数据列表中
                    readMothodCode += new string(' ', retractLevel * 4) + "self.objectList.append(dataItem)" + Environment.NewLine;
                }
                // full read
                else
                {
                    // 生成代码初始化startReadLocation与 repetition 变量
                    readMothodCode += new string(' ', retractLevel * 4) + string.Format("startReadLocation = {0}", startReadRow) + Environment.NewLine;
                    readMothodCode += new string(' ', retractLevel * 4) + string.Format("repetition = {0}", repetition) + Environment.NewLine;
                    if (repetition == -1)
                    {
                        // 生成代码构建一个while循环，循环持续到读取到文件末尾
                        readMothodCode += new string(' ', retractLevel * 4) + "while startReadLocation < len(self.allLines):" + Environment.NewLine;
                    }
                    else
                    {
                        // 生成代码构建一个while循环，当repetition大于0时，循环继续
                        readMothodCode += new string(' ', retractLevel * 4) + "while repetition != 0:" + Environment.NewLine;
                    }
                    // 开始生成循环内容的代码
                    retractLevel += 1;
                    // 生成代码用于从startReadLocation位置开始读取readLength长度的数据
                    readMothodCode += new string(' ', retractLevel * 4) + "line = self.allLines[startReadLocation]" + Environment.NewLine;
                    if (int.Parse(readLength) >= 0)
                    {
                        readMothodCode += new string(' ', retractLevel * 4) + string.Format("dataString = line[{0}:{0}+{1}]", startReadColumn, readLength) + Environment.NewLine;
                    }
                    else if (int.Parse(readLength) == -1)
                    {
                        readMothodCode += new string(' ', retractLevel * 4) + string.Format("dataString = line[{0}:]", startReadColumn, readLength) + Environment.NewLine;
                    }
                    // 生成代码用于将读取的数据转化为elemName表示的数据类型
                    switch (elemName)
                    {
                        case "string":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = dataString" + Environment.NewLine;
                            break;
                        case "integer":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = int(dataString)" + Environment.NewLine;
                            break;
                        case "real":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = float(dataString)" + Environment.NewLine;
                            break;
                        case "boolean":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = bool(dataString)" + Environment.NewLine;
                            break;
                        case "date":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = datetime.datetime.strptime(dataString, '%Y-%m-%d').date()" + Environment.NewLine;
                            break;
                        case "time":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = datetime.datetime.strptime(dataString, '%Y-%m-%d').time()" + Environment.NewLine;
                            break;
                        case "datetime":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = datetime.datetime.strptime(dataString, '%Y-%m-%d'" + Environment.NewLine;
                            break;
                        case "path":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = dataString" + Environment.NewLine;
                            break;
                    }
                    // 生成代码用于将数据加入到数据列表中
                    readMothodCode += new string(' ', retractLevel * 4) + "self.objectList.append(dataItem)" + Environment.NewLine;
                    // 生成代码使startReadLocation增加interval的值
                    readMothodCode += new string(' ', retractLevel * 4) + string.Format("startReadLocation += {0}", interval) + Environment.NewLine;
                    // 生成代码使repetition减1
                    readMothodCode += new string(' ', retractLevel * 4) + "repetition -= 1" + Environment.NewLine;
                    // 结束生成循环内容的代码
                    retractLevel -= 1;
                }
                GenerateCodesProgressBar.Value += GenerateCodesProgressBar.Step;
            }
            readMothodCode += new string(' ', retractLevel * 4) + "return self.objectList" + Environment.NewLine;
            return readMothodCode;
        }

        /// <summary>
        ///  Generate the byte file python read mothod code by linear sequence
        /// </summary>
        /// <param name="linearSequence">用于存储XML元素的线性序列</param>
        /// <returns></returns>
        private string GeneratePythonByteReadMothodCode(List<XmlElement> linearSequence)
        {
            string readMothodCode = string.Empty;
            int retractLevel = 3;
            string elemName = string.Empty;
            string startReadLocation = string.Empty;
            string locationAtt = string.Empty;
            string startLocation = string.Empty;
            string endLocation = string.Empty;
            string readLength = "0";
            int interval = 0;
            int repetition = 0;
            foreach (XmlElement element in linearSequence)
            {
                // Acquire the "name" value of element
                elemName = element.Name;
                // Acquire the "startReadLocation" attribute value of element
                startReadLocation = element.GetAttribute("startReadLocation");
                // Acquire the "length" attribute value of element
                readLength = element.GetAttribute("length");
                // Acquire the "interval" attribute value of element
                interval = int.Parse(element.GetAttribute("interval"));
                // Acquire the "repetition" attribute value of element
                repetition = int.Parse(element.GetAttribute("repetition"));
                // random read
                if (element.HasAttribute("targetIndex"))
                {
                    int targetIndex = int.Parse(element.GetAttribute("targetIndex"));
                    startReadLocation = (int.Parse(startReadLocation) + interval * (targetIndex - 1)).ToString();
                    // 生成代码初始化startReadLocation
                    readMothodCode += new string(' ', retractLevel * 4) + string.Format("startReadLocation = {0}", startReadLocation) + Environment.NewLine;
                    // 生成代码用于从startReadLocation位置开始读取readLength长度的数据
                    readMothodCode += new string(' ', retractLevel * 4) + "self.autoReader.seek(startReadLocation)" + Environment.NewLine;
                    readMothodCode += new string(' ', retractLevel * 4) + string.Format("bytes = self.autoReader.read({0})", readLength) + Environment.NewLine;
                    // 生成代码用于将读取的数据转化为elemName表示的数据类型
                    string byteOrder = element.GetAttribute("byteOrder");
                    if (byteOrder == "bigEndian")
                    {
                        byteOrder = ">";
                    }
                    else if (byteOrder == "littleEndian")
                    {
                        byteOrder = "<";
                    }
                    switch (elemName)
                    {
                        case "string":
                            readMothodCode += new string(' ', retractLevel * 4) + string.Format("dataItem = struct.unpack('{0}{1}i',bytes)[0].decode()", byteOrder, readLength) + Environment.NewLine;
                            break;
                        case "integer":
                            readMothodCode += new string(' ', retractLevel * 4) + string.Format("dataItem = struct.unpack('{0}i',bytes)[0]", byteOrder) + Environment.NewLine;
                            break;
                        case "real":
                            readMothodCode += new string(' ', retractLevel * 4) + string.Format("dataItem = struct.unpack('{0}d',bytes)[0]", byteOrder) + Environment.NewLine;
                            break;
                        case "boolean":
                            readMothodCode += new string(' ', retractLevel * 4) + string.Format("dataItem = struct.unpack('{0}b',bytes)[0]", byteOrder) + Environment.NewLine;
                            break;
                        case "date":
                            readMothodCode += new string(' ', retractLevel * 4) + string.Format("dataItem = struct.unpack('{0}{1}i',bytes)[0].decode()", byteOrder, readLength) + Environment.NewLine;
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = datetime.datetime.strptime(dataItem, '%Y-%m-%d').date()" + Environment.NewLine;
                            break;
                        case "time":
                            readMothodCode += new string(' ', retractLevel * 4) + string.Format("dataItem = struct.unpack('{0}{1}i',bytes)[0].decode()", byteOrder, readLength) + Environment.NewLine;
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = datetime.datetime.strptime(dataItem, '%Y-%m-%d %H:%M:%S').time()" + Environment.NewLine;
                            break;
                        case "datetime":
                            readMothodCode += new string(' ', retractLevel * 4) + string.Format("dataItem = struct.unpack('{0}{1}i',bytes)[0].decode()", byteOrder, readLength) + Environment.NewLine;
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = datetime.datetime.strptime(dataItem, '%Y-%m-%d %H:%M:%S')" + Environment.NewLine;
                            break; ;
                        case "path":
                            readMothodCode += new string(' ', retractLevel * 4) + string.Format("dataItem = struct.unpack('{0}{1}i',dataItem)[0].decode()", byteOrder, readLength) + Environment.NewLine;
                            break;
                    }
                    // 生成代码用于将数据加入到数据列表中
                    readMothodCode += new string(' ', retractLevel * 4) + "self.objectList.append(dataItem)" + Environment.NewLine;
                }
                // full read
                else
                {
                    // 生成代码初始化startReadLocation与 repetition 变量
                    readMothodCode += new string(' ', retractLevel * 4) + string.Format("startReadLocation = {0}", startReadLocation) + Environment.NewLine;
                    readMothodCode += new string(' ', retractLevel * 4) + string.Format("repetition = {0}", repetition) + Environment.NewLine;
                    if (repetition == -1)
                    {
                        // 生成代码构建一个while循环，循环持续到读取到文件末尾
                        readMothodCode += new string(' ', retractLevel * 4) + "while startReadLocation < self.fileLength:" + Environment.NewLine; ;
                    }
                    else
                    {
                        // 生成代码构建一个while循环，当repetition大于0时，循环继续
                        readMothodCode += new string(' ', retractLevel * 4) + "while startReadLocation < self.fileLength and repetition > 0:" + Environment.NewLine; ;
                    }
                    // 开始生成循环内容的代码
                    retractLevel += 1;
                    // 生成代码用于从startReadLocation位置开始读取readLength长度的数据
                    readMothodCode += new string(' ', retractLevel * 4) + "self.autoReader.seek(startReadLocation)" + Environment.NewLine;
                    readMothodCode += new string(' ', retractLevel * 4) + string.Format("bytes = self.autoReader.read({0})", readLength) + Environment.NewLine;
                    // 生成代码用于将读取的数据转化为elemName表示的数据类型
                    string byteOrder = element.GetAttribute("byteOrder");
                    if (byteOrder == "bigEndian")
                    {
                        byteOrder = ">";
                    }
                    else if (byteOrder == "littleEndian")
                    {
                        byteOrder = "<";
                    }
                    switch (elemName)
                    {
                        case "string":
                            readMothodCode += new string(' ', retractLevel * 4) + string.Format("dataItem = struct.unpack('{0}{1}i',bytes)[0].decode()", byteOrder, readLength) + Environment.NewLine;
                            break;
                        case "integer":
                            readMothodCode += new string(' ', retractLevel * 4) + string.Format("dataItem = struct.unpack('{0}i',bytes)[0]", byteOrder) + Environment.NewLine;
                            break;
                        case "real":
                            readMothodCode += new string(' ', retractLevel * 4) + string.Format("dataItem = struct.unpack('{0}d',bytes)[0]", byteOrder) + Environment.NewLine;
                            break;
                        case "boolean":
                            readMothodCode += new string(' ', retractLevel * 4) + string.Format("dataItem = struct.unpack('{0}b',bytes)[0]", byteOrder) + Environment.NewLine;
                            break;
                        case "date":
                            readMothodCode += new string(' ', retractLevel * 4) + string.Format("dataItem = struct.unpack('{0}{1}i',bytes)[0].decode()", byteOrder, readLength) + Environment.NewLine;
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = datetime.datetime.strptime(dataItem, '%Y-%m-%d').date()" + Environment.NewLine;
                            break;
                        case "time":
                            readMothodCode += new string(' ', retractLevel * 4) + string.Format("dataItem = struct.unpack('{0}{1}i',bytes)[0].decode()", byteOrder, readLength) + Environment.NewLine;
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = datetime.datetime.strptime(dataItem, '%Y-%m-%d %H:%M:%S').time()" + Environment.NewLine;
                            break;
                        case "datetime":
                            readMothodCode += new string(' ', retractLevel * 4) + string.Format("dataItem = struct.unpack('{0}{1}i',bytes)[0].decode()", byteOrder, readLength) + Environment.NewLine;
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = datetime.datetime.strptime(dataItem, '%Y-%m-%d %H:%M:%S')" + Environment.NewLine;
                            break; ;
                        case "path":
                            readMothodCode += new string(' ', retractLevel * 4) + string.Format("dataItem = struct.unpack('{0}{1}i',dataItem)[0].decode()", byteOrder, readLength) + Environment.NewLine;
                            break;
                    }
                    // 生成代码用于将数据加入到数据列表中
                    readMothodCode += new string(' ', retractLevel * 4) + "self.objectList.append(dataItem)" + Environment.NewLine;
                    // 生成代码使startReadLocation增加interval的值
                    readMothodCode += new string(' ', retractLevel * 4) + string.Format("startReadLocation += {0}", interval) + Environment.NewLine;
                    // 生成代码使repetition减1
                    readMothodCode += new string(' ', retractLevel * 4) + "repetition -= 1" + Environment.NewLine;
                    // 结束生成循环内容的代码
                    retractLevel -= 1;
                }
                GenerateCodesProgressBar.Value += GenerateCodesProgressBar.Step;
            }
            readMothodCode += new string(' ', retractLevel * 4) + "return self.objectList" + Environment.NewLine;
            return readMothodCode;
        }

        /// <summary>
        /// Defining the python code structure of program reading file
        /// </summary>
        /// <param name="rootElement">The root element of DFML document</param>
        /// <returns></returns>
        private string GeneratePythonCodeStructure(XmlElement rootElement)
        {
            // Initialize a empty string for store the read program code content
            string code = string.Empty;
            // Initialize retract level
            int retractLevel;
            // Append using namespace code into code, including keywords of namespace, name of namespace
            // Set retract level as 0
            retractLevel = 0;
            code += new string(' ', retractLevel * 4) + "import os" + Environment.NewLine;
            code += new string(' ', retractLevel * 4) + "import struct" + Environment.NewLine;
            code += new string(' ', retractLevel * 4) + "import time" + Environment.NewLine;
            code += new string(' ', retractLevel * 4) + "import datetime" + Environment.NewLine;
            // Append type definition code into code, including access modifier, type keyword, type names
            code += new string(' ', retractLevel * 4) + "class " + "Reader(object):" + Environment.NewLine;
            // Append constructor definition code into code, including access modifier, name of constructor, field name, property type
            // Set retract level as 1
            retractLevel = 1;
            code += new string(' ', retractLevel * 4) + "def __init__(self,filePath):" + Environment.NewLine;
            // Append property definition code into code, including access modifier, type of property, name of property
            // Set retract level as 2
            retractLevel = 2;
            // Acquire the value of "mode" attribute of rootElement
            string readMode = rootElement.GetAttribute("mode");
            // According to the read mode, generate the corresponding read mothod code
            if (readMode == "byte")
            {
                // According to the retractLevel, append code into readMothodCode to read file with byte mode
                code += new string(' ', retractLevel * 4) + "self.autoReader = open(filePath,\"rb\")" + Environment.NewLine;
                code += new string(' ', retractLevel * 4) + "self.fileLength = len(self.autoReader.read())" + Environment.NewLine;
            }
            else if (readMode == "char")
            {
                // According to the retractLevel, append code into readMothodCode to read file with character mode
                code += new string(' ', retractLevel * 4) + "self.autoReader = open(filePath,\"r\")" + Environment.NewLine;
                code += new string(' ', retractLevel * 4) + "self.allLines = self.autoReader.readlines()" + Environment.NewLine;
            }
            code += new string(' ', retractLevel * 4) + "self.objectList = list()" + Environment.NewLine;
            // Set retract level as 1
            retractLevel = 1;
            // Append method definition into code, including access modifier, type of return, method name, method parameter
            code += new string(' ', retractLevel * 4) + "def dataItemRead(self):" + Environment.NewLine;
            code += "{0}" + Environment.NewLine;
            return code;
        }

        /// <summary>
        /// Generate Sequence of Basic Data Type
        /// </summary>
        /// <param name="rootElement">The root element of DFML document </param>
        /// <param name="linearSequence">用于存储XML元素的线性序列</param>
        private void ConverDFMLToSequence(XmlElement rootElement, List<XmlElement> linearSequence)
        {
            linearSequence.Clear();
            // 为了与Tree中的节点对应，为每个节点添加一个唯一的TAG
            // int startID = 0;
            // IDElements(rootElement, ref startID);

            string elemName = string.Empty;
            string locationAtt = string.Empty;
            string startLocation = string.Empty;
            string endLocation = string.Empty;
            string groupLength = string.Empty;
            string elemLength = string.Empty;
            string childLength = string.Empty;
            int interval = 0;
            int repetition = 0;
            string childLocationAtt = string.Empty;
            string childStartLocation = string.Empty;
            string childEndLocation = string.Empty;
            string childStartReadLoc = string.Empty;
            foreach (XmlElement element in rootElement)
            {
                // Acquire the name value of element
                elemName = element.Name;
                if (basicDataType.Contains(elemName) || elemName == "group")
                {
                    // Acquire the "location" attribute value of element
                    locationAtt = element.GetAttribute("location");
                    // Parse the start and end location form locationAtt
                    startLocation = locationAtt.Split(',')[0];
                    endLocation = locationAtt.Split(',')[1];
                    if (elemName == "group")
                    {
                        // 该group中的每个子元素出现的间隔
                        interval = CalculateInterval(element);
                        // 如果结束位置是文件末尾
                        if (endLocation == "-1" || endLocation.Split(' ')[0] == "-1")
                        {
                            // 该group元素直到文件末尾才结束
                            groupLength = endLocation;
                            // 该group的子元素重复出现的次数取决于文件长度
                            repetition = -1;
                        }
                        else
                        {
                            // 该group元素从开始位置到结束位置的长度
                            groupLength = CalculateLength(startLocation, endLocation);
                            // group中的子元素重复出现的次数
                            repetition = CalculateReptition(groupLength, interval);
                        }
                        element.SetAttribute("repetition", repetition.ToString());
                        // 为group中的每个子元素赋值
                        foreach (XmlElement childElem in element)
                        {
                            // Acquire the name value of childElem
                            string childElemName = childElem.Name;
                            // if child element is Basic Data Type
                            if (basicDataType.Contains(childElemName))
                            {
                                // Acquire the "location" attribute value of childElem
                                childLocationAtt = childElem.GetAttribute("location");
                                // Parse the start location from childLocationAtt
                                childStartLocation = childLocationAtt.Split(',')[0];
                                // Parse the end location from childLocationAtt
                                childEndLocation = childLocationAtt.Split(',')[1];
                                childStartReadLoc = CalculateStartLocation(startLocation, childStartLocation);
                                childLength = CalculateLength(childStartLocation, childEndLocation);
                                // Add childStartReadLoc to attributes of childElem as "startReadLocation"
                                childElem.SetAttribute("startReadLocation", childStartReadLoc);
                                // Add childLength to attributes of childElem as "length"
                                childElem.SetAttribute("length", childLength.ToString());
                                // Add interval to attributes of childElem as "interval"
                                childElem.SetAttribute("interval", interval.ToString());
                                // Add repetition to attributes of childElem as "repetition"
                                childElem.SetAttribute("repetition", repetition.ToString());
                                // Add childElem to linearSequence
                                linearSequence.Add(childElem);
                            }
                        }

                    }
                    else if (basicDataType.Contains(elemName))
                    {
                        // 该元素不会再次出现
                        interval = 0;
                        // 该元素只出现一次
                        repetition = 1;
                        elemLength = CalculateLength(startLocation, endLocation);
                        // Add startLocation to attributes of element as "startReadLocation"
                        element.SetAttribute("startReadLocation", startLocation);
                        // Add elemLength to attributes of element as "length"
                        element.SetAttribute("length", elemLength.ToString());
                        // Add interval to attributes of element as "interval"
                        element.SetAttribute("repetition", repetition.ToString());
                        // Add repetition to attributes of element as "repetition"
                        element.SetAttribute("interval", interval.ToString());
                        // Add isHeaderElem to attributes of childElem as "isHeaderElem"
                        element.SetAttribute("isHeaderElem", "true");
                        // Add element to linearSequence
                        linearSequence.Add(element);
                    }
                }
            }

            //foreach (XmlElement node in linearSequence)
            //{
            //    CodeContentRichTextBox.Text += "<";
            //    CodeContentRichTextBox.Text += node.Name;
            //    CodeContentRichTextBox.Text += " ";
            //    foreach (XmlAttribute att in node.Attributes)
            //    {
            //        if (att.Name != "ID" && att.Name != "groupName")
            //        {
            //            CodeContentRichTextBox.Text += att.Name;
            //            CodeContentRichTextBox.Text += "=";
            //            CodeContentRichTextBox.Text += "\"";
            //            CodeContentRichTextBox.Text += att.Value;
            //            CodeContentRichTextBox.Text += "\"";
            //            CodeContentRichTextBox.Text += " ";
            //        }
            //    }
            //    CodeContentRichTextBox.Text += ">";
            //    CodeContentRichTextBox.Text += "\n";
            //}
        }

        /// <summary>
        /// Calculate the length of bytes or lines from start location to end location
        /// </summary>
        /// <param name="startLocation">Start location</param>
        /// <param name="endLocation">End location</param>
        /// <returns></returns>
        private string CalculateLength(string startLocation, string endLocation)
        {
            // 文本文件DFML的位置属性
            if (startLocation.Contains(' '))
            {
                int startRow = int.Parse(startLocation.Split(' ')[0]);
                int endRow = int.Parse(endLocation.Split(' ')[0]);
                int startCol = int.Parse(startLocation.Split(' ')[1]);
                int endCol = int.Parse(endLocation.Split(' ')[1]);
                int rowLength;
                int colLength;
                if (endCol != -1)
                {
                    rowLength = endRow - startRow;
                    colLength = endCol - startCol;
                }
                else
                {
                    rowLength = endRow - startRow + 1;
                    colLength = -1;
                }
                return string.Format("{0} {1}", rowLength, colLength);

            }
            // 二进制文件DFML的位置属性
            else
            {
                try
                {
                    int ParseEndLocation = int.Parse(endLocation);
                    int ParseStartLocation = int.Parse(startLocation);
                    return (ParseEndLocation - ParseStartLocation).ToString();
                }
                catch
                {
                    string ParseEndLocation = "(" + endLocation + ")";
                    string ParseStartLocation = "(" + startLocation + ")";
                    return "(" + ParseEndLocation + "-" + ParseStartLocation + ")";
                }

            }
        }

        /// <summary>
        /// 解析使用表达式表示的值
        /// </summary>
        /// <param name="exp">表达式</param>
        /// <returns>表达式对应的值</returns>
        private string DynamicCalculation(string exp)
        {
            // 替换表达式中的变量
            foreach (string VarName in VarsDict.Keys)
            {
                Regex rgx = new Regex(VarName);
                exp = rgx.Replace(exp, VarsDict[VarName].ToString());
            }
            // 判断表达式中是否有运算符
            if (exp.Contains("+") | exp.Contains("-") | exp.Contains("*") | exp.Contains("/"))
            {
                // 计算
                exp = Calculate(exp).ToString();
                // exp = CalcByCalcParenthesesExpression(exp);
                return exp;
            }
            else
                return exp;
        }

        public string CalcByCalcParenthesesExpression(string exp)
        {
            string result = new CalcParenthesesExpression().CalculateParenthesesExpression(exp);
            return result;
        }

        /// <summary>
        /// 计算重复次数
        /// </summary>
        /// <param name="groupLength">group元素从开始位置到结束位置的长度</param>
        /// <param name="interval">该group中的每个子元素全部出现一次的长度</param>
        /// <returns></returns>
        private int CalculateReptition(string groupLength, int interval)
        {
            // 如果是文本的长度，包含了行的长度与列的长度，使用行距离计算重复次数
            if (groupLength.Contains(' '))
            {
                int rowLength = int.Parse(groupLength.Split(' ')[0]);
                return rowLength / interval;            
            }
            else
            {
                return int.Parse(groupLength) / interval;
            }

        }

        /// <summary>
        /// 计算绝对的开始读取位置
        /// </summary>
        /// <param name="anchor">Anchor</param>
        /// <param name="stepLength">StepLength</param>
        /// <returns></returns>
        private string CalculateStartLocation(string anchor, string stepLength)
        {
            // Determine the type of location
            // anchor is character mode location
            if (anchor.Contains(' '))
            {
                int anchorLine = int.Parse(anchor.Split(' ')[0]);
                int anchorRow = int.Parse(anchor.Split(' ')[1]);
                int stepLine = int.Parse(stepLength.Split(' ')[0]);
                int stepRow = int.Parse(stepLength.Split(' ')[1]);

                int absoluteLine = anchorLine + stepLine;
                int absoluteRow = anchorRow + stepRow;
                string absoluteLocation = string.Format("{0} {1}", absoluteLine, absoluteRow);
                return absoluteLocation;
            }
            // anchor is byte mode location
            else
            {
                return (int.Parse(anchor) + int.Parse(stepLength)).ToString();
            }
        }

        /// <summary>
        /// Calculate the sum of length of all child elements in this element
        /// </summary>
        /// <param name="element">Group element</param>
        /// <returns></returns>
        private int CalculateInterval(XmlElement element)
        {
            int interval = 0;
            string locationAtt = string.Empty;
            string startLocation = string.Empty;
            string endLocation = string.Empty;
            foreach (XmlElement childElem in element)
            {
                string childElemName = childElem.Name;
                if (basicDataType.Contains(childElemName))
                {
                    // Acquire the value of "location" attribute of element
                    locationAtt = childElem.GetAttribute("location");
                    // Parse the start and end location form locationAtt
                    startLocation = locationAtt.Split(',')[0];
                    endLocation = locationAtt.Split(',')[1];
                    interval += int.Parse(CalculateLength(startLocation, endLocation).Split(' ')[0]);
                }
            }
            return interval;
        }

        /// <summary>
        /// 生成C#的读取代码
        /// </summary>
        private void GenerateCSharpReadProgram()
        {
            // Defining the code structure of program reading file
            code = GenerateCSharpCodeStructure(rootElement);
            GenerateCodesProgressBar.Value += GenerateCodesProgressBar.Step;
            // Generate Sequence of Basic Data Types
            //ConverDFMLToSequence(rootElement, linearSequence);
            // Acquire the value of "mode" attribute of rootElement
            string readMode = rootElement.GetAttribute("mode");
            string readMothodCode = string.Empty;
            // According to the read mode, generate the corresponding read mothod code by DFML XML document respectively
            int retractLevel = 3;
            if (readMode == "byte")
            {
                readMothodCode += GenerateCSharpByteReadMothodCode(this.DFMLTreeView.Nodes[0], retractLevel);
                readMothodCode += new string(' ', retractLevel * 4) + "return objectList;" + Environment.NewLine;
            }
            else if (readMode == "char")
            {
                readMothodCode = GenerateCSharpCharacterReadMothodCode(linearSequence);
            }
            // Append readMothodCode to the data item read method in code
            code = string.Format(code, "{", "}", readMothodCode);
            CodeContentRichTextBox.Text = code;
            GenerateCodesProgressBar.Value += GenerateCodesProgressBar.Step;
        }

        private void initGenerateCodesProgressBar()
        {
            GenerateCodesProgressBarPanel.BringToFront();
            GenerateCodesProgressBar.Value = 0;
            GenerateCodesProgressBar.Maximum = 100;
            GenerateCodesProgressBar.Minimum = 0;
            GenerateCodesProgressBar.Step = 100 / linearSequence.Count();
        }

        /// <summary>
        ///  generate the character file C# read mothod code by linear sequence
        /// </summary>
        /// <param name="linearSequence">用于存储XML元素的线性序列</param>
        /// <returns></returns>
        private string GenerateCSharpCharacterReadMothodCode(List<XmlElement> linearSequence)
        {
            string readMothodCode = string.Empty;
            int retractLevel = 3;
            string elemName = string.Empty;
            string startReadLocation = string.Empty;
            string readLength = "0";
            int interval = 0;
            int repetition = 0;
            foreach (XmlElement element in linearSequence)
            {
                // Acquire the "name" value of element
                elemName = element.Name;
                // Acquire the "startReadLocation" attribute value of element
                startReadLocation = element.GetAttribute("startReadLocation");
                // 开始读取的行数
                string startReadRow = startReadLocation.Split(' ')[0];
                // 开始读取的列数
                string startReadColumn = startReadLocation.Split(' ')[1];
                // Acquire the "length" attribute value of element
                readLength = element.GetAttribute("length").Split(' ')[1];
                // Acquire the "interval" attribute value of element
                interval = int.Parse(element.GetAttribute("interval"));
                // Acquire the "repetition" attribute value of element
                repetition = int.Parse(element.GetAttribute("repetition"));
                // random read
                if (element.HasAttribute("targetIndex"))
                {
                    int targetIndex = int.Parse(element.GetAttribute("targetIndex"));
                    startReadRow = (int.Parse(startReadRow) + interval * (targetIndex - 1)).ToString();
                    // 生成代码初始化startReadLocation变量
                    readMothodCode += new string(' ', retractLevel * 4) + string.Format("startReadLocation = {0};", startReadRow) + Environment.NewLine;
                    // 生成代码用于从startReadLocation位置开始读取readLength长度的数据
                    readMothodCode += new string(' ', retractLevel * 4) + "line = allLines[startReadLocation];" + Environment.NewLine;
                    if (int.Parse(readLength) >= 0)
                    {
                        readMothodCode += new string(' ', retractLevel * 4) + string.Format("dataString = line.Substring({0},{1});", startReadColumn, readLength) + Environment.NewLine;
                    }
                    else if (int.Parse(readLength) == -1)
                    {
                        readMothodCode += new string(' ', retractLevel * 4) + string.Format("dataString = line.Substring({0},line.Length-{0});", startReadColumn) + Environment.NewLine;
                    }
                    // 生成代码用于将读取的数据转化为elemName表示的数据类型
                    switch (elemName)
                    {
                        case "string":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = dataString;" + Environment.NewLine;
                            break;
                        case "integer":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = int.Parse(dataString);" + Environment.NewLine;
                            break;
                        case "real":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = double.Parse(dataString);" + Environment.NewLine;
                            break;
                        case "boolean":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = Boolean.Parse(dataString);" + Environment.NewLine;
                            break;
                        case "date":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = DateTime.Parse(dataString);" + Environment.NewLine;
                            break;
                        case "time":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = TimeSpan.Parse(dataString);" + Environment.NewLine;
                            break;
                        case "datetime":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = Convert.ToDateTime(dataString);" + Environment.NewLine;
                            break;
                        case "path":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = @dataString;" + Environment.NewLine;
                            break;
                    }
                    // 生成代码用于将数据加入到数据列表中
                    readMothodCode += new string(' ', retractLevel * 4) + "objectList.Add(dataItem);" + Environment.NewLine;
                }
                // full read
                else
                {
                    // 生成代码初始化startReadLocation与 repetition 变量
                    readMothodCode += new string(' ', retractLevel * 4) + string.Format("startReadLocation = {0};", startReadRow) + Environment.NewLine;
                    readMothodCode += new string(' ', retractLevel * 4) + string.Format("repetition = {0};", repetition) + Environment.NewLine;
                    if (repetition == -1)
                    {
                        // 生成代码构建一个while循环，循环持续到读取到文件末尾
                        readMothodCode += new string(' ', retractLevel * 4) + "while(startReadLocation < allLines.Length)" + Environment.NewLine;
                    }
                    else
                    {
                        // 生成代码构建一个while循环，当repetition大于0时，循环继续
                        readMothodCode += new string(' ', retractLevel * 4) + "while(repetition>0)" + Environment.NewLine;
                    }
                    // 开始生成循环内容的代码
                    readMothodCode += new string(' ', retractLevel * 4) + "{" + Environment.NewLine;
                    retractLevel += 1;
                    // 生成代码用于从startReadLocation位置开始读取readLength长度的数据
                    readMothodCode += new string(' ', retractLevel * 4) + "line = allLines[startReadLocation];" + Environment.NewLine;
                    if (int.Parse(readLength) >= 0)
                    {
                        readMothodCode += new string(' ', retractLevel * 4) + string.Format("dataString = line.Substring({0},{1});", startReadColumn, readLength) + Environment.NewLine;
                    }
                    else if (int.Parse(readLength) == -1)
                    {
                        readMothodCode += new string(' ', retractLevel * 4) + string.Format("dataString = line.Substring({0},line.Length-{0});", startReadColumn) + Environment.NewLine;
                    }
                    // 生成代码用于将读取的数据转化为elemName表示的数据类型
                    switch (elemName)
                    {
                        case "string":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = dataString;" + Environment.NewLine;
                            break;
                        case "integer":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = int.Parse(dataString);" + Environment.NewLine;
                            break;
                        case "real":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = double.Parse(dataString);" + Environment.NewLine;
                            break;
                        case "boolean":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = Boolean.Parse(dataString);" + Environment.NewLine;
                            break;
                        case "date":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = DateTime.Parse(dataString);" + Environment.NewLine;
                            break;
                        case "time":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = TimeSpan.Parse(dataString);" + Environment.NewLine;
                            break;
                        case "datetime":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = Convert.ToDateTime(dataString);" + Environment.NewLine;
                            break;
                        case "path":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = @dataString;" + Environment.NewLine;
                            break;
                    }
                    // 生成代码用于将数据加入到数据列表中
                    readMothodCode += new string(' ', retractLevel * 4) + "objectList.Add(dataItem);" + Environment.NewLine;
                    // 生成代码使startReadLocation增加interval的值
                    readMothodCode += new string(' ', retractLevel * 4) + string.Format("startReadLocation += {0};", interval) + Environment.NewLine;
                    // 生成代码使repetition减1
                    readMothodCode += new string(' ', retractLevel * 4) + "repetition -= 1;" + Environment.NewLine;
                    // 结束生成循环内容的代码
                    retractLevel -= 1;
                    readMothodCode += new string(' ', retractLevel * 4) + "}" + Environment.NewLine;
                }
                GenerateCodesProgressBar.Value += GenerateCodesProgressBar.Step;
            }
            readMothodCode += new string(' ', retractLevel * 4) + "return objectList;" + Environment.NewLine;
            return readMothodCode;
        }

        /// <summary>
        ///  generate the byte file C# read mothod code by linear sequence
        /// </summary>
        /// <returns></returns>
        private string GenerateCSharpByteReadMothodCode(TreeNode node, int retractLevel)
        {
            string readMothodCode = string.Empty;
            string elemName = string.Empty;
            string startReadLocation;
            string locationAtt = string.Empty;
            string readLength = "0";
            string interval;
            string repetition;

            TreeNodeTag tag = (TreeNodeTag)node.Tag;
            // Acquire the "name" value of element
            string nodeType = tag.nodeType;
            XmlElement element = tag.nodeElem;
            elemName = element.Name;
            startReadLocation = tag.nodeStartLocation;
            interval = tag.nodeInterval;
            if (basicDataType.Contains(nodeType))
            {
                if (tag.nodeIndex == "full read")
                {
                    repetition = tag.nodeRepitition;
                    readMothodCode += new string(' ', retractLevel * 4) + string.Format("for (repetition=0,startReadLocation={0}; repetition<{1}; repetition--)", startReadLocation,repetition) + Environment.NewLine;
                    readMothodCode += new string(' ', retractLevel * 4) + "{" + Environment.NewLine;
                    retractLevel += 1;
                    // 生成代码初始化startReadLocation
                    // 生成代码使startReadLocation增加interval的值
                    readMothodCode += new string(' ', retractLevel * 4) + string.Format("startReadLocation += ({0})*repetition;", interval) + Environment.NewLine;
                    // 生成代码用于从startReadLocation位置开始读取readLength长度的数据
                    readMothodCode += new string(' ', retractLevel * 4) + "autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);" + Environment.NewLine;
                    readMothodCode += new string(' ', retractLevel * 4) + string.Format("bytes = autoReader.ReadBytes({0});", readLength) + Environment.NewLine;
                    // 生成代码用于将读取的数据转化为elemName表示的数据类型
                    string byteOrder = element.GetAttribute("byteOrder");
                    if (byteOrder == "bigEndian")
                    {
                        readMothodCode += new string(' ', retractLevel * 4) + "Array.Reverse(bytes);" + Environment.NewLine;
                    }
                    switch (elemName)
                    {
                        case "string":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = BitConverter.ToString(bytes);" + Environment.NewLine;
                            break;
                        case "integer":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = BitConverter.ToInt32(bytes,0);" + Environment.NewLine;
                            break;
                        case "real":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = BitConverter.ToDouble(bytes,0);" + Environment.NewLine;
                            break;
                        case "boolean":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = BitConverter.ToBoolean(bytes,0);" + Environment.NewLine;
                            break;
                        case "date":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = DateTime.Parse(BitConverter.ToString(bytes,0));" + Environment.NewLine;
                            break;
                        case "time":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = TimeSpan.Parse(BitConverter.ToString(bytes,0));" + Environment.NewLine;
                            break;
                        case "datetime":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = Convert.ToDateTime(BitConverter.ToString(bytes,0));" + Environment.NewLine;
                            break;
                        case "path":
                            readMothodCode += new string(' ', retractLevel * 4) + "dataItem = @BitConverter.ToString(bytes,0);" + Environment.NewLine;
                            break;
                    }
                    if (element.HasAttribute("VarName"))
                    {
                        string VarName = element.GetAttribute("VarName");
                        readMothodCode += new string(' ', retractLevel * 4) + string.Format("var {0} = dataItem;",VarName) + Environment.NewLine;
                    }
                    // 生成代码用于将数据加入到数据列表中
                    readMothodCode += new string(' ', retractLevel * 4) + "objectList.Add(dataItem);" + Environment.NewLine;
                    retractLevel -= 1;
                    readMothodCode += new string(' ', retractLevel * 4) + "}" + Environment.NewLine; 
                }
            }
            
            foreach (TreeNode childNode in node.Nodes)
            {
                readMothodCode += GenerateCSharpByteReadMothodCode(childNode, retractLevel);
            }
            
            return readMothodCode;
        }

        /// <summary>
        /// Calculate the bytes or character length for read according to locationAtt
        /// </summary>
        /// <param name="startLocation">Start location</param>
        /// <param name="endLocation">End location</param>
        /// <param name="programming language">PrgramLanguage</param>
        /// <returns></returns>
        private string CalculateReadLength(string startLocation, string endLocation, string prgramLanguage = "C#")
        {
            // Determine the type of location
            // startLocation is character mode location
            if (startLocation.Contains(' '))
            {
                int startRow = int.Parse(startLocation.Split(' ')[1]);
                int endRow = int.Parse(endLocation.Split(' ')[1]);
                if (endRow == -1)
                {
                    switch (prgramLanguage)
                    {
                        case "C#":
                            return string.Format("line.Length - {0}", startRow);
                        case "python":
                            return string.Format("len(line) - {0} - 1", startRow);
                        default:
                            return string.Format("line.Length - {0}", startRow);
                    }
                }
                else
                {
                    return (endRow - startRow).ToString();
                }
            }
            // startLocation is byte mode location
            else
            {
                return (int.Parse(endLocation) - int.Parse(startLocation)).ToString();
            }
        }

        /// <summary>
        /// Defining the C# code structure of program reading file
        /// </summary>
        /// <param name="rootElement">The root element of DFML document</param>
        /// <returns></returns>
        private string GenerateCSharpCodeStructure(XmlElement rootElement)
        {
            // Initialize a empty string for store the reading program code content
            string code = string.Empty;
            // Append using namespace code into code, including keywords of namespace, name of namespace
            // Initialize retract level
            int retractLevel;
            // Set retract level as 0
            retractLevel = 0;
            code += new string(' ', retractLevel * 4) + "using System;" + Environment.NewLine;
            code += new string(' ', retractLevel * 4) + "using System.IO;" + Environment.NewLine;
            code += new string(' ', retractLevel * 4) + "using System.Collections.Generic;" + Environment.NewLine;
            code += Environment.NewLine;
            // Acquire the value of "namespace" attribute of rootElement
            string namespaceName = rootElement.GetAttribute("namespace");
            //Append namespace definition code into code, including keywords of namespace definition, name of namespace
            code += new string(' ', retractLevel * 4) + "namespace " + namespaceName + Environment.NewLine;
            code += new string(' ', retractLevel * 4) + "{0}" + Environment.NewLine;
            // Set retract level as 1
            retractLevel = 1;
            // Append type definition code into code, including access modifier, type keyword, type names
            code += new string(' ', retractLevel * 4) + "public class " + "Reader" + Environment.NewLine;
            code += new string(' ', retractLevel * 4) + "{0}" + Environment.NewLine;
            // Set retract level as 2
            retractLevel = 2;
            // Append property definition code into code, including access modifier, type of property, name of property
            code += new string(' ', retractLevel * 4) + "private string readFilePath = string.Empty;" + Environment.NewLine;
            code += new string(' ', retractLevel * 4) + "private List<object> objectList = new List<object>();" + Environment.NewLine;
            code += new string(' ', retractLevel * 4) + "private int startReadLocation;" + Environment.NewLine;
            code += new string(' ', retractLevel * 4) + "private int repetition;" + Environment.NewLine;
            code += new string(' ', retractLevel * 4) + "private int FileLength;" + Environment.NewLine;
            // Acquire the value of "mode" attribute of rootElement
            string readMode = rootElement.GetAttribute("mode");
            // According to the read mode, generate the corresponding read mothod code
            if (readMode == "byte")
            {
                // According to the retractLevel, append code into readMothodCode to read file with byte mode
                code += new string(' ', retractLevel * 4) + "FileStream fs;" + Environment.NewLine;
                code += new string(' ', retractLevel * 4) + "BinaryReader autoReader;" + Environment.NewLine;
                code += new string(' ', retractLevel * 4) + "byte[] bytes;" + Environment.NewLine;
                code += new string(' ', retractLevel * 4) + "object dataItem;" + Environment.NewLine;
            }
            else if (readMode == "char")
            {
                // According to the retractLevel, append code into readMothodCode to read file with character mode
                code += new string(' ', retractLevel * 4) + "string[] allLines;" + Environment.NewLine;
                code += new string(' ', retractLevel * 4) + "string line;" + Environment.NewLine;
                code += new string(' ', retractLevel * 4) + "string dataString;" + Environment.NewLine;
                code += new string(' ', retractLevel * 4) + "object dataItem;" + Environment.NewLine;
            }
            // Append constructor definition code into code, including access modifier, name of constructor, field name, property type
            code += new string(' ', retractLevel * 4) + "public Reader(string filePath)" + Environment.NewLine;
            code += new string(' ', retractLevel * 4) + "{0}" + Environment.NewLine;
            // Set retract level as 3
            retractLevel = 3;
            code += new string(' ', retractLevel * 4) + "readFilePath = filePath;" + Environment.NewLine;
            // According to the read mode, generate the corresponding read mothod code
            if (readMode == "byte")
            {
                // According to the retractLevel, append code into readMothodCode to read file with byte mode
                code += new string(' ', retractLevel * 4) + "fs = new FileStream(readFilePath,FileMode.Open, FileAccess.Read);" + Environment.NewLine;
                code += new string(' ', retractLevel * 4) + "autoReader = new BinaryReader(fs);" + Environment.NewLine;
                code += new string(' ', retractLevel * 4) + "FileLength = (int) autoReader.BaseStream.Length;" + Environment.NewLine;
            }
            else if (readMode == "char")
            {
                // According to the retractLevel, append code into readMothodCode to read file with character mode
                code += new string(' ', retractLevel * 4) + "allLines = File.ReadAllLines(readFilePath);" + Environment.NewLine;
            }
            // Set retract level as 2
            retractLevel = 2;
            code += new string(' ', retractLevel * 4) + "{1}" + Environment.NewLine;
            // Append method definition into code, including access modifier, type of return, method name, method parameter
            code += new string(' ', retractLevel * 4) + "public List<object> dataItemRead()" + Environment.NewLine;
            code += new string(' ', retractLevel * 4) + "{0}" + Environment.NewLine;
            code += "{2}";
            code += new string(' ', retractLevel * 4) + "{1}" + Environment.NewLine;
            // Set retract level as 1
            retractLevel = 1;
            code += new string(' ', retractLevel * 4) + "{1}" + Environment.NewLine;
            // Set retract level as 0
            retractLevel = 0;
            code += new string(' ', retractLevel * 4) + "{1}" + Environment.NewLine;
            return code;
        }

        /// <summary>
        /// 加载DFML文档f
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadButton_Click(object sender, EventArgs e)
        {
            // New open file dialog box
            OpenFileDialog ofd = new OpenFileDialog();
            // Set the initial file directory
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            // Set the open file type
            ofd.Filter = "|*.*";
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                DFMLListBox.DataSource = null;
                // Get the DFML File
                DFML DFMLItem = new DFML(ofd.FileName);
                DFMLList.Add(DFMLItem);
                DFMLListBox.DataSource = DFMLList;
                DFMLListBox.DisplayMember = "fileName";
                DFMLListBox.ValueMember = "filePath";
            }
        }
        
        /// <summary>
        /// 储存生成的代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgramSaveButton_Click(object sender, EventArgs e)
        {
            if (code == string.Empty)
            {
                MessageBox.Show("Please generate code firs.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                string ProgrammingLanguageExtension = string.Empty;
                switch (this.ProgrammingLanguageListBox.SelectedItem.ToString())
                {
                    case "C#":
                        ProgrammingLanguageExtension = "cs";
                        break;
                    case "Python":
                        ProgrammingLanguageExtension = "py";
                        break;
                }
                saveFileDialog.FileName = "AutomaticReadingProgram";
                saveFileDialog.DefaultExt = ProgrammingLanguageExtension;
                saveFileDialog.RestoreDirectory = true;
                // Show save file dialog box
                DialogResult result = saveFileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    programFilePath = saveFileDialog.FileName.ToString();
                    FileStream programfs = new FileStream(programFilePath, FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter programFileWriter = new StreamWriter(programfs);
                    // Write code into programFile
                    programFileWriter.Write(code);
                    programFileWriter.Flush();
                    programFileWriter.Close();
                    System.Windows.Forms.MessageBox.Show("Successfully saved", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// 移除DFML文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveButton_Click(object sender, EventArgs e)
        {
            int selectedIdex = DFMLListBox.SelectedIndex;
            try
            {
                DFMLList.RemoveAt(selectedIdex);
                DFMLListBox.DataSource = null;
                DFMLListBox.DataSource = DFMLList;
                DFMLListBox.DisplayMember = "fileName";
                DFMLListBox.ValueMember = "filePath";
            }
            catch
            {

            }

        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Read_Click(object sender, EventArgs e)
        {
            DFML DFMLFile = (DFML)DFMLListBox.SelectedItem;
            if (DFMLFile == null)
            {
                MessageBox.Show("Dfml document has not been selected.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    
            else
            {
                // select read file
                // New open file dialog box
                OpenFileDialog ofd = new OpenFileDialog();
                // Set the initial file directory
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                // Set the open file type
                ofd.Filter = "|*.*";
                ofd.RestoreDirectory = true;
                string readFilePath = string.Empty;
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    // 获取待读取的文件
                    readFilePath = ofd.FileName;
                }
                if (readFilePath != string.Empty)
                {
                    // initialize 
                    // 筛选出选中的节点
                    UpdateSequence();
                    DataTableLayoutPanel.Controls.Clear();
                    DataTableLayoutPanel.RowCount = 1;
                    DataHeaderTableLayoutPanel = null;
                    DataContentTableLayoutPanel = null;
                    initReadProgressBar();
                    try
                    {
                        TreeNode rootNode = this.DFMLTreeView.Nodes[0];
                        TreeNodeTag rootNodeTag = (TreeNodeTag)rootNode.Tag;
                        rootElement = rootNodeTag.nodeElem;
                        string readMode = rootElement.GetAttribute("mode");
                        if (readMode == "byte")
                        {
                            ReadByteFile(readFilePath, rootNode);
                        }
                        else if (readMode == "char")
                        {
                            ReadCharaterFile(readFilePath);
                        }
                    }
                    catch (System.ArgumentOutOfRangeException)
                    {
                        MessageBox.Show("The read location exceeds the file length.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (System.IndexOutOfRangeException)
                    {
                        MessageBox.Show("The read location exceeds the file length.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    adjustDataTabel();
                }
            }
            ReadDataProgressBarPanel.SendToBack();
            DataTableLayoutPanel.Visible = true;
        }

        private void initReadProgressBar()
        {
            DataTableLayoutPanel.Visible = false;
            ReadDataProgressBarPanel.BringToFront();
            ReadDataProgressBar.Value = 0;
            ReadDataProgressBar.Maximum = 100;
            ReadDataProgressBar.Minimum = 0;
            ReadDataProgressBar.Step = 100 / linearSequence.Count();
        }

        /// <summary>
        /// 读取文本文件
        /// </summary>
        /// <param name="readFilePath"></param>
        private void ReadCharaterFile(string readFilePath)
        {
            string[] allLines = File.ReadAllLines(readFilePath);
            foreach (XmlElement element in linearSequence)
            {
                List<object> dataList = new List<object>();
                dataList = ReadCharaterData(element, allLines);
                string elementName = element.GetAttribute("description");
                // header
                if (element.HasAttribute("isHeaderElem") || elementName == "Unused")
                {
                    addToHeaderTable(elementName, dataList);
                }
                // content
                else
                {
                    addToContentTable(elementName, dataList);
                }
                ReadDataProgressBar.Value += ReadDataProgressBar.Step;
            }
        }

        /// <summary>
        /// 读取element代表的文本数据
        /// </summary>
        /// <param name="element"></param>
        /// <param name="allLines"></param>
        /// <returns></returns>
        private List<object> ReadCharaterData(XmlElement element, string[] allLines)
        {
            List<object> dataList = new List<object>();
            // Acquire the "name" value of element
            string elemName = element.Name;
            // Acquire the "startReadLocation" attribute value of element
            string startReadLocation = element.GetAttribute("startReadLocation");
            int startReadRow = int.Parse(startReadLocation.Split(' ')[0]);
            // 开始读取的列数
            int startReadColumn = int.Parse(startReadLocation.Split(' ')[1]);
            // Acquire the "length" attribute value of element
            int readLength = int.Parse(element.GetAttribute("length").Split(' ')[1]);
            // Acquire the value of "interval" attribute of element
            int interval = int.Parse(element.GetAttribute("interval"));
            // Acquire the value of "repetition" attribute of element
            int repetition = int.Parse(element.GetAttribute("repetition"));
            string dataItem = string.Empty;
            if (element.HasAttribute("targetIndex"))
            {
                int targetIndex = int.Parse(element.GetAttribute("targetIndex"));
                startReadRow = startReadRow + interval * (targetIndex - 1);
                string line = allLines[startReadRow];
                if (readLength >= 0)
                {
                    dataItem = line.Substring(startReadColumn, readLength);
                }
                else
                {
                    dataItem = line.Substring(startReadColumn, line.Length - startReadColumn);
                    dataItem = dataItem.Replace("\n", "");
                    dataItem = dataItem.Replace("\r", "");
                }
                dataList.Add(dataItem);
            }
            else
            {
                while (startReadRow < allLines.Length & repetition != 0)
                {
                    string line = allLines[startReadRow];
                    if (readLength >= 0)
                    {
                        dataItem = line.Substring(startReadColumn, readLength);
                    }
                    else
                    {
                        dataItem = line.Substring(startReadColumn, line.Length - startReadColumn);
                        dataItem = dataItem.Replace("\n", "");
                        dataItem = dataItem.Replace("\r", "");
                    }
                    startReadRow += interval;
                    repetition -= 1;
                    dataList.Add(dataItem);
                }
            }
            return dataList;
        }

        /// <summary>
        /// 读取二进制文件
        /// </summary>
        /// <param name="readFilePath"></param>
        private void ReadByteFile(string readFilePath,TreeNode rootNode)
        {
            FileStream fs = new FileStream(readFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader autoReader = new BinaryReader(fs);
            try
            {
                VarsDict.Add("FileLength", (int)autoReader.BaseStream.Length);
            }
            catch
            {
            }
            ReadByteData(rootNode, autoReader);
        }

        /// <summary>
        /// 调整tabel的样式
        /// </summary>
        private void adjustDataTabel()
        {
            if (DataContentTableLayoutPanel != null)
            {
                addToContentTable(" ", new List<object>());
                RichTextBox rowNameBox = new RichTextBox();
                rowNameBox.ScrollBars = RichTextBoxScrollBars.None;
                rowNameBox.BorderStyle = BorderStyle.None;
                rowNameBox.Text = " ";
                rowNameBox.Height = 20;
                rowNameBox.Width = 90;
                rowNameBox.AutoSize = true;
                rowNameBox.Margin = new System.Windows.Forms.Padding(0);
                DataContentTableLayoutPanel.Controls.Add(rowNameBox, 0, DataContentTableLayoutPanel.RowCount + 1);
            }
            if (DataContentTableLayoutPanel != null && DataHeaderTableLayoutPanel != null)
            {
                foreach (RowStyle rowStyle in DataTableLayoutPanel.RowStyles)
                {
                    rowStyle.SizeType = SizeType.Percent;
                    rowStyle.Height = 50;
                }
            }

            //Label colNameBox = new Label();
            //colNameBox.BorderStyle = BorderStyle.None;
            //colNameBox.Text = "Description";
            //colNameBox.Height = 20;
            //colNameBox.Width = 90;
            //colNameBox.Margin = Padding.Empty;
            //colNameBox.BorderStyle = BorderStyle.None;
            //colNameBox.Margin = new System.Windows.Forms.Padding(0);
            //colNameBox.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            //DataContentTableLayoutPanel.Controls.Add(colNameBox, 0, 0);
            //if (DataContentTableLayoutPanel.RowCount < 10)
            //{
            //    DataContentTableLayoutPanel.RowCount = 10;
            //}
            //if (DataContentTableLayoutPanel.ColumnCount < 10)
            //{
            //    DataContentTableLayoutPanel.ColumnCount = 10;
            //}
            //DataContentTableLayoutPanel.RowCount++;
            //DataContentTableLayoutPanel.ColumnCount++;
            //foreach (RowStyle style in DataContentTableLayoutPanel.RowStyles)
            //{
            //    style.SizeType = SizeType.Absolute;
            //    style.Height = 20;
            //}
            //foreach (ColumnStyle style in DataContentTableLayoutPanel.ColumnStyles)
            //{
            //    style.SizeType = SizeType.Absolute;
            //    style.Width = 90;
            //}
            //DataContentTableLayoutPanel.ColumnStyles[0].Width = 90;
            //DataContentTableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            //DataContentTableLayoutPanel.Refresh();
            //foreach (ColumnStyle style in DataHeaderTableLayoutPanel.ColumnStyles)
            //{
            //    style.SizeType = SizeType.Percent;
            //    style.Width = 100 / DataHeaderTableLayoutPanel.ColumnCount;
            //}
            //DataHeaderTableLayoutPanel.Refresh();
        }

        /// <summary>
        /// 将读取的数据添加到数据内容展示界面
        /// </summary>
        /// <param name="elementName"></param>
        /// <param name="dataList"></param>
        private void addToContentTable(string elementName, List<object> dataList)
        {
            if (DataContentTableLayoutPanel == null)
            {
                initDataContentTableLayouPanel();
            }
            DataContentTableLayoutPanel.ColumnCount++;
            int col = DataContentTableLayoutPanel.ColumnCount - 1;
            int row = 0;
            // table header
            Label colNameBox = new Label();
            colNameBox.BorderStyle = BorderStyle.None;
            colNameBox.Text = elementName;
            colNameBox.Height = 20;
            colNameBox.Width = 90;
            colNameBox.Margin = Padding.Empty;
            colNameBox.BorderStyle = BorderStyle.None;
            colNameBox.Margin = new System.Windows.Forms.Padding(0);
            if (elementName != " ")
            {
                colNameBox.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            }
            DataContentTableLayoutPanel.Controls.Add(colNameBox, col, row);

            // table content
            foreach (object data in dataList)
            {
                RichTextBox dataBox = new RichTextBox();
                dataBox.BorderStyle = BorderStyle.None;
                dataBox.ScrollBars = RichTextBoxScrollBars.None;
                dataBox.Text = data.ToString();
                dataBox.Height = 18;
                dataBox.Width = 90;
                dataBox.BorderStyle = BorderStyle.None;
                dataBox.Margin = new System.Windows.Forms.Padding(0);
                row += 1;
                if (row >= 100)
                {
                    dataBox.Text = "Too much data";
                    DataContentTableLayoutPanel.RowCount = row + 1;
                    RichTextBox rowNameBox = new RichTextBox();
                    rowNameBox.ScrollBars = RichTextBoxScrollBars.None;
                    rowNameBox.BorderStyle = BorderStyle.None;
                    rowNameBox.Text = row.ToString();
                    rowNameBox.Height = 20;
                    rowNameBox.Width = 90;
                    rowNameBox.AutoSize = true;
                    rowNameBox.BorderStyle = BorderStyle.None;
                    rowNameBox.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
                    rowNameBox.Margin = new System.Windows.Forms.Padding(0);
                    DataContentTableLayoutPanel.Controls.Add(dataBox, col, row);
                    break;
                }
                if (DataContentTableLayoutPanel.RowCount < row + 1)
                {
                    DataContentTableLayoutPanel.RowCount = row + 1;
                    RichTextBox rowNameBox = new RichTextBox();
                    rowNameBox.ScrollBars = RichTextBoxScrollBars.None;
                    rowNameBox.BorderStyle = BorderStyle.None;
                    rowNameBox.Text = row.ToString();
                    rowNameBox.Height = 20;
                    rowNameBox.Width = 90;
                    rowNameBox.AutoSize = true;
                    rowNameBox.BorderStyle = BorderStyle.None;
                    rowNameBox.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
                    rowNameBox.Margin = new System.Windows.Forms.Padding(0);
                    DataContentTableLayoutPanel.Controls.Add(rowNameBox, 0, row);
                }
                DataContentTableLayoutPanel.Controls.Add(dataBox, col, row);
            }
        }

        private void initDataContentTableLayouPanel()
        {
            DataContentTableLayoutPanel = new TableLayoutPanel();
            DataContentTableLayoutPanel.ColumnCount = 1;
            DataContentTableLayoutPanel.RowCount = 1;
            DataContentTableLayoutPanel.Dock = DockStyle.Fill;
            DataContentTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            DataContentTableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            Label colNameBox = new Label();
            colNameBox.BorderStyle = BorderStyle.None;
            colNameBox.Text = "Description";
            colNameBox.Height = 20;
            colNameBox.Width = 90;
            colNameBox.Margin = Padding.Empty;
            colNameBox.BorderStyle = BorderStyle.None;
            colNameBox.Margin = new System.Windows.Forms.Padding(0);
            colNameBox.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            DataContentTableLayoutPanel.AutoScroll = true;
            DataContentTableLayoutPanel.Controls.Add(colNameBox, 0, 0);
            DataTableLayoutPanel.Controls.Add(DataContentTableLayoutPanel);
        }

        /// <summary>
        /// 将读取的数据添加到头文件内容展示界面
        /// </summary>
        /// <param name="headerName"></param>
        /// <param name="dataList"></param>
        private void addToHeaderTable(string headerName, List<object> dataList)
        {
            if (DataHeaderTableLayoutPanel == null)
            {
                initDataHeaderTableLayoutPanel();
            }
            foreach (object dataValue in dataList)
            {
                string headerValue = dataValue.ToString();
                RichTextBox headerBox = new RichTextBox();
                headerBox.Text = string.Format("{0}: {1}", headerName, headerValue);
                headerBox.BorderStyle = BorderStyle.None;
                headerBox.Dock = DockStyle.Fill;
                headerBox.ScrollBars = RichTextBoxScrollBars.None;
                headerBox.Font = new Font("Times New Roman", 10);
                headerBox.Margin = new System.Windows.Forms.Padding(0);
                headerBox.Size = new System.Drawing.Size(new System.Drawing.Point(15, 30));
                DataHeaderTableLayoutPanel.Controls.Add(headerBox);
            }    
        }

        private void initDataHeaderTableLayoutPanel()
        {
            DataHeaderTableLayoutPanel = new TableLayoutPanel();
            DataHeaderTableLayoutPanel.ColumnCount = 1;
            DataHeaderTableLayoutPanel.RowCount = 1;
            DataHeaderTableLayoutPanel.Dock = DockStyle.Fill;
            DataHeaderTableLayoutPanel.AutoScroll = true;
            DataHeaderTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            // DataHeaderTableLayoutPanel.Size = new System.Drawing.Size(990, 101);
            DataTableLayoutPanel.Controls.Add(DataHeaderTableLayoutPanel);

        }

        /// <summary>
        /// 读取element代表的二进制数据
        /// </summary>
        /// <param name="element"></param>
        /// <param name="autoReader"></param>
        /// <returns></returns>
        private List<object> ReadByteData(TreeNode node, BinaryReader autoReader)
        {
            TreeNodeTag tag = (TreeNodeTag)node.Tag;
            List<object> dataList = new List<object>();
            if (node.Nodes.Count > 0)
            {
                foreach (TreeNode childNode in node.Nodes)
                {
                    dataList.Add(ReadByteData(childNode, autoReader));
                }
                return dataList;
            }
            else
            {
                // Acquire the "name" value of element
                string elemName = tag.nodeName;
                // Acquire the "startReadLocation" attribute value of element
                int startReadLocation = int.Parse(DynamicCalculation(tag.nodeStartLocation));
                // Acquire the "length" attribute value of element
                int readLength = int.Parse(DynamicCalculation(tag.nodeLength));

                // Acquire the value of "interval" attribute of element
                int interval = int.Parse(DynamicCalculation(tag.nodeInterval));

                // Acquire the value of "repetition" attribute of element
                float repetition = float.Parse(DynamicCalculation(tag.nodeRepitition));
                byte[] bytes;
                object dataItem;
                if (tag.nodeIndex != "full read")
                {
                    int targetIndex = int.Parse(tag.nodeIndex);
                    startReadLocation = startReadLocation + interval * (targetIndex - 1);
                    autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                    bytes = autoReader.ReadBytes(readLength);
                    string byteOrder = tag.nodeElem.GetAttribute("byteOrder");
                    if (byteOrder == "bigEndian")
                    {
                        Array.Reverse(bytes);
                    }
                    switch (tag.nodeType)
                    {
                        case "string":
                            dataItem = BitConverter.ToString(bytes);
                            break;
                        case "integer":
                            dataItem = BitConverter.ToInt32(bytes, 0);
                            break;
                        case "real":
                            dataItem = BitConverter.ToDouble(bytes, 0);
                            break;
                        case "boolean":
                            dataItem = BitConverter.ToBoolean(bytes, 0);
                            break;
                        case "date":
                            dataItem = BitConverter.ToString(bytes, 0);
                            break;
                        case "time":
                            dataItem = TimeSpan.Parse(BitConverter.ToString(bytes, 0));
                            break;
                        case "datetime":
                            dataItem = Convert.ToDateTime(BitConverter.ToString(bytes, 0));
                            break;
                        case "path":
                            dataItem = @BitConverter.ToString(bytes, 0);
                            break;
                        default:
                            dataItem = "unexpected data type";
                            break;
                    }
                    if (tag.nodeElem.HasAttribute("VarName"))
                    {
                        try
                        {
                            VarsDict.Add(tag.nodeElem.GetAttribute("VarName"), dataItem);
                        }
                        catch
                        {
                            VarsDict["VarName"] = dataItem;
                        }
                    }
                    dataList.Add(dataItem);
                }
                else
                {
                    while (startReadLocation + readLength <= autoReader.BaseStream.Length && repetition != 0)
                    {
                        autoReader.BaseStream.Seek(startReadLocation, SeekOrigin.Begin);
                        bytes = autoReader.ReadBytes(readLength);
                        string byteOrder = tag.nodeElem.GetAttribute("byteOrder");
                        if (byteOrder == "bigEndian")
                        {
                            Array.Reverse(bytes);
                        }
                        switch (tag.nodeType)
                        {
                            case "string":
                                dataItem = BitConverter.ToString(bytes);
                                break;
                            case "integer":
                                dataItem = BitConverter.ToInt32(bytes, 0);
                                break;
                            case "real":
                                dataItem = BitConverter.ToDouble(bytes, 0);
                                break;
                            case "boolean":
                                dataItem = BitConverter.ToBoolean(bytes, 0);
                                break;
                            case "date":
                                dataItem = BitConverter.ToString(bytes, 0);
                                break;
                            case "time":
                                dataItem = TimeSpan.Parse(BitConverter.ToString(bytes, 0));
                                break;
                            case "datetime":
                                dataItem = Convert.ToDateTime(BitConverter.ToString(bytes, 0));
                                break;
                            case "path":
                                dataItem = @BitConverter.ToString(bytes, 0);
                                break;
                            default:
                                dataItem = "unexpected data type";
                                break;
                        }
                        if (tag.nodeElem.HasAttribute("VarName"))
                        {
                            try
                            {
                                VarsDict.Add(tag.nodeElem.GetAttribute("VarName"), dataItem);
                            }
                            catch
                            {
                                VarsDict["VarName"] = dataItem;
                            }
                        }
                        dataList.Add(dataItem);
                        startReadLocation += interval;
                        repetition -= 1;
                    }
                }
                if (dataList.Count == 1)
                {
                    addToHeaderTable(elemName, dataList);
                }
                else
                {
                    addToContentTable(elemName, dataList);
                }
                ReadDataProgressBar.Value += ReadDataProgressBar.Step;
            }
            return dataList;
        }

        /// <summary>
        /// 动态生成DFML Tree
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DFMLListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DFML DFMLFile = (DFML)DFMLListBox.SelectedItem;
            if (DFMLFile != null)
            {
                InitTreeView();
            }
        }

        /// <summary>
        /// 生成DFML Tree
        /// </summary>
        private void InitTreeView()
        {

            DFML DFMLFile = (DFML)DFMLListBox.SelectedItem;
            if (DFMLFile != null)
            {
                // 获取选中的DFML文档
                DFMLFilePath = DFMLFile.filePath;
                // Load the DFML data in DFMLFilePath as a XML object
                XmlDocument DFMLXmlObject = new XmlDocument();
                // Load the DFML data in DFMLFilePath as a XML object
                DFMLXmlObject.Load(DFMLFilePath);
                // Acquire the root element of DFMLXmlObject
                rootElement = DFMLXmlObject.DocumentElement;
                DFMLTreeView.Nodes.Clear();
                TreeNode treeNode = new TreeNode();
                TreeNode childTreeNode = new TreeNode();
                TreeNode grandChildTreeNode = new TreeNode();
                string rootName = rootElement.GetAttribute("name");
                if (rootName == string.Empty)
                {
                    rootName = "dataformat";
                }
                treeNode = DFMLTreeView.Nodes.Add(rootName);
                treeNode.NodeFont = new Font("Times New Roman", 9, System.Drawing.FontStyle.Bold);
                treeNode.NodeFont = new Font("Times New Roman", 9, System.Drawing.FontStyle.Bold);
                treeNode.Tag = new TreeNodeTag(rootElement);
                TreeNodeTag rootTag = (TreeNodeTag)treeNode.Tag;
                rootTag.nodeLength = "FileLength";
                //treeNode.Name = rootElement.GetAttribute("ID");
                foreach (XmlElement childElem in rootElement)
                {
                    CreatChildTree(treeNode, childElem);
                }
                checkedAllNodes(DFMLTreeView.Nodes);
                DFMLTreeView.ExpandAll();
                DFMLTreeView.Nodes[0].EnsureVisible();
            }
        }

        private void CreatChildTree(TreeNode treeNode, XmlElement childElem)
        {
            if (basicDataType.Contains(childElem.Name) || childElem.Name == "group")
            {
                TreeNodeTag parentNodeTag = (TreeNodeTag)treeNode.Tag;
                string chilElemDescription = childElem.GetAttribute("description");
                TreeNode childTreeNode = treeNode.Nodes.Add(chilElemDescription);
                //childTreeNode.Name = childElem.GetAttribute("ID");
                
                childTreeNode.Tag = new TreeNodeTag(childElem);
                TreeNodeTag nodeTag = (TreeNodeTag)childTreeNode.Tag;

                if (parentNodeTag.nodeType == "group")
                {
                    try
                    {
                        nodeTag.nodeStartLocation = (int.Parse(nodeTag.nodeStartLocation) + int.Parse(parentNodeTag.nodeStartLocation)).ToString();
                    }
                    catch
                    {
                        nodeTag.nodeStartLocation = string.Format("({0})+({1})", nodeTag.nodeStartLocation, parentNodeTag.nodeStartLocation);
                    }
                    try
                    {
                        nodeTag.nodeEndLocation = (int.Parse(nodeTag.nodeEndLocation) + int.Parse(parentNodeTag.nodeStartLocation)).ToString();
                    }
                    catch
                    {
                        nodeTag.nodeEndLocation = string.Format("({0})+({1})", nodeTag.nodeEndLocation, parentNodeTag.nodeStartLocation);
                    }
                }

                string childElemRepitition = "1";
                if (parentNodeTag.nodeName == "dataformat")
                {
                    childElemRepitition = "1";
                }
                else
                {
                    try
                    {
                        childElemRepitition = (int.Parse(parentNodeTag.nodeLength) / int.Parse(parentNodeTag.nodeInterval)).ToString();
                    }
                    catch
                    {
                        childElemRepitition = string.Format("({0}/ {1})", parentNodeTag.nodeLength, parentNodeTag.nodeInterval);
                    }
                }
                foreach (XmlElement brotherElem in parentNodeTag.nodeElem.ChildNodes)
                {
                    string locationAtt = brotherElem.GetAttribute("location");
                    if (locationAtt.Split(',')[1] == "-1")
                    {
                        childElemRepitition = "1";
                        break;
                      }
                }
                nodeTag.nodeRepitition = childElemRepitition;
                if (chilElemDescription == string.Empty)
                {
                    chilElemDescription = childElem.Name;
                }
                if (childElem.Name == "group")
                {
                    childTreeNode.NodeFont = new Font("Times New Roman", 9, System.Drawing.FontStyle.Bold);
                }
                else
                {
                    childTreeNode.NodeFont = new Font("Times New Roman", 9);
                }

                foreach (XmlElement grandChildElem in childElem.ChildNodes)
                {
                    CreatChildTree(childTreeNode, grandChildElem);
                }
            }
        }

        ///// <summary>
        ///// 标记节点与子节点
        ///// </summary>
        ///// <param name="rootElement">根节点</param>
        ///// <param name="startID">开始标记的ID</param>
        //private void IDElements(XmlElement rootElement, ref int startIDID)
        //{
        //    rootElement.SetAttribute("ID", (startIDID++).ToString());
        //    foreach (XmlElement childElement in rootElement)
        //    {
        //        IDElements(childElement, ref startIDID);
        //    }
        //}

        /// <summary>
        /// 根据选中的子节点，更新线性序列
        /// </summary>
        private void UpdateSequence()
        {
            linearSequence.Clear();
            // 获取所有被选中的叶子节点
            GetAllCheckedLeafNodes(DFMLTreeView.Nodes, linearSequence);
        }

        ///// <summary>
        ///// 为线性序列中
        ///// </summary>
        ///// <param name="linearSequence"></param>
        //private void SetRandomRead(List<XmlElement> linearSequence)
        //{
        //    foreach (XmlElement element in linearSequence)
        //    {
        //        TreeNode targetNode = FindNodeByTag(element.GetAttribute("tag"),DFMLTreeView.Nodes);
        //        if (targetNode != null)
        //        {
        //            try
        //            {
        //                string targetIndex = Regex.Split(targetNode.Text, "   ", RegexOptions.IgnoreCase)[1];
        //                // 去除左右括号
        //                targetIndex = targetIndex.Replace("(", "");
        //                targetIndex = targetIndex.Replace(")", "");
        //                element.SetAttribute("targetIndex", targetIndex);
        //            }
        //            catch
        //            {

        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// 获取所有被选中的叶子节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="linearSequence"></param>
        private void GetAllCheckedLeafNodes(TreeNodeCollection nodes, List<XmlElement> linearSequence)
        {
            foreach (TreeNode node in nodes)
            {
                TreeNodeTag Tag = (TreeNodeTag)node.Tag;
                if(node.Nodes.Count == 0 && node.Checked == true)
                {
                    linearSequence.Add(Tag.nodeElem);
                }
                else
                {
                    GetAllCheckedLeafNodes(node.Nodes, linearSequence);
                }
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedAllNodesButton_Click(object sender, EventArgs e)
        {
            checkedAllNodes(DFMLTreeView.Nodes);
        }

        /// <summary>
        /// 选中DFML Tree中的所有节点
        /// </summary>
        /// <param name="nodes"></param>
        private void checkedAllNodes(TreeNodeCollection nodes)
        {
            foreach(TreeNode node in nodes)
            {
                node.Checked = true;
                checkedAllNodes(node.Nodes);
            }
        }

        /// <summary>
        /// 取消全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deselectButton_Click(object sender, EventArgs e)
        {
            deselectALL(DFMLTreeView.Nodes);
        }

        /// <summary>
        /// 取消选中DFML Tree中的所有节点
        /// </summary>
        /// <param name="nodes"></param>
        private void deselectALL(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                node.Checked = false;
                deselectALL(node.Nodes);
            }
        }

        /// <summary>
        /// 设置子节点与父节点同步选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DFMLTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                foreach (TreeNode node in e.Node.Nodes)
                {
                    node.Checked = e.Node.Checked;
                }
            }
        }

        /// <summary>
        /// 同步更新子节点的targetIndex
        /// </summary>
        /// <param name="node">当前节点</param>
        /// <param name="targetIndex">更新值</param>
        private void UpdateChildTreeNode(TreeNode node, string targetIndex)
        {
            TreeNodeTag Tag = (TreeNodeTag) node.Tag;
            Tag.nodeIndex = targetIndex;
            string nodeName = Tag.nodeName;
            XmlElement nodeElem = (XmlElement) Tag.nodeElem;
            if (targetIndex == "full read")
            {
                nodeElem.RemoveAttribute("targetIndex");
                node.Text = nodeName;
            }
            else
            {
                nodeElem.SetAttribute("targetIndex", targetIndex);
                node.Text = nodeName + new string(' ', 4) + string.Format("({0})", targetIndex);
            }
            foreach (TreeNode childNode in node.Nodes)
            {
                UpdateChildTreeNode(childNode, targetIndex);
            }
        }

        /// <summary>
        /// 遍历树，根据Tag返回对应节点
        /// </summary>
        /// <param name="Tag">目标标签</param>
        /// <param name="nodes">查找的节点集</param>
        /// <returns></returns>
        private TreeNode FindNodeByTag(string Tag,TreeNodeCollection nodes)
        {
            TreeNode targetNode = null;
            foreach(TreeNode node in nodes)
            {
                if (node.Tag.ToString() == Tag)
                {
                    return node;
                }
                else
                {
                    targetNode = FindNodeByTag(Tag,node.Nodes);
                }    
            }
            if (targetNode.Tag.ToString() == Tag)
            {
                return targetNode;
            }
            else
            {
                return null;
            }
            
        }

        /// <summary>
        /// 随机选择节点
        /// </summary>
        /// <param name="targetElement">被选中节点对应的xml元素</param>
        private string RandomSelectNode(TreeNode selectedNode)
        {
            TreeNodeTag tag = (TreeNodeTag)selectedNode.Tag;
            RandomReadForm randomSelectNodeForm = new RandomReadForm();
            //在随机窗口中显示要素的信息
            string description = tag.nodeName;
            addToRandomInfoTable("Description: " + description, randomSelectNodeForm);
            addToRandomInfoTable("", randomSelectNodeForm);
            string elementType = tag.nodeElem.Name;
            addToRandomInfoTable("Type: " + elementType, randomSelectNodeForm);
            try
            {
                randomSelectNodeForm.maxRepetition = int.Parse(tag.nodeRepitition);
            }
            catch
            {
                randomSelectNodeForm.maxRepetition = 1;
            }
            addToRandomInfoTable("Occurrence times: " + randomSelectNodeForm.maxRepetition, randomSelectNodeForm);
            
            randomSelectNodeForm.ShowDialog();
            // 随机读取的Index
            string targetIndex = randomSelectNodeForm.indexResult;
            tag.nodeIndex = targetIndex;
            return targetIndex;
        }

        /// <summary>
        /// 显示在随机读取窗口的信息显示栏中
        /// </summary>
        /// <param name="info">输入的信息</param>
        /// <param name="randomSelectNodeForm">显示信息的随机读取窗口</param>
        private void addToRandomInfoTable(string info, RandomReadForm randomSelectNodeForm)
        {
            Label infoBox = new Label();
            infoBox.Text = info;
            infoBox.AutoSize = true;
            infoBox.Font = new Font("Times New Roman", 10);
            randomSelectNodeForm.ElementTableLayoutPanel.Controls.Add(infoBox);
        }

        private void DFMLTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            //e.Node.ExpandAll();
            //TreeNodeTag Tag = (TreeNodeTag)e.Node.Tag;
            //XmlElement nodeElem = Tag.nodeElem;
            //if (nodeElem.Name != "dataformat")
            //{
            //    string targetIndex = randomSelectNode(nodeElem);
            //    if (targetIndex != string.Empty)
            //    {
            //        // 更新选中节点的所有子节点
            //        UpdateChildTreeNode(e.Node, targetIndex);
            //    }
            //}
        }

        private void DFMLTreeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (this.m_MouseClicks > 1)
            {
                //如果是鼠标双击则禁止结点折叠
                e.Cancel = true;
            }
            else
            {
                //如果是鼠标单击则允许结点折叠
                e.Cancel = false;
            }
        }

        private void DFMLTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (this.m_MouseClicks > 1)
            {
                //如果是鼠标双击则禁止结点展开
                e.Cancel = true;
            }
            else
            {
                //如果是鼠标单击则允许结点展开
                e.Cancel = false;
            }
        }

        /// <summary>
        /// 双击时进入读取模式选择界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DFMLTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.X > e.Node.Bounds.X)
            {
                e.Node.ExpandAll();
                TreeNodeTag Tag = (TreeNodeTag)e.Node.Tag;
                XmlElement nodeElem = Tag.nodeElem;
                if (nodeElem.Name != "dataformat")
                {
                    string targetIndex = RandomSelectNode(e.Node);
                    if (targetIndex != string.Empty)
                    {
                        // 更新选中节点的所有子节点
                        UpdateChildTreeNode(e.Node, targetIndex);
                    }
                }
            }
        }

        /// <summary>
        /// 记录鼠标点击的次数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DFMLTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            this.m_MouseClicks = e.Clicks;
        }
        public static object Calculate(string expression)
        {
            string className = "Calc";
            string methodName = "Run";
            expression = expression.Replace("/", "*1.0/");

            //设置编译参数
            CompilerParameters paras = new CompilerParameters();
            paras.GenerateExecutable = false;
            paras.GenerateInMemory = true;

            //创建动态代码
            StringBuilder classSource = new StringBuilder();
            classSource.Append("public class " + className + "\n");
            classSource.Append("{\n");
            classSource.Append(" public object " + methodName + "()\n");
            classSource.Append(" {\n");
            classSource.Append(" return " + expression + ";\n");
            classSource.Append(" }\n");
            classSource.Append("}");

            //编译代码
            CompilerResults result = new CSharpCodeProvider().CompileAssemblyFromSource(paras, classSource.ToString());

            //获取编译后的程序集。 
            Assembly assembly = result.CompiledAssembly;

            //动态调用方法。 
            object eval = assembly.CreateInstance(className);
            MethodInfo method = eval.GetType().GetMethod(methodName);
            object reobj = method.Invoke(eval, null);
            GC.Collect();
            return reobj;
        }

        /// <summary>
        /// 点击节点时，弹出随机读取选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void DFMLTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        //{
        //    //e.Node.ExpandAll();
        //    //TreeNodeTag Tag = (TreeNodeTag)e.Node.Tag;
        //    //XmlElement nodeElem = Tag.nodeElem;
        //    //if (nodeElem.Name != "dataformat")
        //    //{
        //    //    string targetIndex = randomSelectNode(nodeElem);
        //    //    if (targetIndex != string.Empty)
        //    //    {
        //    //        // 更新选中节点的所有子节点
        //    //        UpdateChildTreeNode(e.Node, targetIndex);
        //    //    }
        //    //}
        //}

    }
    /// <summary>
    /// DFML文档类
    /// </summary>
    class DFML
    {
        public string name;
        public string path;
        public DFML(string filePath)
        {
            path = filePath;
            name = filePath.Split('\\')[filePath.Split('\\').Length-1];

        }
        public string fileName
        {
            get
            {
                return name;
            }
        }
        public string filePath
        {
            get
            {
                return path;
            }
        }
    }
    
    /// <summary>
    /// 树节点的Tag类
    /// </summary>
    class TreeNodeTag
    {
        public string nodeType;
        public string nodeName;
        public XmlElement nodeElem;
        public string nodeLength;
        public string nodeRepitition;
        public string nodeInterval = "FileLength";
        public string nodeIndex;
        public string nodeStartLocation;
        public string nodeEndLocation;
        public TreeNodeTag(XmlElement nodeElem)
        {
            this.nodeType = nodeElem.Name;
            this.nodeIndex = "full read";
            this.nodeRepitition = "1";
           
            if (nodeElem.HasAttribute("description"))
            {
                this.nodeName = nodeElem.GetAttribute("description");
            }
            else
            {
                this.nodeName = "dataformat";
            }
            this.nodeElem = nodeElem;
            string locationAtt = nodeElem.GetAttribute("location");
            // Parse the start and end location form locationAtt
            this.nodeStartLocation = locationAtt.Split(',')[0];
            this.nodeEndLocation = locationAtt.Split(',')[1];
            this.nodeLength = CalculatLength(this.nodeStartLocation, this.nodeEndLocation);

            if (nodeElem.HasAttribute("interval"))
            {
                this.nodeInterval = nodeElem.GetAttribute("interval");
            }
            else
            {
                if (nodeElem.Name == "dataformat")
                {
                    this.nodeInterval = "1";
                }
                else if ((nodeElem.Name == "group"))
                {
                    foreach (XmlElement childElem in nodeElem.ChildNodes)
                    {
                        this.nodeInterval = "0";
                        string childLocation = childElem.GetAttribute("location");
                        // Parse the start and end location form locationAtt
                        string childnodeStartLocation = childLocation.Split(',')[0];
                        string chilnodeEndLocation = childLocation.Split(',')[1];
                        try
                        {
                            this.nodeInterval = (int.Parse(this.nodeInterval) + int.Parse(CalculatLength(childnodeStartLocation, chilnodeEndLocation))).ToString();
                        }
                        catch
                        {
                            this.nodeInterval = string.Format("({0}+{1})", this.nodeInterval, CalculatLength(childnodeStartLocation, chilnodeEndLocation));
                        }
                    }
                }
                else
                {
                    this.nodeInterval = this.nodeLength;
                }
            }
        }
        public string CalculatLength(string startLocation, string endLocation)
        {
            if (endLocation == "-1")
            {
                endLocation = "FileLength";
            }
            try
            {
                return (int.Parse(endLocation) - int.Parse(startLocation)).ToString();
            }
            catch
            {
                return string.Format("({0}-({1}))", endLocation, startLocation);
            }
        }
    }
}
