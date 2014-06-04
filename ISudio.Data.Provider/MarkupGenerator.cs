using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ISudio.Data.Provider
{
    public class MarkupGenerator
    {
        private readonly ObservableCollection<Slide> _slides;

        public MarkupGenerator(ObservableCollection<Slide> aslides)
        {
            _slides = aslides;
        }

        public List<String> GetMarkup()
        {
            char Q = Convert.ToChar(34);
            var markup = new List<string>();
            foreach (Slide slide in _slides)
            {
                string data = @"<div id=" + Q + slide.Id + Q + " class=" + Q + slide.Class + Q +
                       " data-x=" + Q + slide.DataX + Q + " data-y=" + Q + slide.DataY + Q +
                       " data-z=" + Q + slide.DataZ + Q;
                if (slide.DataRotate != 0)
                {
                    data += " data-rotate=" + Q + slide.DataRotate + Q +
                        " data-scale=" + Q + slide.DataScale + Q + " >";
                }
                else
                {
                    data += " data-rotate-x=" + Q + slide.DataRotateX + Q + " data-rotate-y=" + Q + slide.DataRotateY + Q + " data-rotate-z=" + Q + slide.DataRotateZ + Q;

                }
                data += Environment.NewLine;
                data += String.Format("<h2 style={0}text-align: center;{0}>{1}</h2>", Q, slide.Header);
                data += Environment.NewLine;
                data += "<p>&nbsp</p>";
                foreach (string str in slide.SlideMatter)
                {
                    data += Environment.NewLine + str;
                }
                data += " </div>";
                markup.Add(data);
            }
            return markup;
        }
    }
}