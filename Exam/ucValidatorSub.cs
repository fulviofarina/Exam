﻿using System;
using System.Windows.Forms;

namespace Exam
{
    public partial class ucValidatorSub : UserControl
    {
     
        private Interface Interface;

        public string ScanResult
        {
            get;
            set;
        }


        public void AssociateExamOrStudent() //tiene que ser toda una funcion
        {
         
     
            if (string.IsNullOrEmpty(ScanResult))
            {
                Interface.Status = "Texto escaneado vacío";
                return;
            }

       

            bool isStuAnsw = Interface.IValidator.IsStudentScanned(ScanResult);

            if (isStuAnsw) //but if it was scanned as a StuID and Answer...
            {
                string answerRAW = string.Empty; //answer already in the box (typed manually)
                string stuID = string.Empty; //use as default the carnebox Text

                Interface.IValidator.GetScannedData(ScanResult, ref stuID, ref answerRAW);
                ScanResult = string.Empty; //IMPORTANTISIMO RESETEA LA VARIABLE GLOBAL!!! UNA VEZ ACEPTADO EL CARNET*RESPUESTA

                refreshStuIDAndAnswer(answerRAW, stuID);

                Interface.IValidator.AssignStudentAnswer(answerRAW); //assign answer to student

                // FindByStudentID(stuID); //finds the student.
            }
            else
            {
                //this.picBox.Image = null;

                bool valid = Interface.IValidator.AssignExamModel(ScanResult);
                if (valid) ScanResult = string.Empty; //IMPORTANTISIMO RESETEA LA VARIABLE GLOBAL!!! UNA VEZ ENCONTRADO EL EXAMEN
                else return; //el codigo QR está siendo mandando por partes!!!
            }

            this.picBox.Image = Interface.IValidator.Image; //it can be null

            bool evaluated = false;
            this.carneBox.Enabled = false;
            string studeID = this.carneBox.Text;

            evaluated = Interface.IValidator.ValidateExamStudent(); ///then validate the exam!!!

            if (evaluated)
            {
                refreshStuIDAndAnswer(string.Empty, studeID);
            }

            this.carneBox.Enabled = true;
        }

        private void carneBox_TextChanged(object sender, EventArgs e)
        {
            this.verBox.Text = string.Empty;

            string old = carneBox.Text;

            this.carneBox.Text = Interface.IValidator.AssignStudent(old);
        }

      

        private void refreshStuIDAndAnswer(string answerRAW, string stuID)
        {
            this.carneBox.Text = string.Empty;
            this.carneBox.Text = stuID;
            this.verBox.Text = answerRAW;
        }

     

        private void setBindings()
        {
            String txt = "Text";
            DataSourceUpdateMode mode = DataSourceUpdateMode.OnPropertyChanged;

            Binding score = new Binding(txt, Interface.IBS.Student, Interface.IdB.Student.ScoreColumn.ColumnName, true, mode);
            Binding correct = new Binding(txt, Interface.IBS.Student, Interface.IdB.Student.CorrectColumn.ColumnName, true, mode);
            this.scoreBox.TextBox.DataBindings.Add(score);
            this.CorrectBox.TextBox.DataBindings.Add(correct);

            Binding name = new Binding(txt, Interface.IBS.StudentsList, Interface.IdB.StuList.FirstNamesColumn.ColumnName, true, mode);
            Binding surname = new Binding(txt, Interface.IBS.StudentsList, Interface.IdB.StuList.LastNamesColumn.ColumnName, true, mode);
            this.nameBox.TextBox.DataBindings.Add(name);
            this.surnameBox.TextBox.DataBindings.Add(surname);

            Binding title = new Binding(txt, Interface.IBS.Preferences, Interface.IdB.Preferences.TitleColumn.ColumnName, true, mode);
            Binding Classe = new Binding(txt, Interface.IBS.Preferences, Interface.IdB.Preferences.ClassColumn.ColumnName, true, mode);
        }

     

        /// <summary>
        /// VALIDACION DE LOS RESULTADOS DE UN ALUMNO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void validator_Click(object sender, EventArgs e)
        {
            // this.studentDataGridView.SuspendLayout();
            this.carneBox.Enabled = false;
            string stuID = this.carneBox.Text;
            this.verBox.Enabled = false;

            bool ok = Interface.IValidator.AssignStudentAnswer(verBox.Text);
            ok = ok && Interface.IValidator.ValidateExamStudent();
            if (ok)
            {
                this.carneBox.Text = string.Empty;
                this.carneBox.Text = stuID;
                this.verBox.Text = string.Empty;
            }

            this.carneBox.Enabled = true;
            this.verBox.Enabled = true;
        }

        public void Set(ref Interface inter)
        {
          //  Rsx.Dumb.Dumb.FD(ref dB);

            Interface = inter;

            setBindings();

          
        }

        public ucValidatorSub()
        {
            InitializeComponent();
           
        }
    }
}