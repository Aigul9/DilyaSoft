using Microsoft.VisualStudio.TestTools.UnitTesting;
using Csv_parser_new;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Csv_parser_new.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        [TestMethod()]
        public void TriplicationTest()
        {
            List<string> sourceList = new List<string>()
            {
                "one", "two", "three", "four", "five", "six", "seven"
            };

            List<Info> expectedList = new List<Info>()
            {
                new Info{ First = "one", Second = "two", Third = "three"},
                new Info{ First = "four", Second = "five", Third = "six"}
            };

            List<Info> actualList = Program.Union(sourceList);
            if (expectedList.Count == actualList.Count())
            {
                for (int i = 0; i < expectedList.Count; i++)
                {
                    Assert.AreEqual(actualList[i].First, expectedList[i].First);
                    Assert.AreEqual(actualList[i].Second, expectedList[i].Second);
                    Assert.AreEqual(actualList[i].Third, expectedList[i].Third);
                }
            }
            else
            {
                Assert.AreEqual(expectedList.Count, actualList.Count);
            }
        }

        [TestMethod()]
        public void SorterTest()
        {
            List<Info> sourceList = new List<Info>()
            {
                new Info{ First = "one", Second = "two21", Third = "three"},
                new Info{ First = "one", Second = "two12", Third = "three"},
                new Info{ First = "four", Second = "five", Third = "six"},
                new Info{ First = "seven", Second = "eight", Third = "nine"},
                new Info{ First = "ten", Second = "eleven", Third = "twelve"}
            };

            int inputSortProperty = 2;  // eight eleven five two12 two21
            List<Info> expectedList = new List<Info>()
            {
                new Info{ First = "seven", Second = "eight", Third = "nine"},
                new Info{ First = "ten", Second = "eleven", Third = "twelve"},
                new Info{ First = "four", Second = "five", Third = "six"},
                new Info{ First = "one", Second = "two12", Third = "three"},
                new Info{ First = "one", Second = "two21", Third = "three"}
            };

            List<Info> actualList = Program.Sorter(sourceList, inputSortProperty);
            if (expectedList.Count == actualList.Count())
            {
                for (int i = 0; i < expectedList.Count; i++)
                {
                    Assert.AreEqual(actualList[i].First, expectedList[i].First);
                    Assert.AreEqual(actualList[i].Second, expectedList[i].Second);
                    Assert.AreEqual(actualList[i].Third, expectedList[i].Third);
                }
            }
            else
            {
                Assert.AreEqual(expectedList.Count, actualList.Count);
            }
        }

        [TestMethod()]
        public void ParserTest()
        {
            ArrayList sourceList = new ArrayList()
            {
                "\"January 21, 1996\"\" JDK 1.0 The first public release",
                "\"\"\"\"February 19, 1997\", JDK 1.1, \"\"JavaBeans, JDBC, RMI, reflection\"\"\""
            };  // первая строка без запятых (не выведется); несколько кавычек

            List<string> actualList = new List<string>();
            bool fl = Program.Parser(sourceList, out actualList);
            List<string> expectedList = new List<string>()
            {
                "February 19, 1997",
                "JDK 1.1",
                "JavaBeans, JDBC, RMI, reflection"
            };

            CollectionAssert.AreEqual(expectedList, actualList);
        }
    }
}