using Avalonia.Controls;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;
using XmlToExcel.Models;

namespace XmlToExcel.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<NodeElement> NodeElements { get; set; } = [];

        bool _IsFileEnter = false;
        public bool IsFileEnter 
        {
            get => _IsFileEnter;
            set => this.RaiseAndSetIfChanged(ref _IsFileEnter, value); 
        }

        public bool LoadNewFile(string? FilePath)
        {
            try
            {
                XDocument doc = XDocument.Load(FilePath);

                doc.Descendants().ToList().ForEach(x =>
                {
                    var ne = new NodeElement();
                    ne.ElementName = x.Name.LocalName;
                    ne.Attributes = x.Attributes().ToList();

                    NodeElements.Add(ne);
                });

                return true;
            }
            catch 
            {
                return false;
            }
             
        }
    }
}
