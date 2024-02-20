using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
namespace MaquinaRam.Views;

public partial class MainWindow : Window
{
    private TextBox inputTape_;
    private RamController? machine_;
    private TextBox OutputTape_;
    private List<string> registers = new List<string>();


    public MainWindow()
    {
        InitializeComponent();
        inputTape_ = this.FindControl<TextBox>("inputTape");
        OutputTape_ = this.FindControl<TextBox>("OutputTape");
        lstRegistros.ItemsSource = registers;
    }

    private async void LoadFile(object sender, RoutedEventArgs e)
    {
        //  Abrir un archivo de texto y mostrarlo en el TextBox
        OpenFileDialog dialog = new OpenFileDialog
        {
            AllowMultiple = false
        };
        dialog.Filters.Add(new FileDialogFilter() { Name = "Program Files", Extensions = { "txt", "ram" } });
        string[] result = await dialog.ShowAsync(this);
        if (result.Length > 0)
        {
            string text = System.IO.File.ReadAllText(result[0]);
            txtEditor.Text = text;
            try {
                 machine_ = new RamController(txtEditor.Text, inputTape_.Text.Trim());
            } catch (Exception ex) {
                OutputTape_.Text = ex.Message;
            }
        }
    }

    private void Run(object sender, RoutedEventArgs e)
    {
        if (machine_ == null){
            OutputTape_.Foreground = new SolidColorBrush(Colors.Red);
            OutputTape_.Text = "No hay programa cargado";
            return;
        }
        try {
            OutputTape_.Foreground = new SolidColorBrush(Colors.White);
            machine_.Reset(inputTape_.Text, txtEditor.Text);
            machine_.Run();
            UpdateRegisters(machine_.GetRegisters());
            OutputTape_.Text = machine_.GetOutput();

        } catch (Exception ex) {
            OutputTape_.Foreground = new SolidColorBrush(Colors.Red);
            OutputTape_.Text = ex.Message;
        }
    }

    public void RunNumberOfInstruction(object sender, RoutedEventArgs e)
    {
        if (machine_ == null) return;
        try {
            OutputTape_.Foreground = new SolidColorBrush(Colors.White);
            machine_.Reset(inputTape_.Text, txtEditor.Text);
            machine_.RunInstructions();
            UpdateRegisters(machine_.GetRegisters());
            OutputTape_.Text = machine_.GetOutput();
        } catch (Exception ex) {
            OutputTape_.Foreground = new SolidColorBrush(Colors.Red);
            OutputTape_.Text = ex.Message;
        }       
        
    }

    private void UpdateRegisters(Dictionary<int, int> new_registers)
    {
        registers.Clear();
        foreach (var registro in new_registers)
        {
            registers.Add($"R{registro.Key}: {registro.Value}");
        }
        registers.Add($"R0: {machine_.GetAcc()}");
        registers.Reverse();
        lstRegistros.ItemsSource = null;
        lstRegistros.ItemsSource = registers;
    }
}