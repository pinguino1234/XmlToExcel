using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.ReactiveUI;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System.Linq;
using XmlToExcel.ViewModels;

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

            if (!ViewModel!.LoadNewFile(db))
            {
                var box = 
                    MessageBoxManager.GetMessageBoxStandard("Error", "Ups! Al parecer el XML no es válido", ButtonEnum.Ok);

                _ = await box.ShowAsync();
            }
        }

        void EnterFile(object? sender, DragEventArgs e) 
            => ViewModel!.IsFileEnter = true;
    }
}