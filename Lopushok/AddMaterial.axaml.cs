using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Lopushok.Models;
using System.Linq;

namespace Lopushok;

public partial class AddMaterial : Window
{

    private readonly int _currentProductId;

    public AddMaterial()
    {
        InitializeComponent();
    }

    public AddMaterial(int id)
    {
        InitializeComponent();
        _currentProductId = id;
        LoadMaterialBox();

    }

    private void BackButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close();
    }


    private async void AddButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        
    }

    private void LoadMaterialBox()
    {
        var context = new DemoContext();

        var materials = context.Materials.Select(x => x.Title).ToList();

        MaterialBox.ItemsSource = materials;
    }
}