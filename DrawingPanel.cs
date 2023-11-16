using Godot;
using System;

public partial class DrawingPanel : Panel
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	/// <summary>
	/// Runs whenever the RunButton is pressed.
	/// </summary>
	private void _on_run_button_pressed()
	{
		// Get the code edit node and access the code text.
		var codeEdit = GetNode<TextEdit>("../CodeEdit");
		var code = codeEdit.Text;
		
		
	}
}


