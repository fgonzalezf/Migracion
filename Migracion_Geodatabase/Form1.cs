using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Migracion_Geodatabase
{
    public partial class formaMigracion : Form
    {
        public formaMigracion()
        {
            InitializeComponent();
            //Valores combobox
            cmbBoxEsquemaSDE.Items.Add("ADM25MIL");
            cmbBoxEsquemaSDE.Items.Add("ADMCIENMIL");
            cmbBoxEsquemaSDE.Items.Add("ADMPRUEBAS");

        }

        private void btnGeodatabaseEntrada_Click(object sender, EventArgs e)
        {
            Busqueda gdbEntrada = new Busqueda();
            txtBoxGeodatabaseEntrada.Text = gdbEntrada.BuscarEntrada();


            if (txtBoxGeodatabaseEntrada.Text != "" && txtBoxGeodatabaseSalida.Text != "")
            {
                btnMigrar.Enabled = true;
                
            }
        }
        private void btnGeodatabaseSalida_Click(object sender, EventArgs e)
        {
            Busqueda gdbEntrada = new Busqueda();
            txtBoxGeodatabaseSalida.Text = gdbEntrada.BuscarSalida();

            if (txtBoxGeodatabaseSalida.Text.Contains(".sde"))
            {
                cmbBoxEsquemaSDE.Enabled=true;
            }
            else
            {
                cmbBoxEsquemaSDE.Enabled = false;
            }
            if (txtBoxGeodatabaseEntrada.Text != "" && txtBoxGeodatabaseSalida.Text != "")
            {
                btnMigrar.Enabled = true;

            }

        }

        private void btnMigrar_Click(object sender, EventArgs e)
        {
            RecorrerGDB arreglo = new RecorrerGDB();
            string Imprimir="Terminado";
            List<List<string>> ListaFeatuaresClass=new List<List<string>>();
            
            
            Append_Custom append = new Append_Custom();


            // Display the ProgressBar control.
            prgBarProceso.Visible = true;
            // Set Minimum to 1 to represent the first file being copied.
            prgBarProceso.Minimum = 1;
            // Set Maximum to the total number of files to copy.
           
            // Set the initial value of the ProgressBar.
            prgBarProceso.Value = 1;
            // Set the Step property to a value of 1 to represent each file being copied.
            prgBarProceso.Step = 1;


            if (rdBtnAutocrear.Checked==true)
            {
                if (txtBoxGeodatabaseSalida.Text.Contains(".sde"))
                {
                    ListaFeatuaresClass = arreglo.Recorrer(txtBoxGeodatabaseEntrada.Text, txtBoxGeodatabaseSalida.Text, cmbBoxEsquemaSDE.SelectedItem.ToString(), true);
                    MessageBox.Show(cmbBoxEsquemaSDE.SelectedItem.ToString());
                }
                else
                {
                    ListaFeatuaresClass = arreglo.Recorrer(txtBoxGeodatabaseEntrada.Text, txtBoxGeodatabaseSalida.Text, "", true);
                }
                List<string> Rutas_Entrada = ListaFeatuaresClass[0];
                List<string> Rutas_Salida = ListaFeatuaresClass[1];
                List<string> Tipo = ListaFeatuaresClass[2];
                List<string> TipoCargue = ListaFeatuaresClass[3];
                prgBarProceso.Maximum = Rutas_Entrada.Count;
                
            }
            else if (rdBtnRelacionandoAnotaciones.Checked==true)
            {
                if (txtBoxGeodatabaseSalida.Text.Contains(".sde"))
                    {
                        ListaFeatuaresClass = arreglo.Recorrer(txtBoxGeodatabaseEntrada.Text, txtBoxGeodatabaseSalida.Text, cmbBoxEsquemaSDE.SelectedItem.ToString(),false);
                        MessageBox.Show(cmbBoxEsquemaSDE.SelectedItem.ToString());
                    }
                else 
                {
                    ListaFeatuaresClass = arreglo.Recorrer(txtBoxGeodatabaseEntrada.Text, txtBoxGeodatabaseSalida.Text, "", false);
                }
                List<string> Rutas_Entrada = ListaFeatuaresClass[0];
                List<string> Rutas_Salida = ListaFeatuaresClass[1];
                List<string> Tipo = ListaFeatuaresClass[2];
                List<string> TipoCargue = ListaFeatuaresClass[3];
          
                prgBarProceso.Maximum = Rutas_Entrada.Count;
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@txtBoxGeodatabaseEntrada.Text + ".txt"))
                {
                    for (int i = 0; i < Rutas_Entrada.Count; i++)
                    {
                        string FeatAnot="";
                        if (Rutas_Salida[i] != "...")
                        {

                            if (Tipo[i] == "Annotation")
                            {
                                // If the line doesn't contain the word 'Second', write the line to the file.
                                append.InsertFeaturesUsingCursor(txtBoxGeodatabaseEntrada.Text + Path.DirectorySeparatorChar + Rutas_Entrada[i],
                                                                 txtBoxGeodatabaseSalida.Text + Path.DirectorySeparatorChar + Rutas_Salida[i],
                                                                 Tipo[i]
                                                                 
                                    );
                            }
                            else if (TipoCargue[i] == "Cargar" && Tipo[i] != "Raster")
                            {
                                append.AppendTest(txtBoxGeodatabaseEntrada.Text + Path.DirectorySeparatorChar + Rutas_Entrada[i],
                                                            txtBoxGeodatabaseSalida.Text + Path.DirectorySeparatorChar + Rutas_Salida[i]
                                                            
                               );
                            }

                            else if (TipoCargue[i] == "Cargar" && Tipo[i] == "Raster")
                            {
                                append.AppendRaster(txtBoxGeodatabaseEntrada.Text + Path.DirectorySeparatorChar + Rutas_Entrada[i],
                                    txtBoxGeodatabaseSalida.Text + Path.DirectorySeparatorChar + Rutas_Salida[i]);
                            }
                            file.WriteLine(Rutas_Entrada[i] + "    ......      " + Rutas_Salida[i] + "....." + Tipo[i]+ "....."+ TipoCargue[i]);
                            //MessageBox.Show("Cargando" + Rutas_Entrada[i]);                        
                            lblProgreso.Text = "Cargando" + Rutas_Entrada[i];
                            lblProgreso.Refresh();
                            prgBarProceso.PerformStep();
                        }
                    }
                }

            }
            else if (rdBtnSinAnotaciones.Checked==true)
            {
                if (txtBoxGeodatabaseSalida.Text.Contains(".sde"))
                {
                    ListaFeatuaresClass = arreglo.Recorrer(txtBoxGeodatabaseEntrada.Text, txtBoxGeodatabaseSalida.Text, cmbBoxEsquemaSDE.SelectedItem.ToString(), false);
                    MessageBox.Show(cmbBoxEsquemaSDE.SelectedItem.ToString());
                }
                else
                {
                    ListaFeatuaresClass = arreglo.Recorrer(txtBoxGeodatabaseEntrada.Text, txtBoxGeodatabaseSalida.Text, "", false);
                }
                List<string> Rutas_Entrada = ListaFeatuaresClass[0];
                List<string> Rutas_Salida = ListaFeatuaresClass[1];
                List<string> Tipo = ListaFeatuaresClass[2];
                List<string> TipoCargue = ListaFeatuaresClass[3];
                prgBarProceso.Maximum = Rutas_Entrada.Count;
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@txtBoxGeodatabaseEntrada.Text + ".txt"))
                {
                    for (int i = 0; i < Rutas_Entrada.Count; i++)
                    {
                        if (Rutas_Salida[i] != "..." && Tipo[i] != "Annotation" && Tipo[i] != "Raster")
                        {
                            // If the line doesn't contain the word 'Second', write the line to the file.
                            append.AppendTest(txtBoxGeodatabaseEntrada.Text + Path.DirectorySeparatorChar + Rutas_Entrada[i],
                                                             txtBoxGeodatabaseSalida.Text + Path.DirectorySeparatorChar + Rutas_Salida[i]
                                );
                            file.WriteLine(Rutas_Entrada[i] + "    ......      " + Rutas_Salida[i]);
                            //MessageBox.Show("Cargando" + Rutas_Entrada[i]);                        
                            lblProgreso.Text = "Cargando" + Rutas_Entrada[i];
                            lblProgreso.Refresh();
                            prgBarProceso.PerformStep();
                        }
                        else if (Rutas_Salida[i] != "..." && Tipo[i] == "Raster")
                        {
                            append.AppendRaster(txtBoxGeodatabaseEntrada.Text + Path.DirectorySeparatorChar + Rutas_Entrada[i],
                                txtBoxGeodatabaseSalida.Text + Path.DirectorySeparatorChar + Rutas_Salida[i]);
                        }
                    }
                }

            }
            else if (rdBtnSinRelacionarAnotaciones.Checked==true)
            {
                if (txtBoxGeodatabaseSalida.Text.Contains(".sde"))
                {
                    ListaFeatuaresClass = arreglo.Recorrer(txtBoxGeodatabaseEntrada.Text, txtBoxGeodatabaseSalida.Text, cmbBoxEsquemaSDE.SelectedItem.ToString(), false);
                    MessageBox.Show(cmbBoxEsquemaSDE.SelectedItem.ToString());
                }
                else
                {
                    ListaFeatuaresClass = arreglo.Recorrer(txtBoxGeodatabaseEntrada.Text, txtBoxGeodatabaseSalida.Text, "", false);
                }
                List<string> Rutas_Entrada = ListaFeatuaresClass[0];
                List<string> Rutas_Salida = ListaFeatuaresClass[1];
                List<string> Tipo = ListaFeatuaresClass[2];
                List<string> TipoCargue = ListaFeatuaresClass[3];
                prgBarProceso.Maximum = Rutas_Entrada.Count;
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@txtBoxGeodatabaseEntrada.Text + ".txt"))
                {
                    for (int i = 0; i < Rutas_Entrada.Count; i++)
                    {
                        if (Rutas_Salida[i] != "..." && Tipo[i]!="Raster")
                        {
                            // If the line doesn't contain the word 'Second', write the line to the file.
                            append.AppendTest(txtBoxGeodatabaseEntrada.Text + Path.DirectorySeparatorChar + Rutas_Entrada[i],
                                                             txtBoxGeodatabaseSalida.Text + Path.DirectorySeparatorChar + Rutas_Salida[i]
                                );
                            file.WriteLine(Rutas_Entrada[i] + "    ......      " + Rutas_Salida[i]);
                            //MessageBox.Show("Cargando" + Rutas_Entrada[i]);                        
                            lblProgreso.Text = "Cargando" + Rutas_Entrada[i];
                            lblProgreso.Refresh();
                            prgBarProceso.PerformStep();
                        }
                        else if (Rutas_Salida[i] != "..." && Tipo[i]=="Raster")
                        {
                            append.AppendRaster(txtBoxGeodatabaseEntrada.Text + Path.DirectorySeparatorChar + Rutas_Entrada[i],
                                txtBoxGeodatabaseSalida.Text + Path.DirectorySeparatorChar + Rutas_Salida[i]);
                        }
                    }
                }

            }

            
            MessageBox.Show("Finalizado");
        }

        private void rdBtnAutocrear_CheckedChanged(object sender, EventArgs e)
        {
            if (rdBtnAutocrear.Checked==true)
            {
                rdBtnRelacionandoAnotaciones.Checked = false;
                rdBtnSinAnotaciones.Checked = false;
                rdBtnSinRelacionarAnotaciones.Checked = false;
            }
        }

        private void rdBtnSinAnotaciones_CheckedChanged(object sender, EventArgs e)
        {
            if (rdBtnSinAnotaciones.Checked == true)
            {
                rdBtnRelacionandoAnotaciones.Checked = false;
                rdBtnAutocrear.Checked = false;
                rdBtnSinRelacionarAnotaciones.Checked = false;
            }
        }

        private void rdBtnRelacionandoAnotaciones_CheckedChanged(object sender, EventArgs e)
        {
            if (rdBtnRelacionandoAnotaciones.Checked == true)
            {
                rdBtnAutocrear.Checked = false;
                rdBtnSinAnotaciones.Checked = false;
                rdBtnSinRelacionarAnotaciones.Checked = false;
            }
        }

        private void rdBtnSinRelacionarAnotaciones_CheckedChanged(object sender, EventArgs e)
        {
            if (rdBtnSinRelacionarAnotaciones.Checked == true)
            {
                rdBtnRelacionandoAnotaciones.Checked = false;
                rdBtnSinAnotaciones.Checked = false;
                rdBtnAutocrear.Checked = false;
            }
        }

        
        
    }
}
