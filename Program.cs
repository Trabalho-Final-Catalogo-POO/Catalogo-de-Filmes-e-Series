 using backend;
 namespace frontend;
 

   static class Program
 {
    [STAThread]
    static void Main()
    {
       Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
       Application.Run(new MainForm());

        Menu menu = new Menu();
        menu.MenuLogin();
    }
 }
