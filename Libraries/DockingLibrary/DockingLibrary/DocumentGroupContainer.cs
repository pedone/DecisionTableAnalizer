using System.Windows;
using System;

namespace DockingLibrary
{

    public class DocumentGroupContainer : DockingContentControl<DocumentGroup>
    {

        static DocumentGroupContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DocumentGroupContainer), new FrameworkPropertyMetadata(typeof(DocumentGroupContainer)));
        }

    }
}