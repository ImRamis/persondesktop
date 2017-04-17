using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Live_Your_Life
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region FormInternalFunctions

        void SuppressErrors()
        {
            dynamic activeX = this.Titanium.GetType().InvokeMember("ActiveXInstance",
                System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null, this.Titanium, new object[] { });
            activeX.Silent = true;
        }
        void TabControl(int tabNo)
        {
            for (int i = 0; i < tabs.Count; i++)
                if (i == tabNo)
                    tabs[i].Visibility = Visibility.Visible;
                else
                    tabs[i].Visibility = Visibility.Hidden;
        }

        #endregion
        List<Canvas> tabs = new List<Canvas>();
        public MainWindow()
        {
            InitializeComponent();
            tabs.AddRange(new[] { MainMenu,WorkingSpace, CreationSpace, LoaderSpace });
            foreach (var item in Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory).Where(t => !t.Contains("Resources")))
                folders.Items.Add(item.Split('\\').Last());
            TabControl(0);
        }
        int index = 0;
        void CreateNew(object sender, RoutedEventArgs e)
        {
            TabControl(2);
            m.Close();
        }
        void CancelCreate(object sender, RoutedEventArgs e) => TabControl(0);
        void templateInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Template != null)
            {
                Template.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/" + templateInfo.SelectedValue.ToString().Split(' ').Last() + ".png"));
                index = templateInfo.SelectedIndex;
            }
        }
        Process m = new Process();
        void OpenProject(object sender, RoutedEventArgs e) {
            module = Project.LoadProject(folders.SelectedValue.ToString());
  //          m.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
  //          m.StartInfo.FileName = "cmd.exe";
//            m.StartInfo.Arguments = "/C " + String.Format("cd {0} && ionic serve -b -a -p 9100", module.Project);
            TabControl(3);
            new Thread(() =>
            {
    //            m.Start();
                System.Threading.Thread.Sleep(2000);
                Titanium.Dispatcher.Invoke(new Action(() => {
                    TabControl(1);
                    Titanium.Navigate("http://localhost/te/Project/index.html"); }));
            }).Start();
        }
        Module module = null;
        void CreateProject(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(location.Text))
            {
                string text = location.Text;
                TabControl(3);
                new System.Threading.Thread(() => {
                    Project.CreateProject(text, index == 0 ? Templates.blank : index == 1 ? Templates.sidemenu : Templates.tabs);
                    LoaderSpace.Dispatcher.Invoke(new Action(() => {
                        TabControl(1);
                        module = Project.LoadProject(location.Text);
                    }));
                }).Start();
            }
            else
                MessageBox.Show("Project with the same name exist!");
        }
        void Exit(object sender, RoutedEventArgs e)
        {
            Environment.Exit(1);
        }
        void Simulate(object sender, RoutedEventArgs e)
        {
            if (module != null)
            {
            //    m.Close();
          //      m.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
      //          m.StartInfo.FileName = "cmd.exe";
    //            m.StartInfo.Arguments = "/C " + String.Format("cd {0} && ionic serve -a -p 9100", module.Project);
        //        m.Start();
              //  m.WaitForExit();
            }
        }

        private void pageAdder(object sender, RoutedEventArgs e)
        {
        }
    }
    public enum Templates
    {
        blank,
        sidemenu,
        tabs
    }
    class Project
    {
        public static void CreateProject(string ProjectName, Templates template)
        {
            Process p = new Process();
//            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
           // p.StartInfo.FileName = "cmd.exe";
           // p.StartInfo.Arguments = "/C "+String.Format("ionic start {0} {1}",ProjectName,template.ToString());
          //  p.Start();
        //    p.WaitForExit();
        }
        public static Module LoadProject(string ProjectName)
        {
            Module p = new Module() { Project = ProjectName };
            return p;
        }
        public static void RunProject(string ProjectName)
        {

        }
        public static void BuildProject(string ProjectName)
        {

        }
    }
    class Module
    {
        public Routes routes { get; set; }
        public string Project { get; set; }

    }
    class Routes
    {
        public List<Page> routes { get; set; }
        public void RouteAdd(Page page)
        {
            routes.Add(page);
            /*
             * to be implemented app.js routes manipulation 
             */
        }
    }
    class Page
    {
        public string Name { get; set; }
        public View View { get; set; }
        public Controller Controller { get; set; }
        public Dictionary<string,string> parameters { get; set; }
        public Page()
        {
            parameters = new Dictionary<string, string>();
        }

    }
    class View
    {
        public string template { get; set; }
        public View(string template)
        {
            this.template = template;
        }
    }
    class Controller
    {
        public string controller { get; set; }
        public Controller(string controller)
        {
            this.controller = controller;
        }
    }
}