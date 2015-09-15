using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using testGround.Domain;

namespace testGround
{
    internal class Program
    {
        static StringBuilder sb = new StringBuilder();
        static StringWriter sw = new StringWriter(sb);
        static JsonWriter jsonWriter = new JsonTextWriter(sw);
        public static int count, buffer;
        private static List<MyTreeObject> childrenWithSameParent;
        private static string finalBody = "";
        private static TreeNode AddNode(TreeNode node, string pathPart)
        {
            var auxNode = FindNodeByText(pathPart, node);
            if (auxNode != null)
            {
                return auxNode;
            }
            var nNode = new TreeNode(pathPart);
            node.ChildNodes.Add(nNode);
            return nNode;
        }

        private static TreeNode FindNodeByText(string pathPart, TreeNode node)
        {
            var query = node.ChildNodes
                .OfType<TreeNode>()
                .AsQueryable()
                .Where(x => x.Value.Equals(pathPart)).FirstOrDefault();
            return query;
        }

        private static void EnumResults(string path)
        {

        }
        private static void Main(string[] args)
        {
            //OriginalApproach();            

            List<KeyValuePair<string, string>> sharepointData = new List<KeyValuePair<string, string>>();
            sharepointData.Add(new KeyValuePair<string, string>("/Trades Database", "/Trades Database/Admin"));
            sharepointData.Add(new KeyValuePair<string, string>("/Trades Database", "/Trades Database/Something"));
            sharepointData.Add(new KeyValuePair<string, string>("/Trades Database/Something", "/Trades Database/Something/doc1.txt"));
            sharepointData.Add(new KeyValuePair<string, string>("/Trades Database/Something", "/Trades Database/Something/blah.txt"));
            sharepointData.Add(new KeyValuePair<string, string>("/blah", "/blah/Folder"));
            sharepointData.Add(new KeyValuePair<string, string>("/blah/Folder", "/blah/Folder/wow"));
            sharepointData.Add(new KeyValuePair<string, string>("/Yolo", "/Yolo/Something"));
            sharepointData.Add(new KeyValuePair<string, string>("/Yolo", "/Yolo/Folder/1/1/2"));
            sharepointData.Add(new KeyValuePair<string, string>("/Yolo", "/Yolo/Folder/2/3/6"));
            sharepointData.Add(new KeyValuePair<string, string>("/Yolo/Something", "/Yolo/Something/doc1.txt"));
            sharepointData.Add(new KeyValuePair<string, string>("/Yolo/Folder", "/Yolo/Folder/doc1.txt"));
            sharepointData.Add(new KeyValuePair<string, string>("/Yolo", "/Yolo/Folder"));


            Node tree = new Node();

            foreach (KeyValuePair<string, string> sharePointListItem in sharepointData)
            {
                string[] splitNodes = sharePointListItem.Key.Split('/');                
                IEnumerable<string> filterNodes = FilterNodes(splitNodes);
                tree = NodeResolver.ResolveChildren(filterNodes, tree);

                splitNodes = sharePointListItem.Value.Split('/');
                filterNodes = FilterNodes(splitNodes);
                tree = NodeResolver.ResolveChildren(filterNodes, tree);
            }
            Debug.WriteLine("We made it out alive!");
        }

        private static IEnumerable<string> FilterNodes(string[] splitNodes)
        {
            return splitNodes.Where(item => item != string.Empty);
        }

