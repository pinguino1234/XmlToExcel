using Avalonia.Controls;
using Avalonia.Media;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Input;
using System.Xml.Linq;
using XmlToExcel.Models;

namespace XmlToExcel.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<NodeElement> NodeElements { get; set; } = [];
        public ObservableCollection<Concepto> Items { get; set; } = [];

        public ICommand ExportData { get; set; }

        List<string> ExcludedAttributes = [
            "cfdi",
            "xsi",
            "Certificado",
            "NoCertificado",
            "Sello",
            "schemaLocation"
        ];

        bool _IsFileEnter = false;
        public bool IsFileEnter 
        {
            get => _IsFileEnter;
            set => this.RaiseAndSetIfChanged(ref _IsFileEnter, value); 
        }

        bool _ShowMainMessage = true;
        public bool ShowMainMessage
        {
            get => _ShowMainMessage;
            set => this.RaiseAndSetIfChanged(ref _ShowMainMessage, value);
        }

        bool _ShowMainDataGrid = false;
        public bool ShowMainDataGrid
        {
            get => _ShowMainDataGrid;
            set => this.RaiseAndSetIfChanged(ref _ShowMainDataGrid, value);
        }

        IBrush _PanelBackground = Brushes.Black;
        public IBrush PanelBackgorund 
        { 
            get => _PanelBackground;
            set => this.RaiseAndSetIfChanged(ref _PanelBackground, value);
        }

        IBrush _Foreground = Brushes.White;
        public IBrush Foreground
        {
            get => _Foreground;
            set => this.RaiseAndSetIfChanged(ref _Foreground, value);
        }

        string _MainMessage = "Arrastra uno o varios XML para comenzar";
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

                //Obtenemos todos los elementos secundarios
                doc.Descendants().Where(x => x.Name.LocalName != "Conceptos").ToList().ForEach(x =>
                {
                    var ne = CreateNodeElement(x);
                    NodeElements.Add(ne);
                });

                //Obtenemos todos los conceptps
                doc.Descendants().Where(x => x.Name.LocalName == "Concepto").ToList().ForEach(x =>
                    Items.Add(new Concepto(x.Attributes().ToArray()))
                );

                return true;
            }
            catch 
            {
                return false;
            }
             
        }

        NodeElement CreateNodeElement(XElement x)
        {
            var ne = new NodeElement();
            ne.ElementName = x.Name.LocalName;

            x.Attributes().ToList().ForEach(x =>
            {
                if (!ExcludedAttributes.Contains(x.Name.LocalName))
                    ne.Attributes.Add(x);
            });

            return ne;
        }

        public MainWindowViewModel()
        {
            ExportData = ReactiveCommand.Create((string path) =>
            {
                var TextToWrite = "";
                var no = Items.Where(x => x.SaveData).Count();

                if (no > 0)
                {
                    //Obtenemos propiedades   
                    var properties = Items[0].GetType().GetProperties().Where(x => x.Name != "SaveData").ToArray();

                    //Escribir Nombre de propiedades
                    foreach (var property in properties)
                    {
                        TextToWrite += $"{property.Name},";
                    }

                    TextToWrite += Environment.NewLine;

                    //Escribir Valores
                    foreach (var item in Items)
                    {
                        foreach (var property in properties) 
                        {
                            TextToWrite += $"{property.GetValue(item)},";
                        }

                        TextToWrite += Environment.NewLine;
                    }

                    File.WriteAllText(path, TextToWrite);
                }

            });
        }
    }
}
