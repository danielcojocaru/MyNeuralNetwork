using Auxiliar.Common.Enum;
using Auxiliar.Common.Interface;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkOld
{
    public class NeuralNetwork : INeuralNetwork
    {
        public List<Layer> Layers { get; set; } = new List<Layer>();
        public Layer FirstLayer { get; set; }
        public Layer LastLayer { get; set; }

        public INeuralNetworkInitializer NnInitializer { get; set; }

        public NeuralNetwork()
        { }

        public void Create(int[] layers)
        {
            CreateLayers(layers);

            FirstLayer = Layers[0];
            LastLayer = Layers[Layers.Count - 1];
        }

        private void CreateLayers(int[] layers)
        {
            Layer lastLayer = null;
            for (int i = 0; i < layers.Length; i++)
            {
                int nrOfOutputs = layers[i];
                Layer layer = new Layer();

                if (i == 0)
                {
                    layer.Create(this, nrOfOutputs);
                }
                else
                {
                    int nrOfInputs = layers[i - 1];
                    layer.Create(this, nrOfOutputs, nrOfInputs);

                    // ading the neighbors layers references
                    layer.Previous = lastLayer;
                    lastLayer.Next = layer;
                }

                lastLayer = layer;
                Layers.Add(layer);
            }
        }

        public void Initialize()
        {
            FirstLayer.Initialize(index: -1);
        }

        public Matrix<double> Guess(double[] input)
        {
            Matrix<double> output = FirstLayer.Guess(input);
            return output;
        }

        public void Forward(double[] input, double[] targets, bool doBackpropagation = true)
        {
            FirstLayer.Guess(input);

            if (doBackpropagation)
            {
                //Console.WriteLine("Output in NeuralNetworkOld:");
                //Console.WriteLine(LastLayer.O);

                LastLayer.Backwards(targets);
            }
        }

        public void PrintToExcel()
        {
            List<List<double>> state = new List<List<double>>();

            for (int i = 1; i < Layers.Count; i++)
            {
                Layer layer = Layers[i];

                Console.WriteLine(layer.W);
                Console.WriteLine(layer.B);
                Console.WriteLine(layer.E);

                List<double> w = GetListFromMatrix(layer.W);
                List<double> b = GetListFromMatrix(layer.B);
                List<double> e = GetListFromMatrix(layer.E);

                state.Add(w);
                state.Add(b);
                state.Add(e);
            }

            PrintToExcel(state);
        }

        private static int excelIndex = 1;

        private void PrintToExcel(List<List<double>> state)
        {
            Application xlApp = new Application();


            Workbook xlWorkBook;
            Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);
            InsertStateIntoExcel(xlWorkSheet, state);

            xlWorkBook.SaveAs(@"C:\Projects\Trash\NnExcelPrint\csharp-Excel" + excelIndex++ + ".xls", XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlApp.Visible = true;
            //xlWorkBook.Close(true, misValue, misValue);
            //xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);

        }

        private static void InsertStateIntoExcel(Worksheet xlWorkSheet, List<List<double>> state)
        {
            // Insert title bar
            xlWorkSheet.Cells[1, 1] = "W";
            xlWorkSheet.Cells[1, 2] = "B";
            xlWorkSheet.Cells[1, 3] = "Error";
            xlWorkSheet.Cells[1, 4] = "W";
            xlWorkSheet.Cells[1, 5] = "B";
            xlWorkSheet.Cells[1, 6] = "Error";


            for (int i = 0; i < state.Count; i++)
            {
                List<double> s = state[i];
                int col = i + 1;

                for (int j = 0; j < s.Count; j++)
                {
                    double item = s[j];
                    int row = j + 2;

                    xlWorkSheet.Cells[row, col] = item;
                }
            }
        }

        private List<double> GetListFromMatrix(Matrix<double> matrix)
        {
            List<double> list = new List<double>();
            for (int row = 0; row < matrix.RowCount; row++)
            {
                for (int col = 0; col < matrix.ColumnCount; col++)
                {
                    list.Add(matrix[row, col]);
                }
            }
            return list;
        }

    }
}
