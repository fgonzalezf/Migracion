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


namespace Migracion_Geodatabase
{
    public class Append_Custom
    {

        public void InsertFeaturesUsingCursor(string sfeatureClassEntrada, string sfeatureClassSalida, string Type)
        {
                IGPUtilities pGputility = new GPUtilitiesClass();
                string FeatEntrada=@sfeatureClassEntrada.Replace("_Anot", "");
                string FeatSalida = @sfeatureClassSalida.Replace("_Anot", "");
                IFeatureClass featureClassEntrada = pGputility.OpenFeatureClassFromString(FeatEntrada);
                IFeatureClass featureClassSalida = pGputility.OpenFeatureClassFromString(FeatSalida);               
                IFeatureClass featureClassEntradaAnot = pGputility.OpenFeatureClassFromString(@sfeatureClassEntrada);
                IFeatureClass featureClassSalidaAnot = pGputility.OpenFeatureClassFromString(@sfeatureClassSalida);


                Dictionary<int, int> ListOBID =new Dictionary<int, int>();
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
                            if (NombreEntrada.ToUpper() != "SHAPE" && NombreEntrada.ToUpper() != "SHAPE_LENGTH" && NombreEntrada.ToUpper() != "SHAPE_AREA" && NombreEntrada.ToUpper() != "OVERRIDE")
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
                            if (NombreEntrada.ToUpper() != "SHAPE" && NombreEntrada.ToUpper() != "SHAPE_LENGTH" && NombreEntrada.ToUpper() != "SHAPE_AREA" && NombreEntrada.ToUpper() != "OVERRIDE")
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
                    workspaceEdit.StartEditing(true);
                    workspaceEdit.StartEditOperation();
                    

                    IFeatureCursor searchCursor = featureClassEntrada.Search(null, true);
                    IFeatureCursor insertCursor = featureClassSalida.Insert(true);

                    
                    comReleaser.ManageLifetime(insertCursor);
                    comReleaser.ManageLifetime(searchCursor);
  
                    // All of the features to be created are classified as Primary Highways.
                    IFeature feature = null;
                    IFeature featureAnot = null;
                    int intObjectIdIn=-1;
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
                                MessageBox.Show("Error insertando " + ex.Message + " " + campo + sfeatureClassEntrada.Replace("_Anot",""));

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
                    workspaceEdit.StopEditOperation();
                    workspaceEdit.StopEditing(true);

                    IFeatureCursor searchCursorAnot = featureClassEntradaAnot.Search(null, true);
                   // IFeatureCursor insertCursorAnot = featureClassSalidaAnot.Insert(true);

                    
                    comReleaser.ManageLifetime(searchCursorAnot);
                    //comReleaser.ManageLifetime(insertCursorAnot);
                    

                    workspaceEdit.StartEditing(true);
                    workspaceEdit.StartEditOperation();

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


                                if (index != -1 && campo.ToUpper() != "FEATUREID" && campo.ToUpper() != "OBJECTID")
                                {
                                    featureSalidaAnot.set_Value(indexSalida, featureAnot.get_Value(index));
                                    
                                }
                                else if (campo.ToUpper()=="FEATUREID")
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
                  
                    workspaceEdit.StopEditOperation();
                    workspaceEdit.StopEditing(true);

                    
                }
            }
            
    
        

        public void AppendTest(string sfeatureClassEntrada, string sfeatureClassSalida)
        {
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
        }

        public void AppendRaster(string sfeatureClassEntrada, string sfeatureClassSalida)
        {
            try
            {
                IGeoProcessor2 gp = new GeoProcessorClass();
                IVariantArray parameters = new VarArrayClass();
                parameters.Add(@sfeatureClassEntrada);
                parameters.Add(@sfeatureClassSalida);
                
                gp.Execute("Mosaic_Management", parameters, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando El Raster " + sfeatureClassEntrada);


            }
        }
    }
}
