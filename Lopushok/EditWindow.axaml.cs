using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Lopushok.Models;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System;
using System.Linq;

namespace Lopushok;

public partial class EditWindow : Window
{
    private readonly Product _product;
    private string imageName;

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
    }

    private void Back_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
    }

    private void AddProduct_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {

    }

    private void AddImage_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }

    private void GetInfo()
    {

       
        var context = new DemoContext();

        var item = context.Producttypes.Where(x => x.Id == _product.Producttypeid).Select(x=>x.Title);

        NameBox.Text = _product.Title;
        ArticleBox.Text = _product.Articlenumber;
        TypeBox.SelectedItem = item;
        PeopleBox.Text = _product.Productionpersoncount?.ToString();
        ZavodBox.Text = _product.Productionworkshopnumber?.ToString();
        MinCostBox.Text = _product.Mincostforagent.ToString("F2");
        ImageBox.Source = new Bitmap(_product.Image);

        
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
}