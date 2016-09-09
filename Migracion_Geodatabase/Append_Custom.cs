using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geoprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using System.Windows.Forms;

namespace Migracion_Geodatabase
{
    public class Append_Custom
    {

        public void InsertFeaturesUsingCursor(string sfeatureClassEntrada, string sfeatureClassSalida)
        {
            IGPUtilities pGputility = new GPUtilitiesClass(); ;

             IFeatureClass featureClassEntrada = pGputility.OpenFeatureClassFromString(sfeatureClassEntrada);
             IFeatureClass featureClassSalida = pGputility.OpenFeatureClassFromString(sfeatureClassSalida);

            using (ComReleaser comReleaser = new ComReleaser())
            {

                //Listado de campos
                IFields FieldsSalida = featureClassSalida.Fields;
                IFields FieldsEntrada = featureClassEntrada.Fields;
                List<string> camposIguales = new List<string>();
                for (int i = 0; i < FieldsSalida.FieldCount; i++)
                {
                    string NombreSalida = FieldsSalida.get_Field(i).Name;
                    for (int j = 0; j < FieldsEntrada.FieldCount; j++)
                    {
                        string NombreEntrada = FieldsEntrada.get_Field(j).Name;
                        if (NombreEntrada.ToUpper() != "OBJECTID" && NombreEntrada.ToUpper() != "SHAPE" && NombreEntrada.ToUpper() != "SHAPE_LENGTH" && NombreEntrada.ToUpper() != "SHAPE_AREA")
                        { 
                            if(NombreSalida.ToUpper()==NombreEntrada.ToUpper())
                            {
                                camposIguales.Add(NombreSalida);
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
                // Create a feature buffer.
                //IFeatureBuffer featureBuffer = featureClass.CreateFeatureBuffer();
                IFeatureBuffer featureBuffer = featureClassSalida.CreateFeatureBuffer();
                comReleaser.ManageLifetime(featureBuffer);

                // Create an insert cursor.

                IFeatureCursor searchCursor = featureClassEntrada.Search(null, true);
                IFeatureCursor insertCursor = featureClassSalida.Insert(true);
                comReleaser.ManageLifetime(insertCursor);
                

                // All of the features to be created are classified as Primary Highways.
                IFeature feature = null;
                while ((feature = searchCursor.NextFeature()) != null)
                {
                    // Set the feature buffer's shape and insert it.
                    foreach (string campo in camposIguales)
                    {
                        //MessageBox.Show(campo);
                        
                        int index = FieldsEntrada.FindField(campo);
                        int indexSalida = FieldsEntrada.FindField(campo);
                        try{
                            if (index != -1)
                            {
                                
                                featureBuffer.set_Value(indexSalida, feature.get_Value(index));
                            }
                            }
                        catch(Exception  ex){
                            MessageBox.Show("Error insertando " + ex.Message + " " + campo);

                        }
                                            

                    }
                    
                   featureBuffer.Shape = feature.ShapeCopy;                  
                   insertCursor.InsertFeature(featureBuffer);
                }

                // Flush the buffer to the geodatabase.
                insertCursor.Flush();
                workspaceEdit.StopEditOperation();
                workspaceEdit.StopEditing(true);
            }
        }
    }
}
