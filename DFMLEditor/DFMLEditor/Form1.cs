using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Web;
using System.Xml;
using System.IO;

namespace DFMLEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
          
            InitializeComponent();
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //设置背景为DFMLEditor
            //将图片放在程序启动的相对路径中
            string path1 = AppDomain.CurrentDomain.BaseDirectory + @"//background.png";
            this.BackgroundImage = Image.FromFile(path1);
        }
        TreeNode selected_node;    //创建一个被选择节点，将treeview的selectednode传给该节点
        TreeNode rootnode;    //创建树的根节点
        XElement node_element;    //定义一个节点元素用于接收节点对应信息

        //初始化tree
        private void InitTree(TreeView treeView)
        {
            TreeView tv = new TreeView
            {
                LabelEdit = true
            };
            rootnode = new TreeNode();
            rootnode =this.tv.Nodes.Add("dataformat");    //初始化一棵树使根节点为dataformat
            rootnode.Tag = new XElement("dataformat");    //设置根节点里存储的是XElment元素
            node_element = (XElement)rootnode.Tag;    //将根节点属性信息传到节点元素中
            rootnode.Text = "dataformat";
            selected_node = rootnode;
        }

        //添加节点到树视图
        private void AddNodeToTree(string nodename)
        {
            tv.LabelEdit = true;    //使节点处于可编辑状态
            TreeNode CurrentNode = tv.SelectedNode.Nodes.Add(nodename);     //将currentnode传到tree 
            //当前节点tag包含的是xelement元素，name为传进来的nodename
            CurrentNode.Tag = new XElement(nodename);    
            tv.LabelEdit = false;    //使节点处于不可编辑状态
            tv.ExpandAll();    //展开所有树节点
        }

        //删除节点
        private void RemoveNode()
        {
            tv.SelectedNode.Remove();    //将选中节点从树中移除
        }


        //解析节点与元素
        private void ParseNode()
        {
            rootnode = tv.Nodes[0];    //初始化一棵树使根节点为dataformat
            selected_node = tv.SelectedNode;
            if (selected_node.Tag == null)
            {
                selected_node = rootnode;
            }
            node_element = (XElement)selected_node.Tag;

        }



        //显示节点元素
        private void ShowNodeIfo(XElement element)
        {
            selected_node = tv.SelectedNode;
            node_element = (XElement)selected_node.Tag;
            if (selected_node.Text == "dataformat")
            {
                panel_group.Visible = false;
                panel_integer.Visible = false;
                panel_string.Visible = false;
                panel_semicolon.Visible = false;
                panel_real.Visible = false;
                panel_tab.Visible = false;
                panel_cr.Visible = false;
                ShowDataformatAtt();
            }
            if (selected_node.Text == "group")
            {
                panel_dataformat.Visible = false;
                panel_integer.Visible = false;
                panel_string.Visible = false;
                panel_semicolon.Visible = false;
                panel_real.Visible = false;
                panel_tab.Visible = false;
                panel_cr.Visible = false;
                ShowGrouptAtt();
            }
            if (selected_node.Text == "integer")
            {
                panel_dataformat.Visible = false;
                panel_group.Visible = false;
                panel_string.Visible = false;
                panel_semicolon.Visible = false;
                panel_real.Visible = false;
                panel_tab.Visible = false;
                panel_cr.Visible = false;
                ShowIntegerAtt();
            }
            if (selected_node.Text == "string")
            {
                panel_dataformat.Visible = false;
                panel_group.Visible = false;
                panel_integer.Visible = false;
                panel_semicolon.Visible = false;
                panel_real.Visible = false;
                panel_tab.Visible = false;
                panel_cr.Visible = false;
                ShowStringAtt();
            }
            if (selected_node.Text == "semicolon")
            {
                panel_dataformat.Visible = false;
                panel_group.Visible = false;
                panel_integer.Visible = false;
                panel_string.Visible = false;
                panel_real.Visible = false;
                panel_tab.Visible = false;
                panel_cr.Visible = false;

                ShowSemicolonAtt();
            }
            if (selected_node.Text == "real")
            {
                panel_dataformat.Visible = false;
                panel_group.Visible = false;
                panel_integer.Visible = false;
                panel_string.Visible = false;
                panel_semicolon.Visible = false;
                panel_tab.Visible = false;
                panel_cr.Visible = false;
                ShowRealAtt();
            }
            if (selected_node.Text == "tab")
            {
                panel_dataformat.Visible = false;
                panel_group.Visible = false;
                panel_integer.Visible = false;
                panel_string.Visible = false;
                panel_semicolon.Visible = false;
                panel_cr.Visible = false;
                panel_real.Visible = false;
                ShowTabAtt();
            }

            if (selected_node.Text == "cr")
            {
                panel_dataformat.Visible = false;
                panel_group.Visible = false;
                panel_integer.Visible = false;
                panel_string.Visible = false;
                panel_semicolon.Visible = false;
                panel_tab.Visible = false;
                panel_real.Visible = false;
                ShowCrAtt();
            }
        }

        //更新节点属性
        private void UpdateNodeIfo(XElement node_element)
        {
            if (selected_node.Text=="dataformat")
            {
                UpdateDataformatIfo();
            }
            if (selected_node.Text == "group")
            {
                UpdateGroupIfo();
            }
            if (selected_node.Text == "integer")
            {
                UpdateIntegerIfo();
            }
            if (selected_node.Text == "string")
            {
                UpdateStringIfo();
            }
            if (selected_node.Text == "semicolon")
            {
                UpdateSemicolonIfo();
            }
            if (selected_node.Text == "real")
            {
                UpdateRealIfo();
            }
            if (selected_node.Text == "tab")
            {
                UpdateTabIfo();
            }

            if (selected_node.Text == "cr")
            {
                UpdateCrIfo();
            }
           
        }

        //解析xml文档
        private void ParseXMLDoc(string filename)
        {
            XDocument document = XDocument.Load(filename);    //使用xDocument来读取xml文件
            XElement rootElement = document.Root;    //文档根节点赋给根元素
        }

        //从xml文档中解析节点
        private TreeNode CreatNode(XElement element)
        {
            //创建节点元素
            XElement NodeElement = new XElement(element.Name.ToString());
            foreach (XAttribute att in element.Attributes())
            {
                NodeElement.SetAttributeValue(att.Name, att.Value);
            }
            TreeNode node = new TreeNode();
            node.Text = element.Name.ToString();
            node.Tag = NodeElement;
            return node;
        }


        //将xml文档转化为treeview
        private void Xml2TreeView(XElement rootElement, TreeNodeCollection treeNodeCollection)
        {
            //从根节点元素创造节点
            TreeNode node = CreatNode(rootElement);
            //将加点加入树节点集合
            treeNodeCollection.Add(node);
            foreach (XElement element in rootElement.Elements())
            {
                Xml2TreeView(element, node.Nodes);
            }

        }

        //保存为xml文档
        private void SaveToXml()

        {
            XDocument xml = new XDocument();
            foreach (TreeNode node in tv.Nodes)
            {
               XElement e = CreateElements(node);
               xml.Add(e);
            }
            SaveFileDialog fd = new SaveFileDialog();    //选择文件保存位置
            fd.DefaultExt = ".xml";
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
               string filename = fd.FileName;
               xml.Save(filename);
            }

        }

        //创建节点元素
        private XElement CreateElements(TreeNode node)

        {
            XElement root = CreateElement(node);
          
            if (node.Nodes.Count > 0)    //如果非最底节点，则添加节点
            {
                foreach (TreeNode n in node.Nodes)

                {
                        XElement e = CreateElements(n);
                        root.Add(e);

                }
             
            }
            else    //如果是最底节点，添加空，可使显示格式为成对显示
            {
                root.Add("");
            }

            //如果第一次预览后，rootelement会包含旗下所有子节点信息，这时要把rootelement元素重新记录，返回root1

            if (preview_Click)
            {
                XElement root1=new XElement(root.Name .ToString());
                foreach (var att in root.Attributes())
                {
                    root1.SetAttributeValue(att.Name ,att.Value);
                }

                if (node.Nodes.Count > 0)    //如果非最底节点，则添加节点
                {
                    foreach (TreeNode n in node.Nodes)

                    {
                        XElement e = CreateElements(n);
                        root1.Add(e);

                    }

                }
                else    //如果是最底节点，添加空，可使显示格式为成对显示
                {
                    root1.Add("");
                }
                return root1;
            }
            return root;

        }
 

        //创建xml元素
        private XElement CreateElement(TreeNode node)

        {
                return (XElement)node.Tag;    //如果不是根节点，返回当前节点
        }

 
        //实现添加节点功能
        private void groupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNodeToTree("group");     //添加group节点
        }

        private void integerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNodeToTree("integer");    //添加integer节点
        }

        private void stringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNodeToTree("string");    //添加string节点
        }

        private void semicolonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNodeToTree("semicolon");    //添加semicolon节点
        }

        private void realToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNodeToTree("real");    //添加real节点

        }

        private void tabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNodeToTree("tab");    //添加tab节点
        }


        private void crToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNodeToTree("cr");    //添加cr节点
        }

        //实现删除节点功能
        private void 删除节点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveNode();
        }


        //下面是将不同节点对应的属性做成单独控件封装起来
        //dataformat属性控件
        private void ShowDataformatAtt()
        {
            //加载对应控件
            panel_dataformat.Controls.Add(labelDF_nodename);
            panel_dataformat.Controls.Add(textBoxDF_nodename);
            panel_dataformat.Controls.Add(labelDF_name);
            panel_dataformat.Controls.Add(textBoxDF_name);
            panel_dataformat.Controls.Add(labelDF_namespace);
            panel_dataformat.Controls.Add(textBoxDF_namespace);
            panel_dataformat.Controls.Add(labelDF_mode);
            panel_dataformat.Controls.Add(comboBoxDF_mode);
            panel_dataformat.Controls.Add(labelDF_location);
            panel_dataformat.Controls.Add(textBoxDF_location);
            panel_dataformat.Controls.Add(labelDF_description);
            panel_dataformat.Controls.Add(textBoxDF_description);
            panel_dataformat.Controls.Add(button_Update);
            panel_dataformat.Controls.Add(btn_edit);
            //控件可见
            panel_dataformat.Visible = true;
            labelDF_nodename.Visible = true;
            textBoxDF_nodename.Visible = true;
            labelDF_name.Visible = true;
            textBoxDF_name.Visible = true;
            labelDF_namespace.Visible = true;
            textBoxDF_namespace.Visible = true;
            labelDF_mode.Visible = true;
            comboBoxDF_mode.Visible = true;
            labelDF_location.Visible = true;
            textBoxDF_location.Visible = true;
            labelDF_description.Visible = true;
            textBoxDF_description.Visible = true;
            textBoxDF_nodename.Text = "dataformat";
            //设置文本框不可修改
            textBoxDF_nodename.ReadOnly = true;
            textBoxDF_name.ReadOnly = true;
            textBoxDF_namespace.ReadOnly = true;
            textBoxDF_location.ReadOnly = true;
            textBoxDF_description.ReadOnly = true;
            selected_node = tv.SelectedNode;
            if (selected_node.Tag == null)
            {
                selected_node.Tag = rootnode.Tag;
            }
            node_element = (XElement)selected_node.Tag;

            if (node_element.Attribute("name") == null)
            {
                textBoxDF_name.Text = "";
            }
            else
            {
                textBoxDF_name.Text = node_element.Attribute("name").Value;
            }
            if (node_element.Attribute("namespace") == null)
            {
                textBoxDF_namespace.Text = "";
            }
            else
            {
                textBoxDF_namespace.Text = node_element.Attribute("namespace").Value;
            }
            if (node_element.Attribute("mode") == null)
            {
                comboBoxDF_mode.Text = "";
            }
            else
            {
                comboBoxDF_mode.Text = node_element.Attribute("mode").Value;
            }
            if (node_element.Attribute("location") == null)
            {
                textBoxDF_location.Text = "";
            }
            else
            {
                textBoxDF_location.Text = node_element.Attribute("location").Value;
            }
            if (node_element.Attribute("description") == null)
            {
                textBoxDF_description.Text = "";
            }
            else
            {
                textBoxDF_description.Text = node_element.Attribute("description").Value;
            }

        }

        //group属性控件
        private void ShowGrouptAtt()
        {
            panel_group.Controls.Add(labelGP_nodename);
            panel_group.Controls.Add(textBoxGP_nodename);
            panel_group.Controls.Add(labelGP_location);
            panel_group.Controls.Add(textBoxGP_location);
            panel_group.Controls.Add(labelGP_description);
            panel_group.Controls.Add(textBoxGP_description);
            panel_group.Controls.Add(button_Update);
            panel_group.Controls.Add(btn_edit);

            panel_group.Visible = true;
            labelGP_nodename.Visible = true;
            textBoxGP_nodename.Visible = true;
            labelGP_location.Visible = true;
            textBoxGP_location.Visible = true;
            labelGP_description.Visible = true;
            textBoxGP_description.Visible = true;
            labelDF_mode.Visible = true;
            textBoxGP_nodename.Text = "group";
            //设置文本框不可修改
            textBoxGP_nodename.ReadOnly = true;
            textBoxGP_location.ReadOnly = true;
            textBoxGP_description.ReadOnly = true;

            selected_node = tv.SelectedNode;
            if (selected_node.Tag == null)
            {
                selected_node.Tag = rootnode.Tag;
            }
            node_element = (XElement)selected_node.Tag;

            if (node_element.Attribute("location") == null)
            {
                textBoxGP_location.Text = "";
            }
            else
            {
                textBoxGP_location.Text = node_element.Attribute("location").Value;
            }
            if (node_element.Attribute("description") == null)
            {
                textBoxGP_description.Text = "";
            }
            else
            {
                textBoxGP_description.Text = node_element.Attribute("description").Value;
            }
        }

        //integer属性控件
        private void ShowIntegerAtt()
        {
            panel_integer.Controls.Add(labelIT_nodename);
            panel_integer.Controls.Add(textBoxIT_nodename);
            panel_integer.Controls.Add(labelIT_value);
            panel_integer.Controls.Add(textBoxIT_value);
            panel_integer.Controls.Add(labelIT_description);
            panel_integer.Controls.Add(textBoxIT_description);
            panel_integer.Controls.Add(labelIT_byteorder);
            panel_integer.Controls.Add(comboBoxIT_byteorder);
            panel_integer.Controls.Add(labelIT_location);
            panel_integer.Controls.Add(textBox1IT_location);
            panel_integer.Controls.Add(labelIT_number);
            panel_integer.Controls.Add(textBoxIT_number);
            panel_integer.Controls.Add(button_Update);
            panel_integer.Controls.Add(btn_edit);

            panel_integer.Visible = true;
            labelIT_nodename.Visible = true;
            textBoxIT_nodename.Visible = true;
            labelIT_value.Visible = true;
            textBoxIT_value.Visible = true;
            labelIT_description.Visible = true;
            textBoxIT_description.Visible = true;
            labelIT_byteorder.Visible = true;
            comboBoxIT_byteorder.Visible = true;
            labelIT_location.Visible = true;
            textBox1IT_location.Visible = true;
            labelIT_number.Visible = true;
            textBoxIT_number.Visible = true;
            textBoxIT_nodename.Text = "integer";

            textBoxIT_value.ReadOnly = true;
            textBoxIT_description.ReadOnly = true;
            textBox1IT_location.ReadOnly = true;
            textBoxIT_number.ReadOnly = true;
            textBoxIT_nodename.ReadOnly = true;

            selected_node = tv.SelectedNode;
            if (selected_node.Tag == null)
            {
                selected_node.Tag = rootnode.Tag;
            }
            node_element = (XElement)selected_node.Tag;

            if (node_element.Attribute("location") == null)
            {
                textBox1IT_location.Text = "";
            }
            else
            {
                textBox1IT_location.Text = node_element.Attribute("location").Value;
            }
            if (node_element.Attribute("description") == null)
            {
                textBoxIT_description.Text = "";
            }
            else
            {
                textBoxIT_description.Text = node_element.Attribute("description").Value;
            }
            if (node_element.Attribute("value") == null)
            {
                textBoxIT_value.Text = "";
            }
            else
            {
                textBoxIT_value.Text = node_element.Attribute("value").Value;
            }
            if (node_element.Attribute("byteOrder") == null)
            {
                comboBoxIT_byteorder.Text = "";
            }
            else
            {
                comboBoxIT_byteorder.Text = node_element.Attribute("byteOrder").Value;
            }
            if (node_element.Attribute("number") == null)
            {
                textBoxIT_number.Text = "";
            }
            else
            {
                textBoxIT_number.Text = node_element.Attribute("number").Value;
            }

        }

        //string属性控件
        private void ShowStringAtt()
        {
            panel_string.Controls.Add(labelST_nodename);
            panel_string.Controls.Add(textBoxST_nodename);
            panel_string.Controls.Add(labelST_value);
            panel_string.Controls.Add(textBoxST_value);
            panel_string.Controls.Add(labelST_description);
            panel_string.Controls.Add(textBoxST_description);
            panel_string.Controls.Add(labelST_location);
            panel_string.Controls.Add(textBoxST_location);
            panel_string.Controls.Add(button_Update);
            panel_string.Controls.Add(btn_edit);

            panel_string.Visible = true;
            labelST_nodename.Visible = true;
            textBoxST_nodename.Visible = true;
            labelST_value.Visible = true;
            textBoxST_value.Visible = true;
            labelST_description.Visible = true;
            textBoxST_description.Visible = true;
            labelST_location.Visible = true;
            textBoxST_location.Visible = true;
            textBoxST_nodename.Text = "string";

            textBoxST_nodename.ReadOnly = true;
            textBoxST_value.ReadOnly = true;
            textBoxST_description.ReadOnly = true;
            textBoxST_location.ReadOnly = true;

            selected_node = tv.SelectedNode;
            if (selected_node.Tag == null)
            {
                selected_node.Tag = rootnode.Tag;
            }
            node_element = (XElement)selected_node.Tag;

            if (node_element.Attribute("location") == null)
            {
                textBoxST_location.Text = "";
            }
            else
            {
                textBoxST_location.Text = node_element.Attribute("location").Value;
            }
            if (node_element.Attribute("description") == null)
            {
                textBoxST_description.Text = "";
            }
            else
            {
                textBoxST_description.Text = node_element.Attribute("description").Value;
            }
            if (node_element.Attribute("value") == null)
            {
                textBoxST_value.Text = "";
            }
            else
            {
                textBoxST_value.Text = node_element.Attribute("value").Value;
            }

        }

        //semicolon属性控件
        private void ShowSemicolonAtt()
        {
            panel_semicolon.Controls.Add(labelSE_nodename);
            panel_semicolon.Controls.Add(textBoxSE_nodename);
            panel_semicolon.Controls.Add(labelSE_location);
            panel_semicolon.Controls.Add(textBoxSE_location);
            panel_semicolon.Controls.Add(labelSE_description);
            panel_semicolon.Controls.Add(textBoxSE_description);
            panel_semicolon.Controls.Add(button_Update);
            panel_semicolon.Controls.Add(btn_edit);

            panel_semicolon.Visible = true;
            labelSE_nodename.Visible = true;
            textBoxSE_nodename.Visible = true;
            labelSE_location.Visible = true;
            textBoxSE_location.Visible = true;
            labelSE_description.Visible = true;
            textBoxSE_description.Visible = true;
            textBoxSE_nodename.Text = "semicolon";

            textBoxSE_nodename.ReadOnly = true;
            textBoxSE_location.ReadOnly = true;
            textBoxSE_description.ReadOnly = true;

            selected_node = tv.SelectedNode;
            if (selected_node.Tag == null)
            {
                selected_node.Tag = rootnode.Tag;
            }
            node_element = (XElement)selected_node.Tag;

            if (node_element.Attribute("location") == null)
            {
                textBoxSE_location.Text = "";
            }
            else
            {
                textBoxSE_location.Text = node_element.Attribute("location").Value;
            }
            if (node_element.Attribute("description") == null)
            {
                textBoxSE_description.Text = "";
            }
            else
            {
                textBoxSE_description.Text = node_element.Attribute("description").Value;
            }

        }

        //real属性控件
        private void ShowRealAtt()
        {
            panel_real.Controls.Add(labelRE_nodename);
            panel_real.Controls.Add(textBoxRE_nodename);
            panel_real.Controls.Add(labelRE_value);
            panel_real.Controls.Add(textBoxRE_value);
            panel_real.Controls.Add(labelRE_description);
            panel_real.Controls.Add(textBoxRE_description);
            panel_real.Controls.Add(labelRE_location);
            panel_real.Controls.Add(textBoxRE_location);
            panel_real.Controls.Add(labelRE_byteorder);
            panel_real.Controls.Add(comboBoxRE_byteorder);
            panel_real.Controls.Add(button_Update);
            panel_real.Controls.Add(btn_edit);

            panel_real.Visible = true;
            labelRE_nodename.Visible = true;
            textBoxRE_nodename.Visible = true;
            labelRE_value.Visible = true;
            textBoxRE_value.Visible = true;
            labelRE_description.Visible = true;
            textBoxRE_description.Visible = true;
            labelRE_location.Visible = true;
            textBoxRE_location.Visible = true;
            labelRE_byteorder.Visible = true;
            comboBoxRE_byteorder.Visible = true;
            textBoxRE_nodename.Text = "real";

            textBoxRE_nodename.ReadOnly = true;
            textBoxRE_value.ReadOnly = true;
            textBoxRE_description.ReadOnly = true;
            textBoxRE_location.ReadOnly = true;

            selected_node = tv.SelectedNode;
            if (selected_node.Tag == null)
            {
                selected_node.Tag = rootnode.Tag;
            }
            node_element = (XElement)selected_node.Tag;

            if (node_element.Attribute("location") == null)
            {
                textBoxRE_location.Text = "";
            }
            else
            {
                textBoxRE_location.Text = node_element.Attribute("location").Value;
            }
            if (node_element.Attribute("description") == null)
            {
                textBoxRE_description.Text = "";
            }
            else
            {
                textBoxRE_description.Text = node_element.Attribute("description").Value;
            }
            if (node_element.Attribute("value") == null)
            {
                textBoxRE_value.Text = "";
            }
            else
            {
                textBoxRE_value.Text = node_element.Attribute("value").Value;
            }
            if (node_element.Attribute("byteOrder") == null)
            {
                comboBoxRE_byteorder.Text = "";
            }
            else
            {
                comboBoxRE_byteorder.Text = node_element.Attribute("byteOrder").Value;
            }

        }

        //tab属性控件
        private void ShowTabAtt()
        {
            panel_tab.Controls.Add(labelTB_nodename);
            panel_tab.Controls.Add(textBoxTB_nodename);
            panel_tab.Visible=true;
            labelTB_nodename.Visible = true;
            textBoxTB_nodename.Visible = true;
            textBoxTB_nodename.Text = "tab";
        }


        //cr属性控件
        private void ShowCrAtt()
        {
            panel_cr.Controls.Add(labelCR_nodename);
            panel_cr.Controls.Add(textBoxCR_nodename);
            panel_cr.Visible = true;
            labelCR_nodename.Visible = true;
            textBoxCR_nodename.Visible = true;
            textBoxCR_nodename.Text = "cr";
        }

        //下列方法是针对不同节点实现属性可编辑
        //dataformat节点属性可编辑
        private void EditDataformat()
        {
            textBoxDF_nodename.ReadOnly = false;
            textBoxDF_name.ReadOnly = false;
            textBoxDF_namespace.ReadOnly = false;
            textBoxDF_location.ReadOnly = false;
            textBoxDF_description.ReadOnly = false;
        }

        //group节点属性可编辑
        private void EditGroup()
        {
            textBoxGP_nodename.ReadOnly = false;
            textBoxGP_location.ReadOnly = false;
            textBoxGP_description.ReadOnly = false;
        }

        //integer节点属性可编辑
        private void EditInteger()
        {
            textBoxIT_value.ReadOnly = false;
            textBoxIT_description.ReadOnly = false;
            textBox1IT_location.ReadOnly = false;
            textBoxIT_number.ReadOnly = false;
            textBoxIT_nodename.ReadOnly = false;
        }

        //string节点属性可编辑
        private void EditString()
        {
            textBoxST_nodename.ReadOnly = false;
            textBoxST_value.ReadOnly = false;
            textBoxST_description.ReadOnly = false;
            textBoxST_location.ReadOnly = false;
        }

        //semicolon节点属性可编辑
        private void EditSemicolon()
        {
            textBoxSE_nodename.ReadOnly = false;
            textBoxSE_location.ReadOnly = false;
            textBoxSE_description.ReadOnly = false;
        }


        //real节点属性可编辑
        private void EditReal()
        {
            textBoxRE_nodename.ReadOnly = false;
            textBoxRE_value.ReadOnly = false;
            textBoxRE_description.ReadOnly = false;
            textBoxRE_location.ReadOnly = false;
        }

        //设置对应节点不可编辑
        //dataformat节点属性不可编辑
        private void UnEditDataformat()
        {
            textBoxDF_nodename.ReadOnly = true;
            textBoxDF_name.ReadOnly = true;
            textBoxDF_namespace.ReadOnly = true;
            textBoxDF_location.ReadOnly = true;
            textBoxDF_description.ReadOnly = true;
        }

        //group节点属性不可编辑
        private void UnEditGroup()
        {
            textBoxGP_nodename.ReadOnly = true;
            textBoxGP_location.ReadOnly = true;
            textBoxGP_description.ReadOnly = true;
        }

        //integer节点属性不可编辑
        private void UnEditInteger()
        {
            textBoxIT_value.ReadOnly = true;
            textBoxIT_description.ReadOnly = true;
            textBox1IT_location.ReadOnly = true;
            textBoxIT_number.ReadOnly = true;
            textBoxIT_nodename.ReadOnly = true;
        }

        //string节点属性不可编辑
        private void UnEditString()
        {
            textBoxST_nodename.ReadOnly = true;
            textBoxST_value.ReadOnly = true;
            textBoxST_description.ReadOnly = true;
            textBoxST_location.ReadOnly = true;
        }

        //semicolon节点属性不可编辑
        private void UnEditSemicolon()
        {
            textBoxSE_nodename.ReadOnly = true;
            textBoxSE_location.ReadOnly = true;
            textBoxSE_description.ReadOnly = true;
        }


        //real节点属性不可编辑
        private void UnEditReal()
        {
            textBoxRE_nodename.ReadOnly = true;
            textBoxRE_value.ReadOnly = true;
            textBoxRE_description.ReadOnly = true;
            textBoxRE_location.ReadOnly = true;
        }

        //下列方法是针对不同节点更新其属性信息
        //dataformat节点属性更新
        private void UpdateDataformatIfo()
        {
            node_element = (XElement)selected_node.Tag;
            if (selected_node.Tag == null)    //先判断初始化根节点对信息传入有无影响
            {
                selected_node = rootnode;
            }
            if (textBoxDF_name.Text != "")
            {
                node_element.SetAttributeValue("name", textBoxDF_name.Text);
            }

            if (textBoxDF_namespace.Text != "")
            {
                node_element.SetAttributeValue("namespace", textBoxDF_namespace.Text);
            }
            if (comboBoxDF_mode.Text != "")
            {
                node_element.SetAttributeValue("mode", comboBoxDF_mode.Text);
            }
            if (textBoxDF_location.Text != "")
            {
                node_element.SetAttributeValue("location", textBoxDF_location.Text);
            }
            if (textBoxDF_description.Text != "")
            {
                node_element.SetAttributeValue("description", textBoxDF_description.Text);
            }
           
        }

        //group节点属性更新
        private void UpdateGroupIfo()
        {
            node_element = (XElement)selected_node.Tag;
            if (selected_node.Tag == null)
            {
                selected_node = rootnode;
            }
            if (textBoxGP_location.Text != "")
            {
                node_element.SetAttributeValue("location", textBoxGP_location.Text);
            }

            if (textBoxGP_description.Text != "")
            {
                node_element.SetAttributeValue("description", textBoxGP_description.Text);
            }
        }

        //integer节点属性更新
        private void UpdateIntegerIfo()
        {
            node_element = (XElement)selected_node.Tag;
            
            if (textBox1IT_location.Text != "")
            {
                node_element.SetAttributeValue("location", textBox1IT_location.Text);
            }

            if (textBoxIT_description.Text != "")
            {
                node_element.SetAttributeValue("description", textBoxIT_description.Text);
            }
            if (textBoxIT_value.Text != "")
            {
                node_element.SetAttributeValue("value", textBoxIT_value.Text);
            }
            if (comboBoxIT_byteorder.Text != "")
            {
                node_element.SetAttributeValue("byteOrder", comboBoxIT_byteorder.Text);
            }
            if (textBoxIT_number.Text != "")
            {
                node_element.SetAttributeValue("number", textBoxIT_number.Text);
            }
        }
  

        //string节点属性更新
        private void UpdateStringIfo()
        {
            node_element = (XElement)selected_node.Tag;

            if (textBoxST_location.Text != "")
            {
                node_element.SetAttributeValue("location", textBoxST_location.Text);
            }

            if (textBoxST_description.Text != "")
            {
                node_element.SetAttributeValue("description", textBoxST_description.Text);
            }
            if (textBoxST_value.Text != "")
            {
                node_element.SetAttributeValue("value", textBoxST_value.Text);
            }
           
        }

        //semicolon节点属性更新
        private void UpdateSemicolonIfo()
        {
            node_element = (XElement)selected_node.Tag;

            if (textBoxSE_location.Text != "")
            {
                node_element.SetAttributeValue("location", textBoxSE_location.Text);
            }

            if (textBoxSE_description.Text != "")
            {
                node_element.SetAttributeValue("description", textBoxSE_description.Text);
            }

        }

        //real节点属性更新
        private void UpdateRealIfo()
        {
            node_element = (XElement)selected_node.Tag;

            if (textBoxRE_location.Text != "")
            {
                node_element.SetAttributeValue("location", textBoxRE_location.Text);
            }

            if (textBoxRE_description.Text != "")
            {
                node_element.SetAttributeValue("description", textBoxRE_description.Text);
            }
            if (textBoxRE_value.Text != "")
            {
                node_element.SetAttributeValue("value", textBoxRE_value.Text);
            }
            if (comboBoxRE_byteorder.Text != "")
            {
                node_element.SetAttributeValue("byteOrder", comboBoxRE_byteorder.Text);
            }
        }

        //tab节点属性更新
        private void UpdateTabIfo()
        {
            node_element = (XElement)selected_node.Tag;
        }


        //cr节点属性更新
        private void UpdateCrIfo()
        {
            node_element = (XElement)selected_node.Tag;
        }

        //下面是对number属性输入进行规则约束，只能输入正整数
        private void textBoxIT_number_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar == '\b' || (e.KeyChar >= '0' && e.KeyChar <= '9')))
            {
                e.Handled = true;
            }
  
        }

        //当鼠标进入时显示提示信息
        private void textBoxIT_number_Enter(object sender, EventArgs e)
        {
            this.textBoxIT_number.Text = "Please input a valid integer!";
        }

        bool update_btnclick = false;

        //实现节点属性保存功能
        private void button_Update_Click_1(object sender, EventArgs e)
        {
            update_btnclick = true;
            UpdateNodeIfo(node_element);
            MessageBox.Show("Saved Successfully.","Information",MessageBoxButtons.OK, MessageBoxIcon.Information);
            button_Update.Enabled = false;
            btn_edit.Enabled = true;
            UnableEdit();
        }

 
        //点击节点显示当前属性
        private void tv_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            button_Update.Visible = true;
            btn_edit.Visible = true;
            ShowNodeIfo(node_element);//显示节点属性
            textBox_Preview.Visible = false;

        }


        //双击节点，节点不收缩，一直保持展开状态（c#默认双击展开收缩）
        //tv控件节点折叠之前判断鼠标按下的次数，并进行控制
        private void tv_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel =true;
        }

        //设置只有点击edit按钮属性才能编辑
        private void btn_edit_Click(object sender, EventArgs e)
        {
            button_Update.Enabled = true;
            if (selected_node.Text == "dataformat")
            {
                EditDataformat();
            }
            if (selected_node.Text == "group")
            {
                EditGroup();
            }
            if (selected_node.Text == "integer")
            {
                EditInteger();
            }
            if (selected_node.Text == "string")
            {
                EditString();
            }
            if (selected_node.Text == "semicolon")
            {
                EditSemicolon();
            }
            if (selected_node.Text == "real")
            {
                EditReal ();
            }
            btn_edit.Enabled = false;
          
        }

        //点击save后属性不可编辑
        private void  UnableEdit()
        {
            if (selected_node.Text == "dataformat")
            {
                UnEditDataformat();
            }
            if (selected_node.Text == "group")
            {
                UnEditGroup();
            }
            if (selected_node.Text == "integer")
            {
                UnEditInteger();
            }
            if (selected_node.Text == "string")
            {
                UnEditString();
            }
            if (selected_node.Text == "semicolon")
            {
                UnEditSemicolon();
            }
            if (selected_node.Text == "real")
            {
                UnEditReal();
            }

        }

        //解决选中节点滞后问题
        private void tv_MouseDown(object sender, MouseEventArgs e)
        {
            if ((sender as TreeView) != null)
            {
                tv.SelectedNode = tv.GetNodeAt(e.X, e.Y);
            }
        }


        //实现新建文件初始化为根节点为“dataformat”的tree
        private void NewFileFToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = null;
            tv.Nodes.Clear();    //清空树节点
            //初始化tree属性面板先关闭
            panel_dataformat.Visible = false;
            panel_group.Visible = false;
            panel_integer.Visible = false;
            panel_string.Visible = false;
            panel_semicolon.Visible = false;
            panel_cr.Visible = false;
            panel_real.Visible = false;
            panel_tab.Visible = false;
            tv.Visible = true;
            InitTree(tv);    //初始化树
        }

        bool open_click = false;
        //实现文件导入功能
        private void ImportFileEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = null;
            tv.Visible = true;
            panel_dataformat.Visible = false;
            panel_group.Visible = false;
            panel_integer.Visible = false;
            panel_string.Visible = false;
            panel_semicolon.Visible = false;
            panel_tab.Visible = false;
            panel_real.Visible = false;
            panel_cr.Visible = false;
            tv.Nodes.Clear();    //清空树节点，初始化tree
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".xml";    //打开默认xml类型文件
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = ofd.FileName;
                //使用xDocument来读取xml文件
                XDocument document = XDocument.Load(filename);
                XElement rootElement = document.Root;    //这里root包含了所有节点属性，所以保存出错
                //将xml文件的根元素加载到treeview的根节点上
                //用递归加载XML到TreeView中
                Xml2TreeView(rootElement, tv.Nodes);
                tv.ExpandAll();
            }
            open_click = true;
        }

        //实现文档保存功能
        private void SaveTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveToXml();
            MessageBox.Show("Saved Successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //实现文档另存功能
        private void SaveAsHToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveToXml();
            MessageBox.Show("Saved Successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //下面是将xml文档显示到textbox
        private string FormatXml(string sUnformattedXmlFilePath)
        {
            XmlDocument XMLdoc = new XmlDocument();    //创建xml文档对象
            XMLdoc.Load(sUnformattedXmlFilePath);    //载入xml文件
            StringBuilder sb = new StringBuilder();    //字符串容器
            StringWriter sw = new StringWriter(sb);    //定义一个StringWriter
            XmlTextWriter xtw = null;    //创建一个StringWriter实例的XmlTextWriter
            try     //可能会报异常，放到try里
            {
                xtw = new XmlTextWriter(sw);
                xtw.Formatting = Formatting.Indented;    //设置缩进
                xtw.Indentation = 1;    //设置缩进字数
                xtw.IndentChar = '\t';    //用\t字符作为缩进
                XMLdoc.WriteTo(xtw);
            }
            finally     //finally一定会执行
            {
                if (xtw != null)
                    xtw.Close();
            }
            return sb.ToString();
        }

        //预先将编辑文件保存为xml，在通过xml显示在textbox上
        //文件保存的相对路径
        string path = AppDomain.CurrentDomain.BaseDirectory + @"//Temp_Save.xml";
        private void SaveToXmlpreview()

        {
            XDocument xml = new XDocument();
            foreach (TreeNode node in tv.Nodes)
            {
                XElement e = CreateElements(node);
                xml.Add(e);
            }

            xml.Save(path);

        }

        //设置一个bool值用于判断预览功能是否点击
        bool preview_Click = false;

        //实现文档预览功能
        private void previewPToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //编辑后点击保存才能预览
            if (update_btnclick)
            {

                textBox_Preview.Text=null;
                textBox_Preview.Visible = true;
                panel_dataformat.Visible = false;
                panel_group.Visible = false;
                panel_integer.Visible = false;
                panel_string.Visible = false;
                panel_semicolon.Visible = false;
                panel_tab.Visible = false;
                panel_real.Visible = false;
                panel_cr.Visible = false;
                SaveToXmlpreview();
                textBox_Preview.Text = FormatXml(path);
                preview_Click = true;


            }
            //如果为导入文件，导入后可预览
            else if (open_click)
            {
                textBox_Preview.Text = null;
                textBox_Preview.Visible = true;
                panel_dataformat.Visible = false;
                panel_group.Visible = false;
                panel_integer.Visible = false;
                panel_string.Visible = false;
                panel_semicolon.Visible = false;
                panel_tab.Visible = false;
                panel_real.Visible = false;
                panel_cr.Visible = false;
                SaveToXmlpreview();
                textBox_Preview.Text = FormatXml(path);
                preview_Click = true;
            }
            //如果编辑后未保存，弹出提示信息
            else
            {
                MessageBox.Show("Save the information,please!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            

        }

        //实现退出功能
        private void exitXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
