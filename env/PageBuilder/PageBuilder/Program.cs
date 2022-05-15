using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Diagnostics;

class Program
{
	public static bool UsingDefaultSourceDir { get; set; } = true;
	public static bool UsingDefaultDestinationDir { get; set; } = true;
	public static string SourceDir { get; set; } = @"D:\Work\2022\Project\Homepage\blogs";
	public static string DestinationDir { get; set; } = @"D:\Work\2022\Project\Homepage\html\blogs\content";
	public static void PrintUsageAndExit()
	{
		Console.WriteLine("[Usage] build [options]");
		Console.WriteLine("\t -s \t Source Directory");
		Console.WriteLine("\t -d \t Destination Directory");
		Environment.Exit(0);
	}
	public static void Main(string[] args)
	{
		int index;
		for (index = 0; index < args.Length; index++)
		{
			switch (args[index])
			{
				case "-s":
					if (++index >= args.Length) PrintUsageAndExit();
					SourceDir = args[index];
					UsingDefaultSourceDir = false;
					break;
				case "-d":
					if (++index >= args.Length) PrintUsageAndExit();
					DestinationDir = args[index];
					UsingDefaultDestinationDir = false;
					break;
			}
		}
		if (UsingDefaultSourceDir)
			Console.WriteLine($"[Warning] Using Default Source: {SourceDir}");
		if (UsingDefaultDestinationDir)
			Console.WriteLine($"[Warning] Using Default Destination: {DestinationDir}");
		Console.WriteLine();
		Build(SourceDir, DestinationDir);
	}
	public static void Build(string srcRoot, string destRoot)
	{
		if (!Directory.Exists(srcRoot))
		{
			Console.Error.WriteLine($"[{Path.GetFullPath(srcRoot)}] Does Not Exist.");
			return;
		}
		if (Directory.Exists(destRoot)) Directory.Delete(destRoot, true);
		Directory.CreateDirectory(destRoot);
		DirectoryInfo dir = new(srcRoot);
		foreach (FileInfo file in dir.GetFiles())
		{
			if (file.Extension.ToLower() == ".md")
				MarkdownToHTML(file.FullName, Path.ChangeExtension(Path.Combine(destRoot, file.Name), ".html"));
		}
		foreach (DirectoryInfo subdir in dir.GetDirectories())
		{
			string target = Path.Combine(destRoot, subdir.Name);
			if (!Directory.Exists(target)) Directory.CreateDirectory(target);
			Build(subdir.FullName, target);
		}
	}
	public static void MarkdownToHTML(string src, string dest)
	{
		Console.WriteLine($"[Markdown -> HTML] Convert {src} to {dest}");
		Process p = new Process();
		//设置要启动的应用程序
		p.StartInfo.FileName = "cmd.exe";
		//是否使用操作系统shell启动
		p.StartInfo.UseShellExecute = false;
		//接受来自调用程序的输入信息
		p.StartInfo.RedirectStandardInput = true;
		//输出信息
		p.StartInfo.RedirectStandardOutput = true;
		// 输出错误
		p.StartInfo.RedirectStandardError = true;
		//不显示程序窗口
		p.StartInfo.CreateNoWindow = true;
		//启动程序
		p.Start();
		//向cmd窗口发送输入信息
		p.StandardInput.WriteLine($"pandoc \"{src}\" -o \"{dest}\" --self-contained && exit");
		//p.StandardInput.AutoFlush = true;
		//等待程序执行完退出进程
		p.WaitForExit();
		//获取输出信息
		string strOuput = p.StandardOutput.ReadToEnd();
		p.Close();
		//return strOuput;
	}
}