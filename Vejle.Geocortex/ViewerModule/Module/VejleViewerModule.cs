using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using ESRI.ArcGIS.Client;
using Geocortex.EssentialsSilverlightViewer.Infrastructure.Configuration;
using Geocortex.EssentialsSilverlightViewer.Infrastructure.Commands;
using Geocortex.EssentialsSilverlightViewer.Infrastructure.Features;
using Geocortex.EssentialsSilverlightViewer.Infrastructure.Modularity;
using Geocortex.EssentialsSilverlightViewer.Infrastructure.Workflow;
using Geocortex.EssentialsSilverlightViewer.Infrastructure.Diagnostics;
using Geocortex.EssentialsSilverlightViewer.Infrastructure.Models;
using Geocortex.EssentialsSilverlightViewer.Infrastructure.Menus;
using Geocortex.Workflow.Client;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.ServiceLocation;
using Geocortex.EssentialsSilverlightViewer.CoreModules.MapTips;
using Geocortex.EssentialsSilverlightViewer.CoreModules.Results;
using Geocortex.EssentialsSilverlightViewer.CoreModules.Selection;
using Geocortex.EssentialsSilverlightViewer.CoreModules.Callouts;
using Geocortex.EssentialsSilverlightViewer.CoreModules.IWantToMenu;
using System.Linq;
using ESRI.ArcGIS.Client.Tasks;
using ESRI.ArcGIS.Client.Geometry;
using ESRI.ArcGIS.Client.Graphics;
using Geocortex.Essentials.Client;
using System.Threading;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Browser;


namespace Vejle.GeoCortex
{
    [ModuleExport(typeof(VejleViewerModule))]
    public class VejleViewerModule : ViewerModule
    {
        [Import]
        public ViewerWorkflowActivityDispatcher Dispatcher { get; set; }

        [Import]
        public Site Site { get; set; }

        private FeatureDetailsDialog _featureDetails;
        private ResultsViewModel _resultsViewModel;
        private SelectionViewModel _SelectionViewModel;
        private MenuViewModel _MenuViewModel;
       



        public FeatureDetailsDialog featureDetails
        {
            get
            {
                if (_featureDetails == null)
                {
                    IServiceLocator locator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current;

                    if (locator != null)
                    {
                        _featureDetails = locator.GetInstance<FeatureDetailsDialog>();
                    }
                }

                return _featureDetails;
            }
        }

        public ResultsViewModel resultsViewModel
        {
            get
            {
                if (_resultsViewModel == null)
                {
                    IServiceLocator locator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current;

                    if (locator != null)
                    {
                        _resultsViewModel = locator.GetInstance<ResultsViewModel>();
                    }
                }

                return _resultsViewModel;
            }
        }

        public SelectionViewModel selectionViewModel
        {
            get
            {
                if (_SelectionViewModel == null)
                {
                    IServiceLocator locator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current;

                    if (locator != null)
                    {
                        _SelectionViewModel = locator.GetInstance<SelectionViewModel>();
                    }
                }

                return _SelectionViewModel;
            }
        }

        public MenuViewModel menuviewmodel
        {
            get
            {
                if (_MenuViewModel == null)
                {
                    IServiceLocator locator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current;

                    if (locator != null)
                    {
                        _MenuViewModel = locator.GetInstance<MenuViewModel>();
                    }
                }

                return _MenuViewModel;
            }
        }


            
        protected override void Initialize(ModuleConfiguration moduleConfiguration)
        {
            base.Initialize(moduleConfiguration);
            Dispatcher.RegisterExternalIdHandler("GetCurrentFeature", new Action<ActivityContext>(HandleGetCurrentFeatureDetails));
            Dispatcher.RegisterExternalIdHandler("GetResults", new Action<ActivityContext>(HandleGetCurrentResults));
            Dispatcher.RegisterExternalIdHandler("GetSelections", new Action<ActivityContext>(HandleGetSelections));
            Dispatcher.RegisterExternalIdHandler("SetActiveFeatureSetCollection", new Action<ActivityContext>(HandleSetActiveCollection));
            Dispatcher.RegisterExternalIdHandler("ListVisibleLayers", new Action<ActivityContext>(HandleListVisibleLayers));
            Dispatcher.RegisterExternalIdHandler("ListLayerThemes", new Action<ActivityContext>(HandleListLayerThemes));
            Dispatcher.RegisterExternalIdHandler("RemoveMenuItem", new Action<ActivityContext>(HandleRemoveMenuItem));
            Dispatcher.RegisterExternalIdHandler("GetSiteInfo", new Action<ActivityContext>(HandleGetSiteInfo));

        }


