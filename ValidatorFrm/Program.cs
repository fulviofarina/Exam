﻿using System;
using System.Windows.Forms;

namespace Exam
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string path = Application.StartupPath.ToString();

            Interface Interface = new Interface(path);
            ValidFrm form = new ValidFrm();
            form.WindowState = FormWindowState.Maximized;

            form.Set(ref Interface);

        
            Application.Run(form);
        }
    }
}