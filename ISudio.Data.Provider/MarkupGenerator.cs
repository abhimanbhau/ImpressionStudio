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
            char dQuote = Convert.ToChar(34);
            var markup = new List<string>();
            foreach (Slide slide in _slides)
            {
                string data;
                data =
                    String.Format(
                        @"<div id={0}{1}{0} class={0}{2}{0} data-x={0}{3}{0} data-y={0}{4}{0} data-z={0}{5}{0} data-rotate={0}{6}{0} data-rotate-x={0}{7}{0} data-rotate-y={0}{8}{0} data-rotate-z={0}{9}{0} data-scale={0}{10}{0} >",
                        dQuote, slide.Id, slide.Class, slide.DataX, slide.DataY, slide.DataZ, slide.DataRotate,
                        slide.DataRotateX, slide.DataRotateY, slide.DataRotateZ, slide.DataScale);
                data += Environment.NewLine;
                data += String.Format("<h2 style={0}text-align: center;{0}>{1}</h2>", dQuote, slide.Header);
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