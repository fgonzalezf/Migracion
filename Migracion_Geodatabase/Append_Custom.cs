using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geoprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using System.Windows.Forms;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.esriSystem;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Carto;
using System.IO;


namespace Migracion_Geodatabase
{
    public class Append_Custom
    {

        public string InsertFeaturesUsingCursor(string sfeatureClassEntrada, string sfeatureClassSalida, string Type, bool SDE)
        {
            try
            {
                IGPUtilities pGputility = new GPUtilitiesClass();
                string FeatEntrada = @sfeatureClassEntrada.Replace("_Anot", "");
                string FeatSalida = @sfeatureClassSalida.Replace("_Anot", "");
                IFeatureClass featureClassEntrada = pGputility.OpenFeatureClassFromString(FeatEntrada);
                IFeatureClass featureClassSalida = pGputility.OpenFeatureClassFromString(FeatSalida);
                IFeatureClass featureClassEntradaAnot = pGputility.OpenFeatureClassFromString(@sfeatureClassEntrada);
                IFeatureClass featureClassSalidaAnot = pGputility.OpenFeatureClassFromString(@sfeatureClassSalida);

                int countInFeat = featureClassEntrada.FeatureCount(null);
                int countOutFeat = featureClassSalida.FeatureCount(null);
                int countInAnot = featureClassEntradaAnot.FeatureCount(null);
                int countOutAnot = featureClassSalidaAnot.FeatureCount(null);


                Dictionary<int, int> ListOBID = new Dictionary<int, int>();
                using (ComReleaser comReleaser = new ComReleaser())
                {

                    //Listado de campos Features
                    IFields FieldsSalida = featureClassSalida.Fields;
                    IFields FieldsEntrada = featureClassEntrada.Fields;
                    List<string> camposIguales = new List<string>();
                    for (int i = 0; i < FieldsSalida.FieldCount; i++)
                    {
                        string NombreSalida = FieldsSalida.get_Field(i).Name;
                        for (int j = 0; j < FieldsEntrada.FieldCount; j++)
                        {
                            string NombreEntrada = FieldsEntrada.get_Field(j).Name;
                            if (NombreEntrada.ToUpper() != "SHAPE" && 
                                NombreEntrada.ToUpper() != "SHAPE_LENGTH" &&NombreEntrada.ToUpper() != "SHAPE.LENGTH"&&
                                NombreEntrada.ToUpper() != "SHAPE_AREA" && NombreEntrada.ToUpper() != "SHAPE.AREA" &&
                                NombreEntrada.ToUpper() != "OVERRIDE")
                            {
                                if (NombreSalida.ToUpper() == NombreEntrada.ToUpper())
                                {
                                    camposIguales.Add(NombreSalida);
                                }
                            }
                        }
                    }
                    //Listado de campos Anotaciones
                    IFields FieldsSalidaAnot = featureClassSalidaAnot.Fields;
                    IFields FieldsEntradaAnot = featureClassEntradaAnot.Fields;
                    List<string> camposIgualesAnot = new List<string>();
                    for (int i = 0; i < FieldsSalidaAnot.FieldCount; i++)
                    {
                        string NombreSalida = FieldsSalidaAnot.get_Field(i).Name;
                        for (int j = 0; j < FieldsEntradaAnot.FieldCount; j++)
                        {
                            string NombreEntrada = FieldsEntradaAnot.get_Field(j).Name;
                            if (NombreEntrada.ToUpper() != "SHAPE" &&
                                NombreEntrada.ToUpper() != "SHAPE_LENGTH" && NombreEntrada.ToUpper() != "SHAPE.LENGTH" &&
                                NombreEntrada.ToUpper() != "SHAPE_AREA" && NombreEntrada.ToUpper() != "SHAPE.AREA" && 
                                NombreEntrada.ToUpper() != "OVERRIDE")
                            {
                                if (NombreSalida.ToUpper() == NombreEntrada.ToUpper())
                                {
                                    camposIgualesAnot.Add(NombreSalida);
                                }
                            }
                        }
                    }



                    IDataset dataset = (IDataset)featureClassSalida;
                    IWorkspace workspace = dataset.Workspace;
                    //Cast for an IWorkspaceEdit
                    IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)workspace;

                    //Start an edit session and operation
                    if (SDE == false)
                    {
                        workspaceEdit.StartEditing(true);
                        workspaceEdit.StartEditOperation();
                    }
                    else
                    {
                        startEditing(featureClassSalida);
                    }


                    IFeatureCursor searchCursor = featureClassEntrada.Search(null, true);
                    IFeatureCursor insertCursor = featureClassSalida.Insert(true);


                    comReleaser.ManageLifetime(insertCursor);
                    comReleaser.ManageLifetime(searchCursor);

                    // All of the features to be created are classified as Primary Highways.
                    IFeature feature = null;
                    IFeature featureAnot = null;
                    int intObjectIdIn = -1;
                    while ((feature = searchCursor.NextFeature()) != null)
                    {
                        IFeatureBuffer featureBuffer = featureClassSalida.CreateFeatureBuffer();
                        // Set the feature buffer's shape and insert it.
                        foreach (string campo in camposIguales)
                        {

                            int index = FieldsEntrada.FindField(campo);
                            int indexSalida = FieldsSalida.FindField(campo);
                            IField pField = feature.Fields.get_Field(index);
                            try
                            {
                                if (campo.ToUpper() == "OBJECTID")
                                {
                                    intObjectIdIn = Convert.ToInt32(feature.get_Value(index));
                                }
                                if (index != -1 && pField.Editable)
                                {

                                    featureBuffer.set_Value(indexSalida, feature.get_Value(index));
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error insertando " + ex.Message + " " + campo + sfeatureClassEntrada.Replace("_Anot", ""));

                            }


                        }

                        featureBuffer.Shape = feature.ShapeCopy;
                        object X = insertCursor.InsertFeature(featureBuffer);

                        int ObjectIdOut = Convert.ToInt32(X);
                        ListOBID.Add(intObjectIdIn, ObjectIdOut);
                        comReleaser.ManageLifetime(featureBuffer);
                        Marshal.ReleaseComObject(feature);

                    }

                    // Flush the buffer to the geodatabase.
                    insertCursor.Flush();

                    //cargar Anotacion

                    if (SDE == false)
                    {
                        workspaceEdit.StopEditOperation();
                        workspaceEdit.StopEditing(true);
                    }
                    else
                    {
                        stopEditing(featureClassSalida);
                    }
                    

                    IFeatureCursor searchCursorAnot = featureClassEntradaAnot.Search(null, true);
                    // IFeatureCursor insertCursorAnot = featureClassSalidaAnot.Insert(true);


                    comReleaser.ManageLifetime(searchCursorAnot);
                    //comReleaser.ManageLifetime(insertCursorAnot);


                    if (SDE == false)
                    {
                        workspaceEdit.StartEditing(true);
                        workspaceEdit.StartEditOperation();
                    }
                    else
                    {
                        startEditing(featureClassSalidaAnot);
                    }

                    while ((featureAnot = searchCursorAnot.NextFeature()) != null)
                    {

                        // Set the feature buffer's shape and insert it.
                        IGeometry pGeometry = null;
                        IFeature featureSalidaAnot = featureClassSalidaAnot.CreateFeature();


                        foreach (string campo in camposIgualesAnot)
                        {

                            int index = FieldsEntradaAnot.FindField(campo);
                            int indexSalida = FieldsSalidaAnot.FindField(campo);
                            IField pField = featureAnot.Fields.get_Field(index);

                            try
                            {


                                if (index != -1 && campo.ToUpper() != "FEATUREID" && campo.ToUpper() != "OBJECTID" && pField.Editable)
                                {
                                    featureSalidaAnot.set_Value(indexSalida, featureAnot.get_Value(index));

                                }
                                else if (campo.ToUpper() == "FEATUREID")
                                {
                                    try
                                    {
                                        featureSalidaAnot.set_Value(indexSalida, ListOBID[Convert.ToInt32(featureAnot.get_Value(index))]);
                                    }
                                    catch
                                    {
                                        featureSalidaAnot.set_Value(indexSalida, null);
                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error insertando " + ex.Message + " " + campo + sfeatureClassEntrada);

                            }


                        }

                        IAnnotationFeature pAnnotateFeatureAnotIn = featureAnot as IAnnotationFeature;
                        IAnnotationFeature pAnnotateFeatureAnotOut = featureSalidaAnot as IAnnotationFeature;

                        pAnnotateFeatureAnotOut.Annotation = pAnnotateFeatureAnotIn.Annotation;
                        featureSalidaAnot = pAnnotateFeatureAnotOut as IFeature;


                        pGeometry = featureAnot.ShapeCopy;
                        featureSalidaAnot.Shape = pGeometry;
                        featureSalidaAnot.Store();

                        Marshal.ReleaseComObject(featureAnot);
                        Marshal.ReleaseComObject(featureSalidaAnot);


                    }

                    if (SDE == false)
                    {
                        workspaceEdit.StopEditOperation();
                        workspaceEdit.StopEditing(true);
                    }
                    else
                    {
                        stopEditing(featureClassSalidaAnot);
                    }


                }


                // resultados
                int countOutFeatOut = featureClassSalida.FeatureCount(null);
                int countOutAnotOut = featureClassSalidaAnot.FeatureCount(null);

                string[] separators = { "/" };
                string[] RutaCompletaInAnot = sfeatureClassEntrada.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                int indexIn = RutaCompletaInAnot.Length;

                string[] RutaCompletaOutAnot = sfeatureClassSalida.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                int indexOut = RutaCompletaInAnot.Length;

                string Resultado = RutaCompletaInAnot[indexIn - 1] + "," +
                    countInAnot + "," +
                    RutaCompletaOutAnot[indexOut - 1] + "," +
                    (countOutAnotOut - countOutAnot).ToString()+ "\n"+
                    RutaCompletaInAnot[indexIn - 1].Replace("_Anot","") + "," +
                    countInFeat + "," +
                    RutaCompletaOutAnot[indexOut - 1].Replace("_Anot", "") + "," +
                    (countOutFeatOut - countOutFeat).ToString()

                    ;


                return Resultado;
           }
                
            catch(Exception Ex)
            {
                MessageBox.Show("Error Cargando la capa: " + sfeatureClassEntrada + "..." + Ex.Message);
               return "";
           }
        }
  
        public void startEditing(IFeatureClass pFeatureClass)
        {
            IDataset dataset = (IDataset)pFeatureClass;
            IWorkspace workspace = dataset.Workspace;

            IMultiuserWorkspaceEdit muWorkspaceEdit = (IMultiuserWorkspaceEdit)workspace;
            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)workspace;
            muWorkspaceEdit.StartMultiuserEditing(esriMultiuserEditSessionMode.esriMESMVersioned);
            workspaceEdit.StartEditOperation();

        }

        public void stopEditing(IFeatureClass pFeatureClass)
        {
            IDataset dataset = (IDataset)pFeatureClass;
            IWorkspace workspace = dataset.Workspace;

            IMultiuserWorkspaceEdit muWorkspaceEdit = (IMultiuserWorkspaceEdit)workspace;
            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)workspace;
            muWorkspaceEdit.StartMultiuserEditing(esriMultiuserEditSessionMode.esriMESMVersioned);

            workspaceEdit.StopEditOperation();
            try
            {
                // Stop the edit session. The saveEdits parameter indicates the edit session
                // will be committed.
                workspaceEdit.StopEditing(true);
            }
            catch (COMException comExc)
            {
                if (comExc.ErrorCode == (int)fdoError.FDO_E_VERSION_REDEFINED)
                {
                    // Get the version name.
                    IVersion version = (IVersion)workspace;
                    String versionName = version.VersionName;

                    // Reconcile the version. Modify this code to reconcile and handle conflicts
                    // in a manner appropriate for the specific application.
                    IVersionEdit4 versionEdit4 = (IVersionEdit4)workspace;
                    versionEdit4.Reconcile4(versionName, true, false, true, true);

                    // Stop the edit session.
                    workspaceEdit.StopEditing(true);
                }
                else
                {
                    // A different error has occurred. Handle in an appropriate way for the application.
                    workspaceEdit.StopEditing(false);
                }

            }
        }
        

