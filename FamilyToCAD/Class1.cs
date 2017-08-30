using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

[TransactionAttribute(TransactionMode.Manual)]
[RegenerationAttribute(RegenerationOption.Manual)]
public class FamilyToCAD1 : IExternalCommand
{

    public Result Execute(
        ExternalCommandData commandData,
        ref string message,
        ElementSet elements)
    {
        
        UIApplication uiApp = commandData.Application;

        FamilyToCAD.Form2 form = new FamilyToCAD.Form2();
        form.ShowDialog();

        ///Sets up a document to place family into
        Autodesk.Revit.DB.Document uiDoc = null;

        ///determines if user wants to use the current project, or a new project
        ///expecially useful for bring multiple families into the same project
        if (form.ProjectLocation == "New Project")
        {

            Autodesk.Revit.ApplicationServices.Application app = uiApp.Application;
            uiDoc = app.NewProjectDocument(form.TemplateFile);

            if (!Directory.Exists(@"C:\temp\"))
            {
                Directory.CreateDirectory(@"C:\temp\");
            }

            string projname = @"C:\temp\new_project.rvt";
            try
            {
                if (File.Exists(projname))
                {
                    File.Delete(projname);
                }
            }
            catch
            {
                int count = 1;
                while (File.Exists(@"C\temp\newproject" + count + ".rvt") == true)
                {
                    count += 1;
                }
                projname = @"C\temp\newproject" + count + ".rvt";

            }

            SaveAsOptions options1 = new SaveAsOptions();
            options1.OverwriteExistingFile = true;
            uiDoc.SaveAs(projname, options1);

            uiApp.OpenAndActivateDocument(projname);
        }
        else if (form.ProjectLocation == "<Current Project>")
        {
            if (uiApp.ActiveUIDocument != null)
            {
                uiDoc = uiApp.ActiveUIDocument.Document;
            }
            else
            {
                MessageBox.Show("No Currently Loaded Project. Please Rerun addin and select a \"New Project\"");
                return Result.Failed;
            }
        }


        //fails out if the document is not opened properly
        if (uiDoc == null)
        {
            return Result.Failed;
        }

        string filedir = form.FamilyFile.Substring(0,form.FamilyFile.LastIndexOf("\\"));

        if (File.Exists(form.FamilyFile))
        {
            using (Transaction tx = new Transaction(uiDoc))
            {
                tx.Start("Load Family");
                Autodesk.Revit.DB.Family family = null;
                uiDoc.LoadFamily(form.FamilyFile, out family);
                tx.Commit();

                Double Y = 0;
                Double X = 0;

                var collector = new FilteredElementCollector(uiDoc);

                var viewFamilyType = collector
                    .OfClass(typeof(ViewFamilyType))
                    .OfType<ViewFamilyType>()
                    .FirstOrDefault(x => x.ViewFamily == ViewFamily.ThreeDimensional);

                // Export the active view
                ICollection<ElementId> views = new List<ElementId>();

                tx.Start("ChangeView");
                View3D view = View3D.CreateIsometric(uiDoc, viewFamilyType.Id);
                view.SaveOrientationAndLock();
                view.DisplayStyle = DisplayStyle.Shading;
                tx.Commit();


                // Places another family next to original if "Current Document" was selected
                FilteredElementCollector faminstances = new FilteredElementCollector(uiDoc).OfClass(typeof(FamilyInstance));
                FilteredElementCollector txtinstances = new FilteredElementCollector(uiDoc).OfClass(typeof(TextNote));


                ///Finds the max X coordinate of any text currently on the page. Setup of family instances is as such:
                ///fam1_inst1;txt1;fam2_inst1;txt4
                ///fam1_inst2;txt2;fam2_inst2;txt5
                ///fam1_inst3;txt3
                ///
                if (txtinstances.Count() != 0)
                {
                    Boolean first = true;
                    foreach (TextNote txtnote in txtinstances)
                    {
                        if (first == true)
                        {
                            X = txtnote.Coord.X + txtnote.Width;
                        }
                        else
                        {
                            if (txtnote.Coord.X + txtnote.Width > X)
                            {
                                X = txtnote.Coord.X + txtnote.Width;
                            }
                        }
                    }
                }

                //loops through each family instance performing the appropriate instantiation action
                foreach (FamilySymbol famsymbol in family.Symbols)
                {
                    //FamilySymbol famsymbol = family.Document.GetElement(id) as FamilySymbol;
                    XYZ origin = new XYZ(X, Y, 0);
                    tx.Start("Load Family Member");
                    famsymbol.Activate();
                    FamilyInstance instance = uiDoc.Create.NewFamilyInstance(origin, famsymbol, StructuralType.NonStructural);
                    tx.Commit();

                    BoundingBoxXYZ BB = famsymbol.get_BoundingBox(view);

                    XYZ BBMin = BB.Min;

                    Y = Y + BBMin.Y;

                    ///exports currently loaded family member if "Single" was selected within the userform
                    if (form.FileExportSetup.Contains("Multiple"))
                    {

                        tx.Start("ChangeOverallView");
                        View3D overallview = View3D.CreateIsometric(uiDoc, viewFamilyType.Id);
                        overallview.DisplayStyle = DisplayStyle.Shading;
                        views.Add(overallview.Id);
                        tx.Commit();

                        if (form.ExportFileType.StartsWith("DWG"))
                        {
                            ///ACAD export options
                            DWGExportOptions options = new DWGExportOptions();
                            options.ExportOfSolids = SolidGeometry.ACIS;
                            options.ACAPreference = ACAObjectPreference.Object;
                            options.HideUnreferenceViewTags = true;

                            ///exports to chosen ACAD version
                            if (form.ExportFileType.Contains("2000"))
                            {
                                options.FileVersion = ACADVersion.R2000;
                            }
                            else if (form.ExportFileType.Contains("2004"))
                            {
                                options.FileVersion = ACADVersion.R2004;
                            }
                            else if (form.ExportFileType.Contains("2007"))
                            {
                                options.FileVersion = ACADVersion.R2007;
                            }
                            else if (form.ExportFileType.Contains("2010"))
                            {
                                options.FileVersion = ACADVersion.R2010;
                            }
                            else if (form.ExportFileType.Contains("2013"))
                            {
                                options.FileVersion = ACADVersion.R2013;
                            }

                            string dwgfilename = famsymbol.Name + ".dwg";
                            string dwgfullfilename = filedir + dwgfilename;
                            if (File.Exists(dwgfullfilename))
                            {
                                File.Delete(dwgfullfilename);
                            }
                            uiDoc.Export(@filedir, @dwgfilename, views, options);

                        }
                        else if (form.ExportFileType.Contains("SAT"))
                        {
                            SATExportOptions options = new SATExportOptions();
                            
                            string SATfilename = famsymbol.Name + ".sat";
                            string satfullfilename = filedir + SATfilename;
                            if (File.Exists(satfullfilename))
                            {
                                File.Delete(satfullfilename);
                            }
                            uiDoc.Export(@filedir, SATfilename, views, options);
                        }

                        tx.Start("Delete Family Member");
                        uiDoc.Delete(instance.Id);
                        tx.Commit();

                        views.Clear();
                    }
                    else if (form.FileExportSetup.Contains("Single"))
                    {
                        ///Creates Text Note next to the family instance
                        string notetext = famsymbol.Name;

                        XYZ textplacement = new XYZ(X + BB.Max.X, Y-.5*BBMin.Y, BB.Min.Z);
                        
                        tx.Start("Place Annotation");
                        TextNote txnote = uiDoc.Create.NewTextNote(uiDoc.ActiveView,textplacement, XYZ.BasisX, XYZ.BasisY,.06, TextAlignFlags.TEF_ALIGN_LEFT | TextAlignFlags.TEF_ALIGN_BOTTOM, notetext);
                        TextElement text = uiDoc.GetElement(txnote.Id) as TextElement;
                        TextElementType textType = text.Symbol;
                        Parameter textSize = textType.get_Parameter("Text Size");
                        Double oldsize = textSize.AsDouble();
                        double newSize = BB.Max.Y;
                        textSize.Set(newSize/50);

                        TextNoteType tt
                            = new FilteredElementCollector(uiDoc)
                              .OfClass(typeof(TextNoteType))
                              .FirstElement() as TextNoteType;

                        String ttname = tt.Name;

                        Font font = new Font("Arial",17, FontStyle.Regular);
                        Size txtbox = TextRenderer.MeasureText(notetext, font);
                        Double newWidth = ((double)txtbox.Width/6);
                        txnote.Width = newWidth;

                        //Leader lead =  txnote.AddLeader(TextNoteLeaderTypes.TNLT_STRAIGHT_L);
                        tx.Commit();
                    }

                }

                if (form.FileExportSetup.Contains("Single"))
                {
                    tx.Start("ChangeOverallView");
                    View3D oview = View3D.CreateIsometric(uiDoc, viewFamilyType.Id);
                    oview.DisplayStyle = DisplayStyle.Shading;
                    views.Add(oview.Id);
                    tx.Commit();

                    if (form.ExportFileType.StartsWith("DWG"))
                    {

                        DWGExportOptions options = new DWGExportOptions();
                        options.ExportOfSolids = SolidGeometry.ACIS;
                        options.ACAPreference = ACAObjectPreference.Object;
                        options.HideUnreferenceViewTags = true;

                        ///Exports to chosen ACAD version
                        if (form.ExportFileType.Contains("2000"))
                        {
                            options.FileVersion = ACADVersion.R2000;
                        }
                        else if (form.ExportFileType.Contains("2004"))
                        {
                            options.FileVersion = ACADVersion.R2004;
                        }
                        else if (form.ExportFileType.Contains("2007"))
                        {
                            options.FileVersion = ACADVersion.R2007;
                        }
                        else if (form.ExportFileType.Contains("2010"))
                        {
                            options.FileVersion = ACADVersion.R2010;
                        }
                        else if (form.ExportFileType.Contains("2013"))
                        {
                            options.FileVersion = ACADVersion.R2013;
                        }

                        string dwgfilename = family.Name + ".dwg";
                        string dwgfullfilename = filedir + dwgfilename;
                        if (File.Exists(dwgfullfilename))
                        {
                            File.Delete(dwgfullfilename);
                        }
                        uiDoc.Export(@filedir, @dwgfilename, views, options);

                    }
                    else if (form.ExportFileType.Contains("SAT"))
                    {
                        SATExportOptions options = new SATExportOptions();

                        string SATfilename = family.Name + ".sat";
                        string satfullfilename = filedir + SATfilename;
                        if (File.Exists(satfullfilename))
                        {
                            File.Delete(satfullfilename);
                        }
                        uiDoc.Export(@filedir, SATfilename, views, options);
                    }

                }

            }
        }

        return Result.Succeeded;
    }
}