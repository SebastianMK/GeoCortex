using System.Activities;
using System.ComponentModel;
using Geocortex.Workflow.Activities;
using System.Collections.Generic;
using System;




namespace Vejle.GeoCortex
{
    [Description("Get the currently selected item in the feature details, as a FeatureSet serialized to JSON. Set the ExternalID to GetCurrentFeature for this to work.")]
    [WorkflowDesigner(DisplayName = "Get Current Feature", ToolboxCategory = "Results and Selections",IconResourceName="Vejle.png")]
    public sealed class GetCurrentFeature : ExternalActivityBase
    {
        [CategoryAttribute("Out Arguments")]
        [Description("The current item in the Feature Details dialog, return a FeatureSet serialized to JSON string as value")]
        [DefaultValue(null)]
        [DisplayName("CurrentFeature")]
        public OutArgument<string> CurrentFeature { get; set; }
        [CategoryAttribute("Out Arguments")]
        [Description("The Titel of the current item in the Feature Details dialog")]
        [DefaultValue(null)]
        [DisplayName("Titel")]
        public OutArgument<string> Titel { get; set; }
    }


    [Description("Get the current or all results from Results View. Set the ExternalID to GetResults for this to work.")]
    [WorkflowDesigner(DisplayName = "Get Results", ToolboxCategory = "Results and Selections", IconResourceName = "results")]
    public sealed class GetResults : ExternalActivityBase
    {
        [CategoryAttribute("In Arguments")]
        [Description("The layer name of the result set to return. Leave blank to return the result set in the currently active tab.")]
        [DefaultValue(null)]
        [DisplayName("LayerName")]
        public InArgument<string> LayerName { get; set; }

        [CategoryAttribute("Out Arguments")]
        [Description("The current results from Results View, as Dictionary<string,string> Layername as key, a List of FeatureSets serialized to JSON as value")]
        [DefaultValue(null)]
        [DisplayName("CurrentResults")]
        public OutArgument<Dictionary<string, string>> CurrentResults { get; set; }
        [CategoryAttribute("Out Arguments")]
        [Description("All results from Results View, as Dictionary<string,string> Layername as key, a List of FeatureSets serialized to JSON as value")]
        [DefaultValue(null)]
        [DisplayName("AllResults")]
        public OutArgument<Dictionary<string, string>> AllResults { get; set; }
    }

    [Description("Get the selections from Result view. Set the ExternalID to GetSelections for this to work.")]
    [WorkflowDesigner(DisplayName = "Get Selections", ToolboxCategory = "Results and Selections", IconResourceName = "star")]
    public sealed class GetSelections : ExternalActivityBase
    {
        [CategoryAttribute("Out Arguments")]
        [Description("All results from Results View, as Dictionary<string,string> Layername as key, a List of FeatureSets serialized to JSON as value")]
        [DefaultValue(null)]
        [DisplayName("Selections")]
        public OutArgument<Dictionary<string, string>> CurrentSelections { get; set; }
    }

    [Description("Create a active Geocortex FeatureSetCollection, Set the ExternalID to SetActiveFeatureSetCollection for this to work.")]
    [WorkflowDesigner(DisplayName = "Create an active FeatureSetCollection.", ToolboxCategory = "Results and Selections", IconResourceName = "layers")]
    public sealed class SetActiveFeatureSetCollection : ExternalActivityBase
    {
        [CategoryAttribute("In Arguments")]
        [Description("Dictionary<String, String> name of the GeocortexLayer is the key, and the value is the associated JSON representation of the featureset")]
        [DefaultValue(null)]
        [DisplayName("FeatureSet Dictionary")]
        public InArgument<Dictionary<string,string>> FeatureSetDictionary { get; set; }
    }



    [Description("List visible layers in active layertheme, Set the ExternalID to 'ListVisibleLayers' for this to work.")]
    [WorkflowDesigner(DisplayName = "List Visible Layers in active layertheme", ToolboxCategory = "Results and Selections", IconResourceName = "layers")]
    public sealed class ListVisibleLayers : ExternalActivityBase
    {
        [CategoryAttribute("Out Arguments")]
        [Description("Dictionary<string, string>  with the displayname as key and FullyQualifiedName as vale, of all visible layers")]
        [DefaultValue(null)]
        [DisplayName("VisibleLayers")]
        public OutArgument<Dictionary<string,string>> VisibleLayers { get; set; }
        [CategoryAttribute("Out Arguments")]
        [Description("Active Layertheme displayname")]
        [DefaultValue(null)]
        [DisplayName("Layertheme")]
        public OutArgument<string> LayerTheme { get; set; }
    }

    [Description("List layerthemes, Set the ExternalID to 'ListLayerThemes' for this to work.")]
    [WorkflowDesigner(DisplayName = "List layerthemes", ToolboxCategory = "Results and Selections", IconResourceName = "layers")]
    public sealed class ListLayerThemes : ExternalActivityBase
    {
        [CategoryAttribute("Out Arguments")]
        [Description("Dictionary<string,string> a dictionary of string with the ID as Key and the displayname as value")]
        [DefaultValue(null)]
        [DisplayName("LayerThemes")]
        public OutArgument<Dictionary<string, string>> LayerThemes { get; set; }
        [CategoryAttribute("Out Arguments")]
        [Description("Active Layertheme displayname")]
        [DefaultValue(null)]
        [DisplayName("ActiveLayerTheme")]
        public OutArgument<string> ActiveLayerTheme { get; set; }
    }

    [Description("Remove menuitem from the  'I Want To' menu, Set the ExternalID to 'RemoveMenuItem' for this to work.")]
    [WorkflowDesigner(DisplayName = "Remove MenuItem", ToolboxCategory = "Results and Selections", IconResourceName = "layers")]
    public sealed class RemoveMenuItem : ExternalActivityBase
    {
        
        [CategoryAttribute("In Arguments")]
        [Description("<string> Name of the menuitem you want to remove")]
        [DefaultValue(null)]
        [DisplayName("ItemName")]
        public InArgument<string> ItemName { get; set; }
    }

    [Description("Get info from current site, Set the ExternalID to 'GetSiteInfo' for this to work.")]
    [WorkflowDesigner(DisplayName = "Get Site Info", ToolboxCategory = "Results and Selections", IconResourceName = "layers")]
    public sealed class GetSiteInfo : ExternalActivityBase
    {
        [CategoryAttribute("Out Arguments")]
        [Description("<string> GeometryServer Url")]
        [DefaultValue(null)]
        [DisplayName("GeometryServer")]
        public OutArgument<string> GeometryServer { get; set; }
        [CategoryAttribute("Out Arguments")]
        [Description("<Dictionary <string><object>> Site Proberties")]
        [DefaultValue(null)]
        [DisplayName("Site Proberties")]
        public OutArgument<Dictionary<string, object>> Proberties { get; set; }
        [CategoryAttribute("Out Arguments")]
        [Description("<string> Site name")]
        [DefaultValue(null)]
        [DisplayName("Site name")]
        public OutArgument<string> Name { get; set; }
        [CategoryAttribute("Out Arguments")]
        [Description("<string> Site Url")]
        [DefaultValue(null)]
        [DisplayName("Site Url")]
        public OutArgument<string> Url { get; set; }
    }

    
}
