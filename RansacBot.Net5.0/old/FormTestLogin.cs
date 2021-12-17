﻿using RansacRealTime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace RansacBot.old
{
    public partial class FormTestLogin : Form
    {
        public FormTestLogin()
        {
            InitializeComponent();
        }

        #region Обработчики элементов управления

        private void FormTestLogin_Load(object sender, EventArgs e)
        {
            cbFilterMMR.SelectedIndex = 0;
            cbFilterOR.SelectedIndex = 1;
            cbCloseN1.SelectedIndex = 2;
            cbCloseN2.SelectedIndex = 3;
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            MonkeyNFilter monkeyNFilter = new((double)nmcN.Value);
            Vertexes vertexes = new();

            RansacsHystory ransacHystory1 = new(vertexes, ToTypeSigma(cbFilterMMR.Text), (int)nmcLevelMMR.Value - 1);
            RansacsHystory ransacHystory2 = new(vertexes, ToTypeSigma(cbFilterOR.Text), (int)nmcLevelOR.Value - 1);
            RansacsHystory ransacHystory3 = new(vertexes, ToTypeSigma(cbCloseN1.Text), (int)nmcLevelCloseN1.Value - 1);
            RansacsHystory ransacHystory4 = new(vertexes, ToTypeSigma(cbCloseN2.Text), (int)nmcLevelCloseN2.Value - 1);

            monkeyNFilter.NewVertex += vertexes.OnNewVertex;
            RansacsSession ransacObserver = new(vertexes, monkeyNFilter);
            InstrumentObserver.Initialization(ransacObserver, new Instrument(), (double)nmcN.Value, (double)nmcPercentCloseN1.Value);


            DialogResult = DialogResult.OK;
            Close();
        }
        private void btnCansel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            InstrumentObserver.Initialization((double)nmcN.Value, (double)nmcPercentCloseN1.Value);

            DialogResult = DialogResult.OK;
            Close();
        }
        #endregion

        private static TypeSigma ToTypeSigma(string text)
        {
            return text switch
            {
                "ConfidenceInterval" => TypeSigma.СonfidenceInterval,
                "ErrorThreshold" => TypeSigma.ErrorThreshold,
                "SigmaInliers" => TypeSigma.SigmaInliers,
                _ => TypeSigma.Sigma,
            };
        }


    }
}