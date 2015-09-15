using System;
using System.Collections.Generic;
using System.Linq;
using testGround.Domain;

namespace testGround
{
    public static class NodeResolver
    {
        public static Node ResolveChildren(IEnumerable<string> splitNodes, Node node)
        {
            string currentParentNode = splitNodes.First();
            foreach (string splitNode in splitNodes)
            {
                if (node.Children.Any())
                {
                    if (!NodeExistsInHierachy(node.Children, splitNode))
                    {
                        Node matchingParentNode = FindMatchingNodeInHierarchy(node.Children, currentParentNode);
                        if (matchingParentNode == null)
                        {
                            node.Children.Add(new Node(splitNode, currentParentNode));
                        }
                        else
                        {
                            matchingParentNode.Children.Add(new Node(splitNode, currentParentNode));
                        }
                    }
                }
                else
                {
                    node.Children.Add(new Node(splitNode, currentParentNode));
                }
                currentParentNode = splitNode;
            }
            
            return node;
        }

        private static bool NodeExistsInHierachy(IEnumerable<Node> hierachy, string node)
        {
            bool exists = false;
            foreach (Node hierarchyNode in hierachy)
            {
                if (hierarchyNode.Children.Any())
                {
                    if (hierarchyNode.Name == node)
                    {
                        return true;
                    }
                    exists = NodeExistsInHierachy(hierarchyNode.Children, node);
                }
                else
                {
                    exists = hierarchyNode.Name == node;
                }
            }
            return exists;
        }

        private static Node FindMatchingNodeInHierarchy(IEnumerable<Node> hierachy, string node)
        {
            Node matchingNode = null;
            foreach (Node hierarchyNode in hierachy)
            {
                if (hierarchyNode.Name == node)
                {
                    matchingNode = hierarchyNode;
                }
                if (hierarchyNode.Children.Any() && matchingNode == null)
                {
                    matchingNode = FindMatchingNodeInHierarchy(hierarchyNode.Children, node);
                }
            }
            return matchingNode;
        }
    }
}
