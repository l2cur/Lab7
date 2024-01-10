using ClassLibrary2.comment;
using ClassLibrary2.MyClass;
using System.Reflection;
using System.Xml;
using System;

class Program
{
    static void Main()
    {
        XmlDocument doc = new XmlDocument();
        XmlElement rootElement = doc.CreateElement("ClassDiagram");
        doc.AppendChild(rootElement);

        Assembly asm = Assembly.Load("ClassLibrary2");

        Type[] types = asm.GetTypes();
        foreach (Type t in types)
        {
            if (t.Namespace.Contains("ClassLibrary2"))
            {
                string str = null;
                if (t.IsClass) { str = "Class"; }
                if (t.IsEnum) { str = "Enum"; }
                XmlElement element = doc.CreateElement(str);
                rootElement.AppendChild(element);
                element.SetAttribute("name", t.Name);

                CommentAttribute comment = (CommentAttribute)t.GetCustomAttribute(typeof(CommentAttribute));
                XmlElement c_element = doc.CreateElement("comment");
                if (comment != null)
                {
                    c_element.InnerText = comment.Comment;
                    element.AppendChild(c_element);
                }

                object[] propiproperties = t.GetProperties();
                foreach (object p in propiproperties)
                {
                    XmlElement p_element = doc.CreateElement("propiproperties");
                    p_element.InnerText = p.ToString();
                    element.AppendChild(p_element);
                }

                object[] methods = t.GetMethods(BindingFlags.DeclaredOnly);
                foreach (object m in methods)
                {
                    XmlElement m_element = doc.CreateElement("methods");
                    m_element.InnerText = m.ToString();
                    element.AppendChild(m_element);
                }
            }
        }
        doc.Save(Console.Out);
    }
}