        public void HandleGetCurrentFeatureDetails(ActivityContext activityContext)
        {
            string feature = "";
            string titel = "";
            

            if (activityContext == null)
            {
                throw new ArgumentNullException("activityContext");
            }

            if (featureDetails != null)
            {
                if (featureDetails.Feature != null)
                {
                    titel = featureDetails.Title.ToString();
                    feature = new ESRI.ArcGIS.Client.Tasks.FeatureSet(new List<Graphic>() { featureDetails.Feature.EsriFeature }).ToJson();
                }
            }

            activityContext.SetValue("CurrentFeature", feature);
            activityContext.SetValue("Titel", titel);

            activityContext.CompleteActivity();
        }


        public void HandleGetCurrentResults(ActivityContext activityContext)

        {

            var resultDictionary = new Dictionary<string, string>();
            var curretResultDictionaary = new Dictionary<string, string>();

            if (activityContext == null)
            {
                throw new ArgumentNullException("activityContext");
            }

            string layerName = (string)activityContext.GetValue("LayerName");

            if (resultsViewModel != null)
            {
                if (!String.IsNullOrWhiteSpace(layerName))
                {
                    if (resultsViewModel.AllSearchResults != null)
                    {
                        
                        curretResultDictionaary.Add(layerName,  resultsViewModel.AllSearchResults.First(r => r.DisplayName == layerName).EsriFeatureSet.ToJson());
                    }
                }

                else
                {
                    foreach (Geocortex.Essentials.Client.Tasks.FeatureSet feature in resultsViewModel.AllSearchResults)
                    {
                        resultDictionary.Add(feature.DisplayName.ToString(), feature.EsriFeatureSet.ToJson());
                    }

                    if (resultsViewModel.CurrentResults != null)
                    {
                        curretResultDictionaary.Add(resultsViewModel.CurrentResults.DisplayName.ToString(), resultsViewModel.CurrentResults.EsriFeatureSet.ToJson());
                    }
                }
            }

            activityContext.SetValue("CurrentResults", curretResultDictionaary);
            activityContext.SetValue("AllResults", resultDictionary);

            activityContext.CompleteActivity();
        }

        public void HandleGetSelections(ActivityContext activityContext)
        {
            var SelectDictionary = new Dictionary<string, string>();
            

            if (activityContext == null)
            {
                throw new ArgumentNullException("activityContext");
            }
            if (selectionViewModel != null)
            {
               
                foreach (FeatureSetViewModel FSModel in selectionViewModel.Selection.FeatureSets)
                {
                    SelectDictionary.Add(FSModel.DisplayName.ToString(), FSModel.FeatureSet.EsriFeatureSet.ToJson());
                }
                
            }

            activityContext.SetValue("CurrentSelections", SelectDictionary);

            activityContext.CompleteActivity();
        }


        public void HandleSetActiveCollection(ActivityContext activityContext)
        {
            if (activityContext == null)
            {
                throw new ArgumentNullException("context");
            }

            var fsDictionary = (Dictionary<string,string>)activityContext.GetValue("FeatureSetDictionary");
            var fsCollection = new Geocortex.Essentials.Client.Tasks.FeatureSetCollection();

            foreach (var featureSetItem in fsDictionary)
            {
                var layer = Site.EssentialsMap.AllLayers.Where(l => l.Name == featureSetItem.Key && l.SubLayerIds.Count() == 0).FirstOrDefault();
                var gcxFeatureSet = Geocortex.Essentials.Client.Tasks.FeatureSet.CreateFeatureSet(FeatureSet.FromJson(featureSetItem.Value), layer);

                fsCollection.Add(gcxFeatureSet);
            }

            ResultsListCommands.ShowFeatureSetCollection.Execute(fsCollection);

 
            activityContext.CompleteActivity();
        }


