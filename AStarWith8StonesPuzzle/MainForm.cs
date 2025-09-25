using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;

namespace AStarWith8StonesPuzzle
{
    public partial class MainForm : Form
    {
        private int[,] StartState;
        private int[,] TargetState;

        public MainForm(int[,] startState, int[,] targetState)
        {
            InitializeComponent();
            StartState = startState;
            TargetState = targetState;

            UpdateGrid(StartState, tableLayoutPanel1);
            UpdateGrid(TargetState, tableLayoutPanel2);
            ExitBTN.Visible = false;
        }

        private async void SolveButton_Click(object sender, EventArgs e)
        {
            SolveButton.Enabled = false;

            await Task.Run(() =>
            {
                if (!IsSolvable(StartState, TargetState))
                {
                    MessageBox.Show("Bu durum çözülebilir değil!");
                    return;
                }

                var solver = new AStarSolver();
                var path = solver.Solve(StartState, TargetState);

                if (path != null)
                {
                    foreach (var node in path)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            UpdateGrid(node.State, tableLayoutPanel1);
                            AddStepToFlowLayoutPanel(node.State);
                        });

                        if (IsGoalState(node.State, TargetState))
                        {
                            MessageBox.Show("Hedefe ulaşıldı!");
                            break;
                        }
                        System.Threading.Thread.Sleep(500);
                    }
                }
                else
                {
                    MessageBox.Show("Çözüm bulunamadı!");
                }
            });

            ExitBTN.Visible = true;

            SolveButton.Enabled = true;
        }

        private bool IsSolvable(int[,] startState, int[,] goalState)
        {
            int[] startArray = startState.Cast<int>().ToArray();
            int[] goalArray = goalState.Cast<int>().ToArray();

            int startInversions = CountInversions(startArray);
            int goalInversions = CountInversions(goalArray);

            return (startInversions % 2) == (goalInversions % 2);
        }

        private int CountInversions(int[] array)
        {
            int inversions = 0;
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = i + 1; j < array.Length; j++)
                {
                    if (array[i] > array[j] && array[i] != 0 && array[j] != 0)
                        inversions++;
                }
            }
            return inversions;
        }

        private void UpdateGrid(int[,] state, TableLayoutPanel tableLayoutPanel)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var label = (Label)tableLayoutPanel.GetControlFromPosition(j, i);
                    label.Text = state[i, j] == 0 ? "" : state[i, j].ToString();
                }
            }
        }

        private bool IsGoalState(int[,] currentState, int[,] goalState)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (currentState[i, j] != goalState[i, j])
                        return false;
                }
            }
            return true;
        }

        private void AddStepToFlowLayoutPanel(int[,] state)
        {
            var stepPanel = new TableLayoutPanel();
            stepPanel.RowCount = 3;
            stepPanel.ColumnCount = 3;
            stepPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetPartial;
            stepPanel.Dock = DockStyle.Top;
            stepPanel.AutoSize = true;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var label = new Label
                    {
                        Text = state[i, j] == 0 ? "" : state[i, j].ToString(),
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter,
                        AutoSize = true
                    };
                    stepPanel.Controls.Add(label, j, i);
                }
            }

            flowLayoutPanel1.Controls.Add(stepPanel);
        }

        private void ExitBTN_Click(object sender, EventArgs e)
        {
            Application.Exit(); 
        }
    }
}