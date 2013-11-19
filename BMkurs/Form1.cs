#define n

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BMkurs
{
    public partial class Form1 : Form
    {
        public int n;
        public Form1()
        {
            n = new int();            
            InitializeComponent();
            setSize(3);
            setSystems();
        }

        public void setSize(int size)
        {
            n = size;
            dataGridView1.ColumnCount = n;
            dataGridView1.RowCount = n;
            dataXStart.RowCount = n;
            dataGridView3.RowCount = n;
            dataGridView6.RowCount = n;
            for (int i = 0; i < n; i++)
            {
                dataGridView1.Columns[i].Width = 50;
                dataXStart[0, i].Value = Convert.ToString(0);
                dataGridView3[0, i].Value = Convert.ToString(0);
                dataGridView6[0, i].Value = Convert.ToString(0);
                for (int j = 0; j < n; j++)
                    dataGridView1[j, i].Value = Convert.ToString(-2);
            }
        }

        public void newton()
        {
            double[] solution, newsolution;
            solution = new double[n];
            newsolution = new double[n];
            for (int i = 0; i < n; i++)
                solution[i] = Convert.ToDouble(dataXStart[0, i].Value);
            double[,] Fder = new double[n, n];
            double[,] A = new double[n, n];
            double[,] Fa = new double[n, n];
            double[,] temp = new double[n, n];
            double[,] C = new double[n, n];
            double[,] CmX = new double[n,n];
            double[] F = new double[n];
            double[] d = new double[n];
            double[] tempsmall = new double[n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    C[i, j] = Convert.ToDouble(dataGridView1[j, i].Value);
                }
            }
            bool exit = new bool();
            int numiter = new int();
            exit = false;
            dataGridView2.ColumnCount = n;
            dataGridView2.RowCount = 1;
            for (int i = 0; i < n; i++)
                dataGridView2.Columns[i].Width = (dataGridView2.Width - 45) / n;
            while (!exit)
            {
                numiter++;
                for (int i = 0; i < n; i++)
                {
                    F[i] = vm.getVal(i, solution);
                    d[i] = vm.getVal(i, solution);
                    for (int j = 0; j < n; j++)
                    {
                        Fder[i, j] = vm.derVal(i, j, solution);                     
                        CmX[i,j]=C[i,j]-solution[j];
                    }
                }
                for (int anum = 0; anum < n; anum++)
                {
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            if (j == anum)
                                continue;
                            temp[i, j] = CmX[i, j];
                        }
                        temp[i, n - 1] = d[i];
                    }
                    A[0, anum] = det.rmatrixdet(temp, n);
                    if (anum % 2 != 0)
                        A[0, anum] *= -1;
                    if (n % 2 != 0)
                        A[0, anum] *= -1;
                    double deter = det.rmatrixdet(CmX, n);
                    if (deter != 0)
                        A[0, anum] /= deter;
                }
                for (int i = 1; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        A[i, j] = A[0, j];
                    }
                }
                vm.clear(ref temp, n);
                for (int i = 0; i < n; i++)                
                    for (int j = 0; j < n; j++)                    
                        temp[i,j] = Fder[i, j] - A[i, j];
                inv.rmatrixinverse(ref temp, n);
                for (int i = 0; i < n; i++)
                {
                    double tempd=new double();
                    for (int j = 0; j < n; j++)
                        tempd =tempd + temp[i, j] * F[j];
                    tempsmall[i]=tempd;
                }
                for (int i = 0; i < n; i++)
                    newsolution[i] = solution[i] - tempsmall[i];
                exit = checkNewton(newsolution);
                for (int i = 0; i < n; i++)
                    solution[i] = newsolution[i];
                dataGridView2.RowCount = dataGridView2.RowCount + 1;                
                for (int i = 0; i < n; i++)
                    dataGridView2[i, numiter-1].Value = (object)solution[i];
            }
            label3.Text = numiter.ToString();
            dataGridView2.RowCount = dataGridView2.RowCount + 1;
            double neviaz = vm.getVal(0, solution);
            for (int i = 1; i < n; i++)
            {
                if (neviaz < vm.getVal(i, solution))
                    neviaz = vm.getVal(i, solution);
            }
            dataGridView2[0, dataGridView2.RowCount - 1].Value = neviaz.ToString();
            label9.Text = neviaz.ToString();
            

        }

        public bool checkNewton(double[] sol)
        {                     
            double small=0.00000001;
            try
            {
                small = Convert.ToDouble(textBox2.Text);
            }
            catch (FormatException e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);                
            }
            catch(OverflowException e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            for (int i = 0; i < n; i++)
            {
                if (Math.Abs(vm.getVal(i, sol)) > small)
                    return false;
            }
            return true;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            newton();
        }

        public void setSystems()
        {
            String syst;
                syst = "X + X^2 - 2*Y*Z = 0.1" + Environment.NewLine +
                    "Y - Y^2 + 3*Y*Z = -0.2" + Environment.NewLine +
                    "Z + Z^2 + 2*X*Y = 0.3" + Environment.NewLine;
            textBox1.AppendText(syst);
            textBox4.AppendText(syst);
            textBox5.AppendText(syst);
        }
        public void gradient()
        {
            double[] solution, newsolution;
            solution = new double[n];
            newsolution = new double[n];
            for (int i = 0; i < n; i++)
                solution[i] = Convert.ToDouble(dataXStart[0, i].Value);

            double[,] Fder = new double[n, n];
            double[] F = new double[n];
            double M = new double();
            double[] WF = new double[n];
            double[] WWF = new double[n];
            double[] MWF = new double[n];
            bool exit = false;
            int numiter = new int();

            dataGridView4.ColumnCount = n;
            dataGridView4.RowCount = 1;
            for (int i = 0; i < n; i++)
                dataGridView4.Columns[i].Width = (dataGridView4.Width - 45) / n;
            while (!exit)
            {
                numiter++;
                for (int i = 0; i < n; i++)
                {
                    F[i] = vm.getVal(i, solution);
                    for (int j = 0; j < n; j++)                    
                        Fder[i, j] = vm.derVal(i, j, solution);                                           
                }
                for (int i = 0; i < n; i++)
                {
                    double sum=new double();
                    for (int j = 0; j < n; j++)
                        sum = sum + F[j] * Fder[j, i];
                    WF[i] = sum;
                }
                for (int i = 0; i < n; i++)
                {
                    double sum = new double();
                    for (int j = 0; j < n; j++)
                        sum = sum + WF[j] * Fder[i, j];
                    WWF[i] = sum;
                }
                double sum1 = new double();
                double sum2 = new double();
                for (int i = 0; i < n; i++)
                {
                    sum1 = sum1 + F[i] * WWF[i];
                    sum2 = sum2 + AP.Math.Sqr(WWF[i]);
                }
                M = sum1 / sum2;
                for (int i = 0; i < n; i++)
                {
                    double sum = new double();
                    for (int j = 0; j < n; j++)
                        sum = sum + F[j] * Fder[j, i];
                    MWF[i] = sum*M;
                }
                for (int i = 0; i < n; i++)
                    newsolution[i] = solution[i] - MWF[i];
                exit = checkNewton(newsolution);
                for (int i = 0; i < n; i++)
                    solution[i] = newsolution[i];
                dataGridView4.RowCount = dataGridView4.RowCount + 1;
                for (int i = 0; i < n; i++)
                    dataGridView4[i, numiter - 1].Value = (object)solution[i];
            }
            label4.Text = numiter.ToString();
            dataGridView4.RowCount = dataGridView4.RowCount + 1;
            double neviaz = vm.getVal(0, solution);
            for (int i = 1; i < n; i++)
            {
                if (neviaz < vm.getVal(i, solution))
                    neviaz = vm.getVal(i, solution);
            }
            dataGridView4[0, dataGridView4.RowCount - 1].Value=neviaz.ToString();
            label10.Text = neviaz.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            gradient();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            koord();
        }

        public void koord()
        {
            double small = 0.00000001;
            try
            {
                small = Convert.ToDouble(textBox2.Text);
            }
            catch (FormatException e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (OverflowException e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            bool exit = false;
            int numiter = new int();
            double[] F = new double[n];
            for (int i = 0; i < n; i++)
                F[i] = Convert.ToDouble(dataGridView6[0, i].Value);
            dataGridView5.ColumnCount = n;
            dataGridView5.RowCount = 1;
            for (int i = 0; i < n; i++)
                dataGridView5.Columns[i].Width = (dataGridView5.Width - 45) / n;
            while (!exit)
            {
                numiter++;
                dataGridView5.RowCount = dataGridView5.RowCount + 1;
                for (int i = 0; i < n; i++)
                {
                    double temp=new double();
                    vm.brentoptimize(-10, 10, small, ref temp, F, n, i);
                    F[i]=temp;
                }
                exit = checkNewton(F);
                if (numiter > 500)
                    exit = true;
                for (int i = 0; i < n; i++)
                    dataGridView5[i, numiter - 1].Value = (object)F[i];
            }
            label6.Text = numiter.ToString();
            dataGridView5.RowCount = dataGridView5.RowCount + 1;
            double neviaz = vm.getVal(0, F);
            for (int i = 1; i < n; i++)
            {
                if (neviaz < vm.getVal(i, F))
                    neviaz = vm.getVal(i, F);
            }
            dataGridView5[0, dataGridView5.RowCount - 1].Value = neviaz.ToString();
            label11.Text = neviaz.ToString();

            /*

            for (int i = 0; i < n; i++)
                dataGridView5[i, dataGridView5.RowCount - 1].Value = vm.getVal(i, F).ToString();*/
        }

        private void button4_Click(object sender, EventArgs e)
        {
            double d1 = Convert.ToDouble(dataGridView2[0, dataGridView2.RowCount - 1].Value);
            double d2 = Convert.ToDouble(dataGridView4[0, dataGridView4.RowCount - 1].Value);
            double d3 = Convert.ToDouble(dataGridView5[0, dataGridView5.RowCount - 1].Value);
            double d = d1;
            int i = 1;
            if (d < d2)
            {
                d = d2;
                i = 2;
            }
            if (d < d3)
            {
                d = d3;
                i = 3;
            }
            if (d < d2)
            {
                d = d2;
                i = 2;
            }
            switch (i)
            {
                case 1:
                    label8.Text = "Полюсный Ньютона " + d.ToString();
                    break;
                case 2:
                    label8.Text = "Наискорейшего спуска " + d.ToString();
                    break;
                case 3:
                    label8.Text = "Покоординатного спуска " + d.ToString();
                    break;
            }

        }



    }
}
