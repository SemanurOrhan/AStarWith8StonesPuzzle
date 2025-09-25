using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AStarWith8StonesPuzzle
{
    public partial class StartForm : Form
    {
        private List<ComboBox> startStateComboBoxes = new List<ComboBox>();
        private List<ComboBox> targetStateComboBoxes = new List<ComboBox>();

        public int[,] StartState { get; private set; } = new int[3, 3];
        public int[,] TargetState { get; private set; } = new int[3, 3];

        public StartForm()
        {
            InitializeComponent();

            startStateComboBoxes.AddRange(new[] { comboBox1, comboBox2, comboBox3, comboBox4, comboBox5, comboBox6, comboBox7, comboBox8, comboBox9 });
            targetStateComboBoxes.AddRange(new[] { comboBox10, comboBox11, comboBox12, comboBox13, comboBox14, comboBox15, comboBox16, comboBox17, comboBox18 });

            InitializeComboBoxes(startStateComboBoxes);
            InitializeComboBoxes(targetStateComboBoxes);
        }

        private void InitializeComboBoxes(List<ComboBox> comboBoxes)
        {
            foreach (var comboBox in comboBoxes)
            {
                comboBox.Items.AddRange(Enumerable.Range(0, 9).Select(x => x.ToString()).ToArray());
                comboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            }
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var currentComboBox = sender as ComboBox;

            var comboBoxList = startStateComboBoxes.Contains(currentComboBox) ? startStateComboBoxes : targetStateComboBoxes;

            foreach (var comboBox in comboBoxList)
            {
                if (comboBox != currentComboBox && comboBox.Items.Contains(currentComboBox.SelectedItem?.ToString()))
                {
                    comboBox.Items.Remove(currentComboBox.SelectedItem?.ToString());
                }
            }

            foreach (var comboBox in comboBoxList)
            {
                if (comboBox != currentComboBox && !comboBox.Items.Contains(currentComboBox.SelectedItem?.ToString()))
                {
                    comboBox.Items.Add(currentComboBox.SelectedItem?.ToString());
                }
            }
        }

        private void StartStateBtn_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var comboBox = startStateComboBoxes[i * 3 + j];
                    StartState[i, j] = int.Parse(comboBox.SelectedItem?.ToString() ?? "0");
                }
            }

            MessageBox.Show("Başlangıç durumu kaydedildi!");
        }

        private void TargetStateBtn_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var comboBox = targetStateComboBoxes[i * 3 + j];
                    TargetState[i, j] = int.Parse(comboBox.SelectedItem?.ToString() ?? "0");
                }
            }

            MessageBox.Show("Hedef durumu kaydedildi!");
        }
        private void formDegistirBtn_Click(object sender, EventArgs e)
        {
            var mainForm = new MainForm(StartState, TargetState);
            mainForm.Show();
            this.Hide();
        }
    }
}
