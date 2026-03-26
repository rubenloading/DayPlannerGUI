using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;

public class TaskService
{
    private string filePath = "tasks.txt";

    public void AddTask(string input)
    {
        string[] parts = input.Split(',');

        if (parts.Length != 2)
        {
            return;
        }
            

        string taskText = parts[0].Trim();
        string dateInput = parts[1].Trim();

        if (!DateTime.TryParseExact(dateInput, "dd.MM.yyyy", CultureInfo.InvariantCulture,DateTimeStyles.None,out DateTime taskDate))
        {
            return;
        }
           
            

        File.AppendAllText(filePath,$"{taskText},{taskDate:dd.MM.yyyy}{Environment.NewLine}");
    }

    public List<string> GetTodayTasks()
    {
        DateTime today = DateTime.Now.Date;

        if (!File.Exists(filePath))
            return new List<string>();

        return File.ReadAllLines(filePath)
                   .Where(t => {
                       var parts = t.Split(',');
                       if (parts.Length >= 2 && DateTime.TryParseExact(parts[parts.Length - 1], "dd.MM.yyyy",
                           CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime taskDate))
                       {
                           return taskDate.Date == today;
                       }
                       return false;
                   })
                   .ToList();
    }

    public List<string> GetAllTasks()
    {
        if (!File.Exists(filePath))
            return new List<string>();

        return File.ReadAllLines(filePath).ToList();
    }
    public void RemoveTask(int index)
    {
        if (!File.Exists(filePath))
            return;

        var tasks = File.ReadAllLines(filePath).ToList();

        if (index < 0 || index >= tasks.Count)
            return;

        tasks.RemoveAt(index);
        File.WriteAllLines(filePath, tasks);
    }

    public void CompleteTask(int index)
    {
        if (!File.Exists(filePath))
        {
            return;
        }
        

        var tasks = File.ReadAllLines(filePath).ToList();

        if (index < 0 || index >= tasks.Count)
        {
            return;
        }
            
        
        if(!tasks[index].StartsWith("[COMPLETED] "))
        {
            tasks[index] = "[COMPLETED] " + tasks[index];
        }
            
        File.WriteAllLines(filePath, tasks);
    }

    public void CleanOldCompletedTasks()
    {
        if (!File.Exists(filePath))
        {
            return; 
        }
            

        var tasks = File.ReadAllLines(filePath).ToList();
        var filteredTasks = new List<string>();

        foreach (var task in tasks)
        {
            if (task.StartsWith("[COMPLETED] "))
            {
                var parts = task.Split(',');
                if (parts.Length >= 2 && DateTime.TryParseExact(parts[parts.Length - 1], "dd.MM.yyyy",CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime taskDate))
                    
                {
                    if (taskDate.Date >= DateTime.Now.AddDays(-1).Date)
                    {
                        filteredTasks.Add(task);
                    }
                    
                }
                else
                {
                    
                    filteredTasks.Add(task);
                }
            }
            else
            {
                filteredTasks.Add(task);
            }
        }

        File.WriteAllLines(filePath, filteredTasks);
    }
}