        public string AppendTest(string sfeatureClassEntrada, string sfeatureClassSalida)
        {
            IGPUtilities pGputility = new GPUtilitiesClass();
            IFeatureClass pCountFeatureIn;
            IFeatureClass pCountFeatureOutIn;
            IFeatureClass pCountFeatureOut;
            ITable pCountTableIn;
            ITable pCountTableOut;
            ITable pCountTableOutIn;
            String Resultado;
            int countOutIn=0;
            try{
                pCountFeatureOutIn = pGputility.OpenFeatureClassFromString(sfeatureClassSalida);
                countOutIn=pCountFeatureOutIn.FeatureCount(null);
            }
            catch{
                pCountTableOutIn = pGputility.OpenTableFromString(sfeatureClassSalida);
                countOutIn= pCountTableOutIn.RowCount(null);
            }
            
            try
            {
            IGeoProcessor2 gp = new GeoProcessorClass();
            IVariantArray parameters = new VarArrayClass();
            parameters.Add(@sfeatureClassEntrada);
            parameters.Add(@sfeatureClassSalida);
            parameters.Add("NO_TEST");
            gp.Execute("Append_Management", parameters, null);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error cargando la capa " + sfeatureClassEntrada);


            }

            try
            {
               pCountFeatureIn = pGputility.OpenFeatureClassFromString(sfeatureClassEntrada);
               pCountFeatureOut = pGputility.OpenFeatureClassFromString(sfeatureClassSalida);
               string[] separators = {"/"};
               string[] RutaCompletaIn = sfeatureClassEntrada.Split(System.IO.Path.PathSeparator);
               int indexIn = RutaCompletaIn.Length;

               string[] RutaCompletaOut = sfeatureClassSalida.Split(System.IO.Path.PathSeparator);
               int indexOut = RutaCompletaIn.Length;

               Resultado = RutaCompletaIn[indexIn - 1] + "," +
                   pCountFeatureIn.FeatureCount(null).ToString() + "," +
                   RutaCompletaOut[indexOut - 1] + "," +
                   (pCountFeatureOut.FeatureCount(null) - countOutIn).ToString();



            }
            catch
            {
                pCountTableIn = pGputility.OpenTableFromString(sfeatureClassEntrada);
                pCountTableOut = pGputility.OpenTableFromString(sfeatureClassSalida);

                string[] separators = { "/" };
                string[] RutaCompletaIn = sfeatureClassEntrada.Split(System.IO.Path.PathSeparator);
                int indexIn = RutaCompletaIn.Length;

                string[] RutaCompletaOut = sfeatureClassSalida.Split(System.IO.Path.PathSeparator);
                int indexOut = RutaCompletaIn.Length;

                Resultado = RutaCompletaIn[indexIn - 1] + "," +
                    pCountTableIn.RowCount(null).ToString() + "," +
                    RutaCompletaOut[indexOut - 1] + "," +
                    (pCountTableOut.RowCount(null) - countOutIn).ToString();

            }


            return Resultado;
        }

        public string AppendRaster(string sfeatureClassEntrada, string sfeatureClassSalida)
        {
            try
            {
                IGeoProcessor2 gp = new GeoProcessorClass();
                IVariantArray parameters = new VarArrayClass();
                parameters.Add(@sfeatureClassEntrada);
                parameters.Add(@sfeatureClassSalida);
                
                gp.Execute("Mosaic_Management", parameters, null);
                return "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando El Raster " + sfeatureClassEntrada + ex.Message);
                return "";

            }
        }
    }
}
