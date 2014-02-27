using System;

namespace LegacyWPF
{
    public class Container
    {
        public String Code { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public Container(String sign, int number, String check, String description, String image)
        {
            Code = sign + " " + number.ToString().PadLeft(6, '0') + " " + check;
            Description = description;
            Image = image;
        }
    }
}
