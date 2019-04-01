/*
Given a string Text and Trie(Patterns), we can quickly check whether any string from Patterns matches a
prefix of Text. To do so, we start reading symbols from the beginning of Text and see what string these
symbols “spell” as we proceed along the path downward from the root of the trie, as illustrated in the
pseudocode below. For each new symbol in Text, if we encounter this symbol along an edge leading down
from the present node, then we continue along this edge; otherwise, we stop and conclude that no string in
Patterns matches a prefix of Text. If we make it all the way to a leaf, then the pattern spelled out by this
path matches a prefix of Text.
This algorithm is called PrefixTrieMatching.
    PrefixTrieMatching(Text, Trie)
        symbol ← first letter of Text
        𝑣 ← root of Trie
        while forever:
            if 𝑣 is a leaf in Trie:
                return the pattern spelled by the path from the root to 𝑣
            else if there is an edge (𝑣,𝑤) in Trie labeled by symbol:
                symbol ← next letter of Text
                𝑣 ← 𝑤
            else:
                output “no matches found”
                return

PrefixTrieMatching finds whether any strings in Patterns match a prefix of Text. To find whether any
strings in Patterns match a substring of Text starting at position 𝑘, we chop off the first 𝑘 −1 symbols from
Text and run PrefixTrieMatching on the shortened string. As a result, to solve the Multiple Pattern
Matching Problem, we simply iterate PrefixTrieMatching |Text| times, chopping the first symbol off of
Text before each new iteration.
    TrieMatching(Text, Trie)
        while Text is nonempty:
            PrefixTrieMatching(Text, Trie)
            remove first symbol from Text

Note that in practice there is no need to actually chop the first 𝑘 −1 symbols of Text. Instead, we just read
Text from the 𝑘-th symbol.
Problem Description
Task. Implement TrieMatching algorithm.
Input Format. The first line of the input contains a string Text, the second line contains an integer 𝑛,
each of the following 𝑛 lines contains a pattern from Patterns = {𝑝1, . . . , 𝑝𝑛}.
Constraints. 1 ≤ |Text| ≤ 10 000; 1 ≤ 𝑛 ≤ 5 000; 1 ≤ |𝑝𝑖| ≤ 100 for all 1 ≤ 𝑖 ≤ 𝑛; all strings contain only
symbols A, C, G, T; no 𝑝𝑖 is a prefix of 𝑝𝑗 for all 1 ≤ 𝑖 ̸= 𝑗 ≤ 𝑛.
Output Format. All starting positions in Text where a string from Patterns appears as a substring in
increasing order (assuming that Text is a 0-based array of symbols).

Memory Limit. 512Mb.
Sample 1.
Input:
AAA
1
AA
Output:
0 1
Explanation:
The pattern AA appears at positions 0 and 1. Note that these two occurrences of the pattern overlap.
Sample 2.
Input:
AA
1
T
Output:
Explanation:
There are no occurrences of the pattern in the text.
Sample 3.
Input:
AATCGGGTTCAATCGGGGT
2
ATCG
GGGT
Output:
1 4 11 15
Explanation:
The pattern ATCG appears at positions 1 and 11, the pattern GGGT appears at positions 4 and 15. 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrieMatching
{
    public class Node<TVertex, TEdge>
    {
        public TVertex Vertex;
        public TEdge Edge; //incoming edge
        public bool EndOfPattern;
        public List<Node<TVertex, TEdge>> Children;

        public Node(TVertex vertex)
        {
            Vertex = vertex;
            Children = new List<Node<TVertex, TEdge>>();
        }

        public Node(TVertex vertex, TEdge edge)
        {
            Vertex = vertex;
            Edge = edge;
            Children = new List<Node<TVertex, TEdge>>();
        }

        public Node(TVertex vertex, TEdge edge, List<Node<TVertex, TEdge>> children)
        {
            Vertex = vertex;
            Edge = edge;
            Children = children;
        }

        public void AddChild(Node<TVertex, TEdge> child)
        {
            this.Children.Add(child);
        }

        public void print()
        {
            if (this == null)
                return;

            foreach (var child in this.Children)
            {
                Console.WriteLine(this.Vertex.ToString() + "->" + child.Vertex.ToString() + ":" + child.Edge.ToString());
                child.print();
            }
        }
    }

    class Program
    {
        public static Node<int, char> TrieConstruction(string[] Patterns)
        {
            Node<int, char> currentNode, nextNode;
            Node<int, char> Trie = new Node<int, char>(0); //Create a Trie with root node
            char currentSymbol;
            int newVertex = 1;

            foreach (string pattern in Patterns)
            {
                currentNode = Trie;
                for (int i = 0; i < pattern.Length; i++)
                {
                    currentSymbol = pattern[i];
                    nextNode = currentNode.Children.FirstOrDefault(n => n.Edge == currentSymbol);
                    if (nextNode != null)
                    {
                        currentNode = nextNode;
                    }
                    else
                    {
                        nextNode = new Node<int, char>(newVertex++, currentSymbol);
                        currentNode.AddChild(nextNode);
                        currentNode = nextNode;
                    }
                }
                currentNode.EndOfPattern = true;
            }

            return Trie;
        }

        public static bool PrefixTrieMatching(string Text, Node<int, char> Trie)
        {
            int p = 0;
            Node<int, char> currentNode = Trie; //root
            Node<int, char> nextNode;
            while (currentNode != null)
            {
                if(currentNode.Children.Count == 0 || currentNode.EndOfPattern == true)
                {
                    return true;
                }
                if(p >= Text.Length)
                {
                    return false;
                }
                nextNode = currentNode.Children.FirstOrDefault(n => n.Edge == Text[p]);
                if(nextNode != null)
                {
                    p++;
                    currentNode = nextNode;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public static string TrieMatching(string Text, Node<int, char> Trie)
        {
            StringBuilder result = new StringBuilder("");
            for(int i=0;i<Text.Length;i++)
            {
                if (PrefixTrieMatching(Text.Substring(i), Trie))
                    result.Append(i.ToString() + " ");
            }
            return result.ToString();
        }

        static void Main(string[] args)
        {
            string text = Console.ReadLine();
            int lines = int.Parse(Console.ReadLine());
            string[] patterns = new string[lines];

            for (int i = 0; i < lines; i++)
            {
                patterns[i] = Console.ReadLine();
            }

            Node<int, char> root = TrieConstruction(patterns);
            Console.WriteLine(TrieMatching(text, root));

            //Console.ReadLine();
        }
    }
}
