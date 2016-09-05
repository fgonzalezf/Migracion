﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geoprocessing;
using System.Windows.Forms;
using System.IO;

namespace Migracion_Geodatabase
{
    class RecorrerGDB
    {
        public RecorrerGDB()
        {
        }

        IWorkspaceFactory pSdeWorkspaceFactory;
        IWorkspace pSdeWorkspace;
        IEnumDatasetName pEnumDSName;
        IDatasetName pSdeDSName;
        IFeatureWorkspace pFeatureWorkspace;
        IFeatureDataset pfeaturedataset;
        IEnumDataset pEnumDataset;
        IDataset pDataset;
        IName pName;
        ITable ptable;
        IGPUtilities pGputility;
        IFeatureClass pFeatureClass;
        IFeatureClass pFeatureClassout;


        public List<string> RecorrerDatasets(string Geodatabase, string GeodatabaseSalida)
        {


            List<string> Ruta = new List<string>();
            pGputility = new GPUtilitiesClass();

            try
            {
                if (Geodatabase.Contains(".mdb"))
                {
                    pSdeWorkspaceFactory = new AccessWorkspaceFactoryClass();
                    pSdeWorkspace = pSdeWorkspaceFactory.OpenFromFile(Geodatabase, 0);

                }
                else if (Geodatabase.Contains(".gdb"))
                {
                    pSdeWorkspaceFactory = new FileGDBWorkspaceFactoryClass();
                    pSdeWorkspace = pSdeWorkspaceFactory.OpenFromFile(Geodatabase, 0);
                }
                else if (Geodatabase.Contains(".sde"))
                {
                    pSdeWorkspace = ArcSdeWorkspaceFromFile(GeodatabaseSalida);
                }
                pEnumDSName = pSdeWorkspace.get_DatasetNames(esriDatasetType.esriDTAny);
                pSdeDSName = pEnumDSName.Next();


                while (pSdeDSName != null)
                {
                    pFeatureWorkspace = pSdeWorkspace as IFeatureWorkspace;

                    if (pSdeDSName.Type == esriDatasetType.esriDTFeatureDataset)
                    {


                        if (pSdeDSName != null)
                        {
                            Ruta.Add(pSdeDSName.Name);

                        }





                    }
                    pSdeDSName = pEnumDSName.Next();
                }


                return Ruta;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error recorriendo Geodatabase: " + e.Message + pDataset.Name);
                return null;


            }


        }



