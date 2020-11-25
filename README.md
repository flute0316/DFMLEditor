# Computer Code Availability
- **Program language**: C#  
- **Software**: .NET Framework 4.7.2  
- **Developers**: Erjie Hu, Xinghua Cheng, Di Hu.  
- **Contact address**: School of Geography, Nanjing Normal University, No.1 Wenyuan Road, Xianlin University District Nanjing, China.   
- **Year first available**: 2020  
- **The contact telephone number**: (+86) 152-5099-2342  
- **E-mail**: hud316@gmail.com  
- **Program size**: about 784 KB.  
# Guide
## DFML Editor
### Overview
The editor is designed to convert multiple types of file formats into Data Format Markup Language(DFML) uniform Markup documents.The editor provides a concise interface for users to generate DFML documents based on simple rules.Following example will show how to use DFML Document Editor to edit node’s information and  generate DFML document.  

<img align="center" src="./Image/DFML Editor/DFML Editor user interface.jpg">  
<p align="center">Fig 1 DFML Editor user interface</p>  

### Add & Delete tree node.jpg
When user right click the tree node, a menu wiil show and includes two functions **Add nodes** and **Delete nodes**. **Add nodes** can create and add a new node under the selected node. The main elements of Add nodes include: adding Group、 Integer、 String、Semicolon、Real、Tab、Cr. **Delete nodes** can delete the selected node 

<img align="center" src="./Image/DFML Editor/Add & Delete tree node.jpg">  
<p align="center">Fig 2 Add & Delete tree node</p>  

## Automatic Generation of Reading Program V1.1
### Overview  
Automatic Generation of Reading Program(AGRP) is a kind of software which can build code and read data with guidance of DFML document. Following example will show how to use AGRP to generate read code and data reading of binary file and text file.  

<img align="center" src="./Image/AGRP/The user interface of AGRP.jpg">  
<p align="center">Fig 1 The user interface of AGRP</p>  

### Load DFML Document  
Click the **Load** button to load the DFML document, wchich should be **XML** type. Multiple documents can be loaded simultaneously in the AGRP and user can select the documents by click item. User can remove documents by clicking the **Remove** button.
 
<img align="center" src="./Image/AGRP/Load DFML.jpg">  
<p align="center">Fig 2 Load DFML</p>  

### DFML Tree
the DFML document has been loaded and shown as a **DFML Tree**. **DFML Tree** can help user understand the stracture easily and user can select data items by click the tree nodes.  

<img align="center" src="./Image/AGRP/Check the DFML tree to selecte the data to read.jpg">  
<p align="center">Fig 3 DFML tree</p>  

### Read Mode 
AGRP support two kind of read mode, including **Sequentially read** and **Randomly read**. User can set the read mode by double click the tree nodes.  

**Sequentially read** means that AGRP and the code , which AGRP generate, will read data sequentially until read the end of data.
**Randomly read** need one parameter **Index** to indicate the data item will be read, and the data item's index depends on the order that they appear in the data.

<img align="center" src="./Image/AGRP/Double check the tree node can select the read mode.jpg">  
<p align="center">Fig 4 Read mode</p> 

### Code Generation
User can click the **Generate** button to generate the read code corresponding to the selected DFML document. User. AGRP currently support code generation of C# and Python language. Code can be save by clicking **Save** button.

<img align="center" src="./Image/AGRP/Select program language of the code to generate.jpg">  
<p align="center">Fig 5 Code Generation</p> 

### Read File
User can read data by clicking the **Read File** button after DFML document was loaded. Base items will show as a key-value dict and group items will be parsed and show as a table.

<img align="center" src="./Image/AGRP/Click Read File button to read data(Sequentially read).jpg">  
<p align="center">Fig 6 Read file(Sequentially read)</p> 

<img align="center" src="./Image/AGRP/Click Read File button to read data(Randomly read).jpg">  
<p align="center">Fig 7 Read file(Randomly read)</p> 
