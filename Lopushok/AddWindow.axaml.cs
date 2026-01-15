using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Lopushok.Models;
using System;
using System.IO;
using System.Linq;

namespace Lopushok;

public partial class AddWindow : Window
{
    string imageName = Guid.NewGuid().ToString("N");

    public AddWindow()
    {
        InitializeComponent();
        LoadTypeBox();
    }

    private void Back_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var catalogWindow = new MainWindow();
        catalogWindow.Show();
        this.Close();
    }

    private void AddMaterial_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var addMaterialWindow = new AddMaterial();
        addMaterialWindow.ShowDialog(this);
    }

    private async void AddProduct_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        using var context = new DemoContext();
        var p = int.TryParse(PeopleBox.Text, out int people);
        var z = int.TryParse(ZavodBox.Text, out int zavod);
        var min = int.TryParse(MinCostBox.Text, out int minCostForAgent);

        var newProduct = new Product
        {
            Title = NameBox.Text,
            Articlenumber = ArticleBox.Text,
            Description = DescriptionBox.Text,
            Image = imageName,
            Productionpersoncount = people,
            Productionworkshopnumber = zavod,
            Mincostforagent = minCostForAgent

        };

        if (TypeBox.SelectedItem != null)
        {
            var productType = context.Producttypes.FirstOrDefault(x => x.Title == TypeBox.SelectedItem.ToString());
            newProduct.Producttypeid = productType.Id;
        }

        context.Products.Add(newProduct);
        await context.SaveChangesAsync();

        var addMaterialWindow = new AddMaterial(newProduct.Id);
        addMaterialWindow.ShowDialog(this);


    }

    private async void AddImage_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new Avalonia.Platform.Storage.FilePickerSaveOptions
        {
            Title = "Выбор фото",
            FileTypeChoices = new[]
            {
                new FilePickerFileType("Изображения")
                {
                    Patterns = new [] {"*.jpg","*.jpeg", "*.png" }
                }
            }

        });

        if (file != null)
        {
            ImageBox.Source = new Bitmap(file.Path.LocalPath);
            string targetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imageName + Path.GetExtension(file.Name));
            File.Copy(file.Path.LocalPath, targetPath);
            imageName = targetPath;
        }
    }

    private void LoadTypeBox()
    {
        var context = new DemoContext();

        var typeProducts = context.Producttypes.Select(x => x.Title).ToList();

        TypeBox.ItemsSource = typeProducts;
    }

    private void LoadMaterialListBox()
    {

    }
}