// <copyright file="XmlMergeWithNamespaceSupport.cs" company="Engage Software">
// Engage: Survey
// Copyright (c) 2004-2010
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Survey
{
    using System.IO;
    using System.Xml;

    /// <summary>
    /// Based on DNN's <c>XmlMerge</c>, this implements namespaces (from <c>XmlMerge</c> in DNN 5+).
    /// Only supports the <c>insertAfter</c> action
    /// </summary>
    public class XmlMergeWithNamespaceSupport
    {
        /// <summary>
        /// The merge configuration file specifying the changes to make
        /// </summary>
        private readonly XmlDocument sourceConfig;

        /// <summary>
        /// The configuration file to update
        /// </summary>
        private XmlDocument targetConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlMergeWithNamespaceSupport"/> class.
        /// </summary>
        /// <param name="sourceStream">The source stream.</param>
        public XmlMergeWithNamespaceSupport(Stream sourceStream)
        {
            this.sourceConfig = new XmlDocument();
            this.sourceConfig.Load(sourceStream);
        }

        /// <summary>
        /// The UpdateConfig method processes the source file and updates the Target Config Xml Document.
        /// </summary>
        /// <param name="target">>An Xml Document represent the Target Xml File</param>
        public void UpdateConfig(XmlDocument target)
        {
            this.targetConfig = target;

            if (this.targetConfig != null)
            {
                this.ProcessNodes(this.sourceConfig.SelectNodes("/configuration/nodes/node"));
            }
        }

        private void ProcessNodes(XmlNodeList nodes)
        {
            // The nodes definition is not correct so skip changes
            if (this.targetConfig != null)
            {
                foreach (XmlNode node in nodes)
                {
                    this.ProcessNode(node);
                }
            }
        }

        private void ProcessNode(XmlNode node)
        {
            string rootNodePath = node.Attributes["path"].Value;
            XmlNode rootNode;
            if (node.Attributes["nameSpace"] == null)
            {
                rootNode = this.targetConfig.SelectSingleNode(rootNodePath);
            }
            else
            {
                // Use Namespace Manager
                string xmlNamespace = node.Attributes["nameSpace"].Value;
                string xmlNamespacePrefix = node.Attributes["nameSpacePrefix"].Value;

                var namespaceManager = new XmlNamespaceManager(this.targetConfig.NameTable);
                namespaceManager.AddNamespace(xmlNamespacePrefix, xmlNamespace);
                rootNode = this.targetConfig.SelectSingleNode(rootNodePath, namespaceManager);
            }

            if (rootNode == null)
            {
                // TODO: what happens if Node can't be found
            }

            this.InsertNode(rootNode, node);
        }

        private void InsertNode(XmlNode childRootNode, XmlNode actionNode)
        {
            XmlNode rootNode = childRootNode.ParentNode;
            foreach (XmlNode child in actionNode.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Element || child.NodeType == XmlNodeType.Comment)
                {
                    rootNode.InsertAfter(this.targetConfig.ImportNode(child, true), childRootNode);
                }
            }
        }
    }
}