using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace testGround
{
    public class MyTreeObject
    {
        public string Parent { get; set; }
        public string Child { get; set; }
    }
    public class Helper
    {
        public dynamic AddChild(string child, JObject parentOfCurrentChild)
        {
            JProperty current = FindChildByText(child, parentOfCurrentChild);
            if (current.Count > 0)
            {
                return current;
            }
            parentOfCurrentChild.Add(new JProperty("text", child));
            return parentOfCurrentChild;
            }

        private JProperty FindChildByText(string child, JObject obj)
        {
         var p=   obj.Properties().Where(prop => prop.Contains(child)).ToList();
            
            JProperty newChildJObject = new JProperty("text",child);
            return newChildJObject;
        }
    }
}
