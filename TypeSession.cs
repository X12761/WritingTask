using System;
using System.Collections.Generic;

// Singleton
public class TypeSession
{
  private static readonly Lazy<TypeSession> _instance = new Lazy<TypeSession>(() => new TypeSession());
  public static TypeSession Instance => _instance.Value;

  // Demograph
  public static int Id { get; set; }  // Session uniq id
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
  public static QuizLogEntry Quiz { get => _lastQuiz; }
  private static List<QuizLogEntry> _quiz { get; } = new List<QuizLogEntry>();
  public static List<QuizLogEntry> QuizLog { get => _quiz; }

  private TypeSession() // 4 Singleton
  {
    int tmark = (int)(DateTimeOffset.UtcNow.ToUnixTimeSeconds() / 60);
    int guidHash = Math.Abs(Guid.NewGuid().GetHashCode());
    Id = tmark ^ guidHash;
  } 

  // Starts new keystroke logging session
  public static void NewSession(bool withAI)
  {
    _sessions.Add(new SingleSession());
    _lastSession = _sessions[_sessions.Count - 1];
    _lastSession.Started = DateTime.Now;
    _lastSession.withAI = withAI;
  }

  // Complete keystroke session
  public static void Complete()
  {
    _lastSession.Completed = DateTime.Now;
  }

  public static void NewQuiz(string name = "None")
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

  public KeyLogEntry() { }
  public KeyLogEntry(KeyLogEntry source)
  {
    this.KeyName = source.KeyName;
    this.PressedAt = source.PressedAt;
    this.PressedAtMs = source.PressedAtMs;
    this.ReleasedAt = source.ReleasedAt;
    this.ReleasedAtMs = source.ReleasedAtMs;
    this.DurationMs = source.DurationMs;
  }

  public KeyLogEntry Clone() => new KeyLogEntry(this);
}
// -------------------------------------------------------- Quiz list entry
public class QuizLogEntry
{
  private int _rate;

  public QuizLogEntry(string name)
  {
    QuizName = name;
    Start = DateTime.Now;
    aiPredict = '-';
    aiConfidence = 0;
  }

  public string QuizName { get; set; }
  public Char aiPredict { get; set; }
  public double aiConfidence { get; set; }
  public DateTime Start { get; set; }
  public DateTime Complete { get; set; }
  public int Rate { get => _rate; set { _rate = value; Complete = DateTime.Now; } }
}