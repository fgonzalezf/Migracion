using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Migracion_Geodatabase
{
    public partial class formaMigracion : Form
    {
        public formaMigracion()
        {
            InitializeComponent();
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
            txtBoxGeodatabaseSalida.Text = gdbEntrada.BuscarEntrada();


            if (txtBoxGeodatabaseEntrada.Text != "" && txtBoxGeodatabaseSalida.Text != "")
            {
                btnMigrar.Enabled = true;

            }

        }

        private void btnMigrar_Click(object sender, EventArgs e)
        {
            RecorrerGDB arreglo = new RecorrerGDB();
            string Imprimir="Terminado";
            List<List<string>> ListaFeatuaresClass= arreglo.Recorrer(txtBoxGeodatabaseEntrada.Text ,txtBoxGeodatabaseSalida.Text,"Todos");
            List<string> Rutas_Entrada= ListaFeatuaresClass[0];
            List<string> Rutas_Salida= ListaFeatuaresClass[1];
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@txtBoxGeodatabaseEntrada.Text+".txt"))
            {
                for (int i = 0; i <  Rutas_Entrada.Count; i++)
                {
                    // If the line doesn't contain the word 'Second', write the line to the file.
                    
                        file.WriteLine(Rutas_Entrada[i] + "    ......      " +Rutas_Salida[i]);
                    
                }
            }
            MessageBox.Show(Imprimir);
        }

        
    }
}
