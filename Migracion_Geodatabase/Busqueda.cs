using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.CatalogUI;
using ESRI.ArcGIS.Catalog;
using System.Windows.Forms;
using ESRI.ArcGIS.Framework;

namespace Migracion_Geodatabase
{
    public class Busqueda
    {
        private IGxDialog pGxdialog;
        private IGxObjectFilter pShpFilter;
        private IGxObjectFilter pLyrFilter;
        private IGxObjectFilter pSdeFilter;
        private IGxObjectFilterCollection pFiltercol;
        private IEnumGxObject pEnumGx;


        public Busqueda()
        {
            pGxdialog = new ESRI.ArcGIS.CatalogUI.GxDialog() as IGxDialog;
            pShpFilter = new ESRI.ArcGIS.Catalog.GxFilterPersonalGeodatabasesClass() as IGxObjectFilter;
            pLyrFilter = new ESRI.ArcGIS.Catalog.GxFilterFileGeodatabases() as IGxObjectFilter;
            pSdeFilter = new ESRI.ArcGIS.Catalog.GxFilterWorkspaces() as IGxObjectFilter;
            pFiltercol = pGxdialog as IGxObjectFilterCollection;

        }

        public string BuscarSalida()
        {
            string rutaGeoada;
            try
            {
                pGxdialog = new ESRI.ArcGIS.CatalogUI.GxDialog() as IGxDialog;
                pFiltercol = pGxdialog as IGxObjectFilterCollection;
                this.pFiltercol.AddFilter(this.pLyrFilter, true);
                this.pFiltercol.AddFilter(this.pShpFilter, true);
                this.pFiltercol.AddFilter(this.pSdeFilter, true);
                if (pGxdialog.DoModalOpen(0, out this.pEnumGx) == false)
                {
                    return "";
                }
                string RutaTemp = this.pEnumGx.Next().FullName;

                if (RutaTemp.Contains("Database Connections"))
                {

                    string userprofile = System.Environment.GetEnvironmentVariable("UserProfile");
                    //rutaGeoada = RutaTemp.Replace("Database Connections", userprofile + @"\AppData\Roaming\ESRI\Desktop10.2\ArcCatalog");
                    rutaGeoada = RutaTemp.Replace("Database Connections", userprofile + @"\Datos de programa\ESRI\Desktop10.3\ArcCatalog");
                }
                else
                {
                    rutaGeoada = RutaTemp;
                }


                return rutaGeoada;
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Vaya se produjo un error (function buscar.buscar) Error describe" + ex.Message);
                return "";


            }
        }
        public string BuscarEntrada()
        {
            try
            {
                pGxdialog = new ESRI.ArcGIS.CatalogUI.GxDialog() as IGxDialog;
                pFiltercol = pGxdialog as IGxObjectFilterCollection;
                this.pFiltercol.AddFilter(this.pLyrFilter, true);
                this.pFiltercol.AddFilter(this.pShpFilter, true);
                if (pGxdialog.DoModalOpen(0, out this.pEnumGx) == false)
                {
                    return "";
                }

                return this.pEnumGx.Next().FullName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Vaya se produjo un error (function buscar.buscar) Error discribe" + ex.Message);
                return "";


            }
        }
    }
}
