using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GameUnion
{
    public class QueryReturn
    {
        Dictionary<string, object> dictionary;
        Dictionary<String, Object>.ValueCollection values;

        public QueryReturn()
        {
            dictionary = new Dictionary<string, object>();
            //values = dictionary.Values;
        }
    }
    /*
     * ChannelInfo Class
     * Used to parse info from JSON about a channel
     */
    public class ChannelInfo
    {
        public String id;
        public String title;
        public String stream_type;
        public String audio_codec;
        public String category;
        public String format;
        public String geo;
        public String name;
        public String language;
        public String broadcaster;
        public String video_codec;
        public String up_time;
        public Boolean featured;
        public Boolean channel_subscription;
        public Boolean embed_enabled;
        public Boolean abuse_reported;
        public Int32 video_width;
        public Int32 video_height;
        public Int32 embed_count;
        public Int32 channel_count;
        public Int32 site_count;
        public Int32 stream_count;
        public Int32 channel_view_count;
        public Decimal video_bitrate;

        //constructor
        public ChannelInfo()
        {
            title = "";
            featured = false;
            channel_subscription = false;
            audio_codec = "";
            embed_count = 0;
            id = ""; 
            category = "";
            video_height = 0;
            site_count = 0;
            embed_enabled = false;
            up_time = ""; 
            format = "";
            channel_count = 0;
            stream_type = "";
            abuse_reported = false;
            video_width = 0;
            geo = "";
            name = "";
            language = "";
            stream_count = 0;
            video_bitrate = 0;
            broadcaster = "";
            video_codec = "";
            channel_view_count = 0;
        }
    }


    /*
     * TwitchInterface Class
     * Used to get information from the Twitch service
     */
    public class TwitchInterface
    {
        public List<String> channelsToGet;
        public List<ChannelInfo> channelInfoList;

        //constructor
        public TwitchInterface()
        {

            List<ChannelInfo> channels = GetLiveChannels();
            //ChannelInfo chan = QueryChannels("warcraft");
            //Create List of Channels to get info for
            channelsToGet = new List<string>();
            channelsToGet.Add("valyance");
            channelsToGet.Add("esltv_wot");
            channelsToGet.Add("FollowGrubby");

            //add channels info to a list
            channelInfoList = channels;
            foreach (String channelName in channelsToGet)
            {
                ChannelInfo currInfo = GetChannelInfo(channelName);
                //if the stream is offline, do not add it to the list
                if (currInfo.title != "")
                {
                    channelInfoList.Add(currInfo);
                }
            }
        }


        /*
         * method GetChannelInfo
         * Gets information about a channel specified including title, broadcast time etc.
         * Returns a ChannelInfo class
         */
        public ChannelInfo GetChannelInfo(String channelName)
        {
            ChannelInfo currChannel = new ChannelInfo();

            //send HTTP request
            String url = "http://api.justin.tv/api/stream/list.json?channel=" + channelName;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    //parse returned data for channel info
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var obj = js.Deserialize<dynamic>(reader.ReadToEnd());
                    
                    foreach (var o in obj)
                    {
                        currChannel.title = o["title"];
                        currChannel.featured = o["featured"];
                        currChannel.channel_subscription = o["channel_subscription"];
                        currChannel.audio_codec = o["audio_codec"];
                        currChannel.embed_count = o["embed_count"];
                        currChannel.id = o["id"];
                        currChannel.category = o["category"];
                        currChannel.video_height = o["video_height"];
                        currChannel.site_count = o["site_count"];
                        currChannel.embed_enabled = o["embed_enabled"];
                        currChannel.up_time = o["up_time"];
                        currChannel.format = o["format"];
                        currChannel.channel_count = o["channel_count"];
                        currChannel.stream_type = o["stream_type"];
                        currChannel.abuse_reported = o["abuse_reported"];
                        currChannel.video_width = o["video_width"];
                        currChannel.geo = o["geo"];
                        currChannel.name = o["name"];
                        currChannel.language = o["language"];
                        currChannel.stream_count = o["stream_count"];
                        currChannel.video_bitrate = o["video_bitrate"];
                        currChannel.broadcaster = o["broadcaster"];
                        currChannel.video_codec = o["video_codec"];
                        currChannel.channel_view_count = o["channel_view_count"];
                    }
                }
            }

            return currChannel;
        }

        /*
         * method GetLiveChannels
         * Gets information about currently live channels
         * Returns a List of ChannelInfo classes
         */
        public List<ChannelInfo> GetLiveChannels(int limit = 10)
        {
            List<ChannelInfo> channelList = new List<ChannelInfo>();

            //send HTTP request
            String url = "http://api.justin.tv/api/stream/list.json?limit=" + limit;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    //parse returned data for channel info
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var obj = js.Deserialize<dynamic>(reader.ReadToEnd());

                    foreach (var o in obj)
                    {
                        ChannelInfo currChannel = new ChannelInfo();
                        currChannel.title = o["title"];
                        currChannel.featured = o["featured"];
                        currChannel.channel_subscription = o["channel_subscription"];
                        currChannel.audio_codec = o["audio_codec"];
                        currChannel.embed_count = o["embed_count"];
                        currChannel.id = o["id"];
                        currChannel.category = o["category"];
                        if (o.ContainsKey("video_height")) { currChannel.video_height = o["video_height"]; }
                        currChannel.site_count = o["site_count"];
                        currChannel.embed_enabled = o["embed_enabled"];
                        currChannel.up_time = o["up_time"];
                        currChannel.format = o["format"];
                        currChannel.channel_count = o["channel_count"];
                        currChannel.stream_type = o["stream_type"];
                        currChannel.abuse_reported = o["abuse_reported"];
                        if (o.ContainsKey("video_width")) { currChannel.video_width = o["video_width"]; }
                        currChannel.geo = o["geo"];
                        currChannel.name = o["name"];
                        currChannel.language = o["language"];
                        currChannel.stream_count = o["stream_count"];
                        currChannel.video_bitrate = o["video_bitrate"];
                        currChannel.broadcaster = o["broadcaster"];
                        currChannel.video_codec = o["video_codec"];
                        currChannel.channel_view_count = o["channel_view_count"];
                        channelList.Add(currChannel);
                    }
                }
            }

            return channelList;
        }

        /*
         * method QueryChannels
         * Gets information about a channel specified including title, broadcast time etc.
         * Returns a ChannelInfo class
         */
       /* public ChannelInfo QueryChannels(String query)
        {
            ChannelInfo currChannel = new ChannelInfo();

            //send HTTP request
            String url = "https://api.twitch.tv/kraken/search/streams?q=" + query;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    //parse returned data for channel info
                    
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    Dictionary<string, object> obj = js.Deserialize<dynamic>(reader.ReadToEnd());
                    object game = obj.Values.ElementAt(0);
                    
                        currChannel.title = o["Streams"];
                        currChannel.featured = o["featured"];
                        currChannel.channel_subscription = o["channel_subscription"];
                        currChannel.audio_codec = o["audio_codec"];
                        currChannel.embed_count = o["embed_count"];
                        currChannel.id = o["id"];
                        currChannel.category = o["category"];
                        currChannel.video_height = o["video_height"];
                        currChannel.site_count = o["site_count"];
                        currChannel.embed_enabled = o["embed_enabled"];
                        currChannel.up_time = o["up_time"];
                        currChannel.format = o["format"];
                        currChannel.channel_count = o["channel_count"];
                        currChannel.stream_type = o["stream_type"];
                        currChannel.abuse_reported = o["abuse_reported"];
                        currChannel.video_width = o["video_width"];
                        currChannel.geo = o["geo"];
                        currChannel.name = o["name"];
                        currChannel.language = o["language"];
                        currChannel.stream_count = o["stream_count"];
                        currChannel.video_bitrate = o["video_bitrate"];
                        currChannel.broadcaster = o["broadcaster"];
                        currChannel.video_codec = o["video_codec"];
                        currChannel.channel_view_count = o["channel_view_count"];
                    
                }
            }

            return currChannel;
        }*/

        /*
         * Replaces the video feeds innerHTML content to switch the feed in the twitch viewer
         * currChannel is the name of the current channel you are watching
         * ChannelName is the name of the channel to be switched to
         * innerHtml is the content of the video's tag which we modify to change the source
         * 
         * returns new string to be used as the new innerHTML content of the feed
         */
        public String SwitchChannel(String currChannel, String ChannelName, String innerHtml)
        {
            String temp = innerHtml.Replace(currChannel, ChannelName);
            return temp;
        }

        /*
         * Replaces the chat feeds innerHTML content to switch the feed in the twitch viewer
         * currChannel is the name of the current channel you are watching
         * ChannelName is the name of the channel to be switched to
         * innerHtml is the content of the video's tag which we modify to change the source
         * 
         * returns new string to be used as the new innerHTML content of the feed
         */
        public String SwitchChatChannel(String currChannel, String ChannelName, String innerHtml)
        {
            String temp = innerHtml.Replace(currChannel, ChannelName);
            return temp;
        }
    }


    
}