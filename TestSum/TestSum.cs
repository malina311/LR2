using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfApp2;
namespace TestSum
{
    [TestClass]
    public class TestSum
    {
        [TestMethod]
        public void TestMethod_SumAB()
        {
            // Arrange
            var matrixA = new Matrix<int>(2, 2);
            matrixA[0, 0] = 1;
            matrixA[0, 1] = 2;
            matrixA[1, 0] = 3;
            matrixA[1, 1] = 4;

            var matrixB = new Matrix<int>(2, 2);
            matrixB[0, 0] = 5;
            matrixB[0, 1] = 6;
            matrixB[1, 0] = 7;
            matrixB[1, 1] = 8;

            var expected = new Matrix<int>(2, 2);
            expected[0, 0] = 6;
            expected[0, 1] = 8;
            expected[1, 0] = 10;
            expected[1, 1] = 12;

            // Act
            var result = matrixA + matrixB;

            // Assert
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Assert.AreEqual(expected[i, j], result[i, j]);
                }
            }
        }
    }
}