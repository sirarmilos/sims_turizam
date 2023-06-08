using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class Guest1Report : ISerializable
    {
        public int Guest1ReportId { get; set; }

        public Comment Guest1Comment { get; set; }

        public string ReportMaker { get; set; }

        public Guest1Report()
        {

        }

        public Guest1Report(int guest1ReportId, Comment guest1Comment, string reportMaker)
        {
            Guest1ReportId = guest1ReportId;
            Guest1Comment = guest1Comment;
            ReportMaker = reportMaker;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Guest1ReportId.ToString(), Guest1Comment.CommentId.ToString(), ReportMaker };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            // return if the file was empty
            if (string.IsNullOrWhiteSpace(values[0])) return;

            Guest1ReportId = Convert.ToInt32(values[0]);
            Guest1Comment = new Comment() { CommentId = Convert.ToInt32(values[1]) };
            ReportMaker = values[2];
        }
    }
}