        public List<List<string>> Recorrer(string Geodatabase, string GeodatabaseSalida, string DatasetUnico)
        {

            List<string> Ruta1 = new List<string>();
            List<string> Ruta2 = new List<string>();
            List<List<string>> Ruta = new List<List<string>>();
            pGputility = new GPUtilitiesClass();

            try
            {
                if (Geodatabase.Contains(".mdb"))
                {
                    pSdeWorkspaceFactory = new AccessWorkspaceFactoryClass();
                    pSdeWorkspace = pSdeWorkspaceFactory.OpenFromFile(Geodatabase, 0);

                }
                else if (Geodatabase.Contains(".gdb"))
                {
                    pSdeWorkspaceFactory = new FileGDBWorkspaceFactoryClass();
                    pSdeWorkspace = pSdeWorkspaceFactory.OpenFromFile(Geodatabase, 0);
                }
                else if (Geodatabase.Contains(".sde"))
                {
                    pSdeWorkspace = ArcSdeWorkspaceFromFile(GeodatabaseSalida);
                }
                pEnumDSName = pSdeWorkspace.get_DatasetNames(esriDatasetType.esriDTAny);
                pSdeDSName = pEnumDSName.Next();


                while (pSdeDSName != null)
                {
                    pFeatureWorkspace = pSdeWorkspace as IFeatureWorkspace;

                    if (pSdeDSName.Type == esriDatasetType.esriDTFeatureClass)
                    {
                        if (DatasetUnico == "Todos")
                        {
                            pFeatureClass = pFeatureWorkspace.OpenFeatureClass(pSdeDSName.Name);
                            string nameFc = pSdeDSName.Name;
                            if (pFeatureClass.FeatureCount(null) > 0)
                            {
                                if (pFeatureClassout != null)
                                {
                                    Ruta1.Add(nameFc);
                                    Ruta2.Add(nameFc);

                                }
                                else
                                {
                                    Ruta1.Add(nameFc);
                                    Ruta2.Add("...");
                                }
                            }
                        }
                        pSdeDSName = pEnumDSName.Next();


                    }
                    else if (pSdeDSName.Type == esriDatasetType.esriDTFeatureDataset && DatasetUnico == "Todos")
                    {
                        pfeaturedataset = pFeatureWorkspace.OpenFeatureDataset(pSdeDSName.Name);

                        pEnumDataset = pfeaturedataset.Subsets;
                        pDataset = pEnumDataset.Next();
                        while (pDataset != null)
                        {
                            if (pDataset.Type == esriDatasetType.esriDTFeatureClass)
                            {

                                pName = pDataset.FullName;
                                ptable = pName.Open() as ITable;
                                if (ptable.RowCount(null) > 0)
                                {

                                    string InFc = @Geodatabase + Path.DirectorySeparatorChar + pSdeDSName.Name + Path.DirectorySeparatorChar + pDataset.Name;
                                    string OutFc = @GeodatabaseSalida + Path.DirectorySeparatorChar + pSdeDSName.Name + Path.DirectorySeparatorChar + pDataset.Name;
                                    string nameFc = pSdeDSName.Name + Path.DirectorySeparatorChar + pDataset.Name;
                                    try
                                    {
                                        pFeatureClass = pGputility.OpenFeatureClassFromString(OutFc);
                                    }
                                    catch (Exception ex)
                                    {
                                        pFeatureClass = null;
                                    }
                                    if (pFeatureClass != null)
                                    {
                                        Ruta1.Add(nameFc);
                                        Ruta2.Add(nameFc);
                                    }
                                    else
                                    {
                                        Ruta1.Add(nameFc);
                                        Ruta2.Add("...");
                                    }

                                }

                            }

                            pDataset = pEnumDataset.Next();
                        }
                        pSdeDSName = pEnumDSName.Next();
                    }
                    else if (pSdeDSName.Type == esriDatasetType.esriDTFeatureDataset && DatasetUnico != "Todos")
                    {
                        if (pSdeDSName.Name == DatasetUnico)
                        {
                            pfeaturedataset = pFeatureWorkspace.OpenFeatureDataset(pSdeDSName.Name);

                            pEnumDataset = pfeaturedataset.Subsets;
                            pDataset = pEnumDataset.Next();
                            while (pDataset != null)
                            {
                                if (pDataset.Type == esriDatasetType.esriDTFeatureClass)
                                {

                                    pName = pDataset.FullName;
                                    ptable = pName.Open() as ITable;
                                    if (ptable.RowCount(null) > 0)
                                    {

                                        string InFc = @Geodatabase + Path.DirectorySeparatorChar + pSdeDSName.Name + Path.DirectorySeparatorChar + pDataset.Name;
                                        string OutFc = @GeodatabaseSalida + Path.DirectorySeparatorChar + pSdeDSName.Name + Path.DirectorySeparatorChar + pDataset.Name;
                                        string nameFc = pSdeDSName.Name + Path.DirectorySeparatorChar + pDataset.Name;
                                        try
                                        {
                                            pFeatureClass = pGputility.OpenFeatureClassFromString(OutFc);
                                        }
                                        catch (Exception ex)
                                        {
                                            pFeatureClass = null;
                                        }
                                        if (pFeatureClass != null)
                                        {
                                            Ruta1.Add(nameFc);
                                            Ruta2.Add(nameFc);
                                        }
                                        else
                                        {
                                            Ruta1.Add(nameFc);
                                            Ruta2.Add("...");
                                        }

                                    }

                                }

                                pDataset = pEnumDataset.Next();
                            }

                        }
                        pSdeDSName = pEnumDSName.Next();
                    }
                }


                Ruta.Add(Ruta1);
                Ruta.Add(Ruta2);

                return Ruta;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error recorriendo Geodatabase: " + e.Message + pDataset.Name);
                return null;


            }


        }

