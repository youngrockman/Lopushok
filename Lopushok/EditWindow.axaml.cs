using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Lopushok.Models;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System;
using System.Linq;
using Avalonia.Platform.Storage;
using System.IO;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;

namespace Lopushok;

public partial class EditWindow : Window
{
    private readonly Product _product;
    private string imageName;
    private ObservableCollection<Productmaterial> _materials = new ObservableCollection<Productmaterial>();

    public EditWindow()
    {
        InitializeComponent();

    }



    public EditWindow(Models.Product product)
    {
        InitializeComponent();
        _product = product;
        imageName = _product.Image ?? Guid.NewGuid().ToString("N");
        GetInfo();
        LoadTypeBox();
        LoadMaterials();
    }

    private void Back_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
    }


    private void LoadMaterials()
    {
        using var context = new DemoContext();

        var materials = context.Productmaterials.Include(x => x.Material).Where(x => x.Productid == _product.Id).ToList();

        _materials.Clear();

        foreach (var material in materials)
        {
            _materials.Add(material);
        }


        MaterialsListBox.ItemsSource = _materials;
    }

    private void AddMaterial_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var addMaterialWindow = new AddMaterial(_product.Id);
        addMaterialWindow.MaterialAdded += OnMaterialAdded;
        addMaterialWindow.ShowDialog(this);
    }




    private void OnMaterialAdded()
    {
        LoadMaterials();
    }





    private async void AddProduct_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (!Validation())
        {
            return;
        }

        using var context = new DemoContext();


        var productToUpdate = context.Products.FirstOrDefault(x => x.Id == _product.Id);

        var p = int.TryParse(PeopleBox.Text, out int people);
        var z = int.TryParse(ZavodBox.Text, out int zavod);
        var c = decimal.TryParse(MinCostBox.Text, out decimal cost);

        var productTypeName = TypeBox.SelectedItem.ToString();

        var typeId = context.Producttypes.Where(x => x.Title == productTypeName).Select(x => x.Id).FirstOrDefault();

        if (productToUpdate != null)
        {
            productToUpdate.Title = NameBox.Text;
            productToUpdate.Articlenumber = ArticleBox.Text;
            productToUpdate.Image = imageName;
            productToUpdate.Productionpersoncount = people;
            productToUpdate.Productionworkshopnumber = zavod;
            productToUpdate.Mincostforagent = cost;

            if (TypeBox.SelectedItem != null)
            {
                productToUpdate.Producttypeid = typeId;
            }
        }

        await context.SaveChangesAsync();

        var successMessage = MessageBoxManager.GetMessageBoxStandard("Успех", "Продукт изменен", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success);
        await successMessage.ShowAsync();

        var mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();



    }

    private async void AddImage_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new Avalonia.Platform.Storage.FilePickerSaveOptions
        {
            Title = "Выбор изображения",
            FileTypeChoices = new[]
            {
                new FilePickerFileType("Изображения")
                {
                    Patterns = new [] {"*.jpg","*.jpeg", "*.png"}
                }
            }
        });


        if (file != null)
        {
            ImageBox.Source = new Bitmap(file.Path.LocalPath);
            var targetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imageName + Path.GetExtension(file.Name));
            File.Copy(file.Path.LocalPath, targetPath);
            imageName = targetPath;

        }

    }

    private void GetInfo()
    {


        var context = new DemoContext();


        NameBox.Text = _product.Title;
        ArticleBox.Text = _product.Articlenumber;
        PeopleBox.Text = _product.Productionpersoncount?.ToString();
        ZavodBox.Text = _product.Productionworkshopnumber?.ToString();
        MinCostBox.Text = _product.Mincostforagent.ToString("F2");

        if (!string.IsNullOrEmpty(_product.Image))
        {
            ImageBox.Source = new Bitmap(_product.Image);
        }
        else
        {
            ImageBox.Source = new Bitmap("picture.png");
        }





        var productType = context.Producttypes.FirstOrDefault(x => x.Id == _product.Producttypeid.Value);

        if (productType != null)
        {
            TypeBox.SelectedItem = productType.Title;
        }



    }


    private void LoadTypeBox()
    {

        using var context = new DemoContext();
        var typeProducts = context.Producttypes.Select(x => x.Title).ToList();
        TypeBox.ItemsSource = typeProducts;

    }


    private bool Validation()
    {
        if (string.IsNullOrEmpty(NameBox.Text) || string.IsNullOrEmpty(ArticleBox.Text) || string.IsNullOrEmpty(TypeBox.SelectedItem.ToString())
            || string.IsNullOrEmpty(PeopleBox.Text) || string.IsNullOrEmpty(ZavodBox.Text) || string.IsNullOrEmpty(MinCostBox.Text))
        {
            var errorMessage = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Не должно быть пустых полей", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            errorMessage.ShowAsync();
            return false;
        }

        if (!int.TryParse(PeopleBox.Text, out int people))
        {
            var errorMessage = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Количество человек должно быть числом", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            errorMessage.ShowAsync();
            return false;
        }

        if (people <= 0)
        {
            var errorMessage = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Количество человек должно быть положительным числом", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            errorMessage.ShowAsync();
            return false;
        }


        if (!int.TryParse(ZavodBox.Text, out int zavod))
        {
            var errorMessage = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Номер цеха должен быть числом", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            errorMessage.ShowAsync();
            return false;
        }

        if (zavod <= 0)
        {
            var errorMessage = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Номер цеха должен быть положительным числом", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            errorMessage.ShowAsync();
            return false;
        }


        if (!decimal.TryParse(MinCostBox.Text, out decimal minCost))
        {
            var errorMessage = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Минимальная стоимость должна быть числом", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            errorMessage.ShowAsync();
            return false;
        }

        if (minCost <= 0)
        {
            var errorMessage = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Минимальная стоимость должна быть положительным числом", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            errorMessage.ShowAsync();
            return false;
        }

        return true;
    }

    private async void DeleteMaterial_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        using var context = new DemoContext();

        var productMaterialId = (int)(sender as Button)!.Tag!;

        var productMaterial = context.Productmaterials.FirstOrDefault(x => x.Productmaterialid == productMaterialId);


        if (productMaterial != null)
        {
            var qustion = MessageBoxManager.GetMessageBoxStandard("Подтверждение", "Дейтвительно хотите удалить материал?", ButtonEnum.YesNo, MsBox.Avalonia.Enums.Icon.Info);
            var answer = await qustion.ShowAsync();

            if (answer == ButtonResult.Yes)
            {
                context.Productmaterials.Remove(productMaterial);
                await context.SaveChangesAsync();
                LoadMaterials();


                var successMessage = MessageBoxManager.GetMessageBoxStandard("Успех", "Материал удален", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success);
                await successMessage.ShowAsync();
            }



        }


    }

    private async void DeleteProduct_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        using var context = new DemoContext();

       if(_product != null)
        {
            var qustion = MessageBoxManager.GetMessageBoxStandard("Подтверждение", "Дейтвительно хотите удалить продукт?", ButtonEnum.YesNo, MsBox.Avalonia.Enums.Icon.Info);
            var answer = await qustion.ShowAsync();

            if (answer == ButtonResult.Yes)
            {
                context.Products.Remove(_product);
                await context.SaveChangesAsync();

                var successMessage = MessageBoxManager.GetMessageBoxStandard("Успех", "Продукт удален", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success);
                await successMessage.ShowAsync();

                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }

        } 
    }
}