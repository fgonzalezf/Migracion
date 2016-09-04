using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;


namespace Migracion_Geodatabase
{
    public class Migracion : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public Migracion()
        {
        }

        protected override void OnClick()
        {
            formaMigracion VentanaInicial = new formaMigracion();
            VentanaInicial.Show();

        }

        protected override void OnUpdate()
        {
            Enabled = ArcCatalog.Application != null;
        }
    }
}
