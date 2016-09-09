namespace Migracion_Geodatabase
{
    partial class formaMigracion
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblProgreso = new System.Windows.Forms.Label();
            this.rdBtnSinRelacionarAnotaciones = new System.Windows.Forms.RadioButton();
            this.rdBtnRelacionandoAnotaciones = new System.Windows.Forms.RadioButton();
            this.rdBtnSinAnotaciones = new System.Windows.Forms.RadioButton();
            this.rdBtnAutocrear = new System.Windows.Forms.RadioButton();
            this.btnMigrar = new System.Windows.Forms.Button();
            this.prgBarProceso = new System.Windows.Forms.ProgressBar();
            this.btnGeodatabaseSalida = new System.Windows.Forms.Button();
            this.btnGeodatabaseEntrada = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBoxGeodatabaseSalida = new System.Windows.Forms.TextBox();
            this.lblGeodatabaseEntrada = new System.Windows.Forms.Label();
            this.txtBoxGeodatabaseEntrada = new System.Windows.Forms.TextBox();
            this.lblEsquemaSDE = new System.Windows.Forms.Label();
            this.cmbBoxEsquemaSDE = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbBoxEsquemaSDE);
            this.groupBox1.Controls.Add(this.lblEsquemaSDE);
            this.groupBox1.Controls.Add(this.lblProgreso);
            this.groupBox1.Controls.Add(this.rdBtnSinRelacionarAnotaciones);
            this.groupBox1.Controls.Add(this.rdBtnRelacionandoAnotaciones);
            this.groupBox1.Controls.Add(this.rdBtnSinAnotaciones);
            this.groupBox1.Controls.Add(this.rdBtnAutocrear);
            this.groupBox1.Controls.Add(this.btnMigrar);
            this.groupBox1.Controls.Add(this.prgBarProceso);
            this.groupBox1.Controls.Add(this.btnGeodatabaseSalida);
            this.groupBox1.Controls.Add(this.btnGeodatabaseEntrada);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtBoxGeodatabaseSalida);
            this.groupBox1.Controls.Add(this.lblGeodatabaseEntrada);
            this.groupBox1.Controls.Add(this.txtBoxGeodatabaseEntrada);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(664, 223);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Migración Geodatabase";
            // 
            // lblProgreso
            // 
            this.lblProgreso.AutoSize = true;
            this.lblProgreso.Location = new System.Drawing.Point(6, 168);
            this.lblProgreso.Name = "lblProgreso";
            this.lblProgreso.Size = new System.Drawing.Size(16, 13);
            this.lblProgreso.TabIndex = 12;
            this.lblProgreso.Text = "...";
            // 
            // rdBtnSinRelacionarAnotaciones
            // 
            this.rdBtnSinRelacionarAnotaciones.AutoSize = true;
            this.rdBtnSinRelacionarAnotaciones.Location = new System.Drawing.Point(431, 150);
            this.rdBtnSinRelacionarAnotaciones.Name = "rdBtnSinRelacionarAnotaciones";
            this.rdBtnSinRelacionarAnotaciones.Size = new System.Drawing.Size(156, 17);
            this.rdBtnSinRelacionarAnotaciones.TabIndex = 11;
            this.rdBtnSinRelacionarAnotaciones.TabStop = true;
            this.rdBtnSinRelacionarAnotaciones.Text = "Sin Relacionar Anotaciones";
            this.rdBtnSinRelacionarAnotaciones.UseVisualStyleBackColor = true;
            // 
            // rdBtnRelacionandoAnotaciones
            // 
            this.rdBtnRelacionandoAnotaciones.AutoSize = true;
            this.rdBtnRelacionandoAnotaciones.Location = new System.Drawing.Point(267, 150);
            this.rdBtnRelacionandoAnotaciones.Name = "rdBtnRelacionandoAnotaciones";
            this.rdBtnRelacionandoAnotaciones.Size = new System.Drawing.Size(156, 17);
            this.rdBtnRelacionandoAnotaciones.TabIndex = 10;
            this.rdBtnRelacionandoAnotaciones.TabStop = true;
            this.rdBtnRelacionandoAnotaciones.Text = "Relacionando Anotaciones ";
            this.rdBtnRelacionandoAnotaciones.UseVisualStyleBackColor = true;
            // 
            // rdBtnSinAnotaciones
            // 
            this.rdBtnSinAnotaciones.AutoSize = true;
            this.rdBtnSinAnotaciones.Location = new System.Drawing.Point(151, 150);
            this.rdBtnSinAnotaciones.Name = "rdBtnSinAnotaciones";
            this.rdBtnSinAnotaciones.Size = new System.Drawing.Size(102, 17);
            this.rdBtnSinAnotaciones.TabIndex = 9;
            this.rdBtnSinAnotaciones.TabStop = true;
            this.rdBtnSinAnotaciones.Text = "Sin Anotaciones";
            this.rdBtnSinAnotaciones.UseVisualStyleBackColor = true;
            // 
            // rdBtnAutocrear
            // 
            this.rdBtnAutocrear.AutoSize = true;
            this.rdBtnAutocrear.Location = new System.Drawing.Point(9, 150);
            this.rdBtnAutocrear.Name = "rdBtnAutocrear";
            this.rdBtnAutocrear.Size = new System.Drawing.Size(133, 17);
            this.rdBtnAutocrear.TabIndex = 8;
            this.rdBtnAutocrear.TabStop = true;
            this.rdBtnAutocrear.Text = "Autocrear Anotaciones";
            this.rdBtnAutocrear.UseVisualStyleBackColor = true;
            // 
            // btnMigrar
            // 
            this.btnMigrar.Enabled = false;
            this.btnMigrar.Location = new System.Drawing.Point(593, 191);
            this.btnMigrar.Name = "btnMigrar";
            this.btnMigrar.Size = new System.Drawing.Size(51, 23);
            this.btnMigrar.TabIndex = 7;
            this.btnMigrar.Text = "Migrar";
            this.btnMigrar.UseVisualStyleBackColor = true;
            this.btnMigrar.Click += new System.EventHandler(this.btnMigrar_Click);
            // 
            // prgBarProceso
            // 
            this.prgBarProceso.Location = new System.Drawing.Point(9, 193);
            this.prgBarProceso.Name = "prgBarProceso";
            this.prgBarProceso.Size = new System.Drawing.Size(575, 23);
            this.prgBarProceso.TabIndex = 6;
            // 
            // btnGeodatabaseSalida
            // 
            this.btnGeodatabaseSalida.Location = new System.Drawing.Point(593, 94);
            this.btnGeodatabaseSalida.Name = "btnGeodatabaseSalida";
            this.btnGeodatabaseSalida.Size = new System.Drawing.Size(51, 23);
            this.btnGeodatabaseSalida.TabIndex = 5;
            this.btnGeodatabaseSalida.Text = "...";
            this.btnGeodatabaseSalida.UseVisualStyleBackColor = true;
            this.btnGeodatabaseSalida.Click += new System.EventHandler(this.btnGeodatabaseSalida_Click);
            // 
            // btnGeodatabaseEntrada
            // 
            this.btnGeodatabaseEntrada.Location = new System.Drawing.Point(593, 40);
            this.btnGeodatabaseEntrada.Name = "btnGeodatabaseEntrada";
            this.btnGeodatabaseEntrada.Size = new System.Drawing.Size(51, 23);
            this.btnGeodatabaseEntrada.TabIndex = 4;
            this.btnGeodatabaseEntrada.Text = "...";
            this.btnGeodatabaseEntrada.UseVisualStyleBackColor = true;
            this.btnGeodatabaseEntrada.Click += new System.EventHandler(this.btnGeodatabaseEntrada_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.label1.Location = new System.Drawing.Point(3, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(209, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Geodatabase Salida (Personal, File ó SDE)";
            // 
            // txtBoxGeodatabaseSalida
            // 
            this.txtBoxGeodatabaseSalida.Location = new System.Drawing.Point(3, 94);
            this.txtBoxGeodatabaseSalida.Name = "txtBoxGeodatabaseSalida";
            this.txtBoxGeodatabaseSalida.Size = new System.Drawing.Size(581, 20);
            this.txtBoxGeodatabaseSalida.TabIndex = 2;
            // 
            // lblGeodatabaseEntrada
            // 
            this.lblGeodatabaseEntrada.AutoSize = true;
            this.lblGeodatabaseEntrada.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.lblGeodatabaseEntrada.Location = new System.Drawing.Point(6, 20);
            this.lblGeodatabaseEntrada.Name = "lblGeodatabaseEntrada";
            this.lblGeodatabaseEntrada.Size = new System.Drawing.Size(189, 13);
            this.lblGeodatabaseEntrada.TabIndex = 1;
            this.lblGeodatabaseEntrada.Text = "Geodatabase Entrada (Personal o File)";
            // 
            // txtBoxGeodatabaseEntrada
            // 
            this.txtBoxGeodatabaseEntrada.Location = new System.Drawing.Point(6, 40);
            this.txtBoxGeodatabaseEntrada.Name = "txtBoxGeodatabaseEntrada";
            this.txtBoxGeodatabaseEntrada.Size = new System.Drawing.Size(581, 20);
            this.txtBoxGeodatabaseEntrada.TabIndex = 0;
            // 
            // lblEsquemaSDE
            // 
            this.lblEsquemaSDE.AutoSize = true;
            this.lblEsquemaSDE.Location = new System.Drawing.Point(3, 125);
            this.lblEsquemaSDE.Name = "lblEsquemaSDE";
            this.lblEsquemaSDE.Size = new System.Drawing.Size(76, 13);
            this.lblEsquemaSDE.TabIndex = 13;
            this.lblEsquemaSDE.Text = "Esquema SDE";
            // 
            // cmbBoxEsquemaSDE
            // 
            this.cmbBoxEsquemaSDE.Enabled = false;
            this.cmbBoxEsquemaSDE.FormattingEnabled = true;
            this.cmbBoxEsquemaSDE.Location = new System.Drawing.Point(95, 120);
            this.cmbBoxEsquemaSDE.Name = "cmbBoxEsquemaSDE";
            this.cmbBoxEsquemaSDE.Size = new System.Drawing.Size(185, 21);
            this.cmbBoxEsquemaSDE.TabIndex = 14;
            // 
            // formaMigracion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 247);
            this.Controls.Add(this.groupBox1);
            this.Name = "formaMigracion";
            this.Text = "Migración Geodatabase IGAC V1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblProgreso;
        private System.Windows.Forms.RadioButton rdBtnSinRelacionarAnotaciones;
        private System.Windows.Forms.RadioButton rdBtnRelacionandoAnotaciones;
        private System.Windows.Forms.RadioButton rdBtnSinAnotaciones;
        private System.Windows.Forms.RadioButton rdBtnAutocrear;
        private System.Windows.Forms.Button btnMigrar;
        private System.Windows.Forms.ProgressBar prgBarProceso;
        private System.Windows.Forms.Button btnGeodatabaseSalida;
        private System.Windows.Forms.Button btnGeodatabaseEntrada;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBoxGeodatabaseSalida;
        private System.Windows.Forms.Label lblGeodatabaseEntrada;
        private System.Windows.Forms.TextBox txtBoxGeodatabaseEntrada;
        private System.Windows.Forms.ComboBox cmbBoxEsquemaSDE;
        private System.Windows.Forms.Label lblEsquemaSDE;
    }
}