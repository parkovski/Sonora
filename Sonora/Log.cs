namespace Sonora;

public static class Log {
    public static void Print(string fmt, params string[] args) {
        Console.WriteLine(string.Format(fmt, args));
    }

    public static void EPrint(string fmt, params string[] args) {
        Console.Error.WriteLine(string.Format(fmt, args));
    }

    public static void Trace(string fmt, params string[] args) {
        SnrLogTrace(string.Format(fmt, args));
    }

    public static void Debug(string fmt, params string[] args) {
        SnrLogDebug(string.Format(fmt, args));
    }

    public static void Info(string fmt, params string[] args) {
        SnrLogInfo(string.Format(fmt, args));
    }

    public static void Warn(string fmt, params string[] args) {
        SnrLogWarn(string.Format(fmt, args));
    }

    public static void Error(string fmt, params string[] args) {
        SnrLogError(string.Format(fmt, args));
    }

    public static void Critical(string fmt, params string[] args) {
        SnrLogCritical(string.Format(fmt, args));
    }
}
