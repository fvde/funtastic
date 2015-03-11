using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace funtastic.DataModel
{
    public class Joke
    {
        public string Id { get; set; }

        private string content;
        [JsonProperty(PropertyName = "content")]
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        private string authorId;
        [JsonProperty(PropertyName = "authorId")]
        public string AuthorId
        {
            get { return authorId; }
            set
            {
                authorId = value;
            }
        }

        private int rating;
        [JsonProperty(PropertyName = "rating")]
        public int Rating
        {
            get { return rating; }
            set
            {
                rating = value;
            }
        }

        private int reports;
        [JsonProperty(PropertyName = "reports")]
        public int Reports
        {
            get { return reports; }
            set
            {
                reports = value;
            }
        }

        [JsonProperty(PropertyName = "timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }

        public Joke()
        {
            this.Rating = 0;
            this.Reports = 0;
            this.Content = "";
            this.AuthorId = "";
            this.Timestamp = DateTime.Now;
            this.Language = System.Globalization.CultureInfo.CurrentCulture.EnglishName;
        }
    }
}
