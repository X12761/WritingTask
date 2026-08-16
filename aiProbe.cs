using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WritingTask
{
  public class aiProbe
  {
    public List<KeyLogEntry> KeyData;

    public aiProbe(List<KeyLogEntry> KeyLog)
    {
      KeyData = new List<KeyLogEntry>(KeyLog.Count); 
      foreach (KeyLogEntry p in KeyLog) KeyData.Add(p.Clone());
    }

    public async Task MakeData(string outFile="ai\\indata.csv", string outFile2 = "ai\\login.csv")
    {
      using (var writer = new StreamWriter(outFile2, false, Encoding.UTF8)) // Login data
      {
        writer.WriteLine("Age,Gender,Native,English,Keystroke,Start");
        writer.WriteLine($"{TypeSession.age}, {TypeSession.gender}, {TypeSession.lang}, {TypeSession.english}, {TypeSession.prof}, {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
      }
      // Keylog
      using (var writer = new StreamWriter(outFile, false, Encoding.UTF8))
      {
        //await writer.WriteLineAsync("Key,Pressed(ms),Released(ms),Duration(ms)");
        await writer.WriteLineAsync($"{KeyLogEntry.KeyLogHead}");
        foreach (KeyLogEntry p in KeyData)
        {
          string line = p.KeyLogLine();
          await writer.WriteLineAsync(line);
        }
      }
    }

    public async Task<string> ExecAsync(string pyScript = "ai\\predict_session.py")
    {
      var process = new Process
      {
        StartInfo = new ProcessStartInfo  // Prepare process
        {
          FileName = "py", 
          Arguments = $"{Environment.CurrentDirectory}\\{pyScript}",
          UseShellExecute = false,        // for redirection
          RedirectStandardOutput = true,  // std output
          RedirectStandardError = true,   // err output
          CreateNoWindow = true,          // hide console window
          StandardOutputEncoding = Encoding.UTF8, // encoding
          StandardErrorEncoding = Encoding.UTF8
        }
      };

      var outputBuilder = new StringBuilder();
      var errorBuilder = new StringBuilder();

      // Async reading events
      process.OutputDataReceived += (sender, e) =>
      {
        if (!string.IsNullOrEmpty(e.Data))
          outputBuilder.AppendLine(e.Data);
      };

      process.ErrorDataReceived += (sender, e) =>
      {
        if (!string.IsNullOrEmpty(e.Data))
          errorBuilder.AppendLine(e.Data);
      };

      process.Start(); // Exec process
      process.BeginOutputReadLine(); // Start reading async
      process.BeginErrorReadLine();
      await Task.Run(() => process.WaitForExit()); // Wait finish async

      string output = outputBuilder.ToString().Trim(); // Get output
      string errors = errorBuilder.ToString().Trim();

      if (process.ExitCode != 0) return $"Err {process.ExitCode} {errors}";
    
      return output;
    }

    public string Predict()
    {
      return "H 50"; // +D+
    }
  }
}