        public static IWorkspace ArcSdeWorkspaceFromFile(String connectionFile)
        {
            Type factoryType = Type.GetTypeFromProgID(
                "esriDataSourcesGDB.SdeWorkspaceFactory");
            IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance
                (factoryType);
            return workspaceFactory.OpenFromFile(connectionFile, 0);
        }

        public List<string> RecorrerSalida(string GeodatabaseSalida, string DatasetUnico)
        {

            List<string> Ruta = new List<string>();


            if (GeodatabaseSalida.Contains(".mdb"))
            {
                pSdeWorkspaceFactory = new AccessWorkspaceFactoryClass();
                pSdeWorkspace = pSdeWorkspaceFactory.OpenFromFile(GeodatabaseSalida, 0);

            }
            else if (GeodatabaseSalida.Contains(".gdb"))
            {
                pSdeWorkspaceFactory = new FileGDBWorkspaceFactoryClass();
                pSdeWorkspace = pSdeWorkspaceFactory.OpenFromFile(GeodatabaseSalida, 0);
            }
            else if (GeodatabaseSalida.Contains(".sde"))
            {
                pSdeWorkspace = ArcSdeWorkspaceFromFile(GeodatabaseSalida);
            }



            pEnumDSName = pSdeWorkspace.get_DatasetNames(esriDatasetType.esriDTAny);
            pSdeDSName = pEnumDSName.Next();
            while (pSdeDSName != null)
            {
                try
                {

                    pFeatureWorkspace = pSdeWorkspace as IFeatureWorkspace;
                    if (pSdeDSName.Type == esriDatasetType.esriDTFeatureClass && DatasetUnico == "Todos")
                    {

                        string nameFc = pSdeDSName.Name;
                        Ruta.Add(nameFc);
                        pSdeDSName = pEnumDSName.Next();

                    }

                    else if (pSdeDSName.Type == esriDatasetType.esriDTFeatureDataset && DatasetUnico == "Todos")
                    {



                        pFeatureWorkspace = pSdeWorkspace as IFeatureWorkspace;
                        pfeaturedataset = pFeatureWorkspace.OpenFeatureDataset(pSdeDSName.Name);
                        pEnumDataset = pfeaturedataset.Subsets;
                        pDataset = pEnumDataset.Next();
                        while (pDataset != null)
                        {
                            if (pDataset.Type == esriDatasetType.esriDTFeatureClass && pDataset.Name != "MBAG.F02AVIA_VIA_500K_A")
                            {


                                string OutFc = GeodatabaseSalida + Path.DirectorySeparatorChar + pSdeDSName.Name + Path.DirectorySeparatorChar + pDataset.Name;
                                string nameFc = pSdeDSName.Name + Path.DirectorySeparatorChar + pDataset.Name;
                                Ruta.Add(nameFc);


                            }
                            pDataset = pEnumDataset.Next();
                        }



                    }
                    else if (pSdeDSName.Type == esriDatasetType.esriDTFeatureDataset && DatasetUnico != "Todos")
                    {



                        pFeatureWorkspace = pSdeWorkspace as IFeatureWorkspace;
                        pfeaturedataset = pFeatureWorkspace.OpenFeatureDataset(pSdeDSName.Name);
                        pEnumDataset = pfeaturedataset.Subsets;
                        pDataset = pEnumDataset.Next();
                        while (pDataset != null)
                        {
                            if (pDataset.Type == esriDatasetType.esriDTFeatureClass && pDataset.Name == DatasetUnico)
                            {


                                string OutFc = GeodatabaseSalida + Path.DirectorySeparatorChar + pSdeDSName.Name + Path.DirectorySeparatorChar + pDataset.Name;
                                string nameFc = pSdeDSName.Name + Path.DirectorySeparatorChar + pDataset.Name;
                                Ruta.Add(nameFc);


                            }
                            pDataset = pEnumDataset.Next();
                        }



                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Error recorriendo Geodatabase de Salida: " + ex.Message);
                }
                pSdeDSName = pEnumDSName.Next();
            }


            return Ruta;
        }
    }
}