using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ideagen.ClassLib;

namespace Ideagen.ClassLib.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestRegular()
        {
            string[] exps = {
                                "1 + 1",
                                "2 * 2",
                                "1 + 2 + 3",
                                "6 / 2",
                                "11 + 23",
                                "11.1 + 23",
                                "1 + 1 * 3"
                            };

            double[] expectedResults = {
                                1 + 1,
                                2 * 2,
                                1 + 2 + 3,
                                6 / 2,
                                11 + 23,
                                11.1 + 23,
                                1 + 1 * 3
                            };

            for(int i=0; i<exps.Length; i++)
            {
                double? result = Math.Calculate(exps[i]);
                Assert.AreEqual(expectedResults[i], result);
            }
        }

        [TestMethod]
        public void TestHandleBrackets()
        {
            string[] exps = {
                                "( 11.5 + 15.4 ) + 10.1",
                                "23 - ( 29.3 - 12.5 )",
                                "4 + ( 2 - 3 )"
                            };

            double[] expectedResults = {
                                ( 11.5 + 15.4 ) + 10.1,
                                23 - ( 29.3 - 12.5 ),
                                4 + ( 2 - 3 )
                            };

            for(int i=0; i<exps.Length; i++)
            {
                double? result = Math.Calculate(exps[i]);
                Assert.AreEqual(expectedResults[i], result);
            }
        }

        [TestMethod]
        public void TestHandleNestedBrackets()
        {
            string[] exps = {
                               "10 - ( 2 + 3 * ( 7 - 5 ) )"
                            };

            double[] expectedResults = {
                                10 - ( 2 + 3 * ( 7 - 5 ) )
                            };

            for(int i=0; i<exps.Length; i++)
            {
                double? result = Math.Calculate(exps[i]);
                Assert.AreEqual(expectedResults[i], result);
            }
        }

        [TestMethod]
        public void TestHandleException()
        {
            string[] exps = {
                               "10 - ( 2 + 3 * ( 7 - 5 )ZXX)",
                               "11AASDSA#$%%%"
                            };

            double?[] expectedResults = {
                                null,
                                null
                            };

            for(int i=0; i<exps.Length; i++)
            {
                double? result = Math.Calculate(exps[i]);
                Assert.AreEqual(expectedResults[i], result);
            }
        }
    }
}
