using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.Creation;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.ApplicationServices;
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

        if (form.ProjectLocation == "New Project")
        {

            try
            {
                uiApp.ActiveUIDocument.SaveAndClose();
            }
            catch
            {

            }

            try
            {
                uiDoc = uiApp.Application.NewProjectDocument(form.TemplateFile);
            }
            catch
            {
                return Result.Failed;
            }

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

                Double low = 0;

                var collector = new FilteredElementCollector(uiDoc);

                var viewFamilyType = collector
                    .OfClass(typeof(ViewFamilyType))
                    .OfType<ViewFamilyType>()
                    .FirstOrDefault(x => x.ViewFamily == ViewFamily.ThreeDimensional);

                // Export the active view
                ICollection<ElementId> views = new List<ElementId>();

                tx.Start("ChangeView");
                View3D view = View3D.CreateIsometric(uiDoc, viewFamilyType.Id);
                view.DisplayStyle = DisplayStyle.Shading;
                tx.Commit();

                foreach (ElementId id in family.GetFamilySymbolIds())
                {
                    FamilySymbol famsymbol = family.Document.GetElement(id) as FamilySymbol;
                    XYZ origin = new XYZ(0, low, 0);
                    tx.Start("Load Family Member");
                    famsymbol.Activate();
                    FamilyInstance instance = uiDoc.Create.NewFamilyInstance(origin, famsymbol, StructuralType.NonStructural);
                    tx.Commit();

                    BoundingBoxXYZ BB = famsymbol.get_BoundingBox(view);

                    XYZ BBMin = BB.Min;

                    //XYZ trans = new XYZ(origin.X + BB.Min.X, origin.Y + BB.Min.Y, 0);
                    //tx.Start("Element Move");
                    //instance.Location.Move(trans);
                    //tx.Commit();

                    low = low + BBMin.Y;

                    ///exports currently loaded family member if "Single" was selected within the userform
                    if (form.FileExportSetup == "Multiple")
                    {

                        tx.Start("ChangeOverallView");
                        View3D overallview = View3D.CreateIsometric(uiDoc, viewFamilyType.Id);
                        overallview.DisplayStyle = DisplayStyle.Shading;
                        views.Add(overallview.Id);
                        tx.Commit();

                        if (form.ExportFileType.StartsWith("DWG"))
                        {

                            DWGExportOptions options = new DWGExportOptions();
                            options.ExportOfSolids = SolidGeometry.ACIS;
                            options.ACAPreference = ACAObjectPreference.Object;
                            options.HideUnreferenceViewTags = true;

                            if (form.ExportFileType.Contains("2007"))
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

                }

                if (form.FileExportSetup == "Single")
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

                        if (form.ExportFileType.Contains("2007"))
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