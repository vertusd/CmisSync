//   SparkleShare, an instant update workflow to Git.
//   Copyright (C) 2010  Hylke Bons <hylkebons@gmail.com>
//
//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.
//
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with this program.  If not, see <http://www.gnu.org/licenses/>.

using Gtk;
using Notifications;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;


namespace SparkleShare {

	// This is SparkleShare!
	public class SparkleShare {

		
		
		public static SparkleShareUI SparkleShareUI;

		public static void Main (string [] args) {

			// Check if git is installed
			Process Process = new Process();
			Process.StartInfo.RedirectStandardOutput = true;
			Process.StartInfo.UseShellExecute = false;
			Process.StartInfo.FileName = "git";
			Process.Start();
			if (Process.StandardOutput.ReadToEnd().IndexOf ("version") == -1) {
				Console.WriteLine ("Git wasn't found.");
				Console.WriteLine ("You can get it from http://git-scm.com/.");
				Environment.Exit (0);
			}

			// Don't allow running as root
			Process.StartInfo.FileName = "whoami";
			Process.Start();
			if (Process.StandardOutput.ReadToEnd().Trim ().Equals ("root")) {
				Console.WriteLine ("Sorry, you can't run SparkleShare as root.");
				Console.WriteLine ("Things will go utterly wrong."); 
				Environment.Exit (0);
			}

			// Parse the command line arguments
			bool HideUI = false;
			if (args.Length > 0) {
				foreach (string Argument in args) {
					if (Argument.Equals ("--disable-gui") || Argument.Equals ("-d"))
						HideUI = true;
					if (Argument.Equals ("--help") || Argument.Equals ("-h")) {
						ShowHelp ();
					}
				}
			}

			Gtk.Application.Init ();

			SparkleShareUI = new SparkleShareUI (HideUI);
			SparkleShareUI.StartMonitoring ();

			Gtk.Application.Run ();

		}

		public static void ShowHelp () {
			Console.WriteLine ("SparkleShare Copyright (C) 2010 Hylke Bons");
			Console.WriteLine ("");
			Console.WriteLine ("This program comes with ABSOLUTELY NO WARRANTY.");
			Console.WriteLine ("This is free software, and you are welcome to redistribute it ");
			Console.WriteLine ("under certain conditions. Please read the GNU GPLv3 for details.");
			Console.WriteLine ("");
			Console.WriteLine ("SparkleShare syncs the ~/SparkleShare folder with remote repositories.");
			Console.WriteLine ("");
			Console.WriteLine ("Usage: sparkleshare [start|stop|restart] [OPTION]...");
			Console.WriteLine ("Sync SparkleShare folder with remote repositories.");
			Console.WriteLine ("");
			Console.WriteLine ("Arguments:");
			Console.WriteLine ("\t -d, --disable-gui\tDon't show the notification icon.");
			Console.WriteLine ("\t -h, --help\t\tDisplay this help text.");
			Console.WriteLine ("");
			Environment.Exit (0);
		}

	}

	
	
}