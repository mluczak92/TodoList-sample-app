using Autofac;
using System.Windows;
using System.Windows.Threading;
using TodoList_sample_app.Models;
using TodoList_sample_app.Models.Database;
using TodoList_sample_app.ViewModels;

namespace TodoList_sample_app {
    public partial class App : Application {
        string conString = "Server=(local); Database=TodoList; Integrated Security=true";
        IContainer container;

        void App_Startup(object sender, StartupEventArgs e) {
            DispatcherUnhandledException += Application_DispatcherUnhandledException;
            ReadArgs(e.Args);
            RegisterServices();
            ResolveMainWindow();
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) {
            MessageBox.Show(e.Exception.Message, "Unhandled exception occured!",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        void ReadArgs(string[] args) {
            if (args.Length > 0) {
                conString = args[0];
            }
        }

        void RegisterServices() {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<MainVm>()
                .As<IMainVm>()
                .SingleInstance();

            builder.RegisterType<TodoContext>()
                .WithParameter(new TypedParameter(typeof(string), conString))
                .AsSelf();

            builder.RegisterType<DatabaseMigrator>().As<IDatabaseMigrator>();
            builder.RegisterType<EFDaysRepository>().As<IDaysRepository>();
            builder.RegisterType<EFItemsRepository>().As<IItemsRepository>();
            builder.RegisterType<DatesBoundriesSelector>().As<IBoundriesSelector>();
            builder.RegisterType<CalendarVm>().As<ICalendarVm>();
            builder.RegisterType<DayVm>().As<IDayVm>();
            builder.RegisterType<ItemVm>().As<IItemVm>();

            container = builder.Build();
        }

        void ResolveMainWindow() {
            MainWindow mainWindow = new MainWindow {
                DataContext = container.Resolve<IMainVm>()
            };
            mainWindow.Show();
        }
    }
}