        private static void OriginalApproach()
        {
            JObject tree = new JObject();
            JObject obj = new JObject();
            Helper helper = new Helper();
            ;
            var blah = new List<string>();
            List<KeyValuePair<string, string>> itemKeyValuePair = new List<KeyValuePair<string, string>>();
            itemKeyValuePair.Add(new KeyValuePair<string, string>("/Trades Database", "/Trades Database/Admin"));
            itemKeyValuePair.Add(new KeyValuePair<string, string>("/Trades Database", "/Trades Database/Something"));
            itemKeyValuePair.Add(new KeyValuePair<string, string>("/Trades Database/Something",
                "/Trades Database/Something/doc1.txt"));
            itemKeyValuePair.Add(new KeyValuePair<string, string>("/Trades Database/Something",
                "/Trades Database/Something/blah.txt"));
            itemKeyValuePair.Add(new KeyValuePair<string, string>("/Yolo", "/Yolo/Something"));
            itemKeyValuePair.Add(new KeyValuePair<string, string>("/Yolo", "/Yolo/Folder/1/1/2"));
            itemKeyValuePair.Add(new KeyValuePair<string, string>("/Yolo", "/Yolo/Folder/2/3/6"));
            itemKeyValuePair.Add(new KeyValuePair<string, string>("/Yolo/Something", "/Yolo/Something/doc1.txt"));
            itemKeyValuePair.Add(new KeyValuePair<string, string>("/Yolo/Folder", "/Yolo/Folder/doc1.txt"));
            itemKeyValuePair.Add(new KeyValuePair<string, string>("/Yolo", "/Yolo/Folder"));

            var rootNode = new TreeNode("root");
            var treeView = new TreeView();
            var node = rootNode;
            List<KeyValuePair<string, string>> father = new List<KeyValuePair<string, string>>();
            JObject treeJObject = new JObject();
            JArray treeArray = new JArray();
            JProperty treeProperty = new JProperty("");
            childrenWithSameParent = new List<MyTreeObject>();
            MyTreeObject myTreeObject = new MyTreeObject();
            List<MyTreeObject> listOfMyTreeObjects = new List<MyTreeObject>();
            List<MyTreeObject> cleanlistOfMyTreeObjects = new List<MyTreeObject>();
            string currentParent = "";
            StringBuilder sb = new StringBuilder();
            List<KeyValuePair<string, string>> arr = new List<KeyValuePair<string, string>>();
            var itemsList = new { items = arr };

            List<string> branchesList = new List<string>();
            List<MyTreeObject> distinctNodes = new List<MyTreeObject>();
            /*First node*/
            foreach (var pair in itemKeyValuePair)
            {
                branchesList.Add(pair.Value);
            }

            //create list of parent/child objects
            for (int i = 0; i < branchesList.Count; i++)
            {
                var foldersOrFiles = branchesList[i].Split('/');
                for (int j = 0; j < foldersOrFiles.Length - 1; j++)
                {
                    listOfMyTreeObjects.Add(new MyTreeObject { Child = foldersOrFiles[j + 1], Parent = foldersOrFiles[j] });
                }
            }
            List<KeyValueItemModel> resultCollectionList = new List<KeyValueItemModel>();

            //first branch
            var firstBranch = listOfMyTreeObjects.Where(item => item.Parent == "").ToList();
            if (firstBranch.Count > 1)
            {
                var currentBranchparents = GetDistinctParents(firstBranch, new List<MyTreeObject>());
                foreach (var branch in currentBranchparents)
                {
                    string parentInQuestion = branch.Child;
                    foreach (var PCpair in listOfMyTreeObjects)
                    {
                        if (PCpair.Parent == parentInQuestion)
                        {
                            KeyValueItemModel entry = new KeyValueItemModel();
                            entry.Key = Guid.NewGuid();
                            string item = "{text :" + PCpair.Parent + "}";
                            entry.Value = item;

                            resultCollectionList.Add(entry);
                        }
                    }
                }
            }
            else
            {
                foreach (var branch in firstBranch)
                {
                    string parentInQuestion = branch.Child;
                    foreach (var PCpair in listOfMyTreeObjects)
                    {
                        if (PCpair.Parent == parentInQuestion)
                        {
                            KeyValueItemModel entry = new KeyValueItemModel();
                            entry.Key = Guid.NewGuid();
                            string item = "{text :\"" + PCpair.Parent + "\"}";
                            entry.Value = item;

                            resultCollectionList.Add(entry);
                        }
                    }
                }
            }

            listOfMyTreeObjects = RemoveItemsWithNullParentFromTreeObject(listOfMyTreeObjects);
            Guid? oldGuid = null;
            KeyValueItemModel first = null;
            List<KeyValueItemModel> newList = new List<KeyValueItemModel>();
            newList = resultCollectionList;
            for (int i = 0; i < listOfMyTreeObjects.Count; i++)
            {
                var objectInQuestion = listOfMyTreeObjects[i];
                int index = 0;

                for (int index1 = 0; index1 < resultCollectionList.Count; index1++)
                {
                    var storedParent = resultCollectionList[index1];
                    string oldValue = storedParent.Value;
                    if (storedParent.Value.Contains(objectInQuestion.Parent))
                    {
                        index = storedParent.Value.IndexOf(objectInQuestion.Parent, StringComparison.Ordinal) +
                                objectInQuestion.Parent.Length;

                        first = new KeyValueItemModel
                        {
                            Key = storedParent.Key,
                            Value = oldValue.Insert(index, ",items:[{ text: \"" + objectInQuestion.Child + "\"},")
                        };

                        newList.RemoveAll(item => item.Key == first.Key);
                        newList.Add(first);
                        oldGuid = storedParent.Key;
                    }
                }
            }
            var thatRecord = newList.FirstOrDefault(item => item.Key == oldGuid);
            if (thatRecord != null)
            {
                thatRecord.Value = thatRecord.Value + "]}";
                newList.RemoveAll(item => item.Key == oldGuid);
                newList.Add(thatRecord);
            }
            else
            {
                var onlyRecord = newList.FirstOrDefault();
                onlyRecord.Value = onlyRecord.Value + "]}";
                if (oldGuid != null) onlyRecord.Key = (Guid)oldGuid;
                newList.RemoveAt(0);
                newList.Add(onlyRecord);
            }
            newList = RemoveDupes(newList);

            #region oldCode            

            /* var root = listOfMyTreeObjects.Where(item => item.Parent == "").ToList();
            if (root.Count > 1)
            {
                listOfMyTreeObjects.RemoveAll(item => item.Parent == "");
            }


            distinctNodes = GetDistinctChildren(root, distinctNodes);
            foreach (var distinctNode in distinctNodes)
            {
                sb = StartSb(distinctNode.Child, sb);
            }

           

            List<MyTreeObject> collectionOfCurrentParent = new List<MyTreeObject>();
            for (int i = 0; i < listOfMyTreeObjects.Count; i++)
            {
                collectionOfCurrentParent = GetParentReferences(listOfMyTreeObjects, listOfMyTreeObjects[i].Parent);
                collectionOfCurrentParent = GetDistinctParents(collectionOfCurrentParent, new List<MyTreeObject>());
                sb = InsertIntoBody(sb, collectionOfCurrentParent);
                listOfMyTreeObjects = RemoveParent(listOfMyTreeObjects, listOfMyTreeObjects[i].Parent);
            }//  sb = sb.Append("}]");
             */
            #endregion
        }

