using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.ReactiveUI;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System.Linq;
using XmlToExcel.ViewModels;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System.IO;

namespace XmlToExcel.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();

            //Registramos los eventos del D&D
            AddHandler(DragDrop.DropEvent, DropFile);
            AddHandler(DragDrop.DragEnterEvent, EnterFile);
        }

        async void DropFile(object? sender, DragEventArgs e)
        {
            var db = e.Data.GetFiles()?.ToList()[0].Path.LocalPath;

            ViewModel!.IsFileEnter = false;
            ViewModel!.ShowMainMessage = false;

            if (!ViewModel!.LoadNewFile(db))
            {
                var box = 
                    MessageBoxManager.GetMessageBoxStandard("Error", "Ups! Al parecer el XML no es válido", ButtonEnum.Ok);

                _ = await box.ShowAsync();
            }
        }

        void EnterFile(object? sender, DragEventArgs e) 
            => ViewModel!.IsFileEnter = true;

        async void ExportData(object sender, RoutedEventArgs e)
        {
            // Get top level from the current control. Alternatively, you can use Window reference instead.
            var topLevel = TopLevel.GetTopLevel(this);

            // Start async operation to open the dialog.
            var file = await topLevel!.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Exportar a CSV",
                FileTypeChoices = [ExcelFormat]
            });

            if (file is not null)
            {
                var dir = file.Path.AbsolutePath;
                ViewModel!.ExportData.Execute(dir);
            }


        }

        //Custom FilePickerFileType 
        FilePickerFileType ExcelFormat { get; } = new("Archivo Separado por Comas")
        {
            Patterns = new[] { "*.csv"}
        };
    }
}