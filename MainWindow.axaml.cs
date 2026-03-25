using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;
using Avalonia.Markup.Xaml; 

namespace DayPlannerGUI
{
public partial class MainWindow : Window
{
    private TaskService service = new TaskService();
    private List<string> currentTasks = new List<string>();
    public MainWindow()
    {
        InitializeComponent();
        service.CleanOldCompletedTasks();
    }

    private void AddTask_Click(object sender, RoutedEventArgs e)
    {
        service.AddTask(TaskInput.Text ?? "");
        TaskInput.Text = "";
    }

    private void ShowToday_Click(object sender, RoutedEventArgs e)
    {
       currentTasks = service.GetTodayTasks();
       TaskList.ItemsSource = currentTasks;
       if (currentTasks.Count == 0)
       {
           NoTasksMessage.Text = "No tasks for today";
           NoTasksMessage.IsVisible = true;
       }
       else
       {
           NoTasksMessage.IsVisible = false;
       }
    }
    private void ShowAllTasks_Click(object sender, RoutedEventArgs e)
    {
       currentTasks = service.GetAllTasks();
       TaskList.ItemsSource = currentTasks;
       if (currentTasks.Count == 0)
       {
           NoTasksMessage.Text = "No tasks available";
           NoTasksMessage.IsVisible = true;
       }
       else
       {
           NoTasksMessage.IsVisible = false;
       }
    }
    private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
          int index = TaskList.SelectedIndex; 

          if(index == -1)
                return;

            service.RemoveTask(index);  

            currentTasks = service.GetAllTasks();
            TaskList.ItemsSource = currentTasks;
            if (currentTasks.Count == 0)
            {
                NoTasksMessage.Text = "No tasks available";
                NoTasksMessage.IsVisible = true;
            }
            else
            {
                NoTasksMessage.IsVisible = false;
            }
        }
    private void MarkCompleted_Click(object sender, RoutedEventArgs e)
    {
        int index = TaskList.SelectedIndex;

        if(index == -1)
            return;

        service.CompleteTask(index);

        currentTasks = service.GetAllTasks();
        TaskList.ItemsSource = currentTasks;
        if (currentTasks.Count == 0)
        {
            NoTasksMessage.Text = "No tasks available";
            NoTasksMessage.IsVisible = true;
        }
        else
        {
            NoTasksMessage.IsVisible = false;
        }
    }

}}