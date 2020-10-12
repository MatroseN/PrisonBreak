using System;
using System.Collections.Generic;
using System.Text;
using PrisonBreak.Graphing;

namespace PrisonBreak.Searching {
    public class BFS {
        public BFS(Graph graph) {
            this.graph = graph;
        }

        public Dictionary<Node, Node> allPaths(Node s) {
            Queue<Node> queue = new Queue<Node>();
            Node node;
            Node startNode = s;
            Dictionary<Node, Node> parent = new Dictionary<Node, Node>();

            queue.Enqueue(startNode);

            while (queue.Count != 0) {
                node = queue.Dequeue();
                foreach (Node edge in node.Edges.Values) {
                    if (!parent.ContainsKey(edge)) {
                        queue.Enqueue(edge);
                        parent.Add(edge, node);
                    }
                }
            }
            return parent;
        }

        public List<Node> shortestPath(Node s, Node e) {
            Queue<Node> queue = new Queue<Node>();
            Node node;
            Node endNode = e;
            Node startNode = s;
            Dictionary<Node, Node> parent = new Dictionary<Node, Node>();

            queue.Enqueue(startNode);

            while (queue.Count > 0) {
                node = queue.Dequeue();
                if (node == endNode) {
                    break;
                }

                foreach (Node edge in node.Edges.Values) {
                    if (!contains(queue, node)) {
                        if (!parent.ContainsKey(edge)) {
                            parent.Add(edge, node);
                        }
                        queue.Enqueue(edge);
                    }
                }
            }
            return backtrace(parent, startNode, endNode);
        }

        private List<Node> backtrace(Dictionary<Node, Node> parent, Node start, Node end) {
            List<Node> path = new List<Node>();
            Node node = parent[end];

            while (node != start) {
                path.Add(node);
                node = parent[node];
            }
            path.Add(start);

            path.Reverse();

            return path;
        }


        private bool contains(Queue<Node> q, Node n) {
            foreach (Node node in q) {
                if (node.ID == n.ID) {
                    return true;
                }
            }
            return false;
        }

        private Graph graph;
    }
}