        public void HandleListVisibleLayers(ActivityContext activityContext)
        {
            if (activityContext == null)
            {
                throw new ArgumentNullException("context");
            }
            string displayname = "empty";
            var visibleLayers = new Dictionary<string, string>();
   
            displayname = Site.EssentialsMap.LayerThemesInfo.ActiveThemeDisplayName;
            foreach (MapService mapservice in Site.EssentialsMap.MapServicesFilteredView)
                {
                    if (mapservice.IsVisible)
                    {
                        foreach (Geocortex.Essentials.Client.Layer layer in mapservice.LayersFilteredView)
                        {
                            Boolean ParentVisible = false;
                            if (string.IsNullOrEmpty(layer.ParentLayerId))
                            {
                                ParentVisible = true;
                            }
                            else
                            {
                                if (layer.ParentLayer.IsVisible)
                                {
                                    ParentVisible = true;
                                }
                            }
                            if (layer.IsVisible & ParentVisible & layer.SubLayerIds.Length == 0)
                            {
                                visibleLayers.Add(layer.DisplayName,layer.FullyQualifiedName);
                            }
                        }
                    }

                }
            activityContext.SetValue("LayerTheme", displayname);
            activityContext.SetValue("VisibleLayers", visibleLayers);
            activityContext.CompleteActivity();
        }

        public void HandleListLayerThemes(ActivityContext activityContext)
        {
            if (activityContext == null)
            {
                throw new ArgumentNullException("context");
            }
            var Themes = new Dictionary<string, string>();
            foreach (LayerTheme layertheme in Site.EssentialsMap.LayerThemesInfo.Themes)
            {
                string id = "";
                if (string.IsNullOrEmpty(layertheme.ID))
                {
                    id = "9999";
                }
                else
                {
                    id = layertheme.ID;
                }
                Themes.Add(id, layertheme.DisplayName);
            }

            activityContext.SetValue("ActiveLayerTheme", Site.EssentialsMap.LayerThemesInfo.ActiveThemeDisplayName);
            activityContext.SetValue("LayerThemes", Themes);
            activityContext.CompleteActivity();

        }
        
        public void HandleRemoveMenuItem(ActivityContext activityContext)
        {
            if (activityContext == null)
            {
                throw new ArgumentNullException("context");
            }
          
           string ItemName =  (string)activityContext.GetValue("ItemName");
            
           if (String.IsNullOrWhiteSpace(ItemName)) { throw new ArgumentNullException("ItemName can't be empty"); }
           if(menuviewmodel!=null){
          

               if (menuviewmodel.Menu.Items != null)

               {
                  // System.Windows.Browser.HtmlPage.Window.Alert(menuviewmodel.Menu.Items.Count.ToString());
                   foreach (MenuItem menuitem in menuviewmodel.Menu.Items)
                   {
                       System.Windows.Browser.HtmlPage.Window.Alert(menuitem.Text);
                       if (menuitem.Name == ItemName || menuitem.Text == ItemName || menuitem.Description == ItemName)
                       {
                           menuviewmodel.Menu.Items.Remove(menuitem);
                       }
                   }
               }
              // else
               //{
                 //  System.Windows.Browser.HtmlPage.Window.Alert("I Wantto menu is empty");
               //}
           }
         //  else
          // {
            //   System.Windows.Browser.HtmlPage.Window.Alert("I Wantto menu is not set");
           //}
           activityContext.CompleteActivity();

        }

        public void HandleGetSiteInfo(ActivityContext activityContext)
        {
            if (activityContext == null)
            {
                throw new ArgumentNullException("context");
            }
            activityContext.SetValue("GeometryServer", Site.GeometryServiceUri);
            activityContext.SetValue("Properties", Site.Properties);
            activityContext.SetValue("Name", Site.Name);
            activityContext.SetValue("Url", Site.Url);
            activityContext.CompleteActivity();
        }
        


        
    }
}
