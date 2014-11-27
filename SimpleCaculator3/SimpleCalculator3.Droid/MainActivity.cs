using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using Android.App;
using Android.OS;
using Android.Widget;
using ExtendedInterfaces.Pcl;

namespace SimpleCalculator3.Droid
{
    [Activity(Label = "SimpleCalculator3.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        [Import]
        public ICalculator calculator { get; set; }

        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += delegate
            {
                button.Text = string.Format("{0} clicks!",
                    calculator.Calculate(count++ + "+1"));
            };

            var configuration = new ContainerConfiguration()
                .WithAssembly(System.Reflection.Assembly.Load("ExtendedInterfaces.Pcl.dll"))
                .WithAssembly(System.Reflection.Assembly.Load("ExtendedOperations.Pcl.dll"));

            using (var container = configuration.CreateContainer())
            {
                try
                {
                    //test satisfy again
                    container.SatisfyImports(this);
                }
                catch (CompositionFailedException compositionException)
                {
                    System.Diagnostics.Debug.WriteLine(compositionException.ToString());
                }
            }
        }
    }
}