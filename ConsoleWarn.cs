namespace System
{
	public static class ConsoleEx
	{
		public static void Warn(string message)
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine(message);
			Console.ResetColor();
		}
		public static void Warn(string message, object arg0)
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine(message, arg0);
			Console.ResetColor();
		}
		public static void Warn(string message, object arg0, object arg1)
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine(message, arg0, arg1);
			Console.ResetColor();
		}
	}
}