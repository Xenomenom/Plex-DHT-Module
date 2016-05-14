using System;
using System.Collections.Generic;
using System.Net;
using System.Xml.Linq;

namespace Plex
{
    public class Plex : IModule
    {
        public string Name { get; } = "Plex";
        public string Identifier { get; } = "plex";
        public string Description { get; } = "Plex Controlling Module";
        public string Device { get; } = "Any";
        public string Version { get; } = "0.1";

        private WebClient Client { get; set; }

        private IDictionary<string, object> Config { get; set; }

        private XDocument Document { get; set; }

        public bool Initialize(IDictionary<string, object> config)
        {
            this.Config = config;
            this.Client = new WebClient();
            return true;
        }

        public bool Run(string key, int value, string parameter)
        {
            if (parameter.ToLower().StartsWith("refresh"))
            {
                if (this.Config["connection"].ToString().ToLower().Equals("https"))
                {
                    this.Document = XDocument.Load(string.Format("https://{0}:{1}/library/sections/?X-Plex-Token={2}", this.Config["server"], this.Config["serverPort"], this.Config["token"]));

                    if (this.Document.Root.Name == "MediaContainer")
                    {
                        string[] refreshSplit = parameter.Split(new char[] { ' ' }, 2);

                        if (refreshSplit[1].ToLower().Equals("all"))
                        {
                            foreach (XElement element in Document.Root.Elements("Directory"))
                            {
                                this.Client.DownloadString(string.Format("https://{0}:{1}/library/sections/{2}/refresh/?X-Plex-Token={3}", this.Config["server"], this.Config["serverPort"], element.Attribute("key").Value, this.Config["token"]));
                            }
                        }

                        else
                        {
                            foreach (XElement element in Document.Root.Elements("Directory"))
                            {
                                if (element.Attribute("title").Value.ToLower().Equals(refreshSplit[1].ToLower()))
                                {
                                    this.Client.DownloadString(string.Format("https://{0}:{1}/library/sections/{2}/refresh/?X-Plex-Token={3}", this.Config["server"], this.Config["serverPort"], element.Attribute("key").Value, this.Config["token"]));
                                }
                            }
                        }
                    }
                }
                else if (this.Config["connection"].ToString().ToLower().Equals("http"))
                {
                    this.Document = XDocument.Load(string.Format("http://{0}:{1}/library/sections/?X-Plex-Token={2}", this.Config["server"], this.Config["serverPort"], this.Config["token"]));

                    if (this.Document.Root.Name == "MediaContainer")
                    {
                        string[] refreshSplit = parameter.Split(new char[] { ' ' }, 2);

                        if (refreshSplit[1].ToLower().Equals("all"))
                        {
                            foreach (XElement element in Document.Root.Elements("Directory"))
                            {
                                this.Client.DownloadString(string.Format("http://{0}:{1}/library/sections/{2}/refresh/?X-Plex-Token={3}", this.Config["server"], this.Config["serverPort"], element.Attribute("key").Value, this.Config["token"]));
                            }
                        }

                        else
                        {
                            foreach (XElement element in Document.Root.Elements("Directory"))
                            {
                                if (element.Attribute("title").Value.ToLower().Equals(refreshSplit[1].ToLower()))
                                {
                                    this.Client.DownloadString(string.Format("http://{0}:{1}/library/sections/{2}/refresh/?X-Plex-Token={3}", this.Config["server"], this.Config["serverPort"], element.Attribute("key").Value, this.Config["token"]));
                                }
                            }
                        }
                    }
                }
            }
            else if(parameter.ToLower().StartsWith("control"))
            {
                string[] controlSplit = parameter.Split(new char[] { ' ' }, 2);
                
                if(controlSplit[1].ToLower().Equals("home"))
                {
                    this.Client.DownloadString(string.Format("http://{0}:{1}/player/navigation/home", this.Config["client"], this.Config["clientPort"]));
                }
                else if (controlSplit[1].ToLower().Equals("music"))
                {
                    this.Client.DownloadString(string.Format("http://{0}:{1}/player/navigation/music", this.Config["client"], this.Config["clientPort"]));
                }
                else if(controlSplit[1].ToLower().Equals("moveup"))
                {
                    this.Client.DownloadString(string.Format("http://{0}:{1}/player/navigation/moveUp", this.Config["client"], this.Config["clientPort"]));
                }
                else if(controlSplit[1].ToLower().Equals("movedown"))
                {
                    this.Client.DownloadString(string.Format("http://{0}:{1}/player/navigation/moveDown", this.Config["client"], this.Config["clientPort"]));
                }
                else if(controlSplit[1].ToLower().Equals("moveleft"))
                {
                    this.Client.DownloadString(string.Format("http://{0}:{1}/player/navigation/moveLeft", this.Config["client"], this.Config["clientPort"]));
                }
                else if(controlSplit[1].ToLower().Equals("moveright"))
                {
                    this.Client.DownloadString(string.Format("http://{0}:{1}/player/navigation/moveRight", this.Config["client"], this.Config["clientPort"]));
                }
                else if(controlSplit[1].ToLower().Equals("select"))
                {
                    this.Client.DownloadString(string.Format("http://{0}:{1}/player/navigation/select", this.Config["client"], this.Config["clientPort"]));
                }
                else if(controlSplit[1].ToLower().Equals("back"))
                {
                    this.Client.DownloadString(string.Format("http://{0}:{1}/player/navigation/back", this.Config["client"], this.Config["clientPort"]));
                }
                else if (controlSplit[1].ToLower().Equals("play"))
                {
                    this.Client.DownloadString(string.Format("http://{0}:{1}/player/playback/play", this.Config["client"], this.Config["clientPort"]));
                }
                else if (controlSplit[1].ToLower().Equals("pause"))
                {
                    this.Client.DownloadString(string.Format("http://{0}:{1}/player/playback/pause", this.Config["client"], this.Config["clientPort"]));
                }
                else if (controlSplit[1].ToLower().Equals("stop"))
                {
                    this.Client.DownloadString(string.Format("http://{0}:{1}/player/playback/stop", this.Config["client"], this.Config["clientPort"]));
                }
                else if (controlSplit[1].ToLower().Equals("skipnext"))
                {
                    this.Client.DownloadString(string.Format("http://{0}:{1}/player/playback/skipNext", this.Config["client"], this.Config["clientPort"]));
                }
                else if (controlSplit[1].ToLower().Equals("skipprevious"))
                {
                    this.Client.DownloadString(string.Format("http://{0}:{1}/player/playback/skipPrevious", this.Config["client"], this.Config["clientPort"]));
                }
                else if (controlSplit[1].ToLower().Equals("stepforward"))
                {
                    this.Client.DownloadString(string.Format("http://{0}:{1}/player/playback/stepForward", this.Config["client"], this.Config["clientPort"]));
                }
                else if (controlSplit[1].ToLower().Equals("stepback"))
                {
                    this.Client.DownloadString(string.Format("http://{0}:{1}/player/playback/stepBack", this.Config["client"], this.Config["clientPort"]));
                }
                else if (controlSplit[1].ToLower().Equals("nextletter"))
                {
                    this.Client.DownloadString(string.Format("http://{0}:{1}/player/navigation/nextLetter", this.Config["client"], this.Config["clientPort"]));
                }
                else if (controlSplit[1].ToLower().Equals("previousletter"))
                {
                    this.Client.DownloadString(string.Format("http://{0}:{1}/player/navigation/previousLetter", this.Config["client"], this.Config["clientPort"]));
                }
                else if (controlSplit[1].ToLower().Equals("toggleosd"))
                {
                    this.Client.DownloadString(string.Format("http://{0}:{1}/player/navigation/toggleOSD", this.Config["client"], this.Config["clientPort"]));
                }
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
