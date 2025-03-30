using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.ReactiveUI;
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

        void DropFile(object? sender, DragEventArgs e)
        {
            var db = e.Data.GetFiles()?.ToList()[0].Path.LocalPath;

            ViewModel!.IsFileEnter = false;
            ViewModel!.LoadNewFile(db);
        }

        void EnterFile(object? sender, DragEventArgs e) 
            => ViewModel!.IsFileEnter = true;
    }
}