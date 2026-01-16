using Avalonia.Controls;
using Lopushok.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace Lopushok;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        LoadProducts();
        LoadComboBox();
    }

    private async void LoadProducts()
    {
        using var context = new DemoContext();

        var allProducts = context.Products.Include(x => x.Producttype).Include(x => x.Productmaterials).ThenInclude(x => x.Material).ToList();

        if (!string.IsNullOrEmpty(SearchBox.Text))
        {
            var searchText = SearchBox.Text.ToLower();

            allProducts = allProducts.Where(x => (x.Title.ToLower().Contains(searchText)) || (x.Description != null && x.Description.ToLower().Contains(searchText))).ToList();
        }



        switch (SortBox.SelectedIndex)
        {
            case 1:
                allProducts = allProducts.OrderBy(x => x.Title).ToList();
                break;
            case 2:
                allProducts = allProducts.OrderByDescending(x => x.Title).ToList();
                break;
            case 3:
                allProducts = allProducts.OrderBy(x => x.Productionworkshopnumber).ToList();
                break;
            case 4:
                allProducts = allProducts.OrderByDescending(x => x.Productionworkshopnumber).ToList();
                break;
            case 5:
                allProducts = allProducts.OrderBy(x => x.Mincostforagent).ToList();
                break;
            case 6:
                allProducts = allProducts.OrderByDescending(x => x.Mincostforagent).ToList();
                break;
            default:
                allProducts = allProducts.OrderBy(x => x.Id).ToList();
                break;
        }


        if (FilterBox.SelectedItem != null && FilterBox.SelectedItem.ToString() != "Все типы")
        {
            var productTypeTitle = FilterBox.SelectedItem.ToString();

            var productType = context.Producttypes.FirstOrDefault(x => x.Title == productTypeTitle);

            if (productType != null)
            {
                allProducts = allProducts.Where(x => x.Producttypeid == productType.Id).ToList();
            }
        }

        ProductsBox.ItemsSource = allProducts;


    }


    private async void LoadComboBox()
    {
        using var context = new DemoContext();
        var filteredProducts = context.Producttypes.Select(x => x.Title).ToList();
        filteredProducts.Add("Все типы");
        FilterBox.ItemsSource = filteredProducts.OrderByDescending(x => x == "Все типы");
    }



    private void SearchBox_KeyUp(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        LoadProducts();
    }

    private void SortBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        LoadProducts();
    }

    private void FilterBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        LoadProducts();
    }

    private void AddWindow_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var addWindow = new AddWindow();
        addWindow.Show();
        this.Close();
    }

    private void ProductListBox_Click(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        if(ProductsBox.SelectedItem is Product product)
        {
            var editWindow = new EditWindow(product);
            editWindow.Show();
            this.Close();
        }


        
    }

    //Посхалка для Марка
}