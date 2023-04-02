using Discord;
using Discord.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Discord_Groups_Leaver
{
    internal class Program
    {
      


        public static DiscordSocketClient client = new DiscordSocketClient();


        public static void Login(string token, MainWindow mainWindow) => Task.Factory.StartNew(delegate ()
        {
            try { client.Login(token); }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
            client.OnLoggedIn += (sender, args) => client_OnLoggedIn(sender, args, mainWindow);
            client.OnLoggedOut += (sender, args) => client_OnLoggedOut(sender, args, mainWindow);
            Console.WriteLine(client.User.Username);
        });
        private static void client_OnLoggedIn(DiscordSocketClient client, LoginEventArgs args, MainWindow mainWindow)
        {
            // Use Dispatcher.Invoke to update the UI from the correct thread
            mainWindow.Dispatcher.Invoke(() =>
            {
                string username = client.User.Username;
                username += "#";
                username += client.User.Discriminator;
                mainWindow.UserNameLabel.Content = username;
            });
        }

        private static void client_OnLoggedOut(DiscordSocketClient client, LogoutEventArgs args, MainWindow mainWindow)
        {
            // Use Dispatcher.Invoke to update the UI from the correct thread
            mainWindow.Dispatcher.Invoke(() =>
            {
                mainWindow.UserNameLabel.Content = "Logged Out";
            });
        }


  


        public static void Logout() => Task.Factory.StartNew(delegate ()
        {
            client.Logout();
            
        });


        public static class ListControl
        {
            private static List<ListItemInfo> groupsInfo = new List<ListItemInfo>();
            //scrape all groups
            public static void ScrapeGroups(MainWindow mainWindow) => Task.Factory.StartNew(delegate ()
            {
                var groups = client.GetPrivateChannels();
                Console.WriteLine("Scraping..");
                foreach (var group in groups)
                {
                    if (group.Type == ChannelType.Group)
                    {
                        Console.WriteLine(group.Name);
                        mainWindow.Dispatcher.Invoke(() =>
                        {
                            ListBoxItem listItem = new ListBoxItem();
                            listItem.Content = group.Name ?? "Unnamed";
                            listItem.Tag = group;
                            mainWindow.ListControl.Items.Add(listItem);
                        });
                    }
                }
            });
            public static async void LeaveSelectedGroups(MainWindow mainWindow)
            {
                await mainWindow.Dispatcher.BeginInvoke(new Action(async () =>
                {
                    List<Task> leaveTasks = new List<Task>();

                    foreach (ListBoxItem item in mainWindow.List.SelectedItems)
                    {
                        PrivateChannel privateChannel = (PrivateChannel)item.Tag;
                        Console.WriteLine("+ leaving " + privateChannel.Id);

                        leaveTasks.Add(Task.Run(() => privateChannel.Leave()));
                    }

                    await Task.WhenAll(leaveTasks);
                }));
            }
            public class ListItemInfo
            {
                public int Index { get; set; }
                public PrivateChannel PrivateChannel { get; set; }

                public ListItemInfo(int index, PrivateChannel privateChannel)
                {
                    Index = index;
                    PrivateChannel = privateChannel;
                }
            }
        }
        

    }
}
