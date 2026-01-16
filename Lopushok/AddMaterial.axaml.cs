using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Lopushok.Models;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System;
using System.Linq;

namespace Lopushok;

public partial class AddMaterial : Window
{

    private readonly int _productId;
    public event Action MaterialAdded;


    public AddMaterial()
    {
        InitializeComponent();
    }



    public AddMaterial(int id)
    {
        InitializeComponent();
        _productId = id;
        LoadMaterialBox();
    }

   

    private void BackButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close();
    }


    private async void AddButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (MaterialBox.SelectedItem == null)
        {
            var errorMessage = MessageBoxManager.GetMessageBoxStandard("Ошибка","Выберите материал", ButtonEnum.Ok,MsBox.Avalonia.Enums.Icon.Error);
            await errorMessage.ShowAsync();
            return;
            
        }

        
        if (!int.TryParse(CountBox.Text, out int count) || count <= 0)
        {
            var errorMessage = MessageBoxManager.GetMessageBoxStandard( "Ошибка", "Введите корректное количество (целое положительное число)", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            await errorMessage.ShowAsync();
            return;
            
        }

        using var context = new DemoContext();

       
        var selectedMaterialTitle = MaterialBox.SelectedItem.ToString();
        var material = context.Materials.FirstOrDefault(m => m.Title == selectedMaterialTitle);

        

        bool alreadyExists = context.Productmaterials.Any(pm => pm.Productid == _productId && pm.Materialid == material.Id);

        if (alreadyExists)
        {
            var errorMessage = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Этот материал уже добавлен к продукту", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            await errorMessage.ShowAsync();
        }

        var productMaterial = new Productmaterial
        {
            Productid = _productId,
            Materialid = material.Id,
            Count = count
        };

        context.Productmaterials.Add(productMaterial);
        await context.SaveChangesAsync();

        
        MaterialAdded?.Invoke();


        var successMessage = MessageBoxManager.GetMessageBoxStandard("Успех","Материал добавлен", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success);
        await successMessage.ShowAsync();

        
        this.Close();
    }

    private void LoadMaterialBox()
    {
        var context = new DemoContext();

        var materials = context.Materials.Select(x => x.Title).ToList();

        MaterialBox.ItemsSource = materials;
    }
}