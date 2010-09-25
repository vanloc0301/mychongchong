namespace SkyMap.Net.Core
{
    using SkyMap.Net.Gui;
    using System;

    public interface ISecondaryDisplayBinding
    {
        bool CanAttachTo(IViewContent content);
        ISecondaryViewContent[] CreateSecondaryViewContent(IViewContent viewContent);

        bool ReattachWhenParserServiceIsReady { get; }
    }
}

