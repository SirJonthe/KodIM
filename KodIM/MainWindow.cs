using System;
using Gtk;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		txtSubject.Text = System.Environment.UserName + " says";
		txtMessage.GrabFocus ();
		LoadHistory ();
	}

	~MainWindow ()
	{
		SaveHistory ();
	}

	protected void LoadHistory()
	{
		try {
			//Console.WriteLine("Loading history.txt...");
			System.IO.StreamReader reader = new System.IO.StreamReader ("history.txt");
			while (!reader.EndOfStream) {
				string recipient = reader.ReadLine();
				txtRecipient.AppendText (recipient);
				//Console.WriteLine("\t" + recipient);
			}
			reader.Close ();
			//Console.WriteLine("done");
		} catch {
			//Console.WriteLine ("No history.txt");
		}
	}

	protected void SaveHistory()
	{
		//Console.WriteLine ("Saving new history.txt...");
		System.IO.StreamWriter writer = new System.IO.StreamWriter ("history.txt", false);
		Gtk.TreeIter iter;
		if (txtRecipient.Model.GetIterFirst (out iter)) {
			do {
				GLib.Value thisRow = new GLib.Value ();
				txtRecipient.Model.GetValue (iter, 0, ref thisRow);
				string recipient = (thisRow.Val as string);
				writer.WriteLine(recipient);
				//Console.WriteLine("\t" + recipient);
			} while (txtRecipient.Model.IterNext (ref iter));
		}
		writer.Close ();
		//Console.WriteLine ("done");
	}

	protected string SanitizeString(string str)
	{
		String s = str;
		for (int i = 0; i < s.Length; ++i) {
			if (s [i] == ',') {
				s.Remove (i);
				--i;
			}
		}
		return s.ToString ();
	}

	protected void Send(string recipient, string title, string message)
	{
		title = SanitizeString (title);
		message = SanitizeString (message);
		System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo ();
		psi.FileName = "python";
		int duration = (int)(spinbutton1.Value) * 1000;
		psi.Arguments = "kodi-send.py --host=" + recipient + " -a \"Notification(" + title + ", " + message + ", " + duration.ToString() + ")\"";
		//Console.WriteLine ("Filename:  " + psi.FileName);
		//Console.WriteLine ("Arguments: " + psi.Arguments);
		txtStatus.LabelProp = "Sending...";
		System.Diagnostics.Process p = System.Diagnostics.Process.Start (psi);
		p.WaitForExit ();
		if (p.ExitCode < 0) {
			txtStatus.LabelProp = "Sending...failed";
		} else {
			txtStatus.LabelProp = "Sending...done";
		}
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void HandleSend (object sender, EventArgs e)
	{
		if (txtRecipient.ActiveText == "all") {

			Gtk.TreeIter iter;
			if (txtRecipient.Model.GetIterFirst (out iter)) {
				do {
					GLib.Value thisRow = new GLib.Value ();
					txtRecipient.Model.GetValue (iter, 0, ref thisRow);
					string recipient = (thisRow.Val as string);
					Send (recipient, txtSubject.Text, txtMessage.Text);
				} while (txtRecipient.Model.IterNext (ref iter));
			}

		} else {

			Send (txtRecipient.ActiveText, txtSubject.Text, txtMessage.Text);

			bool conflict = false;
			Gtk.TreeIter iter;
			if (txtRecipient.Model.GetIterFirst (out iter)) {
				do {
					GLib.Value thisRow = new GLib.Value ();
					txtRecipient.Model.GetValue (iter, 0, ref thisRow);
					if ((thisRow.Val as string).Equals (txtRecipient.ActiveText)) {
						conflict = true;
						break;
					}
				} while (txtRecipient.Model.IterNext (ref iter));
			}
			if (!conflict) {
				txtRecipient.AppendText (txtRecipient.ActiveText);
			}

		}
		txtMessage.Text = "";
	}
}
