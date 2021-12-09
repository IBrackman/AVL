using AVL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AVLUnitTest
{
    [TestClass]
    public class AvlUnitTest
    {
        [TestMethod]
        public void IntTest()
        {
            var tree = new AvlTree<int>
            {
                5,
                3,
                7,
                2,
                10,
                30,
                70,
                20
            };
            tree.Remove(7);
            Assert.AreEqual(30 ,tree.Find(70).Parent.Value);
            tree.PrintTree(@"H:\Graphviz\bin\dot.exe", "IntTree");
        }

        [TestMethod]
        public void DoubleTest()
        {
            var tree = new AvlTree<double>
            {
                5.3,
                3.5,
                7.2,
                2.1,
                10.7,
                30.0,
                70.9,
                20.6
            };
            tree.Remove(7.2);
            Assert.AreEqual(30.0, tree.Find(70.9).Parent.Value);
            tree.PrintTree(@"H:\Graphviz\bin\dot.exe", "DoubleTree");
        }

        [TestMethod]
        public void CharTest()
        {
            var tree = new AvlTree<char>
            {
                'a',
                'g',
                'p',
                'b',
                'q',
                'A',
                'M',
                '6'
            };
            tree.Remove('p');
            Assert.AreEqual('g', tree.Find('q').Parent.Value);
            tree.PrintTree(@"H:\Graphviz\bin\dot.exe", "CharTree");
        }

        [TestMethod]
        public void StringTest()
        {
            var tree = new AvlTree<string>
            {
                "abc",
                "Def",
                "ijn",
                "ija",
                "ABC",
                "xYz",
                "1247",
                "kghgkg"
            };
            tree.Remove("abc");
            Assert.AreEqual("xYz", tree.Find("kghgkg").Parent.Value);
            tree.PrintTree(@"H:\Graphviz\bin\dot.exe", "StringTree");
        }

        [TestMethod]
        public void ContainsTest()
        {
            var tree = new AvlTree<int>
            {
                5,
                3,
                7,
                2,
                10,
                30,
                70,
                20
            };
            tree.Remove(7);
            Assert.IsFalse(tree.Contains(7));
            Assert.IsTrue(tree.Contains(70));
            tree.PrintTree(@"H:\Graphviz\bin\dot.exe", "ContainsTree");
        }
    }
}