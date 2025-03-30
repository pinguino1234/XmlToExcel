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

        bool _ShowMainMessage = false;
        public bool ShowMainMessage
        {
            get => _ShowMainMessage;
            set => this.RaiseAndSetIfChanged(ref _ShowMainMessage, value);
        }


        string _MainMessage = "";
        public string MainMessage
        {
            get => _MainMessage;
            set => this.RaiseAndSetIfChanged(ref _MainMessage, value);
        }

        public bool LoadNewFile(string? FilePath)
        {
            if (string.IsNullOrEmpty(FilePath)) return false;

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
