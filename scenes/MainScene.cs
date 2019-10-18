using Godot;
using Sulimn.Classes;

public class MainScene : Control
{
    private Button BtnLogin;
    private LineEdit TxtHeroName, PswdPassword;
    private Label LblError;

    #region Load

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey eventKey && eventKey.Pressed)
        {
            if (eventKey.Scancode == (int)KeyList.Enter || (eventKey.Scancode == (int)KeyList.KpEnter && (!BtnLogin.Disabled)))
                _on_BtnLogin_pressed();
            else if (eventKey.Scancode == (int)KeyList.Escape)
                GetTree().Quit();
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        OS.WindowMaximized = true;
        GameState.LoadAll();
        AssignControls();
        TxtHeroName.GrabFocus();
    }

    /// <summary>Assigns all controls to something usable in code.</summary>
    private void AssignControls()
    {
        BtnLogin = (Button)GetNode("CC/VB/Buttons/BtnLogin");
        TxtHeroName = (LineEdit)GetNode("CC/VB/HeroName");
        PswdPassword = (LineEdit)GetNode("CC/VB/Password");
        LblError = (Label)GetNode("LblError");
    }

    #endregion Load

    #region Click

    private void _on_BtnLogin_pressed()
    {
        if (GameState.CheckLogin(TxtHeroName.Text.Trim(), PswdPassword.Text.Trim()))
            Login();
        else
            LblError.Text = "Invalid credentials.";
    }

    private void _on_BtnNewHero_pressed() => GetTree().ChangeScene("scenes/NewHeroScene.tscn");

    #endregion Click

    #region Manage Text Input

    /// <summary>Enabled the login Button if there is text in both fields.</summary>
    private void ToggleButton() => BtnLogin.Disabled = TxtHeroName.Text.Length == 0 || PswdPassword.Text.Length == 0;

    private void _on_HeroName_focus_entered() => TxtHeroName.SelectAll();

    private void _on_HeroName_focus_exited() => TxtHeroName.Deselect();

    private void _on_Password_focus_entered() => PswdPassword.SelectAll();

    private void _on_Password_focus_exited() => PswdPassword.Deselect();

    private void _on_HeroName_text_changed(string new_text) => ToggleButton();

    private void _on_Password_text_changed(string new_text) => ToggleButton();

    #endregion Manage Text Input

    #region Login

    /// <summary>Clears the input boxes.</summary>
    internal void ClearInput()
    {
        TxtHeroName.Text = "";
        PswdPassword.Text = "";
        TxtHeroName.GrabFocus();
    }

    /// <summary>Logs the Hero in.</summary>
    private void Login()
    {
        ClearInput();
        GetTree().ChangeScene("scenes/city/CityScene.tscn");
    }

    #endregion Login

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}