        private static List<MyTreeObject> RemoveItemsWithNullParentFromTreeObject(List<MyTreeObject> listofMyTreeObjects)
        {
            for (int i = 0; i < listofMyTreeObjects.Count; i++)
            {
                var entry = listofMyTreeObjects[i];
                if (string.IsNullOrEmpty(entry.Parent))
                {
                    listofMyTreeObjects.Remove(entry);
                }
            }

            return listofMyTreeObjects;
        }
        private static List<KeyValueItemModel> RemoveDupes(List<KeyValueItemModel> resultCollectionList)
        {
            var collection = new List<KeyValueItemModel>();
            for (int i = 0; i < resultCollectionList.Count - 1; i++)
            {
                string parentInQuestion = resultCollectionList[i].Value;
                if (parentInQuestion != resultCollectionList[i + 1].Value)
                {
                    collection.Add(new KeyValueItemModel { Key = Guid.NewGuid(), Value = resultCollectionList[i].Value });
                    collection.Add(new KeyValueItemModel { Key = Guid.NewGuid(), Value = resultCollectionList[i + 1].Value });
                }
            }
            return collection;
        }

        private static List<MyTreeObject> RemoveParent(List<MyTreeObject> listofMyTreeObjects, string parent)
        {
            List<MyTreeObject> result = new List<MyTreeObject>();
            foreach (var listofMyTreeObject in listofMyTreeObjects)
            {
                if (listofMyTreeObject.Parent != parent)
                {
                    result.Add(new MyTreeObject { Child = listofMyTreeObject.Child, Parent = listofMyTreeObject.Parent });
                }
            }

            return result;
        }

