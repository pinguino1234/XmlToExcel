using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.ReactiveUI;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System.Linq;
using XmlToExcel.ViewModels;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Styling;
using System.Threading.Tasks;
using Velopack;
using Avalonia.Media;

namespace XmlToExcel.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        //Custom FilePickerFileType 
        FilePickerFileType ExcelFormat { get; } = new("Archivo Separado por Comas")
        {
            Patterns = ["*.csv"]
        };

        public MainWindow()
        {
            InitializeComponent();

            //Inicializamos el tema actual
            RequestedThemeVariant = ThemeVariant.Dark;

            //Registramos los eventos del D&D
            AddHandler(DragDrop.DropEvent, DropFile);
            AddHandler(DragDrop.DragEnterEvent, EnterFile);
        }

        async void DropFile(object? sender, DragEventArgs e)
        {
            ViewModel!.ShowMainMessage = false;

            MainLoadingView.IsVisible = true;

            var db = e.Data.GetFiles()?.ToList();

            ViewModel!.IsFileEnter = false;
            ViewModel!.ShowMainMessage = false;
                
            var archivosSinAbrir = 0;
            var archivoCorrecto = false;

            foreach(var item in db)
            {
                if (!ViewModel!.LoadNewFile(item.Path.LocalPath))
                {
                    archivosSinAbrir++;
                    return;
                }

                archivoCorrecto = true;
            }

            MainLoadingView.IsVisible = false; 
                 
            if (archivosSinAbrir > 0)
            {
                var box =
                        MessageBoxManager.GetMessageBoxStandard("Error", $"Ups! Al parecer {archivosSinAbrir} no son válidos", ButtonEnum.Ok);

                _ = await box.ShowAsync();

                ViewModel!.MainMessage = "Arrastra uno o varios XML para comenzar";
                
            }

            if (!archivoCorrecto)
                ViewModel!.ShowMainMessage = true;
            
        }

        void EnterFile(object? sender, DragEventArgs e)
        {
            ViewModel!.IsFileEnter = true;
            ViewModel!.MainMessage = "¡Suéltalo!";
            ViewModel!.ShowMainMessage = true;
        }
           

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

        public void CancelDragDrop(object sender, KeyEventArgs e )
        {
            if (e.Key == Key.Escape)
            {
                ViewModel!.IsFileEnter = false;
            }
        }

        void ToggleTheme(object sender, RoutedEventArgs e)
        {
            if (RequestedThemeVariant == ThemeVariant.Light)
            {
                ViewModel!.PanelBackgorund = Brushes.White;
                ViewModel!.Foreground = Brushes.Black;

                RequestedThemeVariant = ThemeVariant.Dark;
                return;
            }

            ViewModel!.PanelBackgorund = Brushes.Black;
            ViewModel!.Foreground = Brushes.White;

            RequestedThemeVariant = ThemeVariant.Light;
        }

        private static async Task UpdateMyApp()
        {
            var mgr = new UpdateManager("https://github.com/pinguino1234/XmlToExcel/");

            // check for new version
            var newVersion = await mgr.CheckForUpdatesAsync();
            if (newVersion == null)
                return; // no update available

            // download new version
            await mgr.DownloadUpdatesAsync(newVersion);

            // install new version and restart app
            mgr.ApplyUpdatesAndRestart(newVersion);
        }
    }
}