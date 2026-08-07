using System;
using System.Collections.Generic;

// Singleton
public class TypeSession
{
  private static readonly Lazy<TypeSession> _instance = new Lazy<TypeSession>(() => new TypeSession());
  public static TypeSession Instance => _instance.Value;

  // Demograph
  public static int age { get; set; } = 17;
  public static string gender { get; set; }
  public static string lang { get; set; } = "e.g. Korean, English, Mayan";
  public static string english { get; set; } = "B2";
  public static int prof { get; set; } = 10;
  public static string code { get; set; } = "PBNA"; // (P)ositive (B)aseline, (N)euthral (A)I

  // Key stroke session
  private static List<SingleSession> _sessions = new List<SingleSession>();
  public static List<SingleSession> Sessions { get => _sessions; }
  private static SingleSession _lastSession;
  public static List<KeyLogEntry> KeyLog { get => _lastSession.KeyLog; }

  // Quiz
  private static QuizLogEntry _lastQuiz;
  private static QuizLogEntry Quiz { get => _lastQuiz; }
  private static List<QuizLogEntry> _quiz { get; } = new List<QuizLogEntry>();
  public static List<QuizLogEntry> QuizLog { get => _quiz; }

  private TypeSession() { } // 4 Singleton

  // Starts new keystroke logging session
  public static void NewSession()
  {
    _sessions.Add(new SingleSession());
    _lastSession = _sessions[_sessions.Count - 1];
  }

  // Complete keystroke session
  public static void Complete(bool withAI = false)
  {
    _lastSession.Completed = DateTime.Now;
    _lastSession.withAI = withAI;
  }

  public static void NewQuiz(string name)
  {
    _quiz.Add(new QuizLogEntry(name));
    _lastQuiz = _quiz[_quiz.Count - 1];
  }
}

public class SingleSession
{
  public DateTime Started { get; set; }
  public DateTime Completed { get; set; }
  public List<KeyLogEntry> KeyLog { get; } = new List<KeyLogEntry>();
  public bool withAI { get; set; }
  public int AImark { get; set; }

  public SingleSession() 
  {
    Started = DateTime.Now;
  }
}
// -------------------------------------------------------- Log list entry
public class KeyLogEntry
{
  public string KeyName { get; set; }
  public TimeSpan PressedAt { get; set; }
  public double PressedAtMs { get; set; }
  public TimeSpan ReleasedAt { get; set; }
  public double ReleasedAtMs { get; set; }
  public double DurationMs { get; set; }
}
// -------------------------------------------------------- Quiz list entry
public class QuizLogEntry
{
  private int _rate;

  public QuizLogEntry(string name = "default")
  { QuizName = name;
    Asked = DateTime.Now;
    aiPredict = -1;
    aiConfidence = 0;
  }

  public string QuizName { get; set; }
  public int aiPredict { get; set; }
  public double aiConfidence { get; set; }
  public DateTime Asked { get; set; }
  public DateTime Complete { get; set; }
  public int Rate { get => _rate; set { _rate = value; Complete = DateTime.Now; } }
}