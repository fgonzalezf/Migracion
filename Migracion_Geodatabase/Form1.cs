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
            if (txtBoxGeodatabaseSalida.Text.Contains(".sde"))
            {
                ListaFeatuaresClass = arreglo.Recorrer(txtBoxGeodatabaseEntrada.Text, txtBoxGeodatabaseSalida.Text, cmbBoxEsquemaSDE.SelectedItem.ToString());
                MessageBox.Show(cmbBoxEsquemaSDE.SelectedItem.ToString());
            }
            else 
            {
                ListaFeatuaresClass = arreglo.Recorrer(txtBoxGeodatabaseEntrada.Text, txtBoxGeodatabaseSalida.Text, "");
            }
            List<string> Rutas_Entrada= ListaFeatuaresClass[0];
            List<string> Rutas_Salida= ListaFeatuaresClass[1];
            Append_Custom append = new Append_Custom();
            using (System.IO.StreamWriter file =
            
            new System.IO.StreamWriter(@txtBoxGeodatabaseEntrada.Text+".txt"))
            {
                for (int i = 0; i <  Rutas_Entrada.Count; i++)
                {
                    // If the line doesn't contain the word 'Second', write the line to the file.
                    append.InsertFeaturesUsingCursor(txtBoxGeodatabaseEntrada.Text + Path.DirectorySeparatorChar + Rutas_Entrada[i],
                                                     txtBoxGeodatabaseSalida.Text + Path.DirectorySeparatorChar + Rutas_Salida[i]
                        );
                        file.WriteLine(Rutas_Entrada[i] + "    ......      " +Rutas_Salida[i]);
                        MessageBox.Show("Cargando" + Rutas_Entrada[i]);
                    
                }
            }
            MessageBox.Show("Finalizado");
        }

        
    }
}