        private static StringBuilder InsertIntoBody(StringBuilder stringBuilder, List<MyTreeObject> parentsList)
        {
            string body = stringBuilder.ToString();
            bool initiateItemsArray = false;
            bool EndItemsArray = false;
            int index = 0;
            string addChild = "";
            List<MyTreeObject> distinctReferences = new List<MyTreeObject>();
            distinctReferences = GetDistinctChildren(parentsList, distinctReferences);
            parentsList = distinctReferences;
            for (int i = 0; i < parentsList.Count; i++)
            {
                var parent = parentsList[i].Parent;
                var child = parentsList[i].Child;
                if (i == 0)
                {
                    initiateItemsArray = true;
                }
                if (i == parentsList.Count - 1)
                {
                    EndItemsArray = true;
                }
                if (body.Contains(parent))
                {

                    index = body.IndexOf(parent) + parent.Length + addChild.Length + 1;
                    if (initiateItemsArray)
                    {
                        body = body.Insert(index, ",items: [");
                        initiateItemsArray = false;
                    }


                    body = body.Insert(index + 9, "{ text: \"" + child + "\" ");
                    addChild = "{ text: \"" + child + "\" ";

                    if (EndItemsArray)
                    {
                        index = index + addChild.Length + 9;
                        body = body.Insert(index, "],");
                        EndItemsArray = false;
                    }

                }

            }
            if (body.Contains(",,"))
                body = body.Replace(",,", ",");

            return new StringBuilder(body);



        }

        private static List<MyTreeObject> GetParentReferences(List<MyTreeObject> listofMyTreeObjects, string parentInQuestion)
        {
            List<MyTreeObject> parentCollection = new List<MyTreeObject>();

            for (int i = 0; i < listofMyTreeObjects.Count - 1; i++)
            {
                if (listofMyTreeObjects[i].Parent == parentInQuestion)
                {
                    parentCollection.Add(listofMyTreeObjects[i]);
                }

            }

            return parentCollection;

        }
        private static List<MyTreeObject> GetDistinctParents(List<MyTreeObject> root, List<MyTreeObject> containerofMyTreeObjects)
        {
            for (int i = 0; i < root.Count - 1; i++)
            {
                if (!(root[i].Parent == root[i + 1].Parent && root[i].Child == root[i + 1].Child))
                {
                    containerofMyTreeObjects.Add(root[i]);
                    containerofMyTreeObjects.Add(root[i + 1]);
                }
            }
            return containerofMyTreeObjects;

        }

        private static List<MyTreeObject> GetDistinctChildren(List<MyTreeObject> root, List<MyTreeObject> containerofMyTreeObjects)
        {
            for (int i = 0; i < root.Count - 1; i++)
            {
                if (root[i].Child != root[i + 1].Child)
                {
                    containerofMyTreeObjects.Add(root[i]);
                    containerofMyTreeObjects.Add(root[i + 1]);
                }
            }
            return containerofMyTreeObjects;

        }

        private static StringBuilder StartSb(string s, StringBuilder stringBuilder)
        {
            if (!stringBuilder.ToString().Contains("{ data: ["))
                stringBuilder.Append("{ data: [{ text: \"" + s + "\"}]");
            else
            {
                stringBuilder.Append("{ text: \"" + s + "\"");
            }
            return stringBuilder;
        }
    }

}

